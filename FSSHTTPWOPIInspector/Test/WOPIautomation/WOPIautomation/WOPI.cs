using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.CSharp;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.SharePoint.Client;
using System.Security;
using System.Windows.Forms;
using System.Drawing;
namespace WOPIautomation
{
    [TestClass]
    public class WOPI : TestBase
    {
        private static string Excel = ConfigurationManager.AppSettings["Excel"];
        private static string filename = Excel.Split('\\').Last().Split('.').First();

      [TestMethod, TestCategory("WOPI")]
        public void SaveAsCopyAndRename()
        {
            // Upload a document
            SharepointClient.UploadFile(Excel);
            // Refresh web address
            Browser.Goto(Browser.BaseAddress);
            // Find and open document
            Browser.Wait(By.CssSelector("a[href*='" + filename + ".xlsx']"));
            var Docment = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".xlsx']"));
            Browser.Click(Docment);
            // Find and click "Edit WorkBook" tab
            var editExcel = Browser.FindElement(By.XPath("//a[@id='m_excelWebRenderer_ewaCtl_flyoutExcelEdit-Medium20']"), false);
            editExcel.SendKeys(OpenQA.Selenium.Keys.Enter);
            //SendKeys.SendWait("Enter");
            // Find and click "Edit in Browser" tab
            var editInbrowser = Browser.webDriver.FindElement(By.XPath("//a[@id = 'm_excelWebRenderer_ewaCtl_btnEditInWebApp-Menu32']"));
            editInbrowser.SendKeys(OpenQA.Selenium.Keys.Enter);
            //SendKeys.SendWait("Enter");
            // Click the title of the online excel and renamed it by add "new"
            var rename = Browser.webDriver.FindElement(By.XPath("//div[@id='BreadcrumbTitle']"));
            (Browser.webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].innerHTML = arguments[1];", rename, "new" + rename.Text + "\r");
            Actions action = new Actions(Browser.webDriver);
            action.Click(rename).Build().Perform();
            rename.SendKeys(OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(8000);
            // Renamed it back to the original name
            (Browser.webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].innerHTML = arguments[1];", rename, rename.Text.Substring(3) + "\r");
            action.Click(rename).Build().Perform();
            rename.SendKeys(OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(8000);
            // Back to base address
            Browser.Goto(Browser.BaseAddress);
            Thread.Sleep(3000);
            Docment = Browser.webDriver.FindElement(By.CssSelector("a[href*='" + filename + ".xlsx']"));
            Browser.Click(Docment);
            // Find and click tab ...
            var elementInframe = Browser.FindElement(By.XPath("//a[@id='m_excelWebRenderer_ewaCtl_ExcelViewerHeroDockOverflowMenuLauncher-Small20']"), false);
            elementInframe.SendKeys(OpenQA.Selenium.Keys.Enter);
            //SendKeys.SendWait("Enter");
            // Find and click SaveAsCopy tab
            var saveacopy = Browser.webDriver.FindElement(By.XPath("//a[@id='m_excelWebRenderer_ewaCtl_Jewel.SaveACopy-Menu20']"));
            saveacopy.SendKeys(OpenQA.Selenium.Keys.Enter);
            //SendKeys.SendWait("Enter");
            // Input a name for the new copy document
            var saveAs = Browser.webDriver.FindElement(By.XPath("//input[@id='workbookName']"));
            saveAs.SendKeys("Copy" + saveAs.Text);
            // Click save button
            var save = Browser.webDriver.FindElement(By.XPath("//button[@type='submit']"));
            save.SendKeys(OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(8000);
            // Back to base address
            Browser.Goto(Browser.BaseAddress);
            // Delete the new created copy document
            SharepointClient.DeleteFile("Copy" + filename + ".xlsx");
            // Delete the new upload document
            SharepointClient.DeleteFile(filename + ".xlsx");

            StopTrace();
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(captureName);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }
        [TestMethod, TestCategory("WOPI")]
        public void NewFolder()
        {
            // Click the New button to create a new folder
            Browser.Wait(By.XPath("//button[@id='QCB1_Button1']"));
            var newButton = Browser.webDriver.FindElement(By.XPath("//button[@id='QCB1_Button1']"));
            Browser.Click(newButton);
            // Click "New Folder" button in popup window
            var newFolder = Browser.webDriver.FindElement(By.XPath("//a[@id='js-newdocWOPI-divFolder-WPQ4']"));
            Browser.Click(newFolder);
            Thread.Sleep(3000);
            // Enter a name for new folder
            var foldername = Browser.webDriver.FindElement(By.XPath("//input[@id='ccfc_folderNameInput_0_onetidIOFile']"));
            (Browser.webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].setAttribute('value',arguments[1])", foldername,"NewFolder");
            // Click "Create" button in new folder dialog window
            var create = Browser.webDriver.FindElement(By.XPath("//input[@id='csfd_createButton_toolBarTbl_RightRptControls_diidIOSaveItem']"));
            Browser.Click(create);
            Thread.Sleep(1000);
            // Open the new created folder
            var folder = Browser.webDriver.FindElement(By.XPath("//a[text()='NewFolder']"));
            Browser.Click(folder);
            Thread.Sleep(1000);
            // Click the "New" button to create a new note in folder
            Browser.Wait(By.XPath("//button[@id='QCB1_Button1']"));
            var newButton_infolder = Browser.webDriver.FindElement(By.XPath("//button[@id='QCB1_Button1']"));
            Browser.Click(newButton_infolder);
            // Select "New OneNote"
            var newOneNote = Browser.webDriver.FindElement(By.XPath("//a[@id='js-newdocWOPI-divOneNote-WPQ4']"));
            Browser.Click(newOneNote);
            Thread.Sleep(1000);
            // Switch to new OneNote dialog frame
            var frameSrc = Browser.webDriver.FindElement(By.CssSelector("[src*='/_layouts/15/CreateNewDocument.aspx?SaveLocation=%2FShared%20Documents%2FNewFolder']"));
            Browser.webDriver.SwitchTo().Frame(frameSrc);
            // Enter a name for OneNote
            var OneNoteName = Browser.webDriver.FindElement(By.XPath("//input[@id='ctl00_PlaceHolderMain_ctl00_ctl01_textBoxFileName']"));
            (Browser.webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].setAttribute('value',arguments[1])", OneNoteName, "NewNote");
            var OneNoteOk = Browser.webDriver.FindElement(By.XPath("//input[@id='ctl00_PlaceHolderMain_buttonSectionMain_RptControls_buttonOK']"));
            Browser.Click(OneNoteOk);
            // Switch to oneNote frame
            Browser.webDriver.SwitchTo().Frame("WebApplicationFrame");
            Thread.Sleep(10000);
            // Click navigation button
            Browser.Wait(By.XPath("//a[@id='NavigationViewExpandButton']"));
            var navigationView = Browser.webDriver.FindElement(By.XPath("//a[@id='NavigationViewExpandButton']"));
            Browser.Click(navigationView);
            // Click new session button
            var newsession = Browser.webDriver.FindElement(By.XPath("//div[@id='NewSectionButton']/a"));
            Browser.Click(newsession);
            var sessionCancel = Browser.webDriver.FindElement(By.XPath("//button[@id='WACDialogCancelButton'][text()='Cancel']"));
            Browser.Click(sessionCancel);
            // Back to base address
            Browser.Goto(Browser.BaseAddress);
            // Delete the new created folder
            SharepointClient.DeleteFolder("NewFolder");

            StopTrace();
            bool parsingResult = MessageParser.ParseMessageUsingWOPIInspector(captureName);
            Assert.IsTrue(parsingResult, "Case failed, check the details information in error.txt file.");
        }
    }
}
