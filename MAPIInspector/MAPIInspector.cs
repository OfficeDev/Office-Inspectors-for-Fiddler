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
        public BaseStructure baseStructure;

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
            this.oMAPIViewControl = this.oMAPIControl.TreeView1;
            this.oMAPIControl.HexBox1.VScrollBarVisible = true;

            this.oMAPIViewControl.AfterSelect += delegate(object sender, TreeViewEventArgs e)
            {
                int[] offsetAndlength = baseStructure.TreeNodeOffsetAndLength[e.Node];
                this.oMAPIControl.HexBox1.Select(offsetAndlength[0], offsetAndlength[1]);
            };
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
                this.ParseHTTPPayload(this.BaseHeaders, Utilities.GetPaylodFromChunkedBody(this.session.responseBodyBytes), TrafficDirection.Out);
            }
        }

        public void ParseHTTPPayload(HTTPHeaders headers, byte[] bytesFromHTTP, TrafficDirection direction)
        {

            this.oMAPIControl.HexBox1.ByteProvider = new StaticByteProvider(bytesFromHTTP);
            this.oMAPIControl.HexBox1.ByteProvider.ApplyChanges();

            if (bytesFromHTTP.Length == 0 || headers == null || !headers.Exists("X-RequestType"))
            {
                return;
            }
            
            string requestType = headers["X-RequestType"];

            if (requestType == null)
            {
                return;
            }

            Stream stream = new MemoryStream(bytesFromHTTP);            

            if (direction == TrafficDirection.In)
            {
                this.oMAPIViewControl.BeginUpdate();
                TreeNode topNode = new TreeNode(requestType + "Request:");

                switch (requestType)
                {
                    case "Connect":
                        {
                            baseStructure = new ConnectRequestBodyType();
                            baseStructure.Parse(stream);
                            baseStructure.AddTreeChildren(topNode);
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
                            ConnectResponseBodyType ConnectResponse = new ConnectResponseBodyType();
                            ConnectResponse.Parse(stream);
                            ConnectResponse.AddTreeChildren(topNode);
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
