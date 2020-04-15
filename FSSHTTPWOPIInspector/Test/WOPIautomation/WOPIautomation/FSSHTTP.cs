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
using System.Windows.Automation;

namespace WOPIautomation
{
    [TestClass]
    public class FSSHTTP:TestBase
    {
        private static string Word = ConfigurationManager.AppSettings["Word"];
        private static string wordFilename = Word.Split('\\').Last().Split('.').First();
        private static string excel = ConfigurationManager.AppSettings["Excel"];
        private static string excelFilename = excel.Split('\\').Last().Split('.').First();
        private string file = "";
        
     
        [TestMethod, TestCategory("FSSHTTP")]
        public void CoautherWithoutConflict()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            // Open document by office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            
            // Close Microsoft office dialog and access using expected account            
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
                Utility.OfficeSignIn(username, password);
            }

            // Wait for document is opened
            Utility.WaitForDocumentOpenning(wordFilename);
            // Get the opened word process, and edit it
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertAfter("HelloWord");
            // Double click the document in root site 
            Browser.Click(document);
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            Thread.Sleep(3000);
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
            SharepointClient.DeleteFile(wordFilename + ".docx");

            bool result = FormatConvert.SaveSAZ(testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Excel___ExclusivelockCheck()
        {
            // Upload a document
            SharepointClient.UploadFile(excel);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + excelFilename + ".xlsx']"));
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Excel"));
            var elementOpenInExcel = Browser.webDriver.FindElement(By.LinkText("Open in Excel"));
            Browser.Click(elementOpenInExcel);


