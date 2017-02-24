using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using System.Windows.Automation;
using System.Configuration;

namespace MAPIAutomationTest
{

    [TestClass]
    public class CachedModeCase : TestBase
    {
        #region MS-OXCMSG
        [TestCategory("CachedMode"), TestMethod]
        // RopOpenEmbeddedMessage
        public void SendEmailSuccess()
        {
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("message mail");
            // Create another simple mail used to attach to omial
            Outlook.MailItem mailAttach = Utilities.CreateSimpleEmail("attach mail");
            // Add a email attach for new created mail
            Outlook.MailItem omailWithAttach = Utilities.AddAttachsToEmail(omail, new object[] { mailAttach });
            // Send mail
            Utilities.SendEmail(omailWithAttach);
            // Get the latest send mail from send mail folder
            Outlook.MailItem omailSend = Utilities.GetNewestItemInMAPIFolder(sentMailFolder, "message mail");

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXCFICX
        [TestCategory("CachedMode"), TestMethod]
        //ImportMessageMove ImportMessageReadState ImportMessageChange ImportDelete ImportHierarchy
        public void MoveMailToSameMailboxFolder()
        {
            // Create a simple mail and save
            Outlook.MailItem omail = Utilities.CreateSimpleEmail("ImportMessageMove");
            omail.Save();
            bool unread = omail.UnRead;
            omail.UnRead = !unread;
            omail.Save();

            // Add a subfoler named testFolder under the draftsFolders
            Outlook.MAPIFolder testFolder = Utilities.AddSubFolder(draftsFolders, "testFolder");

            // Move mails in draftsFolder to testFolder
            omail.Move(testFolder);

            // Get the latest mail from testFolder folder
            Outlook.MailItem omailInTestFolder = Utilities.GetNewestItemInMAPIFolder(testFolder, "ImportMessageMove");
            omailInTestFolder.Delete();
            int count = 0;
            while (testFolder.Items.Count != 0)
            {
                Thread.Sleep(waittime_item);
                count += waittime_item;
                if (count >= waittime_window)
                {
                    break;
                }
            }

            // Delete all subfolders in draftsFolder
            Utilities.RemoveAllSubFolders(draftsFolders, true);

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion

        #region MS-OXORULE
        [TestCategory("CachedMode"), TestMethod]
        // RopModifyRules RopGetRulesTable
        public void CreateNewRule()
        {
            Outlook.AddressEntry currentUser = oApp.Session.CurrentUser.AddressEntry;
            Outlook.ExchangeUser manager = currentUser.GetExchangeUser();
            Outlook.Rules rules = oApp.Session.DefaultStore.GetRules();
            if (manager != null)
            {
                string displayName = manager.Name;
                int num = rules.Count;
                Outlook.Rule rule = rules.Create(displayName + "_" + num, Outlook.OlRuleType.olRuleReceive);

                // Rule conditions: From condition
                rule.Conditions.From.Recipients.Add(manager.PrimarySmtpAddress);
                rule.Conditions.From.Recipients.ResolveAll();
                rule.Conditions.From.Enabled = true;

                // Sent only to me
                rule.Conditions.ToMe.Enabled = true;
                // Rule actions: MarkAsTask action
                rule.Actions.MarkAsTask.MarkInterval = Outlook.OlMarkInterval.olMarkToday;
                rule.Actions.MarkAsTask.FlagTo = "Follow-up";
                rule.Actions.MarkAsTask.Enabled = true;
                try
                {
                    rules.Save(true);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                bool result = MessageParser.ParseMessage();
                Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
            }
        }
        #endregion

        #region MS-OXCPRPT
        [TestCategory("CachedMode"), TestMethod]
        // RopCommitStream
        public void JunkAddRemoveRecipert()
        {
            // Get account name
            var desktop = AutomationElement.RootElement;
            var nameSpace = oApp.GetNamespace("MAPI");
            Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            string userName = folder.Parent.Name;
            string safeRecipent = ConfigurationManager.AppSettings["safeRecipients"].ToString();

            // Get outlook window
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
            var window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);

            // Get Junk item and expand it
            PropertyCondition cd_Junk = new PropertyCondition(AutomationElement.NameProperty, "Junk");
            AutomationElement item_Junk = Utilities.WaitForElement(window_outlook, cd_Junk, TreeScope.Descendants, 300);
            ExpandCollapsePattern expandCollapsePattern = (ExpandCollapsePattern)item_Junk.GetCurrentPattern(ExpandCollapsePatternIdentifiers.Pattern);
            expandCollapsePattern.Expand();

            // Select "Junk E-mail Options..."
            AutomationElement item_JunkOptions = item_Junk.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, "Junk E-mail Options..."));
            InvokePattern clickPattern_JunkOptions = (InvokePattern)item_JunkOptions.GetCurrentPattern(InvokePattern.Pattern);
            clickPattern_JunkOptions.Invoke();

            // Get Junk E-mail Options window
            PropertyCondition condition_JunkWindow;
            condition_JunkWindow = new PropertyCondition(AutomationElement.NameProperty, "Junk Email Options - " + userName);
            AutomationElement window_JunkWindow = Utilities.WaitForElement(window_outlook, condition_JunkWindow, TreeScope.Children, 10);
            
            if (window_JunkWindow == null)
            {
                condition_JunkWindow = new PropertyCondition(AutomationElement.NameProperty, "Junk E-mail Options - " + userName);
                window_JunkWindow = Utilities.WaitForElement(window_outlook, condition_JunkWindow, TreeScope.Children, 10);
            }

            // Get and click "safe Recipients" button
            Condition cd_SafeRecipent = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem), new PropertyCondition(AutomationElement.NameProperty, "Safe Recipients"));
            AutomationElement item_SafeRecipent = Utilities.WaitForElement(window_JunkWindow, cd_SafeRecipent, TreeScope.Descendants, 10);
            SelectionItemPattern Pattern_SafeRecipent = (SelectionItemPattern)item_SafeRecipent.GetCurrentPattern(SelectionItemPattern.Pattern);
            Pattern_SafeRecipent.Select();

