using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using System.Configuration;
using System.Windows.Automation;
using System;
using System.IO;

namespace MAPIAutomationTest
{
    /// <summary>
    /// Test case for MS_OXCFOLD
    /// </summary>
    [TestClass]
    public class NoneCachedModeCase : TestBase
    {
        #region MS-OXCMSG
        [TestCategory("NoneCachedMode"), TestMethod]
        // RopGetMessageStatus RopSetMessageStatus
        public void ChangeMessagePropertiesInPublicFolder()
        {
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("GetMessageStatus");
            // Move this mail to subPublicFolder
            publicFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders);
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            Utilities.MoveItemToMAPIFolder(firstUserFolder, omail);
            // Get this mail in public Folder and update some properties of it
            Outlook.MailItem oitem = Utilities.GetNewestItemInMAPIFolder(firstUserFolder, "GetMessageStatus");
            Utilities.UpdateItemProperties(oitem);
            // Clean up firstUserFolder
            Utilities.DeleteAllItemInMAPIFolder(firstUserFolder);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        // RopReadRecipients
        public void OpenMailMessageInPublicFolder()
        {
            // Create a simple mail and send it
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("RopReadRecipients");
            Utilities.SendEmail(omail, 40);
            // Get the latest send mail from send mail folder
            Outlook.MailItem omailSend = Utilities.GetNewestItemInMAPIFolder(sentMailFolder, "RopReadRecipients");
            // Move this mail to the subfolder in public folder
            publicFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders);
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            Utilities.MoveItemToMAPIFolder(firstUserFolder, omailSend);
            // Get this mail and display it
            Outlook.MailItem oitem = Utilities.GetNewestItemInMAPIFolder(firstUserFolder, "RopReadRecipients");
            Utilities.DisplayAndCloseItem(oitem);
            // Clean up firstUserFolder
            Utilities.DeleteAllItemInMAPIFolder(firstUserFolder);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCFOLD
        [TestCategory("NoneCachedMode"), TestMethod]
        // RopMoveFolder
        public void FolderOperationsInPublicFolder()
        {
            // Get first user folder in All public folder
            publicFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders);
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            // Add a subfoler named testFolder under the firstUserFolder
            Outlook.MAPIFolder testFolder = Utilities.AddSubFolder(firstUserFolder, "testFolder");
            // Add a subfoler named subTestFolder under the testFolder
            Outlook.MAPIFolder subTestFolder = Utilities.AddSubFolder(testFolder, "subTestFolder");
            // Move subTestFolder to firstUserFolder
            subTestFolder.MoveTo(firstUserFolder);
            // Delete all subfolders in firstUserFolder
            Utilities.RemoveAllSubFolders(firstUserFolder, false);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        // RopDeleteMessages 
        public void DeleteMessageInDeletedItemFolder()
        {
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("DeleteMessage");
            // Send mail
            Utilities.SendEmail(omail);
            // Get the latest send mail from send mail folder
            Outlook.MailItem omailSend = Utilities.GetNewestItemInMAPIFolder(sentMailFolder, "DeleteMessage");
            // Delete this mail in send mail folder
            omailSend.Delete();
            // Get the deleted mail in Deleted folder
            Outlook.MailItem odeleteIItem = Utilities.GetNewestItemInMAPIFolder(deletedItemsFolders, "DeleteMessage");
            // Delete it
            odeleteIItem.Delete();

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        // RopCreateFolder RopDeleteFolder RopCopyFolder
        public void FolderOperationsInboxFolder()
        {
            // Add a subfoler named testFolder under the inboxFolders
            Outlook.MAPIFolder testFolder = Utilities.AddSubFolder(inboxFolders, "testFolder");
            // Add a subfoler named subTestFolder under the testFolder
            Outlook.MAPIFolder subTestFolder = Utilities.AddSubFolder(testFolder, "subTestFolder");
            // Copy subTestFolder to inboxFolders
            subTestFolder.CopyTo(inboxFolders);
            // Delete all subfolders in inboxFolders
            Utilities.RemoveAllSubFolders(inboxFolders, false);
            // Delete all subfolders in deletedItemsFolders
            Utilities.RemoveAllSubFolders(deletedItemsFolders, false);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        // RopSetSearchCritera RopGetSearchCritera RopResetTable
        public void InstantSearch()
        {
            // Get outlook window
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            AutomationElement window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            // Create the recall function for when "Microsoft Outlook" window opening
            AutomationEventHandler eventHandler = new AutomationEventHandler(Utilities.OnWindowOpen);
            // Registers the listener event
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, window_outlook, TreeScope.Children, eventHandler);

            Outlook.Explorer explorer = oApp.Explorers.Add(inboxFolders as Outlook.Folder,
                    Outlook.OlFolderDisplayMode.olFolderDisplayNormal);
            string filter = "subject:" + "\"" + "subject" + "\"" + " received:(last month)";
            explorer.Search(filter, Outlook.OlSearchScope.olSearchScopeAllFolders);
            explorer.Display();

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCPERM
        [TestCategory("NoneCachedMode"), TestMethod]
        // RopModifyPermissions RopGetPermissionsTable
        public void ModifyFolderPermissions()
        {
            // Get account name
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;

            // Get outlook window
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            // Get Folder Tab and select it
            Condition cd_RibbonTabs = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem), new PropertyCondition(AutomationElement.NameProperty, "Folder"));
            AutomationElement item_RibbonTabs = Utilities.WaitForElement(window_outlook, cd_RibbonTabs, TreeScope.Descendants, 300);
            SelectionItemPattern Pattern_RibbonTabs = (SelectionItemPattern)item_RibbonTabs.GetCurrentPattern(SelectionItemPattern.Pattern);
            Pattern_RibbonTabs.Select();

            // Get "Folder Permissions" and select it
            Condition cd_FolderPermissions = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Folder Permissions"));
            AutomationElement item_FolderPermissions = Utilities.WaitForElement(window_outlook, cd_FolderPermissions, TreeScope.Descendants, 10);
            InvokePattern clickPattern_FolderPermissions = (InvokePattern)item_FolderPermissions.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_FolderPermissions.Invoke();

            // Get "Inbox Properties" window
            var condition_permission = new PropertyCondition(AutomationElement.NameProperty, "Inbox Properties");
            var window_FolderProp = Utilities.WaitForElement(window_outlook, condition_permission, TreeScope.Children, 10);

            // Get and select "Create items" 
            Condition cd_write = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox), new PropertyCondition(AutomationElement.NameProperty, "Edit all"));
            AutomationElement item_write = Utilities.WaitForElement(window_FolderProp, cd_write, TreeScope.Descendants, 10);
            TogglePattern Pattern_write = (TogglePattern)item_write.GetCurrentPattern(TogglePattern.Pattern);
            Pattern_write.Toggle();

