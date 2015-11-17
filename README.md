# MAPI Inspector for Fiddler
The Messaging Application Programming Interface (MAPI) Inspector for [Fiddler](http://www.telerik.com/fiddler) decodes the MAPI message payload of an HTTP POST request and response. The MAPI Inspector displays under the *Inspectors* tab in Fiddler.

This repository also includes Jscript that adds an *MS Protocol* column in the Fiddler web session panel. The *MS Protocol* column displays the protocols that are relevant to MAPI messages thereby allowing you to easily identify which HTTP requests and responses contain MAPI message payloads.

The MAPI inspector decodes the MAPI message payload according to [MS-OXCMAPIHTTP](https://msdn.microsoft.com/en-us/library/Dn530952(v=EXCHG.80).aspx).

## Installation 
Installation of the MAPI Inspector involves adding the DLL file and the MAPI script. Before installing these items, you must first install the latest [Fiddler](http://www.telerik.com/fiddler) tool and then run it. Note that Fiddler must be run at least once before installing the MAPI Inspector.

### MAPI Inspector DLL###
1. Copy the file [MAPIFiddlerInspector.dll](https://github.com/OfficeDev/MAPI-Inspector-for-Fiddler/blob/master/MAPIFiddlerInspector.dll) into the C:\Program Files\Fiddler2\Inspectors directory. Alternatively, you can clone this repository, build the MAPIFiddlerInspector.dll, and copy the built .dll to your C:\Program Files\Fiddler2\Inspectors directory.
2. Restart Fiddler. After restarting, the MAPI Inspector will display under the *Inspectors* tab for both the request and the response, as shown in the following screenshot.

    ![alt tag](/README-Images/Figure1-Inspector.png)

### MAPI Script ###

1. From the *Rules* menu, which is shown in the following screenshot, click *Customize Rules*.
    
    ![alt tag](/README-Images/Figure2-mapiscript.png)

2. When the following message box displays, click the *Yes* button to install the FiddlerScript editor.

    ![alt tag](/README-Images/Figure3-mapiscript.png)

3. Restart Fiddler after the installation of the FiddlerScript editor completes. Fiddler displays a new tab, *FiddlerScript*, as shown in the following screenshot. Copy the code from the code from the [MAPI.js](https://github.com/OfficeDev/MAPI-Inspector-for-Fiddler/blob/master/MAPI.js) file and paste it into the definition for the **Handlers** class. Click the *Save Script* button to save the script.

    ![alt tag](/README-Images/Figure4-mapiscript.png)

4. Restart Fiddler. After restarting, the *MS protocol column* can be displayed in session view.

    ![alt tag](/README-Images/Figure5-mapiscript.png)

