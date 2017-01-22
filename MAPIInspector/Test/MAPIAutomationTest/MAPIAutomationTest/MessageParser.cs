using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Windows.Automation;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace MAPIAutomationTest
{
    extern alias FiddlerCore;
    extern alias FiddlerExe;
    using System.Diagnostics;
    public class MessageParser
    {
        public static List<FiddlerCore.Fiddler.Session> oAllSessions;

        /// <summary>
        /// Start Fiddler application to get the capture file
        /// </summary>
        public static void StartFiddler()
        {
            FiddlerCore.Fiddler.FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
            if (!FiddlerCore.Fiddler.FiddlerApplication.IsStarted())
            {
                FiddlerCore.Fiddler.FiddlerApplication.Startup(8888, FiddlerCore.Fiddler.FiddlerCoreStartupFlags.Default);
            }

            // Inside your main object, create a list to hold the sessions
            // This generic list type requires your source file includes #using System.Collections.Generic.
            oAllSessions = new List<FiddlerCore.Fiddler.Session>();
            MessageParser.ClearSessions();

            // Inside your attached event handlers, add the session to the list:
            FiddlerCore.Fiddler.FiddlerApplication.BeforeRequest += delegate (FiddlerCore.Fiddler.Session oS)
            {
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
            };

            string sSAZInfo = "NoSAZ";
            sSAZInfo = Assembly.GetAssembly(typeof(Ionic.Zip.ZipFile)).FullName;

            Fiddler.DNZSAZProvider.fnObtainPwd = () =>
            {
                Console.WriteLine("Enter the password (or just hit Enter to cancel):");
                string sResult = Console.ReadLine();
                Console.WriteLine();
                return sResult;
            };

            FiddlerCore.Fiddler.FiddlerApplication.oSAZProvider = new Fiddler.DNZSAZProvider();
        }

        /// <summary>
        /// Add the event for fiddler application
        /// </summary>
        /// <param name="oSession">All sessions in Fiddler</param>
        public static void FiddlerApplication_AfterSessionComplete(FiddlerCore.Fiddler.Session oSession)
        {
            // Ignore HTTPS connect requests
            if (oSession.RequestMethod == "CONNECT")
                return;
        }

        /// <summary>
        /// Clear the fiddler session list.
        /// </summary>
        public static void ClearSessions()
        {
            Monitor.Enter(oAllSessions);
            oAllSessions.Clear();
            Monitor.Exit(oAllSessions);
        }


        /// <summary>
        /// Save Fiddler sessions to local machine
        /// </summary>
        /// <param name="oAllSessions">All sessions in Fiddler</param>
        public static string SaveSessionsToLocal(string testName)
        {
            bool bSuccess = false;
            string fileName = "";
            string sFilenamePath = TestBase.testingfolderPath + Path.DirectorySeparatorChar + testName;
            string sFileName = DateTime.Now.ToString("hh-mm-ss") + ".saz";
            string fullName = sFilenamePath + Path.DirectorySeparatorChar + sFileName;
            try
            {
                try
                {
                    Monitor.Enter(oAllSessions);
                    string sPassword = null;
                    if (!Directory.Exists(sFilenamePath))
                    {
                        Directory.CreateDirectory(sFilenamePath);
                    }
                    bSuccess = FiddlerCore.Fiddler.Utilities.WriteSessionArchive(fullName, oAllSessions.ToArray(), sPassword, false);
                    if (bSuccess)
                    {
                        fileName = fullName;
                    }
                }
                finally
                {
                    Monitor.Exit(oAllSessions);
                }
            }
            catch (Exception eX)
            {
                Console.WriteLine("Save failed: " + eX.Message);
            }
            return fileName;
        }

        public static bool ParseMessageUsingMAPIInspector(string fileName)
        {
            bool result = true;
            List<FiddlerExe.Fiddler.Session> allSessions = new List<FiddlerExe.Fiddler.Session>();
            FiddlerExe.Fiddler.Session sessionExe;

            List<FiddlerCore.Fiddler.Session> oAllSessionsNew = FiddlerCore.Fiddler.Utilities.ReadSessionArchive(fileName, false, "MAPIAutomationTest").ToList();
            int sessionCount = oAllSessionsNew.Count;

            for (int i = 0; i < sessionCount; i++)
            {
                FiddlerExe.Fiddler.HTTPRequestHeaders requestHeader = new FiddlerExe.Fiddler.HTTPRequestHeaders();
                if (oAllSessionsNew[i].RequestHeaders.Exists("X-RequestType"))
                {
                    requestHeader["X-RequestType"] = oAllSessionsNew[i].RequestHeaders["X-RequestType"];
                }
                if (oAllSessionsNew[i].RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http"))
                {
                    requestHeader["Content-Type"] = oAllSessionsNew[i].RequestHeaders["Content-Type"];
                }

                sessionExe = new FiddlerExe.Fiddler.Session(requestHeader, oAllSessionsNew[i].requestBodyBytes);
                sessionExe.responseBodyBytes = oAllSessionsNew[i].responseBodyBytes;

                if (oAllSessionsNew[i].ResponseHeaders.Exists("Transfer-Encoding"))
                {
                    sessionExe["Transfer-Encoding"] = oAllSessionsNew[i].ResponseHeaders["Transfer-Encoding"];
                }

                if (oAllSessionsNew[i].ResponseHeaders.Exists("X-ResponseCode"))
                {
                    sessionExe["X-ResponseCode"] = oAllSessionsNew[i].ResponseHeaders["X-ResponseCode"];
                }
                if (oAllSessionsNew[i].ResponseHeaders.Exists("Content-Type"))
                {
                    sessionExe["Content-Type"] = oAllSessionsNew[i].ResponseHeaders["Content-Type"];
                }
                sessionExe.fullUrl = oAllSessionsNew[i].fullUrl;
                sessionExe["VirtualID"] = (i + 1).ToString();

                allSessions.Add(sessionExe);
            }

            MapiInspector.MAPIInspector ma = new MapiInspector.MAPIInspector();
            string errorPath = TestBase.testingfolderPath + Path.DirectorySeparatorChar + TestBase.testName;
            result = ma.ParseCaptureFile(allSessions.ToArray(), errorPath);
            return result;
        }

        /// <summary>
        /// Close the fiddler.
        /// </summary>
        public static void CloseFiddler()
        {
            FiddlerCore.Fiddler.FiddlerApplication.Shutdown();
            Thread.Sleep(500);
        }

        /// <summary>
        /// Parse the capture file
        /// </summary>
        /// <returns>The parsing result, true means no error.</returns>
        public static bool ParseMessage()
        {
            bool result = true;
            string fileNameToParse = SaveSessionsToLocal(TestBase.testName);
            //CloseFiddler();
            result = ParseMessageUsingMAPIInspector(fileNameToParse);
            return result;
        }
    }
}