            // Sign in Excel Desktop App.
            Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1500);                
            }
            //Waiting for WindowsSecurity Pop up
            //Thread.Sleep(1000);
            isWindowsSecurityPop = Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1500);
                //Utility.OfficeSignIn(username, password);
            }

            // Wait for excel is opened
            // Sign in Excel Desktop App.
            Utility.WaitForExcelDocumentOpenning2(excelFilename, false);

            // Go back to base address
            Browser.Goto(Browser.DocumentAddress);
            // Reopen the document in word
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + excelFilename + ".xlsx']"));
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Excel Online"));
            var elementOpenOnline = Browser.webDriver.FindElement(By.LinkText("Open in Excel Online"));
            Browser.Click(elementOpenOnline);
            // Sign in Excel Desktop App use UserName.
            Thread.Sleep(1000);
            Excel.Application excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Excel.Workbook excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            //Click 'Edit Workbook'
            Utility.EditWorkbook(excelFilename);
            //Close FileInUsePane in Desktop Excel
            Utility.CloseExcelFileInUsePane(excelFilename);
            // Wait for CheckLockAvailability reqest show up.
            Thread.Sleep(100000);
            // Close and release word process
            excelWorkbook.Close();
            Utility.DeleteDefaultExcelFormat();
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelToOpen);
            
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");
            SharepointClient.DeleteFile(excelFilename + ".xlsx");
            Browser.Goto(Browser.DocumentAddress);

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Excel___Versioning()
        {
            // Upload a document
            SharepointClient.UploadFile(excel);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + excelFilename + ".xlsx']"));
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Excel"));
            var elementOpenInExcel = Browser.webDriver.FindElement(By.LinkText("Open in Excel"));
            Browser.Click(elementOpenInExcel);


            // Sign in Excel Desktop App.
            Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1500);
            }
            //Waiting for WindowsSecurity Pop up
            //Thread.Sleep(1000);
            isWindowsSecurityPop = Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1500);
                //Utility.OfficeSignIn(username, password);
            }

            // Wait for excel is opened
            // Sign in Excel Desktop App.
            Utility.WaitForExcelDocumentOpenning2(excelFilename, false);

            // Go back to base address
            Browser.Goto(Browser.DocumentAddress);
            // Reopen the document in word
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + excelFilename + ".xlsx']"));
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Excel Online"));
            elementOpenInExcel = Browser.webDriver.FindElement(By.LinkText("Open in Excel Online"));
            Browser.Click(elementOpenInExcel);
            // Sign in Excel Desktop App use UserName.
            Thread.Sleep(1000);
            Excel.Application excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Excel.Workbook excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            //Click 'Edit Workbook'
            Utility.EditWorkbook(excelFilename);
            //Close FileInUsePane in Desktop Excel
            Utility.CloseExcelFileInUsePane(excelFilename);
            // Wait for CheckLockAvailability reqest show up.
            Thread.Sleep(100000);
            // Close and release word process
            excelWorkbook.Close();
            Utility.DeleteDefaultExcelFormat();
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelToOpen);

            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");
            SharepointClient.DeleteFile(excelFilename + ".xlsx");
            Browser.Goto(Browser.DocumentAddress);

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void VersioningHistroyTest()
        {
            Excel.Application excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Excel.Workbook excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            Utility.VersionHistroy(excelFilename);
            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void CheckOutOnOpeningExcelTest()
        {
            // Upload a document
            SharepointClient.UploadFile(excel);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + excelFilename + ".xlsx']"));
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Excel"));
            var elementOpenInExcel = Browser.webDriver.FindElement(By.LinkText("Open in Excel"));
            Browser.Click(elementOpenInExcel);

            // Sign in Excel Desktop App.
            Utility.WaitForExcelDocumentOpenning2(excelFilename, true);
            Excel.Application excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Excel.Workbook excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            Utility.CheckOutOnOpeningExcel(excelFilename);

            // Close and release word process
            excelWorkbook.Close();
            Utility.CloseMicrosoftOfficeDialog();
            Utility.DeleteDefaultExcelFormat();
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelToOpen);

            // Delete the new upload document            
            SharepointClient.DeleteFile(excelFilename + ".xlsx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }
        

        [TestMethod, TestCategory("FSSHTTP")]

        public void Excel___SharepointDelete()
        {   
            // Upload a document
            SharepointClient.UploadFile(Word);            // Upload a document
            SharepointClient.UploadFile(excel);            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            SharepointClient.DeleteFile(wordFilename + ".docx");
            Browser.Goto(Browser.DocumentAddress);
            SharepointClient.DeleteFile(excelFilename + ".xlsx");
            Browser.Goto(Browser.DocumentAddress);
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void CoautherWithConflict()
        {
            // Upload a document
            SharepointClient.UploadFile(Word);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));            
            // Open document by office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);

            // Close Microsoft office dialog and access using expected account            
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];            
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
                Utility.OfficeSignIn(username, password);
            }

            // Wait for document is opened
            Utility.WaitForDocumentOpenning(wordFilename);
            // Get the opened word process, and edit it
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Thread.Sleep(1000);
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;            
            oDocument.Content.InsertBefore("HelloWordConfilict");
            // Click the document in root site 
            Browser.Click(document);
            Browser.Wait(By.Id("WebApplicationFrame"));
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            Thread.Sleep(2000);
            // Find and click "Edit Document" tab
            Browser.Wait(By.Id("flyoutWordViewerEdit-Medium20"));
            var editWord = Browser.FindElement(By.XPath("//a[@id='flyoutWordViewerEdit-Medium20']"), false);
            editWord.Click();
            // Find and click "Edit in Browser" tab
            var editInbrowser = Browser.webDriver.FindElement(By.XPath("//a[@id ='btnFlyoutEditOnWeb-Menu32']"));
            editInbrowser.Click();
            // Wait for document is opened
            Thread.Sleep(4000);
            Browser.Wait(By.XPath("//span[@id='BreadcrumbSaveStatus'][text()='Saved']"));
            Thread.Sleep(2000);
            // Edit it in online
            SendKeys.SendWait("HelloOfficeOnlineConflict");
            // Wait for online edit saved
            Thread.Sleep(3000);
            Browser.Wait(By.XPath("//span[@id='BreadcrumbSaveStatus'][text()='Saved']"));
            //saved = Browser.FindElement(By.XPath("//span[@id='BreadcrumbSaveStatus']"), false);
            //Thread.Sleep(6000);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            Thread.Sleep(2000);
            // Save it in office word and close and release word process
            Utility.WordEditSave(wordFilename);
            Thread.Sleep(3000);
            Utility.CloseMicrosoftWordDialog(wordFilename, "OK");
            //Utility.WordConflictMerge(filename);
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");

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
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            Browser.RClick(document);
            // Open document in Edit Word mode
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);           

            // Sign in Word App.
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
                Utility.OfficeSignIn(username, password);
            }
            Utility.WaitForDocumentOpenning(wordFilename);

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
            SharepointClient.DeleteFile(wordFilename + ".docx");

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
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            // Checkout the document
            SharepointClient.LockItem(wordFilename + ".docx");
            // Open it in office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            
            // Sign in Word App.
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(2000);
                Utility.OfficeSignIn(username, password);
            }

            // Wait for document is opened
            Utility.WaitForDocumentOpenning(wordFilename);
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
            SharepointClient.UnLockItem(wordFilename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");

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
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            // Open it in office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);

            // Sign in Microsoft office dialog and access using expected account            
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
                Utility.OfficeSignIn(username, password);
            }

            // Update the document content
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("SchemalockToExclusivelock");
            // Save and close and release word process
            oDocument.Save();
            // CheckOutOnOpeningWord
            //Utility.CheckOutOnOpeningWord(filename);
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(wordFilename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");

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
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            // Open it in office word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);

            // Sign in Microsoft office dialog and access using expected account            
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
                Utility.OfficeSignIn(username, password);
            }

            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            // Check Out it from the info page
            // Manual check out.Utility.CheckOutOnOpeningWord function need to be upated,
            //Utility.CheckOutOnOpeningWord(filename);

            // Update the document content
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Content.InsertBefore("Exclusivelock");
            oDocument.Save();
            // Close the document
            Utility.CloseDocumentByUI(wordFilename);
            Utility.CloseMicrosoftWordDialog(wordFilename, "Yes");
            Utility.CloseCheckInPane(wordFilename, true);
            // Go back to base address
            Browser.Goto(Browser.DocumentAddress);
            // Reopen document in office word
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementToOpen = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementToOpen);

            // Close Microsoft office dialog and access using expected account            
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(2000);
                Utility.OfficeSignIn(username, password);
            }

            // Wait for document is opened
            Utility.WaitForDocumentOpenning(wordFilename);
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
            SharepointClient.UnLockItem(wordFilename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");

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
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            // Checked out it
            SharepointClient.LockItem(wordFilename + ".docx");
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);
            //Utility.CloseMicrosoftOfficeDialog();
            Utility.WaitForDocumentOpenning(wordFilename);
            // Sign in office word with another account and wait for it opening in readonly mode
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["OtherUserName"];
            string password = ConfigurationManager.AppSettings["OtherPassword"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
            
            }
          
            Utility.CloseFileInUsePane(wordFilename);            
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            // Wait for CheckLockAvailability
            Thread.Sleep(3000);
            Utility.CloseFileNowAvailable(wordFilename);
            // Close and release word process
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(wordFilename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");

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
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            // Open it by word
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            var elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);


            // Sign in Word App.
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);
                Utility.OfficeSignIn(username, password);
            }

            // Wait for document is opened
            // Sign in Word App.
            Utility.WaitForDocumentOpenning(wordFilename);            
              
            // Check it out in info page
            //Utility.CheckOutOnOpeningWord(filename);
            // Close word process
            Word.Application wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            Word.Document oDocument = (Word.Document)wordToOpen.ActiveDocument;
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();

            // Go back to base address
            Browser.Goto(Browser.DocumentAddress);
            // Reopen the document in word
            document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + wordFilename + ".docx']"));
            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Word"));
            elementOpenInWord = Browser.webDriver.FindElement(By.LinkText("Open in Word"));
            Browser.Click(elementOpenInWord);

            // Sign in Word App use OtherUserName.
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            /*username = ConfigurationManager.AppSettings["OtherUserName"];
            password = ConfigurationManager.AppSettings["OtherPassword"];
            isWindowsSecurityPop = Utility.WaitForDocumentOpenning(filename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(2000);
                Utility.OfficeSignIn(username, password);
            }*/

            Utility.CloseFileInUsePane(wordFilename);
         
            
            wordToOpen = (Word.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application");
            oDocument = (Word.Document)wordToOpen.ActiveDocument;            // CheckLockAvailability
            Thread.Sleep(6000);
            // Close and release word process
            Utility.CloseFileNowAvailable(wordFilename);
            oDocument.Close();
            Utility.DeleteDefaultWordFormat();
            Marshal.ReleaseComObject(oDocument);
            Marshal.ReleaseComObject(wordToOpen);
            SharepointClient.UnLockItem(wordFilename + ".docx");
            // Delete the new upload document
            SharepointClient.DeleteFile(wordFilename + ".docx");

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }
    }
}
