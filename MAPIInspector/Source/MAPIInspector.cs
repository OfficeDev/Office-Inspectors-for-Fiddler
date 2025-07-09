using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using Fiddler;
using global::MAPIInspector.Parsers;
using static MapiInspector.MAPIParser;

namespace MapiInspector
{
    /// <summary>
    /// MAPIInspector Class
    /// </summary>
    public abstract class MAPIInspector : Inspector2
    {
        /// <summary>
        /// The JsonResult is used to save the Json string which converted by parse result
        /// </summary>
        public static string JsonResult = string.Empty;

        /// <summary>
        /// The JsonFile is used to set a file name to save JsonResult
        /// </summary>
        public static string JsonFile = "Json.txt";

        /// <summary>
        /// The JsonFile is used to set a file name to save error messages when automation test
        /// </summary>
        public static string ErrorFile = "Error.txt";

        /// <summary>
        /// Gets or sets the Tree View control where displayed the MAPI message.
        /// </summary>
        public TreeView MAPIViewControl { get; set; }

        /// <summary>
        /// Gets or sets the control collection where displayed the MAPI parsed message and corresponding hex data.
        /// </summary>
        public MAPIControl MAPIControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame has been changed.
        /// </summary>
        public bool bDirty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the frame is read-only.
        /// </summary>
        public bool bReadOnly { get; set; }

