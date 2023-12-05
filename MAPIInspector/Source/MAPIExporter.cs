using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Fiddler;
using System.Collections.Generic;

[ProfferFormat("MAPI", "Parsed MAPI frames")]
public class MAPIExporter : ISessionExporter  // Ensure class is public, or Fiddler Classic won't see it!
{
    public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions,
        EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
    {
        bool bResult;

        // Determine if we already have a filename from the dictOptions collection
        string sFilename = null;
        if (null != dictOptions && dictOptions.ContainsKey("Filename"))
        {
            sFilename = dictOptions["Filename"] as string;
        }

        if (string.IsNullOrEmpty(sFilename)) sFilename = Utilities.ObtainSaveFilename("Export As " + sFormat, "CSV Files (*.csv)|*.csv");

        if (string.IsNullOrEmpty(sFilename)) return false;

        try
        {
            StreamWriter swOutput = new StreamWriter(sFilename, false, Encoding.UTF8);

            foreach (Session oS in oSessions)
            {
                //swOutput.WriteLine();
            }

            swOutput.Close();
            bResult = true;
        }
        catch (Exception eX)
        {
            MessageBox.Show(eX.Message, "Failed to export");
            bResult = false;
        }

        return bResult;
    }

    public void Dispose()
    {
    }
}