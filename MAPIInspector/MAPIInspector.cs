using System.IO;
using System.Windows.Forms;
using Fiddler;
using MAPIInspector.Parsers;
using Be.Windows.Forms;

namespace MapiInspector
{
    public abstract class MAPIInspector : Inspector2
    {
        public TreeView oMAPIViewControl { get; set; }
        public MAPIControl oMAPIControl { get; set; }
        public bool bDirty { get; set; }
        public bool bReadOnly { get; set; }
        internal Session session { get; set; }
        private byte[] rawBody { get; set; }

        public TrafficDirection Direction
        {
            get
            {
                if (this is IRequestInspector2)
                {
                    return TrafficDirection.In;
                }
                else
                {
                    return TrafficDirection.Out;
                }
            }
        }

        public HTTPHeaders BaseHeaders { get; set; }

        public bool IsMapihttp
        {
            get
            {
                if (this.session != null)
                {
                    if (this is IRequestInspector2)
                    {
                        return this.session.RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                    }
                    else if ((this is IResponseInspector2) && this.session.ResponseHeaders.Exists("X-ResponseCode"))
                    {
                        string xResponseCode = this.session.ResponseHeaders["X-ResponseCode"];
                        if (xResponseCode == "0")
                        {
                            return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                        }
                        else if (xResponseCode != "")
                        {
                            return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "text/html");
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Called by Fiddler to add the inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            o.Text = "MAPI";
            this.oMAPIControl = new MAPIControl();
            o.Controls.Add(this.oMAPIControl);
            this.oMAPIControl.Size = o.Size;
            this.oMAPIControl.Dock = DockStyle.Fill;
            this.oMAPIViewControl = this.oMAPIControl.MAPITreeView;
            this.oMAPIControl.MAPIHexBox.VScrollBarVisible = true;
            this.oMAPIViewControl.AfterSelect -= TreeView_AfterSelect;
            this.oMAPIViewControl.AfterSelect += TreeView_AfterSelect;
        }

        void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.oMAPIControl.MAPIHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);
        }

        public override int GetOrder()
        {
            return 0;
        }

        public void Clear()
        {
            this.oMAPIViewControl.Nodes.Clear();
        }

        public override int ScoreForSession(Session oS)
        {
            if (null == this.session)
            {
                this.session = oS;
            }

            if (null == this.BaseHeaders)
            {
                if (this is IRequestInspector2)
                {
                    this.BaseHeaders = this.session.oRequest.headers;
                }
                else
                {
                    this.BaseHeaders = this.session.oResponse.headers;
                }
            }

            if (this.IsMapihttp)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public override void AssignSession(Session oS)
        {
            this.session = oS;
            base.AssignSession(oS);
        }

        public byte[] body
        {
            get
            {
                return this.rawBody;
            }
            set
            {
                this.rawBody = value;
                this.UpdateView();
            }
        }

        private void UpdateView()
        {
            this.Clear();

            if (this.Direction == TrafficDirection.In)
            {
                this.ParseHTTPPayload(this.BaseHeaders, this.session.requestBodyBytes, TrafficDirection.In);
            }
            else
            {
                //An X-ResponseCode of 0 (zero) means success from the perspective of the protocol transport, and the client SHOULD parse the response body based on the request that was issued.
                if (this.BaseHeaders["X-ResponseCode"] != "0")
                {
                    return;
                }
                this.ParseHTTPPayload(this.BaseHeaders, Utilities.GetPaylodFromChunkedBody(this.session.responseBodyBytes), TrafficDirection.Out);
            }
        }

        public void ParseHTTPPayload(HTTPHeaders headers, byte[] bytesFromHTTP, TrafficDirection direction)
        {
            if (bytesFromHTTP.Length == 0 || headers == null || !headers.Exists("X-RequestType"))
            {
                return;
            }

            string requestType = headers["X-RequestType"];

            if (requestType == null)
            {
                return;
            }

            this.oMAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(bytesFromHTTP);
            this.oMAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
            Stream stream = new MemoryStream(bytesFromHTTP);
            int result = 0;
            if (direction == TrafficDirection.In)
            {
                this.oMAPIViewControl.BeginUpdate();
                TreeNode topNode = new TreeNode(requestType + "Request:");

                switch (requestType)
                {
                    case "Connect":
                        {
                            ConnectRequestBody ConnectRequest = new ConnectRequestBody();
                            ConnectRequest.Parse(stream);
                            topNode = ConnectRequest.AddNodesForTree(ConnectRequest, 0, out result);
                            break;
                        }
                    case "Execute":
                        {
                            ExecuteRequestBody ExecuteRequest = new ExecuteRequestBody();
                            ExecuteRequest.Parse(stream);
                            topNode = ExecuteRequest.AddNodesForTree(ExecuteRequest, 0, out result);
                            break;
                        }
                    case "Disconnect":
                        {
                            DisconnectRequestBody DisconnectRequest = new DisconnectRequestBody();
                            DisconnectRequest.Parse(stream);
                            topNode = DisconnectRequest.AddNodesForTree(DisconnectRequest, 0, out result);
                            break;
                        }
                    case "NotificationWait":
                        {
                            NotificationWaitRequestBody NotificationWaitRequest = new NotificationWaitRequestBody();
                            NotificationWaitRequest.Parse(stream);
                            topNode = NotificationWaitRequest.AddNodesForTree(NotificationWaitRequest, 0, out result);
                            break;
                        }
                    case "Bind":
                        {
                            BindRequest bindRequest = new BindRequest();
                            bindRequest.Parse(stream);
                            topNode = bindRequest.AddNodesForTree(bindRequest, 0, out result);
                            break;
                        }
                    default:
                        break;
                }

                this.oMAPIViewControl.Nodes.Add(topNode);
                topNode.Expand();
                this.oMAPIViewControl.EndUpdate();

            }
            else
            {
                this.oMAPIViewControl.BeginUpdate();
                TreeNode topNode = new TreeNode(requestType + "Response:");

                switch (requestType)
                {
                    case "Connect":
                        {
                            ConnectResponseBody ConnectResponse = new ConnectResponseBody();
                            ConnectResponse.Parse(stream);
                            topNode = ConnectResponse.AddNodesForTree(ConnectResponse, 0, out result);
                            if (ConnectResponse.StatusCode == 0)
                            {
                                string text = topNode.Text.Replace("Response", "SuccessResponse");
                                topNode.Text = text;
                            }
                            else
                            {
                                string text = topNode.Text.Replace("Response", "FailureResponse");
                                topNode.Text = text;
                            }
                            break;
                        }
                    case "Execute":
                        {
                            ExecuteResponseBody ExecuteResponse = new ExecuteResponseBody();
                            ExecuteResponse.Parse(stream);
                            topNode = ExecuteResponse.AddNodesForTree(ExecuteResponse, 0, out result);
                            if (ExecuteResponse.StatusCode == 0)
                            {
                                string text = topNode.Text.Replace("Response", "SuccessResponse");
                                topNode.Text = text;
                            }
                            else
                            {
                                string text = topNode.Text.Replace("Response", "FailureResponse");
                                topNode.Text = text;
                            }
                            break;
                        }
                    case "Disconnect":
                        {

                            DisconnectResponseBody DisconnectResponse = new DisconnectResponseBody();
                            DisconnectResponse.Parse(stream);
                            topNode = DisconnectResponse.AddNodesForTree(DisconnectResponse, 0, out result);
                            if (DisconnectResponse.StatusCode == 0)
                            {
                                string text = topNode.Text.Replace("Response", "SuccessResponse");
                                topNode.Text = text;
                            }
                            else
                            {
                                string text = topNode.Text.Replace("Response", "FailureResponse");
                                topNode.Text = text;
                            }
                            break;
                        }
                    case "NotificationWait":
                        {

                            NotificationWaitResponseBody NotificationWaitResponse = new NotificationWaitResponseBody();
                            NotificationWaitResponse.Parse(stream);
                            topNode = NotificationWaitResponse.AddNodesForTree(NotificationWaitResponse, 0, out result);
                            if (NotificationWaitResponse.StatusCode == 0)
                            {
                                string text = topNode.Text.Replace("Response", "SuccessResponse");
                                topNode.Text = text;
                            }
                            else
                            {
                                string text = topNode.Text.Replace("Response", "FailureResponse");
                                topNode.Text = text;
                            }
                            break;
                        }
                    case "Bind":
                        {
                            BindResponse bindResponse = new BindResponse();
                            bindResponse.Parse(stream);
                            topNode = bindResponse.AddNodesForTree(bindResponse, 0, out result);
                            break;
                        }
                    default:
                        break;
                }

                this.oMAPIViewControl.Nodes.Add(topNode);
                topNode.Expand();
                this.oMAPIViewControl.EndUpdate();
            }
        }

        public enum TrafficDirection
        {
            In,
            Out
        }
    }
}