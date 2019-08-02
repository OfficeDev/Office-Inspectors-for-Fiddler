using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.IE;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using OneNote= Microsoft.Office.Interop.OneNote;
using Microsoft.CSharp;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium.Support.Events;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Office.Interop.Graph;

namespace WOPIautomation
{
    [TestClass]
    public class ONESTORE: TestBase
    {            
		private string file = "";

        [TestMethod, TestCategory("ONESTORE")]
        public void CoauthorWithoutConflict()
        {
            string oneWithFileData = ConfigurationManager.AppSettings["OneWithFileData"];
            string oneWithoutFileData = ConfigurationManager.AppSettings["OneWithoutFileData"];
            Utility.OneNoteCoauthorWithoutConflict(oneWithFileData);
            Utility.OneNoteCoauthorWithoutConflict(oneWithoutFileData);
            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("ONESTORE")]
        public void CoauthorWithConflict()
        {
            string oneWithFileData = ConfigurationManager.AppSettings["OneWithFileData"];
            string oneWithoutFileData = ConfigurationManager.AppSettings["OneWithoutFileData"];
            Utility.OneNoteCoauthorWithConflict(oneWithFileData);
            Utility.OneNoteCoauthorWithConflict(oneWithoutFileData);
            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("ONESTORE")]
        public void AuthorEncryption()
        {
            string oneEncryption = ConfigurationManager.AppSettings["OneEncryption"];
            string filename = oneEncryption.Split('\\').Last().Split('.').First();
            // Upload a document
            SharepointClient.UploadFile(oneEncryption);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find onenote document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".one']"));
            // Click the document in root site 
            Browser.Click(document);
            // Open Encryption onenote file online in Browser.
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            Thread.Sleep(10000); 
            SendKeys.SendWait("{Enter}");
            Thread.Sleep(3000);
            SendKeys.SendWait("{Enter}");
            Thread.Sleep(5000);
            SendKeys.SendWait("Password01!");
            Thread.Sleep(5000);
            SendKeys.SendWait("{Enter}");            
            Thread.Sleep(5000);
            SendKeys.SendWait("Insert by onenote App~");
            Thread.Sleep(3000); 
            // Close Onenote App
            SendKeys.SendWait("%{f4}");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".one");
            
            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("ONESTORE")]
        public void CoauthorTableOfContents()
        {
            string oneWithFileData = ConfigurationManager.AppSettings["NotebookTableOfContents"];
            string filename = oneWithFileData.Split('\\').Last().Split('.').First();
            // Upload a document
            SharepointClient.UploadFile(oneWithFileData);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement onetoc2 = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".onetoc2']"));
            string DocumentWinHandle = Browser.webDriver.CurrentWindowHandle;
            
            // Open onetoc2 file in local Onenote App.
            Browser.RClick(onetoc2);
            Browser.Wait(By.LinkText("Open in OneNote"));
            var elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);
            Utility.WaitForOneNoteDocumentOpenning(filename, false, true);
            // Create a new section in local Onenote App.
            SendKeys.SendWait("^t");
            Thread.Sleep(8000);

            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find new create onenote section document on site
            IWebElement onenote = Browser.webDriver.FindElement(By.CssSelector("a[href*='New Section 1.one']"));
            Browser.Click(onenote);
            Thread.Sleep(4000);            
            SendKeys.SendWait("New Page");          
            //Thread.Sleep(10000);
            Thread.Sleep(3000);
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            // Wait for online edit saved
            Thread.Sleep(3000);          
            Browser.Wait(By.XPath("//a[@id='lblSyncStatus-Medium']/span[2][text()='Saving...']"));
            Thread.Sleep(10000);
            Browser.Wait(By.XPath("//a[@id='lblSyncStatus-Medium']/span[2][text()='Saved']"));
            Thread.Sleep(2000);

            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            Thread.Sleep(3000);
            onenote = Browser.webDriver.FindElement(By.CssSelector("a[href*='New Section 1.one']"));
            // Open OneNote document in local Onenote App
            Browser.RClick(onenote);
            Thread.Sleep(1000);
            Browser.Wait(By.LinkText("Open in OneNote"));
            elementOpenInOneNote = Browser.webDriver.FindElement(By.LinkText("Open in OneNote"));
            Browser.Click(elementOpenInOneNote);

            // Get the opened OneNote process, and read the page title.
            OneNote.Application oneNoteApp = new OneNote.Application();
            string oneNoteXml;
            var oneNoteWindow = oneNoteApp.Windows.CurrentWindow;
            oneNoteApp.GetPageContent(oneNoteWindow.CurrentPageId, out oneNoteXml);
            var xmlDoc = new XmlDocument();            
            xmlDoc.LoadXml(oneNoteXml);
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("one", "http://schemas.microsoft.com/office/onenote/2013/onenote");
            string titleXpath = "//one:Page/one:Title/one:OE/one:T"; 
            System.Xml.XmlCDataSection titleNode = xmlDoc.SelectSingleNode(titleXpath, nsmgr).FirstChild as System.Xml.XmlCDataSection;            
            // If its title in local Onenote App is not updated and wait.
            while (!titleNode.Value.Contains("New Page"))
            {
                Thread.Sleep(5000);
                oneNoteWindow = oneNoteApp.Windows.CurrentWindow;
                oneNoteApp.GetPageContent(oneNoteWindow.CurrentPageId, out oneNoteXml);
                xmlDoc.LoadXml(oneNoteXml);
                titleNode = xmlDoc.SelectSingleNode(titleXpath, nsmgr).FirstChild as System.Xml.XmlCDataSection;
            }

            // Closed OneNote App.            
            oneNoteApp.Windows.CurrentWindow.Active = true;
            SendKeys.SendWait("%{F4}");
            // Delete the new created section document
            SharepointClient.DeleteFile("New Section 1" + ".one");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".onetoc2");
            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }
    }
}
