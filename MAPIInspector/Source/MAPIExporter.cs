using System;
using System.Windows.Forms;
using Fiddler;
using System.Collections.Generic;
using MapiInspector;

[ProfferFormat("MAPI", "Parsed MAPI frames")]
public class MAPIExporter : ISessionExporter
{
    public bool ExportSessions(string sFormat, Session[] oSessions, Dictionary<string, object> dictOptions,
        EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
    {
        bool result;

        string filePath = null;
        if (null != dictOptions && dictOptions.ContainsKey("FilePath"))
        {
            filePath = dictOptions["FilePath"] as string;
        }

        if (string.IsNullOrEmpty(filePath)) filePath = Fiddler.Utilities.ObtainSaveFilename("Export As " + sFormat, "JSON File (*.json)|*.json");

        if (string.IsNullOrEmpty(filePath)) return false;

        try
        {
            MAPIParser.ParseCaptureFile(oSessions, filePath);
            result = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Failed to export");
            result = false;
        }

        return result;
    }

    public void Dispose() { }
}