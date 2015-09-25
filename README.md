# MAPI Inspector for Fiddler
Messaging Application Programming Interface (MAPI) Inspector for Fiddler helps you to further decode the HTTP payload to MAPI message according to MAPI protocol Open Specifications.  


## Introduction
Use this inspector to add a MS Protocol column in Fiddler web session panel and MAPI Inspector under fiddler Inspectors.

This is an inspector of [Fiddler](http://www.telerik.com/fiddler) that provides further decoding methods for the message, which are transferred via MAPI over HTTP transport. Install this inspector, you can live capture and parse the MAPI message to know what happened on the wire. 


## Installation 
### MAPI Inspector ###
1. Install the latest [Fiddler](http://www.telerik.com/fiddler) tool. Fiddler must be run at least once before installing MAPI Inspector. 
2. Copy the file [MAPIFiddlerInspector.dll](https://github.com/OfficeDev/MAPI-Inspector-for-Fiddler/MAPIFiddlerInspector.dll) into  C:\Program Files\Fiddler2\Inspectors  and restart Fiddler. Or if you want to put in more effort, clone this repo, build it, and copy the built MAPIFiddlerInspector.dll to your C:\Program Files\Fiddler2\Inspectors directory.
3. Restart Fiddler and then the MAPI Inspector will display under Inspectors.
![alt tag](https://cloud.githubusercontent.com/assets/13864956/10044516/329c59c4-622e-11e5-9b68-8ef920d4ead8.jpg)

### MAPI Script ###
1.	Click the Rules->Customize Rules…  
	![alt tag](https://cloud.githubusercontent.com/assets/13864956/10038377/a6e8d104-61f7-11e5-9a84-989fcf135f2c.png)
2.	Firstly use the Customize Rules, system will notice to set up Script Editor. 
    Click “Yes” button and set up FiddlerScript Editor.  
    ![alt tag](https://cloud.githubusercontent.com/assets/13864956/10044513/2d5a400c-622e-11e5-9b1c-3cb44c11eb15.jpg) 
3.	Finish the installation, the Fiddler will have a new Tab FiddlerScript. 
    ![alt tag](https://cloud.githubusercontent.com/assets/13864956/10044452/b959a3be-622d-11e5-8ca5-acc297b98623.jpg) 
4.	Insert the code in [MAPI.js](https://github.com/OfficeDev/MAPI-Inspector-for-Fiddler/MAPI.js) to the class “Handlers” and click “Save Script” button to save script.  
    ![alt tag](https://cloud.githubusercontent.com/assets/13864956/10044440/9fc2b382-622d-11e5-8308-a642768bf28a.jpg)
5.	Restart Fiddler, MS protocol column will be displayed in session view. 



## Feature

- Add MS Protocol in session view to identify which sessions are MAPI message. 
![alt tag](https://cloud.githubusercontent.com/assets/13864956/10044752/f405a25e-622f-11e5-8dca-c2f5c5521445.jpg)
- Decode the HTTP payload to MAPI message and display the parsed message in MAPI Inspector. The MAPI Inspector includes TreeView (left side) and HexView (right side), when clicking the node in TreeView, the corresponding hex data in HexView can be highlighted.
![alt tag](https://cloud.githubusercontent.com/assets/13864956/10045590/6689c232-6236-11e5-9751-0caf596b3bba.jpg)
- Fourteen Microsoft protocols will be supported in this inspector. For example, MS-OXCFOLD protocol, it specifies how to manipulate folder and its contents. 
![alt tag](https://cloud.githubusercontent.com/assets/13864956/10046218/f1a3c338-6239-11e5-9d3f-38f8d99e42df.jpg)

## Reference
 * [MS-OXCMAPIHTTP](https://msdn.microsoft.com/en-us/library/Dn530952(v=EXCHG.80).aspx)
 * [MS-OXNSPI](https://msdn.microsoft.com/en-us/library/hh354767(v=exchg.80).aspx)
 * [MS-OXCROPS](https://msdn.microsoft.com/en-us/library/cc425494(v=exchg.80).aspx)
 * [MS-OXCFOLD](https://msdn.microsoft.com/en-us/library/cc433475(v=exchg.80).aspx)
 * [MS-OXCFXICS](https://msdn.microsoft.com/en-us/library/cc463916(v=exchg.80).aspx)
 * [MS-OXCMSG](https://msdn.microsoft.com/en-us/library/cc463900(v=exchg.80).aspx)
 * [MS-OXCNOTIF](https://msdn.microsoft.com/en-us/library/cc463898(v=exchg.80).aspx)
 * [MS-OXCPERM](https://msdn.microsoft.com/en-us/library/cc463904(v=exchg.80).aspx)
 * [MS-OXCPRPT](https://msdn.microsoft.com/en-us/library/Cc425503(v=EXCHG.80).aspx)
 * [MS-OXCSTOR](https://msdn.microsoft.com/en-us/library/Cc433479(v=EXCHG.80).aspx)
 * [MS-OXCTABL](https://msdn.microsoft.com/en-us/library/cc433478(v=exchg.80).aspx)
 * [MS-OXORULE](https://msdn.microsoft.com/en-us/library/Cc463893(v=EXCHG.80).aspx)
 * [MS-OXCDATA](https://msdn.microsoft.com/en-us/library/cc425496(v=exchg.80).aspx)
 * [MS-OXPROPS](https://msdn.microsoft.com/en-us/library/cc433490(v=exchg.80).aspx)

