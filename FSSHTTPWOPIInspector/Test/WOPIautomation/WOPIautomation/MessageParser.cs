using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Windows.Automation;
using System.Linq;
using FSSHTTPandWOPIInspector;

namespace WOPIautomation
{
    extern alias FiddlerCore;
    extern alias FiddlerExe;
    using System.Diagnostics;
    public class MessageParser
    {
        public static List<FiddlerCore.Fiddler.Session> oAllSessions;
        public static FiddlerCore.Fiddler.Proxy oSecureEndpoint;
        public static string sSecureEndpointHostname = "localhost";
        public static int iSecureEndpointPort = 7777;

        /// <summary>
        /// Start Fiddler application to get the capture file
        /// </summary>
        public static void StartFiddler()
        {
            oAllSessions = new List<FiddlerCore.Fiddler.Session>();

            FiddlerCore.Fiddler.FiddlerApplication.BeforeRequest += delegate(FiddlerCore.Fiddler.Session oS)
            {
                // Console.WriteLine("Before request for:\t" + oS.fullUrl);
                // In order to enable response tampering, buffering mode MUST
                // be enabled; this allows FiddlerCore to permit modification of
                // the response in the BeforeResponse handler rather than streaming
                // the response to the client as the response comes in.
                oS.bBufferResponse = false;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);

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

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
            };


            FiddlerCore.Fiddler.FiddlerApplication.AfterSessionComplete += delegate(FiddlerCore.Fiddler.Session oS)
            {
                //Console.WriteLine("Finished session:\t" + oS.fullUrl); 
                Console.Title = ("Session list contains: " + oAllSessions.Count.ToString() + " sessions");
            };


            string sSAZInfo = "NoSAZ";
            sSAZInfo = Assembly.GetAssembly(typeof(Ionic.Zip.ZipFile)).FullName;

            // You can load Transcoders from any different assembly if you'd like, using the ImportTranscoders(string AssemblyPath) 
            // overload.
            //
            //if (!FiddlerApplication.oTranscoders.ImportTranscoders(Assembly.GetExecutingAssembly()))
            //{
            //    Console.WriteLine("This assembly was not compiled with a SAZ-exporter");
            //}

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

            // ... but you can allow a specific (even invalid) certificate by implementing and assigning a callback...
            // FiddlerApplication.OnValidateServerCertificate += new System.EventHandler<ValidateServerCertificateEventArgs>(CheckCert);

            FiddlerCore.Fiddler.FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            // For forward-compatibility with updated FiddlerCore libraries, it is strongly recommended that you 
            // start with the DEFAULT options and manually disable specific unwanted options.
            FiddlerCore.Fiddler.FiddlerCoreStartupFlags oFCSF = FiddlerCore.Fiddler.FiddlerCoreStartupFlags.Default;
            int iPort = 8877;
            if (!FiddlerCore.Fiddler.FiddlerApplication.IsStarted())
            {
                FiddlerCore.Fiddler.FiddlerApplication.Startup(iPort, oFCSF);
            }
                     
            oSecureEndpoint = FiddlerCore.Fiddler.FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);