        /// <summary>
        /// Gets the direction of the traffic
        /// </summary>
        public MAPIParser.TrafficDirection Direction
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
        /// Gets a value indicating whether the message is MAPI protocol message.
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
                    else if ((this is IResponseInspector2) && this.session.id != 0)
                    {
                        if ((this is IResponseInspector2) && this.session.ResponseHeaders.Exists("X-ResponseCode"))
                        {
                            string responseCode = this.session.ResponseHeaders["X-ResponseCode"];
                            if (responseCode == "0")
                            {
                                return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "application/mapi-http");
                            }
                            else if (responseCode != string.Empty)
                            {
                                return this.session.ResponseHeaders.ExistsAndContains("Content-Type", "text/html");
                            }
                        }
                    }
                    else if ((this is IResponseInspector2) && this.session["X-ResponseCode"] != null)
                    {
                        string responseCode = this.session["X-ResponseCode"];
                        if (responseCode == "0")
                        {
                            return this.session["Content-Type"] != null && this.session["Content-Type"] == "application/mapi-http";
                        }
                        else if (responseCode != string.Empty)
                        {
                            return this.session["Content-Type"] != null && this.session["Content-Type"] == "text/html";
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the Session object to pull frame data from Fiddler.
        /// </summary>
        internal Session session { get; set; }

        /// <summary>
        /// Gets or sets the raw bytes from the frame
        /// </summary>
        private byte[] rawBody { get; set; }

        /// <summary>
        /// Called by Fiddler to add the MAPI inspector tab
        /// </summary>
        /// <param name="o">The tab control for the inspector</param>
        public override void AddToTab(TabPage o)
        {
            o.Text = "MAPI";
            this.MAPIControl = new MAPIControl();
            o.Controls.Add(this.MAPIControl);
            this.MAPIControl.Size = o.Size;
            this.MAPIControl.Dock = DockStyle.Fill;
            this.MAPIViewControl = this.MAPIControl.MAPITreeView;
            this.MAPIControl.MAPIHexBox.VScrollBarVisible = true;
            this.MAPIViewControl.AfterSelect -= this.TreeView_AfterSelect;
            this.MAPIViewControl.AfterSelect += this.TreeView_AfterSelect;
            DecodingContext dc = new DecodingContext();
        }

        /// <summary>
        /// Represents the method, which is used to handle the AfterSelect event of a TreeView.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">A System.Windows.Forms.TreeViewEventArgs that contains the event data.</param>
        public void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int request = sender.ToString().LastIndexOf("Request");
            int response = sender.ToString().LastIndexOf("Response");

            if (e.Node.Tag == null)
            {
                this.MAPIControl.MAPIHexBox.Select(0, 0);
                this.MAPIControl.CROPSHexBox.Select(0, 0);
            }
            else
            {
                var pos = e.Node.Tag as BaseStructure.Position;
                if (pos != null)
                {
                    if (pos.IsCompressedXOR)
                    {
                        if (pos.IsAuxiliayPayload)
                        {
                            this.MAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(AuxPayLoadCompressedXOR);
                        }
                        else
                        {
                            if (request > response)
                            {
                                this.MAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(InputPayLoadCompressedXOR[pos.BufferIndex]);
                            }
                            else
                            {
                                this.MAPIControl.CROPSHexBox.ByteProvider = new StaticByteProvider(OutputPayLoadCompressedXOR[pos.BufferIndex]);
                            }
                        }

                        this.MAPIControl.CROPSHexBox.Select(pos.StartIndex, pos.Offset);
                        this.MAPIControl.MAPIHexBox.Select(0, 0);
                        this.MAPIControl.CROPSHexBox.Visible = true;
                        ToolTip toolTip = new ToolTip();
                        toolTip.SetToolTip(this.MAPIControl.CROPSHexBox, "This is decompressed payload data.");
                        this.MAPIControl.SplitContainer.Panel2Collapsed = false;
                    }
                    else
                    {
                        this.MAPIControl.MAPIHexBox.Select(pos.StartIndex, pos.Offset);
                        this.MAPIControl.CROPSHexBox.Visible = false;
                        this.MAPIControl.SplitContainer.Panel2Collapsed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method that returns a sorting hint
        /// </summary>
        /// <returns>An integer indicating where we should order ourselves</returns>
        public override int GetOrder()
        {
            return 0;
        }

        /// <summary>
        /// Method Fiddler calls to clear the display
        /// </summary>
        public void Clear()
        {
            this.MAPIViewControl.Nodes.Clear();
            this.MAPIControl.MAPIRichTextBox.Visible = false;
            this.MAPIControl.MAPIRichTextBox.Clear();
            this.MAPIControl.CROPSHexBox.Visible = false;
            byte[] empty = new byte[0];
            this.MAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(empty);
            this.MAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
            this.MAPIControl.SplitContainer.Panel2Collapsed = true;
        }

        /// <summary>
        /// Called by Fiddler to determine how confident this inspector is that it can
        /// decode the data.  This is only called when the user hits enter or double-
        /// clicks a session.
        /// If we score the highest out of the other inspectors, Fiddler will open this
        /// inspector's tab and then call AssignSession.
        /// </summary>
        /// <param name="oS">the session object passed by Fiddler</param>
        /// <returns>Int between 0-100 with 100 being the most confident</returns>
        public override int ScoreForSession(Session oS)
        {
            if (null == this.session)
            {
                this.session = oS;
            }

            if (null == BaseHeaders)
            {
                if (this is IRequestInspector2)
                {
                    BaseHeaders = this.session.oRequest.headers;
                }
                else
                {
                    BaseHeaders = this.session.oResponse.headers;
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
        /// This is called every time this inspector is shown
        /// </summary>
        /// <param name="oS">Session object passed by Fiddler</param>
        public override void AssignSession(Session oS)
        {
            this.session = oS;
            base.AssignSession(oS);
        }

        /// <summary>
        /// Gets or sets the body byte[], called by Fiddler with session byte[]
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
        /// Display the object in Tree View
        /// </summary>
        /// <param name="obj">The object to display</param>
        /// <param name="bytesForHexview">The byte array provided for HexView</param>
        public void DisplayObject(object obj, byte[] bytesForHexview)
        {
            if (obj == null)
            {
                return;
            }

            this.MAPIViewControl.BeginUpdate();
            try
            {
                int result;
                TreeNode topNode = BaseStructure.AddNodesForTree("DisplayObjectRoot", obj, 0, out result);
                this.MAPIViewControl.Nodes.Add(topNode);
                topNode.ExpandAll();
                this.MAPIControl.MAPIHexBox.ByteProvider = new StaticByteProvider(bytesForHexview);
                this.MAPIControl.MAPIHexBox.ByteProvider.ApplyChanges();
                if (this.MAPIViewControl.Nodes.Count != 0)
                {
                    this.MAPIViewControl.Nodes[0].EnsureVisible();
                }
            }
            catch (Exception e)
            {
                this.MAPIControl.MAPIRichTextBox.Visible = true;
                this.MAPIControl.MAPIRichTextBox.Text = e.ToString();
            }
            finally
            {
                this.MAPIViewControl.EndUpdate();
            }
        }

        /// <summary>
        /// Reset public parameters.
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        public void AfterCallDoImport(object sender, EventArgs e)
        {
            ResetHandleInformation();
            Partial.ResetPartialContextInformation();
            Partial.ResetPartialParameters();
        }

        /// <summary>
        /// Update the view with parsed and diagnosed data
        /// </summary>
        private void UpdateView()
        {
            this.Clear();
            byte[] bytesForHexView;
            object parserResult;
            IsLooperCall = false;
            TargetHandle = new Stack<Dictionary<RopIdType, Dictionary<int, uint>>>();
            ContextInformationCollection = new List<ContextInformation>();
            Partial.ResetPartialParameters();

            if (this.IsMapihttp)
            {
                List<Session> allSessionsList = new List<Session>();
                Session session0 = new Session(new byte[0], new byte[0]);
                Session[] sessionsInFiddler = FiddlerApplication.UI.GetAllSessions();
                allSessionsList.AddRange(sessionsInFiddler);
                FiddlerApplication.OnLoadSAZ += this.AfterCallDoImport;
                allSessionsList.Sort(delegate (Session p1, Session p2)
                {
                    return p1.id.CompareTo(p2.id);
                });
                allSessionsList.Insert(0, session0);
                AllSessions = allSessionsList.ToArray();
                int allSessionLength = AllSessions.Length;

                if (allSessionLength > 0 && AllSessions[allSessionLength - 1]["Number"] == null)
                {
                    SetIndexForContextRelatedMethods();
                }

                try
                {
                    if (this.Direction == TrafficDirection.In)
                    {
                        parserResult = ParseHTTPPayload(BaseHeaders, this.session, this.session.requestBodyBytes, TrafficDirection.In, out bytesForHexView);
                    }
                    else
                    {
                        // An X-ResponseCode of 0 (zero) means success from the perspective of the protocol transport, and the client SHOULD parse the response body based on the request that was issued.
                        if (BaseHeaders["X-ResponseCode"] != "0")
                        {
                            return;
                        }

                        parserResult = ParseHTTPPayload(BaseHeaders, this.session, this.session.responseBodyBytes, TrafficDirection.Out, out bytesForHexView);
                    }

                    this.DisplayObject(parserResult, bytesForHexView);
                }
                catch (Exception e)
                {
                    parserResult = e.ToString();
                }
                finally
                {
                    DecodingContext.Notify_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>>();
                    DecodingContext.RowRops_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>>();
                    TargetHandle = new Stack<Dictionary<RopIdType, Dictionary<int, uint>>>();
                    ContextInformationCollection = new List<ContextInformation>();
                    IsLooperCall = true;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Parse the sessions from capture file using the MAPI Inspector
        /// Used in test automation
        /// </summary>
        /// <param name="sessionsFromCore">The sessions which from FiddlerCore to parse</param>
        /// <param name="pathName">The path for save result file</param>
        /// <param name="autoCaseName">The test case name to parse</param>
        /// <param name="allRops">All ROPs contained in list</param>
        /// <returns>Parse result, true means success</returns>
        public bool ParseCaptureFile(Fiddler.Session[] sessionsFromCore, string pathName, string autoCaseName, out List<string> allRops)
        {
            var errorStringList = new List<string>();
            StringBuilder stringBuilder = new StringBuilder();
            AllSessions = sessionsFromCore;
            DecodingContext decodingContext = new DecodingContext();
            Partial.ResetPartialParameters();
            Partial.ResetPartialContextInformation();
            ResetHandleInformation();
            for (int i = 0; i < AllSessions.Length; i++)
            {
                var session = AllSessions[i];
                Session val = AllSessions[i];
                if (AllSessions[i]["VirtualID"] != null)
                {
                    ParsingSession = val;
                }
                if (AllSessions.Length > 0 && AllSessions[AllSessions.Length - 1]["Number"] == null)
                {
                    SetIndexForContextRelatedMethods();
                }
                if (IsMapihttpWithoutUI())
                {
                    try
                    {
                        IsLooperCall = false;
                        Partial.ResetPartialParameters();
                        BaseHeaders = val.RequestHeaders;
                        byte[] bytes;
                        object obj = ParseHTTPPayload(BaseHeaders, val, val.requestBodyBytes, TrafficDirection.In, out bytes);
                        JsonResult += Utilities.ConvertCSharpToJson(i, isRequest: true, obj);
                        if (val["X-ResponseCode"] == "0")
                        {
                            object obj2 = ParseHTTPPayload(BaseHeaders, val, val.responseBodyBytes, TrafficDirection.Out, out bytes);
                            JsonResult += Utilities.ConvertCSharpToJson(i, isRequest: false, obj2);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorStringList.Add(string.Format("{0}. Error: Frame#{1} Error Message:{2}", errorStringList.Count + 1, val["VirtualID"], ex.Message));
                    }
                    finally
                    {
                        DecodingContext.Notify_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[], string>>>();
                        DecodingContext.RowRops_handlePropertyTags = new Dictionary<uint, Dictionary<int, Tuple<string, string, string, PropertyTag[]>>>();
                        TargetHandle = new Stack<Dictionary<RopIdType, Dictionary<int, uint>>>();
                        ContextInformationCollection = new List<ContextInformation>();
                        IsLooperCall = true;
                    }
                }
                if (i % 10 == 0 && JsonResult.Length != 0)
                {
                    string path = pathName + Path.DirectorySeparatorChar.ToString() + autoCaseName + "-" + JsonFile;
                    if (!File.Exists(path))
                    {
                        using (StreamWriter streamWriter = File.CreateText(path))
                        {
                            streamWriter.WriteLine(JsonResult);
                        }
                    }
                    else
                    {
                        using (StreamWriter streamWriter2 = File.AppendText(path))
                        {
                            streamWriter2.WriteLine(JsonResult);
                        }
                    }
                    JsonResult = string.Empty;
                }
            }
            allRops = AllRopsList;
            foreach (string errorString in errorStringList)
            {
                stringBuilder.AppendLine(errorString);
            }
            if (stringBuilder.Length != 0)
            {
                string path2 = pathName + Path.DirectorySeparatorChar.ToString() + autoCaseName + "-" + ErrorFile;
                File.WriteAllText(path2, stringBuilder.ToString());
                return false;
            }
            return true;
        }
    }
}