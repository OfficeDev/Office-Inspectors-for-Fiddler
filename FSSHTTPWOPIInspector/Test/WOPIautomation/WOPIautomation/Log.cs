using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WOPIautomation
{
    public class Log
    {
        public static void WriteLog(string result)
        {
            string logFile = "d:\\test.txt";
            // Write the string to a file.
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
            System.IO.StreamWriter file = new System.IO.StreamWriter(logFile);
            file.WriteLine(result);

            file.Close();   
            
        }
    }
}
