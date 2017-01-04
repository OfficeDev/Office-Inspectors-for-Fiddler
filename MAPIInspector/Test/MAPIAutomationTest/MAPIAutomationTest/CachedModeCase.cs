using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;

namespace MAPIAutomationTest
{

    [TestClass]
    public class CachedModeCase: TestBase
    {
        #region MS-OXCMSG
        [TestCategory("CachedMode"), TestMethod]
        public void SendEmailSuccess()
        {
            // Create a simple mail
            Outlook.MailItem omail = Utilities.CreateSimpleEmail();
            // Create another simple mail used to attach to omial
            Outlook.MailItem mailAttach = Utilities.CreateSimpleEmail("attach mail");
            // Add a email attach for new created mail
            Outlook.MailItem omailWithAttach = Utilities.AddAttachsToEmail(omail, new object[] { mailAttach });
            // Send mail
            Utilities.SendEmail(omail);
        }
        #endregion
    }
}
