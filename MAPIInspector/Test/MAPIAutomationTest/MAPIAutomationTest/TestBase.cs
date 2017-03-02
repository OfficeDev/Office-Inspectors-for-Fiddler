using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using System.Configuration;
using System.Management.Automation;
using System.Windows.Automation;

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
        protected static Outlook.MAPIFolder draftsFolders;
        protected static int waittime_window;
        protected static int waittime_item;
        public static string testingfolderPath;
        public static string testName;

        public TestContext TestContext { get; set; }
        #endregion

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            MessageParser.StartFiddler();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            MessageParser.CloseFiddler();
        }

        /// <summary>
        /// Test initialization
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            GetTestCatgoryInformation();
            EndStartedOutLook();
            Thread.Sleep(10000);
            MessageParser.ClearSessions();
            string outLookPath = ConfigurationManager.AppSettings["OutLookPath"].ToString();
            waittime_window = Int32.Parse(ConfigurationManager.AppSettings["WaitTimeWindow"].ToString());
            waittime_item = Int32.Parse(ConfigurationManager.AppSettings["WaitTimeItem"].ToString());
            Process p = Process.Start(outLookPath);

            AutomationElement outlookWindow = null;
            var desktop = AutomationElement.RootElement;
            string userName = ConfigurationManager.AppSettings["Office365Account"].ToString();
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");

            int count = 0;
            while (outlookWindow == null)
            {
                outlookWindow = desktop.FindFirst(TreeScope.Children, condition_Outlook);
                Thread.Sleep(waittime_item / 10);
                count += (waittime_item / 10);
                if (count >= waittime_window)
                {
                    break;
                }
            }


            Process[] pp = Process.GetProcesses();
            if (pp.Count() > 0)
            {
                foreach (Process pp1 in pp)
                {
                    if (pp1.ProcessName != "OUTLOOK" && pp1.ProcessName != "explorer" && pp1.MainWindowHandle != IntPtr.Zero)
                    {
                        AutomationElement element = AutomationElement.FromHandle(pp1.MainWindowHandle);
                        if (element != null)
                        {
                            try
                            {
                                element.SetFocus();
                            }
                            catch { continue; }
                        }
                        break;
                    }
                }
            }
            Thread.Sleep(waittime_item);
            try
            {
                oApp = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
            }
            catch
            {
                throw new Exception("Get active outlook application failed, please check if outlook is running");
            }

            inboxFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            sentMailFolder = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);
            deletedItemsFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems);
            draftsFolders = oApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDrafts);
        }

        /// <summary>
        /// Test clean up
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            Utilities.DeleteAllItemInMAPIFolder(sentMailFolder);
            Utilities.DeleteAllItemInMAPIFolder(deletedItemsFolders);
            Utilities.DeleteAllItemInMAPIFolder(inboxFolders);
            EndStartedOutLook();
        }

        /// <summary>
        /// End all started outlook application
        /// </summary>
        protected static void EndStartedOutLook()
        {
            // Check whether there is an Outlook process running.
            if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
            {
                Outlook.Application oAppExist;
                try
                {
                    oAppExist = Marshal.GetActiveObject("Outlook.Application") as Microsoft.Office.Interop.Outlook.Application;
                }
                catch (Exception e)
                {
                    throw new Exception("Get the running Outlook application failed");
                }
                ReleaseObj(oAppExist);
            }
        }

        /// <summary>
        /// Update the outlook cached mode configuration from on to off.
        /// </summary>
        public static void UpdateOutlookConfiguration(bool isEnable)
        {
            string outlookVersion = ConfigurationManager.AppSettings["OutlookVersion"].ToString();
            string scriptPath = ConfigurationManager.AppSettings["PowershellScript_path"].ToString();
            string fullPath = Path.GetFullPath(scriptPath);

            // Configure the PowerShell execution policy to run script
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                string script = "Set-ExecutionPolicy -Scope currentuser -ExecutionPolicy bypass; Get-ExecutionPolicy"; // the second command to know the ExecutionPolicy level
                PowerShellInstance.AddScript(script);
                var someResult = PowerShellInstance.Invoke();
            }

            // Run script to set the outlook configuration as enable or not.
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.Arguments = "/user:Administrator cmd /c " + "powershell " + fullPath + " " + outlookVersion + " " + isEnable.ToString();
            cmd.Start();
            cmd.WaitForExit();
        }

        /// <summary>
        /// Get test method CachedMode information
        /// </summary>
        public void GetTestCatgoryInformation()
        {
            Type classType = Type.GetType(TestContext.FullyQualifiedTestClassName);
            MethodBase method = classType.GetMethod(TestContext.TestName);
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

            // Initialize the test output folder path
            testingfolderPath = TestContext.TestDeploymentDir;
            testName = TestContext.TestName;
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
            Thread.Sleep(waittime_item);
        }
    }
}
