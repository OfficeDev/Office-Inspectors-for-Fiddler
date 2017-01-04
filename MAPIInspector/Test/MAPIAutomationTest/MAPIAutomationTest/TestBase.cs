using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;

namespace MAPIAutomationTest
{
    [TestClass]
    public class TestBase
    {
        #region Variables
        protected static Outlook.Application oApp = null;
        protected static Outlook.MAPIFolder inboxFolders;
        protected static Outlook.MAPIFolder sentMailFolder;
        protected static Outlook.MAPIFolder publicFolders;
        protected static Outlook.MAPIFolder deletedItemsFolders;
        protected static int waittime_window;
        protected static int waittime_item;

        public TestContext TestContext { get; set; }
        #endregion

        /// <summary>
        /// Test initialization
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            GetTestCatgoryInformation(TestContext.FullyQualifiedTestClassName);
            EndStartedOutLook();
            string outLookPath = ConfigurationManager.AppSettings["OutLookPath"].ToString();
            waittime_window = Int32.Parse(ConfigurationManager.AppSettings["WaitTimeWindow"].ToString());
            waittime_item = Int32.Parse(ConfigurationManager.AppSettings["WaitTimeItem"].ToString());
            Process p = Process.Start(outLookPath);
            Thread.Sleep(waittime_window);
            oApp = Marshal.GetActiveObject("Outlook.Application") as Microsoft.Office.Interop.Outlook.Application;
            inboxFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            sentMailFolder = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);
            publicFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders);
            deletedItemsFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems);
        }

        /// <summary>
        /// End all started outlook application
        /// </summary>
        protected static void EndStartedOutLook()
        {
            // Check whether there is an Outlook process running.
            while (Process.GetProcessesByName("OUTLOOK").Count() > 0)
            {
                Outlook.Application oAppExist;
                try
                {
                    oAppExist = Marshal.GetActiveObject("Outlook.Application") as Microsoft.Office.Interop.Outlook.Application;
                }
                catch (System.Exception e)
                {
                    // try some other way to get the object
                    oAppExist = Activator.CreateInstance(Type.GetTypeFromProgID("Outlook.Application")) as Microsoft.Office.Interop.Outlook.Application;
                }
                ReleaseObj(oAppExist);
            }
        }

        /// <summary>
        /// Update the outlook cached mode configuration from on to off.
        /// </summary>
        public static void UpdateOutlookConfiguration(bool isEnable)
        {
            string scriptPath = ConfigurationManager.AppSettings["PowershellScript_path"].ToString();
            string outlookVersion = ConfigurationManager.AppSettings["OutlookVersion"].ToString();
            var newProcessInfo = new System.Diagnostics.ProcessStartInfo();
            newProcessInfo.FileName = ConfigurationManager.AppSettings["Powershellpath"].ToString();
            newProcessInfo.Verb = "runas";
            newProcessInfo.Arguments = scriptPath + " " + outlookVersion + "" + isEnable.ToString();
            newProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(newProcessInfo);
        }

        /// <summary>
        /// Get test method CachedMode information
        /// </summary>
        public void GetTestCatgoryInformation(string className)
        {
            MethodBase method = typeof(CachedModeCase).GetMethod(TestContext.TestName);
            object[] CustomAttributes = method.GetCustomAttributes(typeof(TestCategoryAttribute), true);
            if (CustomAttributes.Length > 0)
            {
                TestCategoryAttribute attr = (TestCategoryAttribute)CustomAttributes[0];
                if (attr != null && attr.TestCategories.Count > 0)
                {
                    if (attr.TestCategories[0] == "CachedMode")
                    {
                        UpdateOutlookConfiguration(true);
                    }
                    else
                    {
                        UpdateOutlookConfiguration(false);
                    }
                }
            }
        }

        /// <summary>
        /// Release com object
        /// </summary>
        /// <param name="obj">Com object</param>
        private static void ReleaseObj(object obj)
        {
            try
            {
                if (obj is Outlook.Application)
                {
                    Outlook.NameSpace nameSpace = (obj as Outlook.Application).GetNamespace("MAPI");
                    if (nameSpace != null)
                    {
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(nameSpace);
                    }
                    (obj as Outlook._Application).Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                obj = null;
            }

            Thread.Sleep(waittime_item*15);
        }
    }
}
