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
using Microsoft.CSharp;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium.Support.Events;
using System.Windows.Forms;

namespace WOPIautomation
{
    [TestClass]
    public class FSSHTTP:TestBase
    {
        private static string Word = ConfigurationManager.AppSettings["Word"];
        private static string filename = Word.Split('\\').Last().Split('.').First();
		private string file = "";
        
     
        [TestMethod, TestCategory("FSSHTTP")]
        public void CoautherWithoutConflict()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            // Open document by office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(filename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
            }
            // Wait for document is opened
            Utility.WaitForDocumentOpenning(filename);
            // Get the opened word process, and edit it
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertAfter("HelloWord");
            // Double click the document in root site 
            Browser.Click(document);
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            Thread.Sleep(10000);
            // Find and click "Edit Document" tab
            var editWord = Browser.FindElement(By.XPath("//a[@id='flyoutWordViewerEdit-Medium20']"), false);
            editWord.Click();
            // Find and click "Edit in Browser" tab
            Browser.Wait(By.Id("btnFlyoutEditOnWeb-Menu32"));
            var editInbrowser = Browser.webDriver.FindElement(By.XPath("//a[@id ='btnFlyoutEditOnWeb-Menu32']"));
            editInbrowser.Click();
            // Wait for document is opened
            Browser.Wait(By.XPath("//span[@id='BreadcrumbSaveStatus'][text()='Saved']"));
            oDocument.Save();
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            // Refresh web address
            Browser.Goto(Browser.BaseAddress);
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void CoautherWithConflict()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));            
            // Open document by office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(filename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
            }
            // Wait for document is opened
            Utility.WaitForDocumentOpenning(filename);
            // Get the opened word process, and edit it
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Thread.Sleep(1000);
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("HelloWordConfilict");
            // Click the document in root site 
            Browser.Click(document);
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            Thread.Sleep(20000);
            // Find and click "Edit Document" tab
            Browser.Wait(By.Id("flyoutWordViewerEdit-Medium20"));
            var editWord = Browser.FindElement(By.XPath("//a[@id='flyoutWordViewerEdit-Medium20']"), false);
            editWord.Click();
            // Find and click "Edit in Browser" tab
            var editInbrowser = Browser.webDriver.FindElement(By.XPath("//a[@id ='btnFlyoutEditOnWeb-Menu32']"));
            editInbrowser.Click();
            // Wait for document is opened
            Thread.Sleep(2000);
            Browser.Wait(By.XPath("//span[@id='BreadcrumbSaveStatus'][text()='Saved']"));
            // Edit it in online
            SendKeys.SendWait("HelloOfficeOnlineConflict");
            // Wait for online edit saved
            Thread.Sleep(2000);
            Browser.Wait(By.XPath("//span[@id='BreadcrumbSaveStatus'][text()='Saved']"));
            //saved = Browser.FindElement(By.XPath("//span[@id='BreadcrumbSaveStatus']"), false);
            Thread.Sleep(60000);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            Thread.Sleep(50000);
            // Save it in office word and close and release word process
            Utility.WordEditSave(filename);
            Thread.Sleep(10000);
            Utility.CloseMicrosoftWordDialog(filename, "OK");
            Utility.WordConflictMerge(filename);
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");

        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Schemalock()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            Browser.RClick(document);
            // Open document in Edit Word mode
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            Utility.WaitForDocumentOpenning(filename);
            // Update the document content
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("Schemalock");
            // Save and close document
            oDocument.Save();
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Exclusivelock()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            // Checkout the document
            SharepointClient.LockItem(filename + ".docx");
            // Open it in office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            // Wait for document is opened
            Utility.WaitForDocumentOpenning(filename);
            // Update the document content
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("Exclusivelock");
            // Save and close and release word process
            oDocument.Save();
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(filename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void SchemalockToExclusivelock()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            // Open it in office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            // Sign in office word and wait for it opening
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            Utility.WaitForDocumentOpenning(filename);
            // Update the document content
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("SchemalockToExclusivelock");
            // Save and close and release word process
            oDocument.Save();
            Utility.CheckOutOnOpeningWord(filename);
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(filename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void ExclusiveLockGetlock()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            // Open it in office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            Utility.WaitForDocumentOpenning(filename);
            // Check Out it from the info pag
            Utility.CheckOutOnOpeningWord(filename);
            // Update the document content
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("Exclusivelock");
            oDocument.Save();
            // Close the document
            Utility.CloseDocumentByUI(filename);
            Utility.CloseMicrosoftWordDialog(filename,"Yes");
            Utility.CloseCheckInPane(filename,true);
            // Go back to base address
            Browser.Goto(Browser.DocumentAddress);
            // Reopen document in office word
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementToOpen = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementToOpen);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            username = ConfigurationManager.AppSettings["UserName"];
            password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            Utility.WaitForDocumentOpenning(filename);
            // Edit it 
            wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("ExclusiveLockGetlock");
            // Save and close word process
            oDocument.Save();
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(filename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void SchemalockCheck()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            // Checked out it
            SharepointClient.LockItem(filename + ".docx");
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            // Sign in office word with another account and wait for it opening in readonly mode
            string username = ConfigurationManager.AppSettings["OtherUserName"];
            string password = ConfigurationManager.AppSettings["OtherPassword"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            Utility.CloseFileInUsePane(filename);
            Utility.WaitForDocumentOpenning(filename, true);
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            // Wait for CheckLockAvailability
            Thread.Sleep(60000);
            Utility.CloseFileNowAvailable(filename);
            // Close and release word process
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(filename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void ExclusivelockCheck()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" +filename+ ".docx']"));
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            Utility.OfficeSignIn(username, password);
            Utility.OfficeSignIn(username, password);
            Utility.WaitForDocumentOpenning(filename);
            // Check it out in info page
            Utility.CheckOutOnOpeningWord(filename);
            // Close word process
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            // Go back to base address
            Browser.Goto(Browser.DocumentAddress);
            // Reopen the document in word
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".docx']"));
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            // Close Microsoft office dialog and access using expected account
            Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(filename);
            username = ConfigurationManager.AppSettings["OtherUserName"];
            password = ConfigurationManager.AppSettings["OtherPassword"];
            Utility.OfficeSignIn(username, password);
            Utility.CloseFileInUsePane(filename);
            Utility.OfficeSignIn(username, password);
            Utility.WaitForDocumentOpenning(filename, true);
            
            wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            oDocument = (Word.Document)wordToOpen.ActiveDocument;
            // CheckLockAvailability
            Thread.Sleep(60000);
            // Close and release word process
            Utility.CloseFileNowAvailable(filename);
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(filename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }
    }
}
