namespace MAPIAutomationTest
{
    extern alias FiddlerCore;
    extern alias FiddlerExe;

    using Fiddler;
    using MAPIInspector.Parsers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// The class is used to parse the capture files.
    /// </summary>
    public class MessageParser
    {
        /// <summary>
        /// The all sessions read
        /// </summary>
        public static List<FiddlerCore.Fiddler.Session> OAllSessions;

        /// <summary>
        /// The secure endpoint to use
        /// </summary>
        public static FiddlerCore.Fiddler.Proxy OSecureEndpoint;

        /// <summary>
        /// The secure host name
        /// </summary>
        public static string SSecureEndpointHostname = "localhost";

        /// <summary>
        /// The secure endpoint port
        /// </summary>
        public static int ISecureEndpointPort = 7777;

        /// <summary>
        /// Start Fiddler application to get the capture file
        /// </summary>
        public static void StartFiddler()
        {
            OAllSessions = new List<FiddlerCore.Fiddler.Session>();

            FiddlerCore.Fiddler.FiddlerApplication.BeforeRequest += delegate(FiddlerCore.Fiddler.Session oS)
            {
                // Console.WriteLine("Before request for:\t" + oS.fullUrl);
                // In order to enable response tampering, buffering mode MUST
                // be enabled; this allows FiddlerCore to permit modification of
                // the response in the BeforeResponse handler rather than streaming
                // the response to the client as the response comes in.
                oS.bBufferResponse = false;
                Monitor.Enter(OAllSessions);
                OAllSessions.Add(oS);
                Monitor.Exit(OAllSessions);

                // Set this property if you want FiddlerCore to automatically authenticate by
                // answering Digest/Negotiate/NTLM/Kerberos challenges itself
                // oS["X-AutoAuth"] = "(default)";

                /* If the request is going to our secure endpoint, we'll echo back the response.
                
                Note: This BeforeRequest is getting called for both our main proxy tunnel AND our secure endpoint, 
                so we have to look at which Fiddler port the client connected to (pipeClient.LocalPort) to determine whether this request 
                was sent to secure endpoint, or was merely sent to the main proxy tunnel (e.g. a CONNECT) in order to *reach* the secure endpoint.

                As a result of this, if you run the demo and visit https://localhost:7777 in your browser, you'll see

                Session list contains...
                 
                    1 CONNECT http://localhost:7777
                    200                                         <-- CONNECT tunnel sent to the main proxy tunnel, port 8877

                    2 GET https://localhost:7777/
                    200 text/html                               <-- GET request decrypted on the main proxy tunnel, port 8877

                    3 GET https://localhost:7777/               
                    200 text/html                               <-- GET request received by the secure endpoint, port 7777
                */

                if ((oS.oRequest.pipeClient.LocalPort == ISecureEndpointPort) && (oS.hostname == SSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://" + SSecureEndpointHostname + ":" + ISecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
            };

            FiddlerCore.Fiddler.FiddlerApplication.AfterSessionComplete += delegate(FiddlerCore.Fiddler.Session oS)
            {
                Console.Title = "Session list contains: " + OAllSessions.Count.ToString() + " sessions";
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

            // For the purposes of this demo, we'll forbid connections to HTTPS 
            // sites that use invalid certificates. Change this from the default only
            // if you know EXACTLY what that implies.
            FiddlerCore.Fiddler.CONFIG.IgnoreServerCertErrors = false;
            FiddlerCore.Fiddler.FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            // For forward-compatibility with updated FiddlerCore libraries, it is strongly recommended that you 
            // start with the DEFAULT options and manually disable specific unwanted options.
            FiddlerCore.Fiddler.FiddlerCoreStartupFlags oFCSF = FiddlerCore.Fiddler.FiddlerCoreStartupFlags.Default;
            int fiddlerPort = 8877;
            if (!FiddlerCore.Fiddler.FiddlerApplication.IsStarted())
            {
                FiddlerCore.Fiddler.FiddlerApplication.Startup(fiddlerPort, oFCSF);
            }
                     
            OSecureEndpoint = FiddlerCore.Fiddler.FiddlerApplication.CreateProxyEndpoint(ISecureEndpointPort, true, SSecureEndpointHostname);
        }

        /// <summary>
        /// Clear the fiddler session list.
        /// </summary>
        public static void ClearSessions()
        {
            Monitor.Enter(OAllSessions);
            OAllSessions.Clear();
            Monitor.Exit(OAllSessions);
        }

        /// <summary>
        /// Save Fiddler sessions to local machine
        /// </summary>
        /// <param name="testName">The test case name</param>
        /// <returns>Saved result, true means success</returns>
        public static string SaveSessionsToLocal(string testName)
        {
            bool isSuccessful = false;
            string fileName = string.Empty;
            string secureFilenamePath = TestBase.TestingfolderPath + Path.DirectorySeparatorChar + testName;
            string secureFileName = DateTime.Now.ToString("hh-mm-ss") + ".saz";
            //string fullName = secureFilenamePath + Path.DirectorySeparatorChar + secureFileName;
            string fullName = @"C:\MAPIInspector\new" + secureFileName;
            List<FiddlerCore.Fiddler.Session> allSessionsNew = new List<FiddlerCore.Fiddler.Session>();
            allSessionsNew = OAllSessions;
            try
            {
                try
                {
                    Monitor.Enter(allSessionsNew);
                    string password = null;
                    if (!Directory.Exists(secureFilenamePath))
                    {
                        Directory.CreateDirectory(secureFilenamePath);
                    }

                    isSuccessful = FiddlerCore.Fiddler.Utilities.WriteSessionArchive(fullName, allSessionsNew.ToArray(), password, false);
                    if (isSuccessful)
                    {
                        fileName = fullName;
                    }
                }
                finally
                {
                    Monitor.Exit(allSessionsNew);
                }
            }
            catch (Exception eX)
            {
                Console.WriteLine("Save failed: " + eX.Message);
            }

            return fileName;
        }

        /// <summary>
        /// Parse the capture file using the MAPI Inspector
        /// </summary>
        /// <param name="fileName">The file name to parse</param>
        /// <param name="allRops">All ROPs contained in list</param>
        /// <returns>Parse result, true means success</returns>
        public static bool ParseMessageUsingMAPIInspector(string fileName, out List<string> allRops)
        {
            bool result = true;
            List<FiddlerExe.Fiddler.Session> allSessions = new List<FiddlerExe.Fiddler.Session>();
            FiddlerExe.Fiddler.Session sessionExe;
            List<FiddlerCore.Fiddler.Session> allSessionsNew = FiddlerCore.Fiddler.Utilities.ReadSessionArchive(fileName, false, "MAPIAutomationTest").ToList();
            int sessionCount = allSessionsNew.Count;

            for (int i = 0; i < sessionCount; i++)
            {
                FiddlerExe.Fiddler.HTTPRequestHeaders requestHeader = new FiddlerExe.Fiddler.HTTPRequestHeaders();
                if (allSessionsNew[i].RequestHeaders.Exists("X-RequestType"))
                {
                    requestHeader["X-RequestType"] = allSessionsNew[i].RequestHeaders["X-RequestType"];
                }

                if (allSessionsNew[i].RequestHeaders.ExistsAndContains("Content-Type", "application/mapi-http"))
                {
                    requestHeader["Content-Type"] = allSessionsNew[i].RequestHeaders["Content-Type"];
                }

                if (allSessionsNew[i].RequestHeaders.Exists("X-ClientInfo"))
                {
                    requestHeader["X-ClientInfo"] = allSessionsNew[i].RequestHeaders["X-ClientInfo"];
                }

                sessionExe = new FiddlerExe.Fiddler.Session(requestHeader, allSessionsNew[i].requestBodyBytes);
                sessionExe.responseBodyBytes = allSessionsNew[i].responseBodyBytes;
                if (allSessionsNew[i].ResponseHeaders.Exists("Transfer-Encoding"))
                {
                    sessionExe["Transfer-Encoding"] = allSessionsNew[i].ResponseHeaders["Transfer-Encoding"];
                }
                if (allSessionsNew[i].ResponseHeaders.Exists("X-ResponseCode"))
                {
                    sessionExe["X-ResponseCode"] = allSessionsNew[i].ResponseHeaders["X-ResponseCode"];
                }
                if (allSessionsNew[i].ResponseHeaders.Exists("Content-Type"))
                {
                    sessionExe["Content-Type"] = allSessionsNew[i].ResponseHeaders["Content-Type"];
                }
                if (allSessionsNew[i].LocalProcess != string.Empty)
                {
                    sessionExe["LocalProcess"] = allSessionsNew[i].LocalProcess;
                }
                sessionExe.fullUrl = allSessionsNew[i].fullUrl;
                sessionExe["VirtualID"] = allSessionsNew[i].id.ToString();
                allSessions.Add(sessionExe);
            }
            MapiInspector.MAPIRequestInspector ma = new MapiInspector.MAPIRequestInspector();
            string filepath = TestBase.TestingfolderPath + Path.DirectorySeparatorChar + TestBase.TestName;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            result = ma.ParseCaptureFile(allSessions.ToArray(), filepath, TestBase.TestName, out allRops);
            if (!result)
            {
                string filenameNew = fileName.Split('\\').Last().Split('.').First();
                File.Move(filepath + "\\" + "error.txt", filepath + "\\" + filenameNew + ".txt");
            }
            return result;
        }

        /// <summary>
        /// Close the fiddler.
        /// </summary>
        public static void CloseFiddler()
        {
            if (null != OSecureEndpoint)
            {
                OSecureEndpoint.Dispose();
                FiddlerCore.Fiddler.FiddlerApplication.Shutdown();
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Parse the capture file
        /// </summary>
        /// <param name="allRops">All ROP list covered</param>
        /// <returns>The parsing result, true means no error</returns>
        public static bool ParseMessage(out List<string> allRops)
        {
            bool result = true;
            string fileNameToParse = SaveSessionsToLocal(TestBase.TestName);
            result = ParseMessageUsingMAPIInspector(fileNameToParse, out allRops);
            return result;
        }
    }
}
