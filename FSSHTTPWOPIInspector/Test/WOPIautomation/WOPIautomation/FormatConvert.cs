using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.IO;

namespace WOPIautomation
{
    class FormatConvert
    {
        /// <summary>
        /// Convert .cap file to .saz file
        /// </summary>
        /// <param name="inputFile">The cap file path</param>
        /// <param name="outputpath">The output path for created saz file</param>
        /// <param name="captureName">The name for saz</param>
        /// <returns>The result to convert, ture means success.</returns>
        public static bool Convert(string inputFile, string outputpath, string captureName)
        {
            bool result = false;

            // Set cap file always opened by fiddler as precondition
            bool isStarted = StartFiddler(inputFile);
            string fullpath = "";
            do
            {
                Thread.Sleep(5000);
            }
            while (!isStarted);
            result = SaveSAZ(outputpath, captureName, out fullpath);
            
            return result;         
        }

        /// <summary>
        /// Start fiddler exe application.
        /// </summary>
        /// <returns>The started result</returns>
        public static bool StartFiddlerExe()
        {
            string fiddlerPath = ConfigurationManager.AppSettings["FiddlerPath"];
            string fiddlerEXE = "fiddler.exe";
            string fullPath = Path.Combine(fiddlerPath, fiddlerEXE);
            return StartFiddler(fullPath);
        }

        /// <summary>
        /// Start .cap or fiddler exe
        /// </summary>
        /// <param name="FiddlerOrCapturePath">The cap file path</param>
        /// <returns>The started result</returns>
        public static bool StartFiddler(string FiddlerOrCapturePath)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = FiddlerOrCapturePath;
            Utility.WaitFile(FiddlerOrCapturePath);
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();

            IntPtr fiddler = SimulateKeyInput.FindWindow("WindowsForms10.Window.8.app.0.34f5582_r11_ad1", "Telerik Fiddler Web Debugger");
            int count =0;
            do
            {
                Thread.Sleep(3000);
                count += 3;
                fiddler = SimulateKeyInput.FindWindow("WindowsForms10.Window.8.app.0.34f5582_r11_ad1", "Telerik Fiddler Web Debugger");
                if (count > 20)
                {
                    break;
                }
            }
            while (fiddler != IntPtr.Zero);

            if (fiddler == IntPtr.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Save sessions in Fiddler to testing output path
        /// </summary>
        /// <param name="outputpath">The saz file will be put</param>
        /// <param name="captureName">The saz file name</param>
        /// <returns></returns>
        public static bool SaveSAZ(string outputpath, string captureName, out string fullPathFile)
        {
            bool result = false;
            fullPathFile = "";
            try
            {
                string MyBatchFile = ConfigurationManager.AppSettings["BatPath"];
                string fiddlerPath = ConfigurationManager.AppSettings["FiddlerPath"];
                string userName = ConfigurationManager.AppSettings["defaultUserNameForFiddler"];
                string outputPath = outputpath;
                string newName = captureName;
                var process = new Process
                {
                    StartInfo =
                    {
                        Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", fiddlerPath, userName, outputPath, newName)
                    }
                };
                string fileName = newName + ".saz";
                process.StartInfo.FileName = MyBatchFile;
                bool b = process.Start();
                Utility.WaitFile(Path.Combine(outputpath, fileName));
                result = true;
                fullPathFile = Path.Combine(outputpath, fileName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message);
                return false;
            }
        }
    }
}
