using System.IO;
using System.Windows.Forms;
using Fiddler;
using MAPIInspector.Parsers;
using Be.Windows.Forms;
using System;

namespace MapiInspector
{
    public abstract class MAPIInspector : Inspector2
    {
        /// <summary>
        /// Gets or sets the Tree View control where displayed the MAPI message.
        /// </summary>
        public TreeView oMAPIViewControl { get; set; }
        
        /// <summary>
        /// Gets or sets the control collection where displayed the MAPI parsed message and corresponding hex data.
        /// </summary>
        public MAPIControl oMAPIControl { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether or not the frame has been changed.
        /// </summary>
        public bool bDirty { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether or not the frame is read-only.
        /// </summary>
        public bool bReadOnly { get; set; }
        
        /// <summary>
        /// Gets or sets the Session object to pull frame data from Fiddler.
        /// </summary>
        internal Session session { get; set; }
        
        /// <summary>
        /// Gets or sets the raw bytes from the frame.
        /// </summary>
        private byte[] rawBody { get; set; }
        
        /// <summary>
        /// Gets the direction of the traffic.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the base HTTP headers assigned by the request or response.
        /// </summary>
        public HTTPHeaders BaseHeaders { get; set; }
        
        /// <summary>
        /// Gets whether the message is a MAPI protocol message.
        /// </summary>
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
        /// Called by Fiddler to add the MAPI inspector tab.
        /// </summary>
        /// <param name="o">The tab control for the inspector.</param>
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

        /// <summary>
        /// Represents the method, which is used to handle the AfterSelect event of a TreeView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A System.Windows.Forms.TreeViewEventArgs that contains the event data.</param>
        void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.oMAPIControl.MAPIHexBox.Select(((BaseStructure.Position)e.Node.Tag).StartIndex, ((BaseStructure.Position)e.Node.Tag).Offset);
        }

        /// <summary>
        /// Method that returns a sorting hint.
        /// </summary>
        /// <returns>An integer indicating where we should order ourselves.</returns>
        public override int GetOrder()
        {
            return 0;
        }

        /// <summary>
        /// Method Fiddler calls to clear the display.
        /// </summary>
        public void Clear()
        {
            this.oMAPIViewControl.Nodes.Clear();
            this.oMAPIControl.MAPIRichTextBox.Visible = false;
            this.oMAPIControl.MAPIRichTextBox.Clear();
            byte[] empty = new byte[0];
            this.oMAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(empty);
            this.oMAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
        }

        /// <summary>
        /// Called by Fiddler to determine how confident this inspector is that it can
        /// decode the data.  This is only called when the user hits enter or double-
        /// clicks a session.  
        /// If we score the highest out of the other inspectors, Fiddler will open this
        /// inspector's tab and then call AssignSession.
        /// </summary>
        /// <param name="oS">The session object passed by Fiddler.</param>
        /// <returns>Int between 0-100 with 100 being the most confident.</returns>
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

        /// <summary>
        /// This is called every time this inspector is shown.
        /// </summary>
        /// <param name="oS">Session object passed by Fiddler.</param>
        public override void AssignSession(Session oS)
        {
            this.session = oS;
            base.AssignSession(oS);
        }

        /// <summary>
        /// Gets or sets the body byte[], called by Fiddler with session byte[].
        /// </summary>
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

        /// <summary>
        /// Update the view with parsed and diagnosed data.
        /// </summary>
        private void UpdateView()
        {
            this.Clear();

            if (this.IsMapihttp)
            {
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
                    this.ParseHTTPPayload(this.BaseHeaders, this.session.responseBodyBytes, TrafficDirection.Out);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Parse the HTTP payload to MAPI message.
        /// </summary>
        /// <param name="headers">The HTTP header.</param>
        /// <param name="bytesFromHTTP">The raw data from HTTP layer.</param>
        /// <param name="direction">The direction of the traffic.</param>
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
            try
            {
                if (direction == TrafficDirection.Out && headers.Exists("Transfer-Encoding") && headers["Transfer-Encoding"] == "chunked")
                {
                    bytesFromHTTP = Utilities.GetPaylodFromChunkedBody(bytesFromHTTP);
                    this.oMAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(bytesFromHTTP);
                }
                else
                {
                    this.oMAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(bytesFromHTTP);
                }

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
                        {
                            this.oMAPIControl.MAPIRichTextBox.Visible = true;
                            this.oMAPIControl.MAPIRichTextBox.Text = "Unavailable Request Type.";
                            break;
                        }
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
                            if (bindResponse.StatusCode == 0)
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
                    default:
                        {
                            this.oMAPIControl.MAPIRichTextBox.Visible = true;
                            this.oMAPIControl.MAPIRichTextBox.Text = "Unavailable Response Type.";
                            break;
                        }
                }

                    this.oMAPIViewControl.Nodes.Add(topNode);
                    topNode.Expand();
                    this.oMAPIViewControl.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                this.oMAPIControl.MAPIRichTextBox.Visible = true;
                this.oMAPIControl.MAPIRichTextBox.Text = ex.ToString();
                this.oMAPIViewControl.EndUpdate();
            }
        }

        /// <summary>
        /// Enum for traffic direction.
        /// </summary>
        public enum TrafficDirection
        {
            In,
            Out
        }
    }
}