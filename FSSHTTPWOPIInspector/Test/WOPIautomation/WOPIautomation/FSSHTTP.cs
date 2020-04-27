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
    public enum DocType { WORD, EXCEl };

    [TestClass]
    public class FSSHTTP : TestBase
    {
        private static string Word = ConfigurationManager.AppSettings["Word"];
        private static string wordFilename = Word.Split('\\').Last().Split('.').First();
        private static string excel = ConfigurationManager.AppSettings["Excel"];
        private static string excelFilename = excel.Split('\\').Last().Split('.').First();
        private string file = "";


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
            Utility.WordConflictMerge(wordFilename);
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

            // Discard check out on opening excel if a newer version of this file is available on the server.
            if (Utility.FindCondition(DocType.EXCEl,excelFilename, "A newer version of this file is available on the server."))
            {
                Utility.DiscardCheckOutOnOpeningExcel(DocType.EXCEl, excelFilename);
            }

            Thread.Sleep(1000);
            // Resolve 'UPLOAD FAILED'  
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "We're sorry, someone updated the server copy and it's not possible to upload your changes now."))
            {
                Utility.ResloveUploadFailed(excelFilename, false);
            }

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
            // Click 'Edit Workbook' button if we opened this workbook read-only from the server.
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "We opened this workbook read-only from the server."))
            {
                Utility.EditExcelWorkbook(excelFilename);
            }
            //Close FileInUsePane in Desktop Excel
            Utility.CloseExcelFileInUsePane(excelFilename);
            // Wait for CheckLockAvailability reqest show up.
            Thread.Sleep(100000);

            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "File Now Available"))
            {
                Utility.CloseExcelFileNowAvailable(excelFilename);
            }

            // Find 'READ-ONLY' close button.
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "This workbook is locked for editing by another user."))
            {
                Utility.CloseThisMessage();
            }

            // Close and release word process
            excelWorkbook.Close();
            excelToOpen.Quit();
            Marshal.ReleaseComObject(excelWorkbook);
            Marshal.ReleaseComObject(excelToOpen);

            // Delete the new upload document
            SharepointClient.DeleteFile(excelFilename + ".xlsx");
            Browser.Goto(Browser.DocumentAddress);

            bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(file);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Excel___CheckOutOnOpeningExcelTest()
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
        public void VersioningHistroy()
        {
            // Upload a excel document.
            SharepointClient.UploadFile(excel);
            // Refresh web address
            Browser.Goto(Browser.DocumentAddress);
            // Find document on site
            IWebElement document = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + excelFilename + ".xlsx']"));
            // Open it by desktop Excel.
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
            }
    
            // Wait for excel is opened            
            Utility.WaitForExcelDocumentOpenning2(excelFilename, true);

            Excel.Application excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Excel.Workbook excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            // Discard check out on opening excel if a newer version of this file is available on the server.
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "A newer version of this file is available on the server."))
            {
                Utility.DiscardCheckOutOnOpeningExcel(DocType.EXCEl,excelFilename);
            }

            // Click 'Edit Workbook' button if we opened this workbook read-only from the server.
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "We opened this workbook read-only from the server."))
            {
                Utility.EditExcelWorkbook(excelFilename);
            }

            Thread.Sleep(3000);

            // Edit Excel Cell Content.
            for (int i = 1; i < 2; i++)
                excelWorkSheet.Cells[i, 1] = DateTime.Now.ToString();

            // Close excel file.
            excelWorkbook.Save();
            excelWorkbook.Close();
            excelToOpen.Quit();

            // Open Excel File on Sharepoint Server again. Open it by Desktop Excel.

            Browser.RClick(document);
            Browser.Wait(By.LinkText("Open in Excel"));
            elementOpenInExcel = Browser.webDriver.FindElement(By.LinkText("Open in Excel"));
            Browser.Click(elementOpenInExcel);

            Thread.Sleep(6000);
            excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            excelWorkSheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            Thread.Sleep(6000);
            // Resolve 'UPLOAD FAILED'  
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "We're sorry, someone updated the server copy and it's not possible to upload your changes now."))
            {
                Utility.ResloveUploadFailed(excelFilename, false);
            }

            //Version History Restore
            Utility.VersionHistroyRestore(excelFilename);            

            // Click 'Edit Workbook' button if we opened this workbook read-only from the server.
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "We opened this workbook read-only from the server."))
            {
                Utility.EditExcelWorkbook(excelFilename);
            }

            // Close and release excel process      
            excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            Utility.DeleteDefaultExcelFormat();
            excelWorkbook.Close();
            excelToOpen.Quit();
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
        public void TestParserFile()
        {
            //bool result = FormatConvert.SaveSAZ(TestBase.testResultPath, testName, out file);
            //Assert.IsTrue(result, "The saz file should be saved successfully.");
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(@"C:\Users\plugdevuser19\Documents\Fiddler2\Captures\dump.saz");
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Excel___FlagExcelTest()
        {
            // Get EXCEL Process
            AutomationElement excel = Utility.GetExcelOnlineWindow("Excel");
            // Find 'File Now Available' window
            Condition Con_FileNowAvaiable= new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window), new PropertyCondition(AutomationElement.NameProperty, "File Now Available"));
            AutomationElement item_Con_ReadOnlyFileNowAvaiable = excel.FindFirst(TreeScope.Descendants, Con_FileNowAvaiable);
           
            // Click 'Cancel' in 'File Now Available' window
            if (item_Con_ReadOnlyFileNowAvaiable!=null)
            {
                Condition Con_Cancel = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Cancel"));
                AutomationElement item_Cancel = excel.FindFirst(TreeScope.Descendants, Con_Cancel);
                InvokePattern Pattern_Cancel = (InvokePattern)item_Cancel.GetCurrentPattern(InvokePattern.Pattern);
                Pattern_Cancel.Invoke();
            }

            // Find 'READ-ONLY' close button.
            if (Utility.FindCondition(DocType.EXCEl, excelFilename, "This workbook is locked for editing by another user."))
            {
                // Click 'Close this message' button.
                Condition Con_CloseThisMessage = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Close this message"));
                AutomationElement item_CloseThisMessage = excel.FindFirst(TreeScope.Descendants, Con_CloseThisMessage);
                if (item_CloseThisMessage != null)
                {
                    InvokePattern Pattern_CloseThisMessage = (InvokePattern)item_CloseThisMessage.GetCurrentPattern(InvokePattern.Pattern);
                    Pattern_CloseThisMessage.Invoke();
                }
            }
            Excel.Application excelToOpen = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Excel.Workbook excelWorkbook = (Excel.Workbook)excelToOpen.ActiveWorkbook;
            Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;
            
            excelWorkbook.Close();
            excelToOpen.Quit();
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Word___CheckOutFileTest()
        {
            // Get EXCEL Process
            Utility.CheckOutOnOpeningWord(wordFilename);
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Word___FlagTest()
        {
            //Assert.IsTrue(Utility.FindCondition(DocType.WORD, wordFilename, "Some of your changes conflict with other updates made to the file."));
            //Utility.WordConflictMerge_Yanfei(wordFilename);
            //Utility.WordSignInBanner(wordFilename);
            // Discard check out on opening word if a newer version of this file is available on the server.
            if (Utility.FindCondition(DocType.WORD, wordFilename, "A newer version of this file is available on the server."))
            {
                Utility.DiscardCheckOutOnOpeningExcel(DocType.WORD, wordFilename);
            }
        }

        [TestMethod, TestCategory("FSSHTTP")]
        public void Excel___TwoExcelWindowTest()
        {
            AutomationElement excelRestore =Utility.GetExcelRestoreWindow("Excel");
            Condition Con_Restore = new AndCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button), new PropertyCondition(AutomationElement.NameProperty, "Restore"));
            Condition Con_RestoreName = new PropertyCondition(AutomationElement.NameProperty, "Restore");
            AutomationElement item_Restore = excelRestore.FindFirst(TreeScope.Descendants, Con_RestoreName);
            InvokePattern Pattern_Restore = (InvokePattern)item_Restore.GetCurrentPattern(InvokePattern.Pattern);
            Pattern_Restore.Invoke();
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
        public void SuccessCoautherWithConflict()
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

            // Access the opening document using expected account            
            Utility.WaitForDocumentOpenning(wordFilename, false, true);
            string username = ConfigurationManager.AppSettings["UserName"];
            string password = ConfigurationManager.AppSettings["Password"];
            bool isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1000);              
            }
            Thread.Sleep(2000);
            isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
            if (isWindowsSecurityPop)
            {
                Utility.OfficeSignIn(username, password);
                Thread.Sleep(1500);
            }
            // Sign in if cached credentials have expired.
            if (Utility.FindCondition(DocType.WORD, wordFilename, "We can't upload or download your changes because your cached credentials have expired."))
            {
                Utility.WordSignInBanner(wordFilename);
                // Sign in if Windows Security pop up.
                isWindowsSecurityPop = Utility.WaitForDocumentOpenning(wordFilename, false, true);
                if (isWindowsSecurityPop)
                {
                    Utility.OfficeSignIn(username, password);
                    Thread.Sleep(1500);
                }

                // Discard check out on opening word if a newer version of this file is available on the server.
                if (Utility.FindCondition(DocType.WORD, wordFilename, "A newer version of this file is available on the server."))
                {
                    Utility.DiscardCheckOutOnOpeningExcel(DocType.WORD, wordFilename);
                } 
            }

            // Discard check out on opening word if a newer version of this file is available on the server.
            if (Utility.FindCondition(DocType.WORD, wordFilename, "A newer version of this file is available on the server."))
            {
                Utility.DiscardCheckOutOnOpeningExcel(DocType.WORD, wordFilename);
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
            
            Utility.WordConflictMerge(wordFilename);
            oDocument.Close();
            // Delete the defaut word empty format
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
            Utility.CheckOutOnOpeningWord(wordFilename);
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
            Utility.CheckOutOnOpeningWord(wordFilename);

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
            Utility.CheckOutOnOpeningWord(wordFilename);
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