            // Get recipientlist item
            Condition cd_recipentList = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem), new PropertyCondition(AutomationElement.NameProperty, safeRecipent));
            var setRecipent = window_JunkWindow.FindFirst(TreeScope.Descendants, cd_recipentList);
            if (setRecipent == null)
            {
                // Click Add button 
                Condition cd_recipentAdd = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Add..."));
                var item_recipentAdd = Utilities.WaitForElement(window_JunkWindow, cd_recipentAdd, TreeScope.Descendants, 10);
                InvokePattern Pattern_recipentAdd = (InvokePattern)item_recipentAdd.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_recipentAdd.Invoke();

                // Get "Add address or domain" window
                var condition_AddWindow = new PropertyCondition(AutomationElement.NameProperty, "Add address or domain");
                var window_AddWindow = Utilities.WaitForElement(window_JunkWindow, condition_AddWindow, TreeScope.Children, 10);

                // Input the address need to added 
                var condition_edit = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);
                var item_edit = Utilities.WaitForElement(window_AddWindow, condition_edit, TreeScope.Children, 10);
                ValuePattern Pattern_edit = (ValuePattern)item_edit.GetCurrentPattern(ValuePattern.Pattern);
                item_edit.SetFocus();
                Pattern_edit.SetValue(safeRecipent);

                // Click OK in "Add address or domain" window
                var condition_AddOK = new PropertyCondition(AutomationElement.NameProperty, "OK");
                var item_AddOK = Utilities.WaitForElement(window_AddWindow, condition_AddOK, TreeScope.Children, 10);
                InvokePattern clickPattern_AddOK = (InvokePattern)item_AddOK.GetCurrentPattern(InvokePattern.Pattern);
                clickPattern_AddOK.Invoke();

                // Click OK in "Junk E-mail Options" window
                var condition_JunkOK = new PropertyCondition(AutomationElement.NameProperty, "OK");
                var item_JunkOK = Utilities.WaitForElement(window_JunkWindow, condition_JunkOK, TreeScope.Children, 10);
                InvokePattern clickPattern_JunkOK = (InvokePattern)item_JunkOK.GetCurrentPattern(InvokePattern.Pattern);
                clickPattern_JunkOK.Invoke();
            }
            else
            {
                SelectionItemPattern Pattern_S = (SelectionItemPattern)setRecipent.GetCurrentPattern(SelectionItemPattern.Pattern);
                Pattern_S.Select();

                // Click Add button 
                Condition cd_recipentAdd = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Add..."));
                var item_recipentAdd = Utilities.WaitForElement(window_JunkWindow, cd_recipentAdd, TreeScope.Descendants, 10);
                InvokePattern Pattern_recipentAdd = (InvokePattern)item_recipentAdd.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_recipentAdd.Invoke();

                // Get "Add address or domain" window
                var condition_AddWindow = new PropertyCondition(AutomationElement.NameProperty, "Add address or domain");
                var window_AddWindow = Utilities.WaitForElement(window_JunkWindow, condition_AddWindow, TreeScope.Children, 10);

                // Close the "Add address or domain" window:  this step is used to enable the edit button in window_JunkWindow
                var condition_cancel = new PropertyCondition(AutomationElement.NameProperty, "Cancel");
                var item_cancel = Utilities.WaitForElement(window_AddWindow, condition_cancel, TreeScope.Children, 10);
                InvokePattern Pattern_cancel = (InvokePattern)item_cancel.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_cancel.Invoke();

                // Click remove button in "Junk E-mail Options" window
                Condition cd_recipentEdit = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Remove"));
                var item_recipentEdit = Utilities.WaitForElement(window_JunkWindow, cd_recipentEdit, TreeScope.Descendants, 10);
                InvokePattern Pattern_recipentEdit = (InvokePattern)item_recipentEdit.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_recipentEdit.Invoke();

                // Click OK in "Junk E-mail Options" window
                var condition_JunkOK = new PropertyCondition(AutomationElement.NameProperty, "OK");
                var item_JunkOK = Utilities.WaitForElement(window_JunkWindow, condition_JunkOK, TreeScope.Children, 10);
                InvokePattern clickPattern_JunkOK = (InvokePattern)item_JunkOK.GetCurrentPattern(InvokePattern.Pattern);
                clickPattern_JunkOK.Invoke();
            }

            bool result = MessageParser.ParseMessage();
            Assert.IsTrue(result, "Case failed, check the details information in error.txt file.");
        }
        #endregion
    }
}
