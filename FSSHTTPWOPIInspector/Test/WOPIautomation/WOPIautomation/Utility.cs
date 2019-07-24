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
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OneNote = Microsoft.Office.Interop.OneNote;
using System.Xml;
using System.Collections.ObjectModel;


namespace WOPIautomation
{
    public static class Utility
    {
        /// <summary>
        /// Coauther OneNote file WithoutConflict
        /// </summary>
        /// <param name="filename">The coauthered OneNote file name</param>
        public static void OneNoteCoauthorWithoutConflict(string oneNote)
        {
            string filename = oneNote.Split('\\').Last().Split('.').First();
            // Upload a document
            SharepointClient.UploadFile(oneNote);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".one']"));
            string curWinHandle = Browser.webDriver.CurrentWindowHandle;
            // Open OneNote document in local Onenote App
            Browser.RClick(document);
            Thread.Sleep(1000);
            Browser.Wait(By.LinkText("Open in OneNote"));
            var elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);
            Utility.WaitForOneNoteDocumentOpenning(filename, false, true);
            Thread.Sleep(2000);
            SendKeys.SendWait("Local");
            Thread.Sleep(3000);

            // Switch To Web Browser
            Browser.webDriver.SwitchTo().Window(curWinHandle);
            Thread.Sleep(2000);

            // Click OneNote file on Sharepoint Web Server
            Browser.Click(document);                  
            Thread.Sleep(3000);
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            // Wait for online edit saved
            Thread.Sleep(3000);            
            Browser.Wait(By.XPath("//a[@id='lblSyncStatus-Medium']/span[2][text()='Saved']"));
            Thread.Sleep(3000);
            SendKeys.SendWait("Online");
            Thread.Sleep(3000);
            Browser.Wait(By.XPath("//a[@id='lblSyncStatus-Medium']/span[2][text()='Saving...']"));
            Thread.Sleep(10000);
            Browser.Wait(By.XPath("//a[@id='lblSyncStatus-Medium']/span[2][text()='Saved']"));
            Thread.Sleep(5000);


            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            Thread.Sleep(2000);
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".one']"));
            // Open OneNote document in local Onenote App
            Browser.RClick(document);
            Thread.Sleep(1000);
            Browser.Wait(By.LinkText("Open in OneNote"));
            elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);
            //Utility.WaitForOneNoteDocumentOpenning(filename, false, true);

            /////////////////////////////////////////////////////////////////////////////////
            // Get the opened OneNote process, and read the page title.
            OneNote.Application oneNoteApp = new OneNote.Application();
            string oneNoteXml;
            var oneNoteWindow = oneNoteApp.Windows.CurrentWindow;
            oneNoteApp.GetPageContent(oneNoteWindow.CurrentPageId, out oneNoteXml);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(oneNoteXml);
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("one", "http://schemas.microsoft.com/office/onenote/2013/onenote");
            string titleXpath = "//one:Page/one:Title/one:OE/one:T";
            System.Xml.XmlCDataSection titleNode = xmlDoc.SelectSingleNode(titleXpath, nsmgr).FirstChild as System.Xml.XmlCDataSection;
            // If its title in local Onenote App is not updated and wait.
            while (!titleNode.Value.ToString().Contains("OnlineLocal"))
            {
                Thread.Sleep(5000);
                oneNoteWindow = oneNoteApp.Windows.CurrentWindow;
                oneNoteApp.GetPageContent(oneNoteWindow.CurrentPageId, out oneNoteXml);
                xmlDoc.LoadXml(oneNoteXml);
                titleNode = xmlDoc.SelectSingleNode(titleXpath, nsmgr).FirstChild as System.Xml.XmlCDataSection;
            }
            ///////////////////////////////////////////////////////////////////////////////////
            // Closed OneNote App.  
            

            oneNoteApp.Windows.CurrentWindow.Active = true;
            SendKeys.SendWait("%{F4}");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".one");            
        }

        /// <summary>
        /// Coauther OneNote file WithConflict
        /// </summary>
        /// <param name="oneNote"></param>
        public static void OneNoteCoauthorWithConflict(string oneNote)
        {
            string filename = oneNote.Split('\\').Last().Split('.').First();
            // Upload a document
            SharepointClient.UploadFile(oneNote);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement onenote = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".one']"));
            string DocumentWinHandle = Browser.webDriver.CurrentWindowHandle;

            // Open OneNote document in local Onenote App
            Browser.RClick(onenote);
            Browser.Wait(By.LinkText("Open in OneNote"));
            var elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);
            Utility.WaitForOneNoteDocumentOpenning(filename, false, true);
            SendKeys.SendWait("Insert by onenote App");
            Thread.Sleep(2000);


            // Click the document in root site
            Browser.RClick(onenote);
            Browser.Wait(By.LinkText("Open in OneNote Online"));
            var elementOpenOnline = Browser.webDriver.FindElement(By.LinkText("Open in OneNote Online"));
            Browser.Click(elementOpenOnline);
            //Browser.webDriver.FindElement(By.XPath("//a[@id='lblSyncStatus-Medium']/span[2][text()='Saved']"));
            Thread.Sleep(5000);
            SendKeys.SendWait("^a"); ;
            Thread.Sleep(1000);
            SendKeys.SendWait("{DELETE}");

            // Switch To Web Browser
            Browser.webDriver.SwitchTo().Window(DocumentWinHandle);
            // Open OneNote document in local Onenote App
            Browser.RClick(onenote);
            Browser.Wait(By.LinkText("Open in OneNote"));
            elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);
            Thread.Sleep(2000);
            // Save current window handle
            string curWinHandle = Browser.webDriver.CurrentWindowHandle;
            SendKeys.SendWait("^a");
            SendKeys.SendWait("{DELETE}");
            Thread.Sleep(2000);

            Browser.RClick(onenote);
            Browser.Wait(By.LinkText("Open in OneNote Online"));
            elementOpenOnline = Browser.webDriver.FindElement(By.LinkText("Open in OneNote Online"));
            Browser.Click(elementOpenOnline);        
            Thread.Sleep(40000);
            
            //var merge = Browser.webDriver.FindElement(By.XPath("//span[@class='WACBusinessBarBody'][text()='This page contains conflicting changes. Click here to show versions of the page with unmerged changes.']"));
            Browser.RClick(onenote);
            Browser.Wait(By.LinkText("Open in OneNote"));
            elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);
            Thread.Sleep(30000);

            //Delete conflict page version in OneNote local App.
            SendKeys.SendWait("+(^w)");
            Thread.Sleep(2000);
            SendKeys.SendWait("+(^w)");
            Thread.Sleep(2000);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(2000);     


            // Get the opened OneNote process, and edit it
            OneNote.Application oneNoteApp = new OneNote.Application();
            var oneNoteWindow = oneNoteApp.Windows.CurrentWindow;           

            // Closed OneNote App.
            oneNoteApp.CloseNotebook(oneNoteWindow.CurrentNotebookId);
            SendKeys.SendWait("%{f4}");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".one");
        }

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
        /// <param name="popWindowsSecurity">A bool value indicate if pop Windows Security</param>
        public static bool WaitForDocumentOpenning(string docName, bool isreadonly = false, bool popWindowsSecurity = false)
        {
            var desktop = AutomationElement.RootElement;
            AutomationElement document = null;
            if (isreadonly)
            {
                Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + " [Read-Only] - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"), new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"));
                document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
            }
            else
            {
                Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"));
                document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
            }

            if (popWindowsSecurity)
            {
                Condition windowsSecurity = new PropertyCondition(AutomationElement.NameProperty, "Windows Security");
                AutomationElement securityWindow = WaitForElement(document, windowsSecurity, TreeScope.Children);
                if (securityWindow != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Wait for document opening with word
        /// </summary>
        /// <param name="docName">Wait for document opening with Office</param>
        /// <param name="docType">A string specify the document type</param>
        /// <param name="isreadonly">A bool value indicate if the document is readonly</param>
        /// <param name="popWindowsSecurity">A bool value indicate if pop Windows Security</param>
        /// <returns>A bool value indicate if the document is opening.</returns>
        public static bool WaitForDocumentOpenning(string docName, string docType,bool isreadonly = false, bool popWindowsSecurity = false)
        {
            var desktop = AutomationElement.RootElement;
            AutomationElement document = null;
            switch (docType)
            {
                case "Word":
                    if (isreadonly)
                    {
                        Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + " [Read-Only] - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"), new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"));
                        document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
                    }
                    else
                    {
                        Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + " - Word"), new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"), new PropertyCondition(AutomationElement.NameProperty, "Word"));
                        document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
                    }

                    if (popWindowsSecurity)
                    {
                        Condition windowsSecurity = new PropertyCondition(AutomationElement.NameProperty, "Windows Security");
                        AutomationElement securityWindow = WaitForElement(document, windowsSecurity, TreeScope.Children);
                        if (securityWindow != null)
                        {
                            return true;
                        }
                    }                    
                    break;
                case "OneNote":
                    if (isreadonly)
                    {
                        Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + ".one [Read-Only] - OneNote"), new PropertyCondition(AutomationElement.NameProperty, docName + " [Read-Only] - OneNote"), new PropertyCondition(AutomationElement.NameProperty, "OneNote"), new PropertyCondition(AutomationElement.NameProperty, "OneNote"), new PropertyCondition(AutomationElement.NameProperty, "Untitled page - OneNote"), new PropertyCondition(AutomationElement.NameProperty, docName + ".one - OneNote"));
                        document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
                    }
                    else
                    {
                        Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + " - OneNote"), new PropertyCondition(AutomationElement.NameProperty, "Untitled page - OneNote"), new PropertyCondition(AutomationElement.NameProperty, docName + ".one - OneNote"), new PropertyCondition(AutomationElement.NameProperty, "OneNote"));
                        document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
                    }

                    if (popWindowsSecurity)
                    {
                        Condition windowsSecurity = new PropertyCondition(AutomationElement.NameProperty, "Windows Security");
                        AutomationElement securityWindow = WaitForElement(document, windowsSecurity, TreeScope.Children);
                        if (securityWindow != null)
                        {
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// Wait for document opening with word
        /// </summary>
        /// <param name="docName">Wait for document opening with Office</param>
        /// <param name="docType">A string specify the document type</param>
        /// <param name="isreadonly">A bool value indicate if the document is readonly</param>
        /// <param name="popWindowsSecurity">A bool value indicate if pop Windows Security</param>
        /// <returns>A bool value indicate if the document is opening.</returns>        
        public static bool WaitForOneNoteDocumentOpenning(string docName, bool isreadonly = false, bool popWindowsSecurity = false)
        {
            var desktop = AutomationElement.RootElement;
            AutomationElement document = null;
            if (isreadonly)
            {
                Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + ".one [Read-Only] - OneNote"), new PropertyCondition(AutomationElement.NameProperty, docName + " [Read-Only] - OneNote"), new PropertyCondition(AutomationElement.NameProperty, "OneNote"), new PropertyCondition(AutomationElement.NameProperty, "OneNote"), new PropertyCondition(AutomationElement.NameProperty, "Untitled page - OneNote"), new PropertyCondition(AutomationElement.NameProperty, docName + ".one - OneNote"));
                document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
            }
            else
            {
                Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + " - OneNote"), new PropertyCondition(AutomationElement.NameProperty, "Untitled page - OneNote"), new PropertyCondition(AutomationElement.NameProperty, docName + ".one - OneNote"), new PropertyCondition(AutomationElement.NameProperty, "OneNote"));
                document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
            }

            if (popWindowsSecurity)
            {
                Condition windowsSecurity = new PropertyCondition(AutomationElement.NameProperty, "Windows Security");
                AutomationElement securityWindow = WaitForElement(document, windowsSecurity, TreeScope.Children);
                if (securityWindow != null)
                {
                    return true;
                }
            }
   
            return false;
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
                document = WaitForElement(desktop, new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"), TreeScope.Children, true);
            }
            else
            {
                document = WaitForElement(desktop, new PropertyCondition(AutomationElement.NameProperty, docName + ".docx - Word"), TreeScope.Children, true);
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
            AutomationElement documentFormat = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
            AutomationElement FileInUseDialog = WaitForElement(documentFormat, new PropertyCondition(AutomationElement.NameProperty, "File In Use"), TreeScope.Children, true);
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
            Condition multiCondition = new OrCondition(new PropertyCondition(AutomationElement.NameProperty, docName + ".docx [Read-Only] - Word"),
                new PropertyCondition(AutomationElement.NameProperty, docName + @".docx  -  Read-Only - Word"));
            AutomationElement document = WaitForElement(desktop, multiCondition, TreeScope.Children, true);
            AutomationElement FileNowAvailableDialog = WaitForElement(document, new PropertyCondition(AutomationElement.NameProperty, "File Now Available"), TreeScope.Children, true);
            if (FileNowAvailableDialog == null)
            {
                return;
            }
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
            AutomationElement item_Info = docOnline.FindFirst(TreeScope.Descendants, Group_Info);

            Condition Con_ManageVersions = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem), new PropertyCondition(AutomationElement.NameProperty, "Manage Document"));
            AutomationElement item_ManageVersions = item_Info.FindFirst(TreeScope.Descendants, Con_ManageVersions);

            ExpandCollapsePattern Pattern_ManageVersions = (ExpandCollapsePattern)item_ManageVersions.GetCurrentPattern(ExpandCollapsePatternIdentifiers.Pattern);
            Pattern_ManageVersions.Expand();

            Condition Con_CheckOut = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem), new PropertyCondition(AutomationElement.NameProperty, "Check Out"));
            AutomationElement item_CheckOut = item_Info.FindFirst(TreeScope.Descendants, Con_CheckOut);

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
            CloseMicrosoftWordDialog(name, "Yes");

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
            AutomationElement item_IE = WaitForElement(desktop, Con_IE, TreeScope.Children, true);
            PropertyCondition Con_IEDialog = new PropertyCondition(AutomationElement.NameProperty, "Internet Explorer");
            AutomationElement item_IEDialog = item_IE.FindFirst(TreeScope.Descendants, Con_IEDialog);
            if (item_IEDialog != null)
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
            AutomationElement item_Office = WaitForElement(desktop, Con_Office, TreeScope.Children, true);

            if (item_Office != null)
            {
                Condition Con_Yes = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Yes"));
                AutomationElement item_Yes = item_Office.FindFirst(TreeScope.Descendants, Con_Yes);
                InvokePattern Pattern_Yes = (InvokePattern)item_Yes.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_Yes.Invoke();
            }
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
            if (Accept == "OK")
            {
                Thread.Sleep(2000);
                Condition Con_Word = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane), new PropertyCondition(AutomationElement.NameProperty, "Microsoft Word"));
                AutomationElement item_Word = WaitForElement(item_Document, Con_Word, TreeScope.Children, false);
                if (item_Word != null)
                {
                    Con_Acc = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "OK"));
                    item_Acc = item_Word.FindFirst(TreeScope.Descendants, Con_Acc);
                }
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
            AutomationElement item_Document = desktop.FindFirst(TreeScope.Children, Con_Document);
            Condition Con_Checkin = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, "Check In"));
            AutomationElement item_Checkin = WaitForElement(item_Document, Con_Checkin, TreeScope.Children, true);

            if (keepCheckOut)
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
            Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                new OrCondition(new PropertyCondition(AutomationElement.NameProperty, filename + ".docx - Word"), new PropertyCondition(AutomationElement.NameProperty, filename + " - Word")));
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

            //Microsoft.Office.Interop.Word.Application wordToOpen = (Microsoft.Office.Interop.Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            //Microsoft.Office.Interop.Word.Document oDocument = (Microsoft.Office.Interop.Word.Document)wordToOpen.ActiveDocument;

            Condition Con_Document = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                new OrCondition(new PropertyCondition(AutomationElement.NameProperty, filename + ".docx - Word"), new PropertyCondition(AutomationElement.NameProperty, filename + " - Word")));
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
        public static AutomationElement WaitForElement(AutomationElement parent, Condition condition, TreeScope scop, bool isWindowElement = false)
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
                }
                else
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
            string password = ConfigurationManager.AppSettings["Password"];
            password = GetExecuteScripPassword(password);
            string path = ConfigurationManager.AppSettings["Path"];
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
            if (!Directory.Exists(TestBase.testResultPath))
            {
                Directory.CreateDirectory(TestBase.testResultPath);
            }

            if (isStart)
            {
                startInfo.Arguments = "/user:Administrator cmd /c " + "powershell " + scriptPath + " " + userName + " " + password;
            }
            else
            {
                startInfo.Arguments = $@"/user:Administrator cmd /c powershell -command {scriptPath} -username {userName} -password {password} -RemoteCapturePath {path} -NewName {WOPIautomation.TestBase.testName} -LocalCapturePath '{TestBase.testResultPath}'";
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

        public static string GetExecuteScripPassword(string orignialPass)
        {
            string result = orignialPass;
            if (orignialPass.Contains("|"))
            {
                result = orignialPass.Replace("|", "`^|");
            }
            return result;
        }

    }
}
