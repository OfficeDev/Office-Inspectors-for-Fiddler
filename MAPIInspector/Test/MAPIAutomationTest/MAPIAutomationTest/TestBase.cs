namespace MAPIAutomationTest
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Automation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Outlook = Microsoft.Office.Interop.Outlook;

    /// <summary>
    /// The test case class used to common initialize or cleanup
    /// </summary>
    [TestClass]
    public class TestBase
    {
        #region Variables

        /// <summary>
        /// The outlook APP
        /// </summary>
        protected static Outlook.Application outlookApp = null;

        /// <summary>
        /// The Inbox folder of outlook
        /// </summary>
        protected static Outlook.MAPIFolder inboxFolders;

        /// <summary>
        /// The sent folder of outlook
        /// </summary>
        protected static Outlook.MAPIFolder sentMailFolder;

        /// <summary>
        /// The public folder of outlook
        /// </summary>
        protected static Outlook.MAPIFolder publicFolders;

        /// <summary>
        /// The delete folder of outlook
        /// </summary>
        protected static Outlook.MAPIFolder deletedItemsFolders;

        /// <summary>
        /// The draft folder of outlook
        /// </summary>
        protected static Outlook.MAPIFolder draftsFolders;

        /// <summary>
        /// The wait time for automation windows
        /// </summary>
        protected static int waittimeWindow;

        /// <summary>
        /// The wait time for outlook item
        /// </summary>
        protected static int waittimeItem;

        /// <summary>
        /// The testing folder path
        /// </summary>
        private static string testingfolderPath;

        /// <summary>
        /// The current running test name
        /// </summary>
        private static string testName;

        public static string filePath = "";

        /// <summary>
        /// Gets or sets the test name
        /// </summary>
        public static string TestName
        {
            get
            {
                return testName;
            }

            set
            {
                testName = value;
            }
        }

        /// <summary>
        /// Gets or sets the testing folder path
        /// </summary>
        public static string TestingfolderPath
        {
            get
            {
                return testingfolderPath;
            }

            set
            {
                testingfolderPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the test context information
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        /// <summary>
        /// Update the outlook cached mode configuration from on to off.
        /// </summary>
        /// <param name="isEnable">Enabled or not</param>
        public static void UpdateOutlookConfiguration(bool isEnable)
        {
            string outlookVersion = ConfigurationManager.AppSettings["OutlookVersion"].ToString();
            string scriptPath = ConfigurationManager.AppSettings["PowershellScript_path"].ToString();
            string fullPath = Path.GetFullPath(scriptPath);

            // Configure the PowerShell execution policy to run script
            using (PowerShell powerShellInstance = PowerShell.Create())
            {
                string script = "Set-ExecutionPolicy -Scope currentuser -ExecutionPolicy bypass; Get-ExecutionPolicy"; // the second command to know the ExecutionPolicy level
                powerShellInstance.AddScript(script);
                var someResult = powerShellInstance.Invoke();
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
        /// Start fiddlercore
        /// </summary>
        /// <param name="context">Test context information</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            MessageParser.StartFiddler();
        }

        /// <summary>
        /// Close fiddlercore and generate coverage report
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            MessageParser.CloseFiddler();
            GenerateReport.GenerateCoverageReport();
        }

        /// <summary>
        /// Test initialization
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.GetTestCatgoryInformation();
            EndStartedOutLook();
            Thread.Sleep(10000);
            MessageParser.ClearSessions();
            string outLookPath = ConfigurationManager.AppSettings["OutLookPath"].ToString();
            waittimeWindow = int.Parse(ConfigurationManager.AppSettings["WaitTimeWindow"].ToString());
            waittimeItem = int.Parse(ConfigurationManager.AppSettings["WaitTimeItem"].ToString());
            Process p = Process.Start(outLookPath);

            AutomationElement outlookWindow = null;
            var desktop = AutomationElement.RootElement;
            string userName = ConfigurationManager.AppSettings["Office365Account"].ToString();
            var condition_Outlook = new PropertyCondition(AutomationElement.NameProperty, "Inbox - " + userName + " - Outlook");

            int count = 0;
            while (outlookWindow == null)
            {
                outlookWindow = desktop.FindFirst(TreeScope.Children, condition_Outlook);
                Thread.Sleep(waittimeItem / 10);
                count += waittimeItem / 10;
                if (count >= waittimeWindow)
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
                            catch
                            {
                                continue;
                            }
                        }

                        break;
                    }
                }
            }

            Thread.Sleep(waittimeItem);
            try
            {
                outlookApp = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
            }
            catch
            {
                throw new Exception("Get active outlook application failed, please check if outlook is running");
            }

            inboxFolders = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
            sentMailFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);
            deletedItemsFolders = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems);
            draftsFolders = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDrafts);
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
        /// Get test method CachedMode information
        /// </summary>
        public void GetTestCatgoryInformation()
        {
            Type classType = Type.GetType(TestContext.FullyQualifiedTestClassName);
            MethodBase method = classType.GetMethod(TestContext.TestName);
            object[] customAttributes = method.GetCustomAttributes(typeof(TestCategoryAttribute), true);
            if (customAttributes.Length > 0)
            {
                TestCategoryAttribute attr = (TestCategoryAttribute)customAttributes[0];
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
            TestingfolderPath = TestContext.TestDeploymentDir;
            TestName = TestContext.TestName;
        }

        /// <summary>
        /// End all started outlook application
        /// </summary>
        protected static void EndStartedOutLook()
        {
            // Check whether there is an Outlook process running.
            if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
            {
                Outlook.Application appExist;
                try
                {
                    appExist = Marshal.GetActiveObject("Outlook.Application") as Microsoft.Office.Interop.Outlook.Application;
                }
                catch (Exception)
                {
                    throw new Exception("Get the running Outlook application failed");
                }

                ReleaseObj(appExist);
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

            Thread.Sleep(waittimeItem);
        }
    }
}
