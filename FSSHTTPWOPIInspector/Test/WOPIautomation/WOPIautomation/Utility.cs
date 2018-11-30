using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Windows.Automation;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Management.Automation;
using System.Windows.Forms;

namespace WOPIautomation
{
    public static class Utility
    {
        /// <summary>
        /// Sign in office with right account
        /// </summary>
        /// <param name="userName">username used to sign in</param>
        /// <param name="Password">Password for the relative username</param>
        public static void OfficeSignIn(string userName, string Password)
        {
            User32API.KeybdInput(userName);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, 0, 0);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, 2, 0);
            User32API.KeybdInput(Password);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Enter, 0, 0, 0);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Enter, 0, 2, 0);
        }


        /// <summary>
        /// Sign in "Windows Security" alert with right account
        /// </summary>
        /// <param name="username">username used to sign in</param>
        /// <param name="password">Password for the relative username</param>
        public static void SigninWindowsSecurity(string username, string password)
        {
            User32API.KeybdInput(username);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, 0, 0);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Tab, 0, 2, 0);
            User32API.KeybdInput(password);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Enter, 0, 0, 0);
            User32API.keybd_event((byte)System.Windows.Forms.Keys.Enter, 0, 2, 0);
        }

        /// <summary>
        /// Transfer special symbol to AutoIt format
        /// </summary>
        /// <param name="originalStr">Original String</param>
        public static string AutoITStringFormat(string originalStr)
        {
            string result = string.Empty;
            List<int> targetIndexs = new List<int>();
            List<char> tmp = new List<char>();
            for (int i = 0; i < originalStr.Length; i++)
            {
                if (originalStr[i] == '{' || originalStr[i] == '}' || originalStr[i] == '^' || originalStr[i] == '+' || originalStr[i] == '!' || originalStr[i] == '#')
                {
                    tmp.Add('{');
                    tmp.Add(originalStr[i]);
                    tmp.Add('}');
                }
                else
                {
                    tmp.Add(originalStr[i]);
                }
            }
            result = new string(tmp.ToArray());
            return result;
        }

        /// <summary>
        /// Wait for document opening with word
        /// </summary>
        /// <param name="docName">Doc name</param>
        /// <param name="isreadonly">A bool value indicate if the document is readonly</param>
        public static void WaitForDocumentOpenning(string docName, bool isreadonly= false)
        {
            var desktop = AutomationElement.RootElement;
            if(isreadonly)
            {                
                Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + " [Read-Only] - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"), new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"));
                AutomationElement document = WaitForElement(desktop, multiCondition, TreeScope.Children,true);
            }
            else
            {
                Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"));
                AutomationElement document = WaitForElement(desktop, multiCondition, TreeScope.Children,true);
            } 
        }

        /// <summary>
        /// Wait for document opening online
        /// </summary>
        /// <param name="docName">Document name</param>
        /// <returns>A AutomationElement indicate opened document in word online</returns>
        public static AutomationElement WaitForOnlineDocument(string docName)
        {
            var desktop = AutomationElement.RootElement;
            AutomationElement document = WaitForElement(desktop, new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Internet Explorer"), TreeScope.Children, true);
            return document;
        }

        /// <summary>
        /// Close opened document by UI automation
        /// </summary>
        /// <param name="docName">Document name</param>
        /// <param name="isreadonly">A bool value indicate if the document is readonly</param>
        public static void CloseDocumentByUI(string docName, bool isreadonly = false)
        {
            var desktop = AutomationElement.RootElement;
            AutomationElement document = null;
            if (isreadonly)
            {
                document = WaitForElement(desktop, new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"), TreeScope.Children,true);
            }
            else
            {
                document = WaitForElement(desktop, new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"), TreeScope.Children,true);
            }
            Condition Close_button = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Close"));
            AutomationElement item_Close = document.FindFirst(TreeScope.Descendants, Close_button);
            InvokePattern Pattern_Close = (InvokePattern)item_Close.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Close.Invoke();
        }

        /// <summary>
        /// Close fileInUse window
        /// </summary>
        /// <param name="docName">Document name</param>
        public static void CloseFileInUsePane(string docName)
        {
            var desktop = AutomationElement.RootElement;
            Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"));
            AutomationElement documentFormat = WaitForElement(desktop, multiCondition, TreeScope.Children,true);
            AutomationElement FileInUseDialog = WaitForElement(documentFormat, new PropertyCondition(AutomationElement.NameProperty, "File In Use"), TreeScope.Children,true);
            Condition OK_button = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "OK"));
            AutomationElement item_OK = FileInUseDialog.FindFirst(TreeScope.Descendants, OK_button);
            InvokePattern Pattern_OK = (InvokePattern)item_OK.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_OK.Invoke();
        }


        /// <summary>
        /// Close FileNowAvailable window
        /// </summary>
        /// <param name="docName">Document name</param>
        public static void CloseFileNowAvailable(string docName)
        {
            var desktop = AutomationElement.RootElement;
            AutomationElement document = WaitForElement(desktop, new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"), TreeScope.Children, true);
            AutomationElement FileNowAvailableDialog = WaitForElement(document, new PropertyCondition(AutomationElement.NameProperty, "File Now Available"), TreeScope.Children, true);
            Condition Cancel_button = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Cancel"));
            AutomationElement item_Cancel = FileNowAvailableDialog.FindFirst(TreeScope.Descendants, Cancel_button);
            InvokePattern Pattern_Cancel = (InvokePattern)item_Cancel.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Cancel.Invoke();
        }

        /// <summary>
        /// Get the Opened document window in word online
        /// </summary>
        /// <param name="name">Document name</param>
        /// <returns>A AutomationElement indicate opened document in word online</returns>
        public static AutomationElement GetWordOnlineWindow(string name)
        {
            Process[] pro = Process.GetProcessesByName("WINWORD");
            string title = "";
            AutomationElement ele = null;
            WaitForElement(AutomationElement.RootElement, new PropertyCondition(AutomationElement.NameProperty, name + ".docx - Word"), TreeScope.Descendants);

            foreach (Process p in pro)
            {
                title = p.MainWindowTitle;
                if (title == (name + ".docx - Word"))
                {
                    var desktop = AutomationElement.RootElement;                    
                    ele = desktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, title));
                    break;
                }
            }
            return ele;
        }


        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        static extern bool SetFocus(IntPtr hWndLock);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Check out a document on opening word
        /// </summary>
        /// <param name="name">Document name</param>
        public static void CheckOutOnOpeningWord(string name)
        {
            AutomationElement docOnline = GetWordOnlineWindow(name);
            Condition File_Tab = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "File Tab"));
            WaitForElement(docOnline, File_Tab, TreeScope.Descendants);
            AutomationElement item_File = docOnline.FindFirst(TreeScope.Descendants, File_Tab);
            InvokePattern Pattern_File = (InvokePattern)item_File.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_File.Invoke();

            Condition Group_Info = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group), new PropertyCondition(AutomationElement.NameProperty, "Info"));
            AutomationElement item_Info = docOnline.FindFirst(TreeScope.Descendants,Group_Info);
            Condition Con_ManageVersions = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem), new PropertyCondition(AutomationElement.NameProperty, "Manage Document"));
            AutomationElement item_ManageVersions = item_Info.FindFirst(TreeScope.Descendants, Con_ManageVersions);

            ExpandCollapsePattern Pattern_ManageVersions = (ExpandCollapsePattern)item_ManageVersions.GetCurrentPattern(ExpandCollapsePatternIdentifiers.Pattern);
            Pattern_ManageVersions.Expand();

            // Find Word Window
            //IntPtr wordWindowHandle = FindWindow("OpusApp", "Doc.docx - Word");
            //SetForegroundWindow(wordWindowHandle);            
            //SetFocus(wordWindowHandle);
            //// Show Window SW_SHOWMAXIMIZED 3
            //ShowWindow(wordWindowHandle, 3);
            //// Send shortcut key Alt+i
            //SendKeys.SendWait("%i");
            //// Send shortcut Key R
            //SendKeys.SendWait("r");
            Condition Con_CheckOut = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem), new PropertyCondition(AutomationElement.NameProperty, "Check Out"));
            AutomationElement item_CheckOut = item_Info.FindFirst(TreeScope.Descendants,Con_CheckOut);

            InvokePattern Pattern_CheckOut = (InvokePattern)item_CheckOut.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_CheckOut.Invoke();
            Thread.Sleep(8000);
            Condition File_Save = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Save"));
            AutomationElement item_Save = docOnline.FindFirst(TreeScope.Descendants, File_Save);
            InvokePattern Pattern_Save = (InvokePattern)item_Save.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Save.Invoke();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Discard check out a document on opening word
        /// </summary>
        /// <param name="name">Document name</param>
        public static void DiscardCheckOutOnOpeningWord(string name)
        {
            AutomationElement docOnline = GetWordOnlineWindow(name);
            Condition File_Tab = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "File Tab"));
            AutomationElement item_File = docOnline.FindFirst(TreeScope.Descendants, File_Tab);
            InvokePattern Pattern_File = (InvokePattern)item_File.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_File.Invoke();

            Condition Group_Info = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group), new PropertyCondition(AutomationElement.NameProperty, "Info"));
            AutomationElement item_Info = docOnline.FindFirst(TreeScope.Descendants, Group_Info);
            Condition Con_AlertCheckOut = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group), new PropertyCondition(AutomationElement.NameProperty, "Alert - Checked Out Document"));
            AutomationElement item_AlertCheckOut = item_Info.FindFirst(TreeScope.Descendants, Con_AlertCheckOut);

            Condition Con_DiscardCheckOut = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Discard Check Out"));
            AutomationElement item_DiscardCheckOut = item_AlertCheckOut.FindFirst(TreeScope.Descendants, Con_DiscardCheckOut);
            InvokePattern Pattern_CheckOut = (InvokePattern)item_DiscardCheckOut.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_CheckOut.Invoke();
            CloseMicrosoftWordDialog(name,"Yes");
            
            Condition File_Save = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Save"));
            AutomationElement item_Save = docOnline.FindFirst(TreeScope.Descendants, File_Save);
            InvokePattern Pattern_Save = (InvokePattern)item_Save.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Save.Invoke();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Delete the defaut word empty format
        /// </summary>
        public static void DeleteDefaultWordFormat()
        {
            Process[] pro = Process.GetProcessesByName("WINWORD");
            string title = "";
            foreach (Process p in pro)
            {
                title = p.MainWindowTitle;
                if (title == "Word")
                {
                    var desktop = AutomationElement.RootElement;
                    AutomationElement wordFormat = desktop.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Word"));
                    Condition Close_button = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Close"));
                    AutomationElement item_Close = wordFormat.FindFirst(TreeScope.Descendants, Close_button);
                    InvokePattern Pattern_Close = (InvokePattern)item_Close.GetCurrentPattern(InvokePattern.Pattern);
                    Pattern_Close.Invoke();
                    break;
                }
            }
        }

        /// <summary>
        /// Close Internet explorer dialog
        /// </summary>
        public static void CloseInternetExplorerDialog()
        {
            var desktop = AutomationElement.RootElement;
            Condition Con_IE = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane), new PropertyCondition(AutomationElement.NameProperty, "Home - Home - Internet Explorer"));
            AutomationElement item_IE = WaitForElement(desktop, Con_IE, TreeScope.Children,true);
            PropertyCondition Con_IEDialog = new PropertyCondition(AutomationElement.NameProperty, "Internet Explorer");
            AutomationElement item_IEDialog = item_IE.FindFirst(TreeScope.Descendants,Con_IEDialog);
            if(item_IEDialog != null)
            {
                Condition Con_Close = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Close"));
                AutomationElement item_Close = item_IEDialog.FindFirst(TreeScope.Descendants, Con_Close);
                InvokePattern Pattern_Close = (InvokePattern)item_Close.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_Close.Invoke();
            }            
        }

        /// <summary>
        /// Close microsoft office dialog
        /// </summary>
        public static void CloseMicrosoftOfficeDialog()
        {
            var desktop = AutomationElement.RootElement;
            Condition Con_Office = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, "Microsoft Office"));
            AutomationElement item_Office = WaitForElement(desktop, Con_Office, TreeScope.Children,true);
            Condition Con_Yes = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Yes"));
            AutomationElement item_Yes = item_Office.FindFirst(TreeScope.Descendants,Con_Yes);
            InvokePattern Pattern_Yes = (InvokePattern)item_Yes.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Yes.Invoke();
        }

        /// <summary>
        /// Close microsoft word dialog
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="Accept">A string value specifies the value of accept button in dialog</param>
        public static void CloseMicrosoftWordDialog(string filename, string Accept)
        {
            var desktop = AutomationElement.RootElement;
            Condition orCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, filename + " - Word"), new PropertyCondition(AutomationElement.NameProperty, filename + ".docx - Word"));
            Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), orCondition);
            //AutomationElement item_Document = WaitForWindow(desktop, Con_Document, TreeScope.Children);
            AutomationElement item_Document = desktop.FindFirst(TreeScope.Children, Con_Document);
            Condition Con_Acc = null;
            AutomationElement item_Acc = null;
            if(Accept == "OK")
            {
                Thread.Sleep(2000);
                Condition Con_Word = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane), new PropertyCondition(AutomationElement.NameProperty, "Microsoft Word"));
                AutomationElement item_Word = WaitForElement(item_Document, Con_Word, TreeScope.Children, false);
                Con_Acc = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "OK"));
                item_Acc = item_Word.FindFirst(TreeScope.Descendants, Con_Acc);
            }
            else if (Accept == "Yes")
            {
                Condition Con_Word = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, "Microsoft Word"));
                AutomationElement item_Word = WaitForElement(item_Document, Con_Word, TreeScope.Children, true);
                Con_Acc = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Yes"));
                item_Acc = item_Word.FindFirst(TreeScope.Descendants, Con_Acc);
            }
            if (item_Acc != null)
            {
                InvokePattern Pattern_Yes = (InvokePattern)item_Acc.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_Yes.Invoke();
            }
        }

        /// <summary>
        /// Close checkin pane in opening word
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="keepCheckOut">Bool value indicate whether to keep check out when do checkIn</param>
        public static void CloseCheckInPane(string filename, bool keepCheckOut)
        {
            var desktop = AutomationElement.RootElement;
            Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, filename + ".docx - Word"));
            AutomationElement item_Document = desktop.FindFirst(TreeScope.Children,Con_Document);
            Condition Con_Checkin = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane), new PropertyCondition(AutomationElement.NameProperty, "Check In"));
            AutomationElement item_Checkin = WaitForElement(item_Document, Con_Checkin, TreeScope.Children,true);

            if(keepCheckOut)
            {
                Condition Con_CheckBox = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox), new PropertyCondition(AutomationElement.NameProperty, "Keep the document checked out after checking in this version."));
                AutomationElement item_CheckBox = item_Checkin.FindFirst(TreeScope.Descendants, Con_CheckBox);
                TogglePattern Pattern_CheckBox = (TogglePattern)item_CheckBox.GetCurrentPattern(TogglePattern.Pattern);
                Pattern_CheckBox.Toggle();
            }

            Condition Con_Yes = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "OK"));
            AutomationElement item_Yes = item_Checkin.FindFirst(TreeScope.Descendants, Con_Yes);
            InvokePattern Pattern_Yes = (InvokePattern)item_Yes.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Yes.Invoke();
        }

        /// <summary>
        /// Save a document which is opening and editing in office word
        /// </summary>
        /// <param name="filename">Document name</param>
        public static void WordEditSave(string filename)
        {
            var desktop = AutomationElement.RootElement;
            //Orignal: Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, filename + ".docx - Word"));
            Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, filename + " - Word"));
            AutomationElement item_Document = desktop.FindFirst(TreeScope.Children, Con_Document);
            Condition Con_Save = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Save"));
            AutomationElement item_Save = WaitForElement(item_Document, Con_Save, TreeScope.Descendants, false);
            InvokePattern Pattern_Save = (InvokePattern)item_Save.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Save.Invoke();
        }

        /// <summary>
        /// Merge document with conflict
        /// </summary>
        /// <param name="filename">file name</param>
        public static void WordConflictMerge(string filename)
        {
            var desktop = AutomationElement.RootElement;
            Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, filename + ".docx - Word"));
            AutomationElement item_Document = desktop.FindFirst(TreeScope.Children, Con_Document);
            Condition Con_Resolve = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Resolve"));
            AutomationElement item_Resolve = WaitForElement(item_Document, Con_Resolve, TreeScope.Descendants, false);
            item_Resolve = item_Document.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Resolve"));
            InvokePattern Pattern_Resolve = (InvokePattern)item_Resolve.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Resolve.Invoke();

            Condition Con_AcceptMyChange = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.SplitButton), new PropertyCondition(AutomationElement.NameProperty, "Accept My Change"));
            AutomationElement item_AcceptMyChange = WaitForElement(item_Document, Con_AcceptMyChange, TreeScope.Descendants, false);
            ExpandCollapsePattern Pattern_AcceptMyChange = (ExpandCollapsePattern)item_AcceptMyChange.GetCurrentPattern(ExpandCollapsePatternIdentifiers.Pattern);
            Pattern_AcceptMyChange.Expand();
            Condition Con_AcceptAll = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem), new PropertyCondition(AutomationElement.NameProperty, "Accept All Conflicting Changes in Document"));
            AutomationElement item_AcceptAll = WaitForElement(item_Document, Con_AcceptAll, TreeScope.Descendants, false);
            InvokePattern Pattern_AcceptAll = (InvokePattern)item_AcceptAll.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_AcceptAll.Invoke();
            Thread.Sleep(4000);
            Condition Con_SaveCloseView = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Save and Close View"));
            AutomationElement item_SaveCloseView = WaitForElement(item_Document, Con_SaveCloseView, TreeScope.Descendants, false);
            InvokePattern Pattern_SaveCloseView = (InvokePattern)item_SaveCloseView.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_SaveCloseView.Invoke();
            
        }

        /// <summary>
        /// Find a element in UI automation
        /// </summary>
        /// <param name="parent">parent element</param>
        /// <param name="condition">The find condition</param>
        /// <param name="scop">The find scop</param>
        /// <param name="isWindowElement">A bool value indicate if element is a window</param>
        /// <returns></returns>
        public static AutomationElement WaitForElement(AutomationElement parent, Condition condition, TreeScope scop,bool isWindowElement = false)
        {
            AutomationElement window = null;
            int Count = 0;
            while (window == null)
            {
                window = parent.FindFirst(scop, condition);
                Thread.Sleep(500);
                Count += 1;
                if (isWindowElement)
                {
                    if (Count >= 180)
                    {
                        break;
                    }
                }else
                {
                    if (Count >= 2)
                    {
                        break;
                    }
                }
                
            }
            return window;
        }

        /// <summary>
        /// Execute script method
        /// </summary>
        /// <param name="scriptPath">The script path</param>
        /// <param name="isStart">A bool value indicates whether script is for start trace</param>
        public static void ExecuteScript(string scriptPath, bool isStart = true)
        {
            string powershellPath = ConfigurationManager.AppSettings["Powershell_Path"];
            string userName = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["PasswordForExecuteScript"];
            string path = ConfigurationManager.AppSettings["Path"];
            string destination = ConfigurationManager.AppSettings["Destination"];

            // Configure the PowerShell execution policy to run script
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                string script = "Set-ExecutionPolicy -Scope currentuser -ExecutionPolicy bypass; Get-ExecutionPolicy"; // the second command to know the ExecutionPolicy level
                PowerShellInstance.AddScript(script);
                var someResult = PowerShellInstance.Invoke();
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "CMD.exe";
            startInfo.Verb = "runas";
            if (isStart)
            {
                startInfo.Arguments = "/user:Administrator cmd /c " + "powershell " + scriptPath + " " + userName + " " + password;
            }
            else
            {                
                startInfo.Arguments = "/user:Administrator cmd /c " + "powershell " + scriptPath + " " + userName + " " + password + " " + path + " " + TestBase.testResultPath + " " + WOPIautomation.TestBase.testName;
            }
            System.Diagnostics.Process.Start(startInfo);
        
            if (!isStart)
            {
                string captureFulPath = TestBase.testResultPath + Path.DirectorySeparatorChar + WOPIautomation.TestBase.testName + ".cap";
                bool result = FormatConvert.Convert(captureFulPath, TestBase.testResultPath, WOPIautomation.TestBase.testName);
            }
            else
            {
                Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// Wait for a fiddler file ready
        /// </summary>
        /// <param name="fileName">file name</param>
        public static void WaitFile(string fileName)
        {
            do
            {
                Thread.Sleep(6000);
            }
            while (!System.IO.File.Exists(fileName));
        }
        
    }
}