            /*
            // Inside your main object, create a list to hold the sessions
            // This generic list type requires your source file includes #using System.Collections.Generic.
            oAllSessions = new List<FiddlerCore.Fiddler.Session>();
            //MessageParser.ClearSessions();

            // Inside your attached event handlers, add the session to the list:
            FiddlerCore.Fiddler.FiddlerApplication.BeforeRequest += delegate (FiddlerCore.Fiddler.Session oS)
            {
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
            };

            FiddlerCore.Fiddler.FiddlerApplication.AfterSessionComplete += delegate(FiddlerCore.Fiddler.Session oS)
            {
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

            if (!FiddlerCore.Fiddler.FiddlerApplication.IsStarted())
            {
                FiddlerCore.Fiddler.FiddlerApplication.Startup(8888, FiddlerCore.Fiddler.FiddlerCoreStartupFlags.Default);
            }
            */
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
            List<FiddlerCore.Fiddler.Session> oAllSessionsNew = new List<FiddlerCore.Fiddler.Session>();
            oAllSessionsNew = oAllSessions;
            try
            {
                try
                {
                    Monitor.Enter(oAllSessionsNew);
                    string sPassword = null;
                    if (!Directory.Exists(sFilenamePath))
                    {
                        Directory.CreateDirectory(sFilenamePath);
                    }
                    bSuccess = FiddlerCore.Fiddler.Utilities.WriteSessionArchive(fullName, oAllSessionsNew.ToArray(), sPassword, false);
                    if (bSuccess)
                    {
                        fileName = fullName;
                    }
                }
                finally
                {
                    Monitor.Exit(oAllSessionsNew);
                }
            }
            catch (Exception eX)
            {
                Console.WriteLine("Save failed: " + eX.Message);
            }
            return fileName;
        }

        public static bool ParseMessageUsingWOPIInspector(string fileName)
        {
            bool result = true;
            List<FiddlerExe.Fiddler.Session> allSessions = new List<FiddlerExe.Fiddler.Session>();
            FiddlerExe.Fiddler.Session sessionExe;

            List<FiddlerCore.Fiddler.Session> oAllSessionsNew = FiddlerCore.Fiddler.Utilities.ReadSessionArchive(fileName, false, "MAPIAutomationTest").ToList();
            int sessionCount = oAllSessionsNew.Count;

            for (int i = 0; i < sessionCount; i++)
            {
                FiddlerExe.Fiddler.HTTPRequestHeaders requestHeader = new FiddlerExe.Fiddler.HTTPRequestHeaders();
                if (oAllSessionsNew[i].RequestHeaders.Exists("Content-Encoding"))
                {
                    requestHeader["Content-Encoding"] = oAllSessionsNew[i].RequestHeaders["Content-Encoding"];
                }

                if (oAllSessionsNew[i].RequestHeaders.Exists("SOAPAction"))
                {
                    requestHeader["SOAPAction"] = oAllSessionsNew[i].RequestHeaders["SOAPAction"];
                }

                // X-WOPI-Override
                if (oAllSessionsNew[i].RequestHeaders.Exists("X-WOPI-Override"))
                {
                    requestHeader["X-WOPI-Override"] = oAllSessionsNew[i].RequestHeaders["X-WOPI-Override"];
                }

                // X-WOPI-OldLock
                if (oAllSessionsNew[i].RequestHeaders.Exists("X-WOPI-OldLock"))
                {
                    requestHeader["X-WOPI-OldLock"] = oAllSessionsNew[i].RequestHeaders["X-WOPI-OldLock"];
                }

                // X-WOPI-RelativeTarget
                if (oAllSessionsNew[i].RequestHeaders.Exists("X-WOPI-RelativeTarget"))
                {
                    requestHeader["X-WOPI-RelativeTarget"] = oAllSessionsNew[i].RequestHeaders["X-WOPI-RelativeTarget"];
                }

                sessionExe = new FiddlerExe.Fiddler.Session(requestHeader, oAllSessionsNew[i].requestBodyBytes);
                sessionExe.responseBodyBytes = oAllSessionsNew[i].responseBodyBytes;

                if (oAllSessionsNew[i].ResponseHeaders.Exists("Transfer-Encoding"))
                {
                    sessionExe["Transfer-Encoding"] = oAllSessionsNew[i].ResponseHeaders["Transfer-Encoding"];
                }

                if (oAllSessionsNew[i].ResponseHeaders.Exists("Content-Encoding"))
                {
                    sessionExe["Content-Encoding"] = oAllSessionsNew[i].ResponseHeaders["Content-Encoding"];
                }

                // HTTPResponseStatus
                sessionExe["HTTPResponseStatus"] = oAllSessionsNew[i].ResponseHeaders.HTTPResponseStatus;
                
                // StatusDescription
                sessionExe["StatusDescription"] = oAllSessionsNew[i].ResponseHeaders.StatusDescription;
                
                sessionExe.fullUrl = oAllSessionsNew[i].fullUrl;
                sessionExe["VirtualID"] = (i + 1).ToString();

                allSessions.Add(sessionExe);
            }

            FSSHTTPandWOPIInspector.FSSHTTPandWOPIInspector WOPIInspector = new FSSHTTPandWOPIInspector.FSSHTTPandWOPIInspector();
            string errorPath = TestBase.testingfolderPath;
            result = WOPIInspector.ParseCaptureFile(allSessions.ToArray(), TestBase.testResultPath);
            
            return result;
        }

        /// <summary>
        /// Close the fiddler.
        /// </summary>
        public static void CloseFiddler()
        {
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
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
            result = ParseMessageUsingWOPIInspector(fileNameToParse);
            return result;
        }
    }
}