            // Click OK in Microsoft Outlook dialog box
            var condition_Dailog = new PropertyCondition(AutomationElement.NameProperty, "Microsoft Outlook");
            var window_Dailog = Utilities.WaitForElement(window_FolderProp, condition_Dailog, TreeScope.Children, 10);
            var condition_DailogOK = new PropertyCondition(AutomationElement.AutomationIdProperty, "6");
            var item_DailogOK = Utilities.WaitForElement(window_Dailog, condition_DailogOK, TreeScope.Children, 10);
            InvokePattern clickPattern_DailogOK = (InvokePattern)item_DailogOK.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_DailogOK.Invoke();

            // Click OK in "Inbox Properties" window
            var condition_FolderPropOK = new PropertyCondition(AutomationElement.AutomationIdProperty, "1");
            var item_FolderPropertyOK = Utilities.WaitForElement(window_FolderProp, condition_FolderPropOK, TreeScope.Children, 10);
            InvokePattern clickPattern_FolderPropertyOK = (InvokePattern)item_FolderPropertyOK.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_FolderPropertyOK.Invoke();

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCPRPT
        [TestCategory("NoneCachedMode"), TestMethod]
        // RopCopyTo RopGetPropertyIdsFromNames RopSetProperties RopReloadCachedInformation
        public void NewNoteAndForward()
        {
            // Create a new note
            Outlook.NoteItem oNote = Utilities.NewNote();
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("Attach Note");
            // Add the new note as an attach for new created mail
            Outlook.MailItem omailWithAttach = Utilities.AddAttachsToEmail(omail, new object[] { oNote });
            // Send mail
            Utilities.SendEmail(omailWithAttach);
            // Get the latest send mail from send mail folder
            Outlook.MailItem omailSend = Utilities.GetNewestItemInMAPIFolder(sentMailFolder, "Attach Note");

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        // CopyProperties
        public void ReceiveEmailAndForward()
        {
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("CopyProperties");
            // Add a email attach for new created mail
            string AttachDocument = ConfigurationManager.AppSettings["AttachDocument"].ToString();
            string fullPath = Path.GetFullPath(AttachDocument);
            Outlook.MailItem omailWithAttach = Utilities.AddAttachsToEmail(omail, new object[] { fullPath });
            // Send mail
            Utilities.SendEmail(omailWithAttach);

            // Get the latest send mail from send mail folder
            Outlook.MailItem omailReveived = Utilities.GetNewestItemInMAPIFolder(inboxFolders, "CopyProperties");
            omailReveived.Body = omailReveived.Body + "Edited.";
            omailReveived.Save();
            omailReveived.Forward();

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCFICX
        [TestCategory("NoneCachedMode"), TestMethod]
        // CopyTo CopyFolder GetNuffer TellVersion Destination 
        public void NewMailAndMoveToSubPublicFolder()
        {
            // Create a simple mail and save
            Outlook.MailItem omailOne = Utilities.CreateSimpleEmail("FastTransferCopyTo");
            omailOne.Save();
            // Create a simple mail and save
            Outlook.MailItem omailTwo = Utilities.CreateSimpleEmail("FastTransferCopyTo");
            omailTwo.Save();
            // Get first user folder in All public folder
            publicFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders);
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            // Add a subfoler named testFolder under the firstUserFolder
            Outlook.MAPIFolder testFolder = Utilities.AddSubFolder(firstUserFolder, "testFolder");
            // Move the new created mail to public folder
            omailOne.Copy().Move(testFolder);
            omailTwo.Copy().Move(testFolder);
            testFolder.CopyTo(inboxFolders);
            // Delete all subfolders in firstUserFolder
            Utilities.RemoveAllSubFolders(firstUserFolder, false);
            // Delete all subfolders in inboxFolders
            Utilities.RemoveAllSubFolders(inboxFolders, false);
            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        // FastTransferSourceCopyProperties SeekStream
        public void NewRecurringMeetingAndUpdateOneStartTime()
        {
            // Get account name
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;
            // Get outlook window
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);
            // Get New Items and click it to new create meeting 
            Condition condition_NewItems = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem), new PropertyCondition(AutomationElement.NameProperty, "New Items"));
            var item_NewItems = Utilities.WaitForElement(window_outlook, condition_NewItems, TreeScope.Descendants, 10);
            ExpandCollapsePattern Pattern_NewItems = (ExpandCollapsePattern)item_NewItems.GetCurrentPattern(ExpandCollapsePatternIdentifiers.Pattern);
            Pattern_NewItems.Expand();
            AutomationElement listItem = item_NewItems.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Meeting"));
            InvokePattern clickPattern_listItem = (InvokePattern)listItem.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_listItem.Invoke();
            // Get Untitled - Meeting Window
            var condition_MeetingWindow = new PropertyCondition(AutomationElement.NameProperty, "Untitled - Meeting  ");
            var window_Meeting = Utilities.WaitForElement(desktop, condition_MeetingWindow, TreeScope.Children, 10);
            // Add recipient in "To" text
            Condition cd_to = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit), new PropertyCondition(AutomationElement.NameProperty, "To"));
            AutomationElement item_to = Utilities.WaitForElement(window_Meeting, cd_to, TreeScope.Descendants, 10);
            ValuePattern Pattern_to = (ValuePattern)item_to.GetCurrentPattern(ValuePattern.Pattern);
            item_to.SetFocus();
            string safeRecipent = ConfigurationManager.AppSettings["safeRecipients"].ToString();
            Pattern_to.SetValue(safeRecipent);
            // Add subject in subject text
            Condition cd_Subject = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document), new PropertyCondition(AutomationElement.NameProperty, "Subject"));
            AutomationElement item_Subject = Utilities.WaitForElement(window_Meeting, cd_Subject, TreeScope.Descendants, 10);
            ValuePattern Pattern_Subject = (ValuePattern)item_Subject.GetCurrentPattern(ValuePattern.Pattern);
            item_Subject.SetFocus();
            Pattern_Subject.SetValue("Meeting test");
            // Add value for location
            Condition cd_Location = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ComboBox), new PropertyCondition(AutomationElement.NameProperty, "Location"));
            AutomationElement item_Location = Utilities.WaitForElement(window_Meeting, cd_Location, TreeScope.Descendants, 10);
            ValuePattern Pattern_Location = (ValuePattern)item_Location.GetCurrentPattern(ValuePattern.Pattern);
            item_Location.SetFocus();
            Pattern_Location.SetValue("1");
            // click recurrence button to make this meeting recurrence
            Condition cd_Recurrence = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Recurrence..."));
            AutomationElement item_Recurrence = Utilities.WaitForElement(window_Meeting, cd_Recurrence, TreeScope.Descendants, 10);
            TogglePattern Pattern_TogRecurrence = (TogglePattern)item_Recurrence.GetCurrentPattern(TogglePattern.Pattern);
            Pattern_TogRecurrence.Toggle();
            PropertyCondition cd_RecurrenceWindow = new PropertyCondition(AutomationElement.NameProperty, "Appointment Recurrence");
            AutomationElement item_RecurrenceWindow = Utilities.WaitForElement(window_Meeting, cd_RecurrenceWindow, TreeScope.Descendants, 10);
            Condition cd_EndByEdit = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit), new PropertyCondition(AutomationElement.NameProperty, "End by:"));
            AutomationElement item_EndByEdit = Utilities.WaitForElement(item_RecurrenceWindow, cd_EndByEdit, TreeScope.Descendants, 10);
            ValuePattern Pattern_EndByEdit = (ValuePattern)item_EndByEdit.GetCurrentPattern(ValuePattern.Pattern);
            item_EndByEdit.SetFocus();
            Pattern_EndByEdit.SetValue(DateTime.Today.AddDays(1).Day.ToString());
            Condition cd_OKButton = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "OK"));
            AutomationElement item_OKButton = Utilities.WaitForElement(item_RecurrenceWindow, cd_OKButton, TreeScope.Descendants, 10);
            InvokePattern Pattern_OK = (InvokePattern)item_OKButton.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_OK.Invoke();
            // Get Untitled - Meeting Window
            var condition_MeetingSeriesWindow = new PropertyCondition(AutomationElement.NameProperty, "Meeting test - Meeting Series  ");
            var window_MeetingSeriesWindow = Utilities.WaitForElement(desktop, condition_MeetingSeriesWindow, TreeScope.Children, 10);
            // Check the receiver name
            Condition cd_CheckName = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Check Names"));
            AutomationElement item_CheckName = Utilities.WaitForElement(window_MeetingSeriesWindow, cd_CheckName, TreeScope.Descendants, 10);
            InvokePattern Pattern_CheckName = (InvokePattern)item_CheckName.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_CheckName.Invoke();
            Condition cd_send = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Send"));
            AutomationElement item_send = Utilities.WaitForElement(window_MeetingSeriesWindow, cd_send, TreeScope.Descendants, 10);
            InvokePattern Pattern_send = (InvokePattern)item_send.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_send.Invoke();
            // Get the newest meeting and update it
            Outlook.AppointmentItem appointmentSended = Utilities.GetAppointment();
            // Create the recall function for when "meeting" window opening
            AutomationEventHandler eventHandler = new AutomationEventHandler(Utilities.OnWindowOpen);
            // Registers the listener event
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, desktop, TreeScope.Children, eventHandler);
            appointmentSended.Display(true);

            Outlook.Folder calFolder = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar) as Outlook.Folder;
            Utilities.DeleteAllItemInMAPIFolder(calFolder);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCTABLE
        [TestCategory("NoCachedMode"), TestMethod]
        // RopResetTable RopExpandRow RopCollapseRow RopGetCollapseState RopSetCollapseState
        public void ModifyMailViewArrage()
        {
            // Create and send a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail();
            Utilities.SendEmail(omail);

            // Get account name
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;

            // Get outlook window
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            // Get View tab and select it
            Condition cd_RibbonTabs = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem), new PropertyCondition(AutomationElement.NameProperty, "View"));
            AutomationElement item_RibbonTabs = Utilities.WaitForElement(window_outlook, cd_RibbonTabs, TreeScope.Descendants, 300);
            SelectionItemPattern Pattern_RibbonTabs = (SelectionItemPattern)item_RibbonTabs.GetCurrentPattern(SelectionItemPattern.Pattern);
            Pattern_RibbonTabs.Select();

            // Get the window visual states, and makesure the window is in maximized size
            WindowPattern Pattern_window = (WindowPattern)window_outlook.GetCurrentPattern(WindowPatternIdentifiers.Pattern);
            WindowVisualState windowVisualState = Pattern_window.Current.WindowVisualState;
            if (windowVisualState != WindowVisualState.Maximized)
            {
                Condition cd_Max = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Maximize"));
                AutomationElement item_Max = Utilities.WaitForElement(window_outlook, cd_Max, TreeScope.Descendants, 300);
                InvokePattern Pattern_Max1 = (InvokePattern)item_Max.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_Max1.Invoke();
            }

            // Select Categories item in view tab window
            AutomationElement item_categories = window_outlook.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Categories"));
            InvokePattern Pattern_categories = (InvokePattern)item_categories.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_categories.Invoke();

            // Get the latest received mail in inbox folder
            Outlook.MailItem omailReceived = Utilities.GetNewestItemInMAPIFolder(inboxFolders, "Email subject");

            // Callapse the mail grouped by Categories
            int inboxItemCount = inboxFolders.Items.Count;
            int inboxUnreadCount = inboxFolders.UnReadItemCount;
            Condition cd_cateExpandGroup;
            cd_cateExpandGroup = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group), new PropertyCondition(AutomationElement.NameProperty, string.Format("Group By: Expanded: Categories: (none): {0} item(s)", inboxItemCount)));
            AutomationElement item_cateExpandGroup = Utilities.WaitForElement(window_outlook, cd_cateExpandGroup, TreeScope.Descendants, 300);
            if (item_cateExpandGroup == null)
            {
                cd_cateExpandGroup = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group), new PropertyCondition(AutomationElement.NameProperty, string.Format("Group By: Expanded: Categories: (none): {0} item(s), {1} unread", inboxItemCount, inboxUnreadCount)));
                item_cateExpandGroup = Utilities.WaitForElement(window_outlook, cd_cateExpandGroup, TreeScope.Descendants, 300);
            }
            ExpandCollapsePattern Pattern_cateExpandGroup = (ExpandCollapsePattern)item_cateExpandGroup.GetCurrentPattern(ExpandCollapsePatternIdentifiers.Pattern);
            Pattern_cateExpandGroup.Collapse();

            // Select Date item in view tab window
            AutomationElement item_date = window_outlook.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Date"));
            InvokePattern Pattern_date = (InvokePattern)item_date.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_date.Invoke();
            Thread.Sleep(waittime_item);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCROPS
        [TestCategory("NoCachedMode"), TestMethod]
        // RopSetSpooler
        public void SendReceiveAllFolder()
        {
            // Get account name
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;

            // Get outlook window
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            // Select Send / Receive tab
            Condition cd_RibbonTabs = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem), new PropertyCondition(AutomationElement.NameProperty, "Send / Receive"));
            AutomationElement item_RibbonTabs = Utilities.WaitForElement(window_outlook, cd_RibbonTabs, TreeScope.Descendants, 300);
            SelectionItemPattern Pattern_RibbonTabs = (SelectionItemPattern)item_RibbonTabs.GetCurrentPattern(SelectionItemPattern.Pattern);
            Pattern_RibbonTabs.Select();

            // Click the "Send/Receive All Folders" button
            Condition cd_sendReceiveFolders = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Send/Receive All Folders"));
            AutomationElement item_sendReceiveFolders = Utilities.WaitForElement(window_outlook, cd_sendReceiveFolders, TreeScope.Descendants, 300);
            InvokePattern Pattern_cateExpandGroup = (InvokePattern)item_sendReceiveFolders.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_cateExpandGroup.Invoke();

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCMAPIHTTP
        [TestCategory("NoCachedMode"), TestMethod]
        // SeekEntries UpdateState
        public void AddressBook()
        {
            // Get account name
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;

            // Get outlook window
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            // Select Home tab
            Condition cd_RibbonTabs = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem), new PropertyCondition(AutomationElement.NameProperty, "Home"));
            AutomationElement item_RibbonTabs = Utilities.WaitForElement(window_outlook, cd_RibbonTabs, TreeScope.Descendants, 300);
            SelectionItemPattern Pattern_RibbonTabs = (SelectionItemPattern)item_RibbonTabs.GetCurrentPattern(SelectionItemPattern.Pattern);
            if (Pattern_RibbonTabs.Current.IsSelected == false)
            {
                Pattern_RibbonTabs.Select();
            }

            // Select Address Book tab
            Condition cd_AddressButton = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Address Book..."));
            AutomationElement item_AddressButton = Utilities.WaitForElement(window_outlook, cd_AddressButton, TreeScope.Descendants, 300);
            InvokePattern Pattern_AddressButton = (InvokePattern)item_AddressButton.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_AddressButton.Invoke();

            // Get the pop up Address Book window
            var condition_AddressWin = new PropertyCondition(AutomationElement.NameProperty, "Address Book: Global Address List");
            var window_AddressWin = Utilities.WaitForElement(desktop, condition_AddressWin, TreeScope.Children, 10);

            // Find NameOnly checkBox and select it
            PropertyCondition cd_nameOnly = new PropertyCondition(AutomationElement.NameProperty, "Name only");
            AutomationElement item_nameOnly = Utilities.WaitForElement(window_AddressWin, cd_nameOnly, TreeScope.Descendants, 10);
            SelectionItemPattern Pattern_nameOnly = (SelectionItemPattern)item_nameOnly.GetCurrentPattern(SelectionItemPattern.Pattern);
            if (Pattern_nameOnly.Current.IsSelected == false)
            {
                Pattern_nameOnly.Select();
            }

            // Focus on search text and input a saerch value
            Condition cd_textSearch = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit), new PropertyCondition(AutomationElement.NameProperty, "Search:"));
            AutomationElement item_textSearch = Utilities.WaitForElement(window_AddressWin, cd_textSearch, TreeScope.Descendants, 10);
            ValuePattern Pattern_textGoValue = (ValuePattern)item_textSearch.GetCurrentPattern(ValuePattern.Pattern);
            item_textSearch.SetFocus();
            Pattern_textGoValue.SetValue("hi");

            Condition cd_close = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Close"));
            AutomationElement item_close = Utilities.WaitForElement(window_AddressWin, cd_close, TreeScope.Descendants, 10);
            InvokePattern Pattern_close = (InvokePattern)item_close.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_close.Invoke();

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion  
    }
}
