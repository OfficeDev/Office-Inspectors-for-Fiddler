using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;

using System.Windows.Automation;

//using Interop.UIAutomationCore;
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
        public void ChangeMessagePropertiesInPublicFolder()
        {
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("GetMessageStatus");
            // Move this mail to subPublicFolder
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            Utilities.MoveItemToMAPIFolder(firstUserFolder, omail);
            // Get this mail in public Folder and update some properties of it
            Outlook.MailItem oitem = Utilities.GetNewestItemInMAPIFolder(firstUserFolder, "GetMessageStatus");
            Utilities.UpdateItemProperties(oitem);
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        public void OpenMailMessageInPublicFolder()
        {
            // Create a simple mail and send it
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("RopReadRecipients");
            Utilities.SendEmail(omail, 40);
            // Get the latest send mail from send mail folder
            Outlook.MailItem omailSend = Utilities.GetNewestItemInMAPIFolder(sentMailFolder, "RopReadRecipients");
            // Move this mail to the subfolder in public folder
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            Utilities.MoveItemToMAPIFolder(firstUserFolder, omailSend);
            // Get this mail and display it
            Outlook.MailItem oitem = Utilities.GetNewestItemInMAPIFolder(firstUserFolder, "RopReadRecipients");
            Utilities.DisplayAndCloseItem(oitem);
        }
        #endregion

        #region MS-OXCFOLD
        [TestCategory("NoneCachedMode"), TestMethod]
        public void FolderOperationsInPublicFolder()
        {
            // Get first user folder in All public folder
            Outlook.MAPIFolder firstUserFolder = Utilities.GetUserFolderInAllPublicFolder(publicFolders);
            // Add a subfoler named testFolder under the firstUserFolder
            Outlook.MAPIFolder testFolder = firstUserFolder.Folders.Add("testFolder");
            // Add a subfoler named subTestFolder under the testFolder
            Outlook.MAPIFolder subTestFolder = firstUserFolder.Folders.Add("subTestFolder");
            // Move subTestFolder to firstUserFolder
            subTestFolder.MoveTo(firstUserFolder);
            // Delete all subfolders in firstUserFolder
            Utilities.RemoveAllSubFolders(firstUserFolder);
        }

        [TestCategory("NoneCachedMode"), TestMethod]
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
        }

        [TestCategory("NoneCachedMode"), TestMethod]
        public void FolderOperationsInboxFolder()
        {
            // Add a subfoler named testFolder under the inboxFolders
            Outlook.MAPIFolder testFolder = inboxFolders.Folders.Add("testFolder");
            // Add a subfoler named subTestFolder under the testFolder
            Outlook.MAPIFolder subTestFolder = testFolder.Folders.Add("subTestFolder");
            // Copy subTestFolder to inboxFolders
            subTestFolder.CopyTo(inboxFolders);
            // Delete all subfolders in inboxFolders
            Utilities.RemoveAllSubFolders(inboxFolders);
            // Delete all subfolders in deletedItemsFolders
            Utilities.RemoveAllSubFolders(deletedItemsFolders);
        }
        #endregion

        #region MS-OXCPERM
        [TestCategory("NoneCachedMode"), TestMethod]
        public void ModifyFolderPermissions()
        {
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;

            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            Condition cd_RibbonTabs = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem), new PropertyCondition(AutomationElement.NameProperty, "Folder"));
            AutomationElement item_RibbonTabs = Utilities.WaitForElement(window_outlook, cd_RibbonTabs, TreeScope.Descendants, 300);
            SelectionItemPattern Pattern_RibbonTabs = (SelectionItemPattern)item_RibbonTabs.GetCurrentPattern(SelectionItemPattern.Pattern);
            Pattern_RibbonTabs.Select();

            Condition cd_FolderPermissions = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Folder Permissions"));
            AutomationElement item_FolderPermissions = Utilities.WaitForElement(window_outlook, cd_FolderPermissions, TreeScope.Descendants, 10);
            InvokePattern clickPattern_FolderPermissions = (InvokePattern)item_FolderPermissions.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_FolderPermissions.Invoke();

            var condition_permission = new PropertyCondition(AutomationElement.NameProperty, "Inbox Properties");
            var window_FolderProp = Utilities.WaitForElement(window_outlook, condition_permission, TreeScope.Children, 10);

            Condition cd_write = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.CheckBox), new PropertyCondition(AutomationElement.NameProperty, "Create items"));
            AutomationElement item_write = Utilities.WaitForElement(window_FolderProp, cd_write, TreeScope.Descendants, 10);

            TogglePattern Pattern_write = (TogglePattern)item_write.GetCurrentPattern(TogglePattern.Pattern);
            Pattern_write.Toggle();

            var condition_Dailog = new PropertyCondition(AutomationElement.NameProperty, "Microsoft Outlook");
            var window_Dailog = Utilities.WaitForElement(window_FolderProp, condition_Dailog, TreeScope.Children, 10);

            var condition_DailogOK = new PropertyCondition(AutomationElement.AutomationIdProperty, "6");
            var item_DailogOK = Utilities.WaitForElement(window_Dailog, condition_DailogOK, TreeScope.Children, 10);
            InvokePattern clickPattern_DailogOK = (InvokePattern)item_DailogOK.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_DailogOK.Invoke();

            var condition_FolderPropOK = new PropertyCondition(AutomationElement.AutomationIdProperty, "1");
            var item_FolderPropertyOK = Utilities.WaitForElement(window_FolderProp, condition_FolderPropOK, TreeScope.Children, 10);
            InvokePattern clickPattern_FolderPropertyOK = (InvokePattern)item_FolderPropertyOK.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_FolderPropertyOK.Invoke();
        }
        #endregion

        #region MS-OXCPRPT
        [TestCategory("NoneCachedMode"), TestMethod]
        // RopCopyTo RopGetPropertyIdsFromNames RopSetProperties reloadCach
        public void NewNoteAndForward()
        {
            // Create a new note
            Outlook.NoteItem oNote = Utilities.NewNote();
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail();
            // Add the new note as an attach for new created mail
            Outlook.MailItem omailWithAttach = Utilities.AddAttachsToEmail(omail, new object[] { oNote });
            // Send mail
            Utilities.SendEmail(omail);
        }
        #endregion
    }
}
