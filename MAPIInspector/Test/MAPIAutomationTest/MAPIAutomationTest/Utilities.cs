namespace MAPIAutomationTest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Automation;
    using System.Xml;
    using Outlook = Microsoft.Office.Interop.Outlook;

    /// <summary>
    /// The class is used to provide methods for test case execution
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// The time to wait
        /// </summary>
        private static int wait = int.Parse(ConfigurationManager.AppSettings["WaitTimeItem"].ToString());

        /// <summary>
        /// Get the first recurring appointments from now.
        /// </summary>
        /// <returns>The appoint to search</returns>
        public static Outlook.AppointmentItem GetAppointment()
        {
            Outlook.AppointmentItem appointment = null;
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.Folder calFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar) as Outlook.Folder;
            DateTime start = DateTime.Now;
            DateTime end = start.AddDays(7);
            Outlook.Items rangeAppts = GetAppointmentsInRange(calFolder, start, end);
            if (rangeAppts != null)
            {
                foreach (Outlook.AppointmentItem appt in rangeAppts)
                {
                    appointment = appt;
                    break;
                }
            }

            return appointment;
        }

        /// <summary>
        /// Get recurring appointments in date range.
        /// </summary>
        /// <param name="folder">The folder to get</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <returns>Outlook Items</returns>
        public static Outlook.Items GetAppointmentsInRange(Outlook.Folder folder, DateTime startTime, DateTime endTime)
        {
            string filter = "[Start] >= '"
                + startTime.ToString("g")
                + "' AND [End] <= '"
                + endTime.ToString("g") + "'";
            try
            {
                Outlook.Items calItems = folder.Items;
                calItems.IncludeRecurrences = true;
                calItems.Sort("[Start]", Type.Missing);
                Outlook.Items restrictItems = calItems.Restrict(filter);
                if (restrictItems.Count > 0)
                {
                    return restrictItems;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Create a new note
        /// </summary>
        /// <param name="body">the note body</param>
        /// <returns>Outlook Note item</returns>
        public static Outlook.NoteItem NewNote(string body = "")
        {
            Outlook.Application outlookApp = new Outlook.Application();
           
            // Create a new note item.
            Outlook.NoteItem outlookNote = (Outlook.NoteItem)outlookApp.CreateItem(Outlook.OlItemType.olNoteItem);

            // Set the note body
            if (body != string.Empty)
            {
                outlookNote.Body = body;
            }
            else
            {
                outlookNote.Body = ConfigurationManager.AppSettings["Note_body"].ToString();
            }

            outlookNote.Save();

            return outlookNote;
        }

        /// <summary>
        /// Create Email
        /// </summary>
        /// <param name="subject">The subject of email to create</param>
        /// <param name="body">The body of the email to create</param>
        /// <returns>Outlook MailItem with subject and body</returns>
        public static Outlook.MailItem CreateSimpleEmail(string subject = "", string body = "")
        {
            Outlook.Application outlookApp = new Outlook.Application();
            
            // Create a new mail item.
            Outlook.MailItem outlookMsg = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

            // Set the email subject
            if (subject != string.Empty)
            {
                outlookMsg.Subject = subject;
            }
            else
            {
                outlookMsg.Subject = ConfigurationManager.AppSettings["Email_subject"].ToString();
            }

            // Set the email body
            if (body != string.Empty)
            {
                outlookMsg.HTMLBody = body;
            }
            else
            {
                outlookMsg.HTMLBody = ConfigurationManager.AppSettings["Email_body"].ToString();
            }

            return outlookMsg;
        }

        /// <summary>
        /// Add attach for a mail item
        /// </summary>
        /// <param name="mailItem">Mail item</param>
        /// <param name="attachs">Attach files</param>
        /// <returns>Outlook MailItem with attachment</returns>
        public static Outlook.MailItem AddAttachsToEmail(Outlook.MailItem mailItem, object[] attachs)
        {
            Outlook.MailItem outlookMailItem = mailItem;
            if (attachs != null && attachs.Length != 0)
            {
                if (attachs.Length > 1)
                {
                    foreach (var file in attachs)
                    {
                        outlookMailItem.Attachments.Add(file, Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                    }
                }
                else
                {
                    outlookMailItem.Attachments.Add(attachs[0], Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                }
            }

            return outlookMailItem;
        }

        /// <summary>
        /// Add attach for a mail item
        /// </summary>
        /// <param name="mItem">Mail item</param>
        /// <param name="attachs">Attach files</param>
        /// <returns>Outlook MailItem with attachment</returns>
        public static Outlook.MailItem RemoveAttachsToEmail(Outlook.MailItem mItem)
        {
            Outlook.MailItem oMailItem = mItem;
            Outlook.Attachments attas = oMailItem.Attachments;

            if (attas.Count > 0)
            {
                attas.Remove(1);
            }
            
            return oMailItem;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="mail">Mail item</param>
        /// <param name="recepientCount">The recipient count to send </param>
        /// <param name="recipient">Mail send to </param>
        /// <param name="cc">Mail cc</param>
        /// <param name="bcc">Mail bcc</param>
        public static void SendEmail(Outlook.MailItem mail, int recepientCount = 0, string recipient = "", string cc = "", string bcc = "")
        {
            Outlook.MailItem outlookMailItem = mail;
            try
            {
                // Set value to recipient
                if (recipient != string.Empty)
                {
                    outlookMailItem.To = recipient;
                }
                else
                {
                    string receipent = ConfigurationManager.AppSettings["Email_to"];
                    StringBuilder receipents = new StringBuilder();
                    if (recepientCount != 0)
                    {
                        do
                        {
                            receipents.Append(receipent + ";");
                            recepientCount--;
                        }
                        while (recepientCount > 0);
                        outlookMailItem.To = receipents.ToString();
                    }
                    else
                    {
                        outlookMailItem.To = receipent.ToString();
                    }
                }

                // Set value to cc
                if (cc != string.Empty)
                {
                    outlookMailItem.CC = cc;
                }
                else
                {
                    outlookMailItem.CC = ConfigurationManager.AppSettings["Email_cc"].ToString();
                }

                // Set value to bcc
                if (bcc != string.Empty)
                {
                    outlookMailItem.BCC = bcc;
                }
                else
                {
                    outlookMailItem.BCC = ConfigurationManager.AppSettings["Email_bcc"].ToString();
                }

                // Send Email
                (outlookMailItem as Outlook._MailItem).Send();
            }
            catch (Exception e)
            {
                // Return Error Message
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Move a mail item to public folder
        /// </summary>
        /// <param name="publicFolder">The target public folder</param>
        /// <param name="item">Mail item which used to move to public folder</param>
        public static void MoveItemToMAPIFolder(Outlook.MAPIFolder publicFolder, object item)
        {
            try
            {
                if (item is Outlook.MailItem)
                {
                    (item as Outlook.MailItem).Move(publicFolder);
                }
                else if (item is Outlook.AppointmentItem)
                {
                    (item as Outlook.AppointmentItem).Move(publicFolder);
                }
                else if (item is Outlook.ContactItem)
                {
                    (item as Outlook.ContactItem).Move(publicFolder);
                }
                else if (item is Outlook.TaskItem)
                {
                    (item as Outlook.TaskItem).Move(publicFolder);
                }
                else if (item is Outlook.MeetingItem)
                {
                    (item as Outlook.MeetingItem).Move(publicFolder);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Update items properties
        /// </summary>
        /// <param name="item">The target item</param>
        public static void UpdateItemProperties(object item)
        {
            try
            {
                object[] args = new object[] { };
                object retVal = item.GetType().InvokeMember("Class", BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty, null, item, args);
                Outlook.OlObjectClass outlookItemClass = (Outlook.OlObjectClass)retVal;
                switch (outlookItemClass)
                {
                    case Outlook.OlObjectClass.olMail:
                        Outlook.MailItem omail = (Outlook.MailItem)item;
                        omail.Categories = "黄色类别";
                        omail.Save();
                        break;
                    case Outlook.OlObjectClass.olDocument:
                        Outlook.DocumentItem odocument = (Outlook.DocumentItem)item;
                        odocument.Categories = "黄色类别";
                        odocument.Save();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get the last mail items in sentItem folder
        /// </summary>
        /// <param name="mapiFolder">The target folder</param>
        /// <param name="itemSubject">Item subject</param>
        /// <returns>the newest mail item in sendItem folder</returns>
        public static Outlook.MailItem GetNewestItemInMAPIFolder(Outlook.MAPIFolder mapiFolder, string itemSubject)
        {
            Outlook.MailItem outlookItem;
            int count = 0;
            try
            {
                outlookItem = mapiFolder.Items.GetFirst();

                if (outlookItem == null || outlookItem.Subject != itemSubject)
                {
                    do
                    {
                        Thread.Sleep(wait);
                        outlookItem = mapiFolder.Items.GetFirst();
                        count++;
                        if (count >= 30)
                        {
                            break;
                        }
                    }
                    while (outlookItem == null || outlookItem.Subject != itemSubject);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return outlookItem;
        }

        /// <summary>
        /// Get the last mail items in sentItem folder
        /// </summary>
        /// <param name="mapiFolder">The target folder</param>
        /// <returns>The newest mail item in sendItem folder</returns>
        public static Outlook.MailItem[] GetAllItemInMAPIFolder(Outlook.MAPIFolder mapiFolder)
        {
            List<Outlook.MailItem> outlookItems = new List<Outlook.MailItem>();
            Outlook.MailItem outlookItem;
            int count = mapiFolder.Items.Count;
            if (count == 0)
            {
                return null;
            }
            else
            {
                try
                {
                    do
                    {
                        outlookItem = mapiFolder.Items.GetNext();
                        if (outlookItem != null)
                        {
                            outlookItems.Add(outlookItem);
                            count--;
                        }
                    }
                    while (count > 0);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return outlookItems.ToArray();
        }

        /// <summary>
        /// Delete all mail items in folder
        /// </summary>
        /// <param name="mapiFolder">The folder need to delete all mails</param>
        public static void DeleteAllItemInMAPIFolder(Outlook.MAPIFolder mapiFolder)
        {
            if (mapiFolder.Items != null)
            {
                int count = mapiFolder.Items.Count;
                if (count == 0)
                {
                    return;
                }
                else
                {
                    try
                    {
                        do
                        {
                            if (mapiFolder.Items.GetFirst() is Outlook.MailItem)
                            {
                                Outlook.MailItem outlookMail = (Outlook.MailItem)mapiFolder.Items.GetFirst();
                                if (outlookMail != null)
                                {
                                    outlookMail.Delete();
                                    Marshal.ReleaseComObject(outlookMail);
                                    count--;
                                }
                            }
                            else if (mapiFolder.Items.GetFirst() is Outlook.PostItem)
                            {
                                Outlook.PostItem outlookPost = (Outlook.PostItem)mapiFolder.Items.GetFirst();
                                if (outlookPost != null)
                                {
                                    outlookPost.Delete();
                                    Marshal.ReleaseComObject(outlookPost);
                                    count--;
                                }
                            }
                            else if (mapiFolder.Items.GetFirst() is Outlook.MeetingItem)
                            {
                                Outlook.MeetingItem outlookMeeting = (Outlook.MeetingItem)mapiFolder.Items.GetFirst();
                                if (outlookMeeting != null)
                                {
                                    outlookMeeting.Delete();
                                    Marshal.ReleaseComObject(outlookMeeting);
                                    count--;
                                }
                            }
                            else if (mapiFolder.Items.GetFirst() is Outlook.AppointmentItem)
                            {
                                Outlook.AppointmentItem outlookAppointment = (Outlook.AppointmentItem)mapiFolder.Items.GetFirst();
                                if (outlookAppointment != null)
                                {
                                    outlookAppointment.Delete();
                                    Marshal.ReleaseComObject(outlookAppointment);
                                    count--;
                                }
                            }
                        }
                        while (count > 0);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Open an item in outlook folder
        /// </summary>
        /// <param name="item">Outlook item used to open</param>
        public static void DisplayAndCloseItem(object item)
        {
            try
            {
                Outlook.Application outlookApp = new Outlook.Application();
                object[] args = new object[] { };
                object retVal = item.GetType().InvokeMember("Class", BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty, null, item, args);
                Outlook.OlObjectClass outlookItemClass = (Outlook.OlObjectClass)retVal;
                switch (outlookItemClass)
                {
                    case Outlook.OlObjectClass.olMail:
                        Outlook.MailItem omail = (Outlook.MailItem)item;
                        omail.Display(false);
                        omail.Close(Outlook.OlInspectorClose.olSave);
                        break;
                    case Outlook.OlObjectClass.olDocument:
                        Outlook.DocumentItem odocument = (Outlook.DocumentItem)item;
                        odocument.Display(true);
                        odocument.Close(Outlook.OlInspectorClose.olSave);
                        break;
                    case Outlook.OlObjectClass.olRecurrencePattern:

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get first customer created folder under all public folder except PublicFolder folder
        /// </summary>
        /// <param name="allPublicFolder">All public folders</param>
        /// <returns>The MAPI folder</returns>
        public static Outlook.MAPIFolder GetUserFolderInAllPublicFolder(Outlook.MAPIFolder allPublicFolder)
        {
            Outlook.MAPIFolder userFolder = null;
            if (allPublicFolder != null && allPublicFolder.Folders.Count >= 1)
            {
                foreach (Outlook.MAPIFolder folder in allPublicFolder.Folders)
                {
                    if (folder.Name != "PublicFolder")
                    {
                        userFolder = folder;
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("Need Create another folder in public folder");
            }

            return userFolder;
        }

        /// <summary>
        /// Remove all sub-folders in folder
        /// </summary>
        /// <param name="mapiFolder">MAPI Folder</param>
        /// <param name="isCachMode">Boolean value indicates if in cached mode</param>
        public static void RemoveAllSubFolders(Outlook.MAPIFolder mapiFolder, bool isCachMode)
        {
            Outlook.Folders folders = mapiFolder.Folders;
            try
            {
                while (folders.Count != 0)
                {
                    if (isCachMode)
                    {
                        Thread.Sleep(wait * 10);
                    }

                    folders.Remove(folders.Count);
                    Thread.Sleep(wait);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Add a sub-folder in parent folder
        /// </summary>
        /// <param name="parent">parent folder</param>
        /// <param name="subFolder">new folder name</param>
        /// <returns>the new created folder</returns>
        public static Outlook.MAPIFolder AddSubFolder(Outlook.MAPIFolder parent, string subFolder)
        {
            Outlook.MAPIFolder testFolder;
            try
            {
                testFolder = parent.Folders[subFolder];
            }
            catch
            {
                testFolder = parent.Folders.Add(subFolder);
            }

            return testFolder;
        }

        /// <summary>
        /// Wait for UI automation elements
        /// </summary>
        /// <param name="parent">the parent element</param>
        /// <param name="condition">the search condition</param>
        /// <param name="scop">search scope</param>
        /// <param name="milisecondTimeout">time out</param>
        /// <returns>Automation element</returns>
        public static AutomationElement WaitForElement(AutomationElement parent, Condition condition, TreeScope scop, int milisecondTimeout)
        {
            var waitTime = 0;
            var element = parent.FindFirst(scop, condition);

            while (element == null)
            {
                if (waitTime >= milisecondTimeout)
                {
                    break;
                }

                Thread.Sleep(wait);
                waitTime += wait;
                element = parent.FindFirst(scop, condition);
            }

            return element;
        }

        /// <summary>
        /// The method used to close Microsoft Outlook window
        /// </summary>
        /// <param name="src">AutomationElement type window which need to close</param>
        /// <param name="e">The automation event argument</param>
        public static void OnWindowOpen(object src, AutomationEventArgs e)
        {
            if (e.EventId != WindowPattern.WindowOpenedEvent)
            {
                return;
            }

            AutomationElement sourceElement = null;

            try
            {
                sourceElement = src as AutomationElement;
                AutomationElement desktop = AutomationElement.RootElement;
                if (sourceElement.Current.IsEnabled == true)
                {
                    if (sourceElement.Current.Name == "Microsoft Outlook")
                    {
                        // Get outlook window
                        Outlook.Application outlookApp = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
                        var nameSpace = outlookApp.GetNamespace("MAPI");
                        Outlook.MAPIFolder folder = nameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
                        string userName = folder.Parent.Name;
                        var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");
                        AutomationElement window_outlook = Utilities.WaitForElement(desktop, condition_Outlook, TreeScope.Children, 10);
                        
                        // Click OK in Microsoft Outlook dialog box
                        var condition_Dailog = new PropertyCondition(AutomationElement.NameProperty, "Microsoft Outlook");
                        var window_Dailog = Utilities.WaitForElement(window_outlook, condition_Dailog, TreeScope.Children, 10);
                        var condition_DailogOK = new PropertyCondition(AutomationElement.NameProperty, "OK");
                        var item_DailogOK = Utilities.WaitForElement(window_Dailog, condition_DailogOK, TreeScope.Children, 10);
                        InvokePattern clickPattern_DailogOK = (InvokePattern)item_DailogOK.GetCurrentPattern(InvokePattern.Pattern);
                        clickPattern_DailogOK.Invoke();
                    }
                    else if (sourceElement.Current.Name.Contains(" - Meeting Occurrence"))
                    {
                        // Get the first recurring meeting and change the meeting time
                        Outlook.AppointmentItem appointment = Utilities.GetAppointment();
                        
                        // Get Meeting Window
                        var condition_MeetingWindow = new PropertyCondition(AutomationElement.NameProperty, appointment.Subject + " - Meeting Occurrence  ");
                        var window_Meeting = Utilities.WaitForElement(desktop, condition_MeetingWindow, TreeScope.Children, 10);

                        // update start time and end time
                        Condition cd_start = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit), new PropertyCondition(AutomationElement.NameProperty, "Start time"));
                        AutomationElement item_start = Utilities.WaitForElement(window_Meeting, cd_start, TreeScope.Descendants, 10);
                        ValuePattern pattern_start = (ValuePattern)item_start.GetCurrentPattern(ValuePattern.Pattern);
                        item_start.SetFocus();
                        pattern_start.SetValue(DateTime.Now.AddMinutes(30).Hour.ToString() + ":" + DateTime.Now.AddMinutes(30).Minute.ToString());
                        Condition cd_end = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit), new PropertyCondition(AutomationElement.NameProperty, "End time"));
                        AutomationElement item_end = Utilities.WaitForElement(window_Meeting, cd_end, TreeScope.Descendants, 10);
                        ValuePattern pattern_end = (ValuePattern)item_end.GetCurrentPattern(ValuePattern.Pattern);
                        item_end.SetFocus();
                        pattern_end.SetValue(DateTime.Now.AddMinutes(60).Hour.ToString() + ":" + DateTime.Now.AddMinutes(60).Minute.ToString());
                        
                        // Check receiver name and send update
                        Condition cd_CheckName = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Check Names"));
                        AutomationElement item_CheckName = Utilities.WaitForElement(window_Meeting, cd_CheckName, TreeScope.Descendants, 10);
                        InvokePattern pattern_CheckName = (InvokePattern)item_CheckName.GetCurrentPattern(InvokePattern.Pattern);
                        pattern_CheckName.Invoke();
                        Condition cd_send = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Send Update"));
                        AutomationElement item_send = Utilities.WaitForElement(window_Meeting, cd_send, TreeScope.Descendants, 10);
                        InvokePattern pattern_send = (InvokePattern)item_send.GetCurrentPattern(InvokePattern.Pattern);
                        pattern_send.Invoke();
                    }
                }
            }
            catch (ElementNotAvailableException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update the XML file via parsing the all ROP list covered in trace
        /// </summary>
        /// <param name="allRopLists">All ROP list covered</param>
        public static void UpdateXMLFile(List<string> allRopLists)
        {
            // Update XML attribute
            XmlDocument xmlDoc = new XmlDocument();
            string fileName = ConfigurationManager.AppSettings["RopSourcexml"].ToString();
            string resultXml = TestBase.TestingfolderPath + Path.DirectorySeparatorChar + "RopsCopy.xml";
            if (File.Exists(resultXml))
            {
                xmlDoc.Load(resultXml);
            }
            else
            {
                xmlDoc.Load(fileName);
            }

            foreach (var rop in allRopLists)
            {
                XmlNode node = xmlDoc.SelectSingleNode(string.Format("data-set/structure/RopName[text()='" + rop + "']"));
                if (node != null)
                {
                    node.NextSibling.InnerText = "True";
                    string allTestCase = node.NextSibling.NextSibling.InnerText;
                    if (allTestCase != "null")
                    {
                        if (!allTestCase.Contains(TestBase.TestName))
                        {
                            node.NextSibling.NextSibling.InnerText = allTestCase.Insert(allTestCase.Length, "," + TestBase.TestName);
                        }
                    }
                    else
                    {
                        node.NextSibling.NextSibling.InnerText = TestBase.TestName;
                    }
                }
            }

            xmlDoc.Save(resultXml);
        }
    }
}
