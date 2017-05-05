using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;

namespace WOPIautomation
{
    [TestClass]
    public class TestBase
    {
        #region Variables
        protected static int waittime_window;
        protected static int waittime_item;
        public static string testingfolderPath;
        public static string testName;
        public static bool isWOPI;
        public static string testResultPath;
        public static string captureName;


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

        [TestInitialize()]
        public void Initialize()
        {
            MessageParser.ClearSessions();
            
            GetTestCatgoryInformation();
            if (!isWOPI)
            {
                FormatConvert.StartFiddlerExe();
            }
            else
            {
                GetTestingFolder();
                StartTrace();
            }
			Browser.Initialize();
        }

        /// <summary>
        /// Test clean up
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            Browser.Goto(Browser.BaseAddress);
            Browser.Close();
        }

        /// <summary>
        /// Start capture remote trace on WOPI server
        /// </summary>
        public void StartTrace()
        {
            string startScriptPath = ConfigurationManager.AppSettings["StartScriptPath"];
            string FullPath = Path.GetFullPath(startScriptPath);
            Utility.ExecuteScript(FullPath);

        }

        /// <summary>
        /// Stop capture remote trace on WOPI server
        /// </summary>
        public void StopTrace()
        {
            string stopScriptPath = ConfigurationManager.AppSettings["StopScriptPath"];
            string FullPath = Path.GetFullPath(stopScriptPath);
            Utility.ExecuteScript(FullPath, false);
        }

        /// <summary>
        /// Get test method CachedMode information
        /// </summary>
        public void GetTestCatgoryInformation()
        {
            // Initialize the test output folder path
            testingfolderPath = TestContext.TestDeploymentDir;
            testName = TestContext.TestName;

            Type classType = Type.GetType(TestContext.FullyQualifiedTestClassName);
            MethodBase method = classType.GetMethod(TestContext.TestName);
            object[] CustomAttributes = method.GetCustomAttributes(typeof(TestCategoryAttribute), true);
            if (CustomAttributes.Length > 0)
            {
                TestCategoryAttribute attr = (TestCategoryAttribute)CustomAttributes[0];
                if (attr != null && attr.TestCategories.Count > 0)
                {
                    if (attr.TestCategories[0] == "WOPI")
                    {
                        isWOPI = true;
                    }
                    else
                    {
                        isWOPI = false;
                    }
                }
            }
        }

        /// <summary>
        /// Get test folder information
        /// </summary>
        public static void GetTestingFolder()
        {
            testResultPath = string.Empty;
            string folderPath = Regex.Replace(testingfolderPath, @"\s+", "");
            testResultPath = folderPath + Path.DirectorySeparatorChar + testName;
            captureName = testResultPath + Path.DirectorySeparatorChar + testName + ".saz";
        }
    }
}
