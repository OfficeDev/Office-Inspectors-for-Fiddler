using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Interactions;

namespace WOPIautomation
{
    public static class Browser
    {
        internal static IWebDriver webDriver;
        //static string defaultTitle;
        static string defaultHandle;
        //internal static string homeTitle;
        static int defaultWaitTime;
        
        /// <summary>
        /// Get base address
        /// </summary>
        public static string BaseAddress
        {
            get
            {
                string address = ConfigurationManager.AppSettings["BaseAddress"];
                return address.EndsWith("/") ? address.Substring(0, address.Length - 1) : address;
            }
        }

        public static string DocumentAddress
        {
            get
            {
                string address = ConfigurationManager.AppSettings["DocumentAddress"];
                return address.EndsWith("/") ? address.Substring(0, address.Length - 1) : address;
            }
        }

        /// <summary>
        ///  Browser initialize
        /// </summary>
        /// <param name="postfix">string value indicate address postfix</param>
        public static void Initialize(string postfix = "Shared%20Documents/Forms/AllItems.aspx")
        {
            switch (ConfigurationManager.AppSettings["Browser"].ToLower())
            {
                case ("ie32"):
                    InternetExplorerOptions IEOption32 = new InternetExplorerOptions();
                    IEOption32.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    IEOption32.RequireWindowFocus = true;
                    IEOption32.PageLoadStrategy = InternetExplorerPageLoadStrategy.Normal;
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "") + @"\Drivers\IE32\", IEOption32, TimeSpan.FromSeconds(60));                  
                    break;
                case ("ie64"):
                    InternetExplorerOptions IEOption64 = new InternetExplorerOptions();
                    IEOption64.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    IEOption64.RequireWindowFocus = true;
                    IEOption64.PageLoadStrategy = InternetExplorerPageLoadStrategy.Normal;
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "") + @"\Drivers\IE64\", IEOption64, TimeSpan.FromSeconds(60));
                    break;
                default:
                    break;
            }
            defaultHandle = webDriver.CurrentWindowHandle;
            defaultWaitTime = int.Parse(ConfigurationManager.AppSettings["DefaultWaitTime"]);
            defaultHandle = webDriver.CurrentWindowHandle;
            SetWaitTime(TimeSpan.FromSeconds(defaultWaitTime));
            string address = BaseAddress;
            if (postfix != "")
            {
                address = BaseAddress + "/" + postfix;
            }
            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl(address);
            SignIncheckAlert();
            SwitchToClassicMode();
        }

        public static void SwitchToClassicMode()
        {
            bool isNewMode = false;
            if (Browser.Wait(By.XPath("//div[2]/div/div/div/a/span")))
            {
                System.Threading.Thread.Sleep(3000);

                var element = Browser.webDriver.FindElement(By.XPath("//div[2]/div/div/div/a/span"));
                if (element.Text.Equals("Return to classic SharePoint"))
                {
                    isNewMode = true;
                }
            }

            if (isNewMode)
            {
                Browser.Wait(By.Id("O365_MainLink_Settings"));
                var settings = Browser.webDriver.FindElement(By.Id("O365_MainLink_Settings"));
                Browser.Click(settings);
                Browser.Wait(By.Id("O365_SubLink_SuiteMenu_LibrarySettings"));
                settings = Browser.webDriver.FindElement(By.Id("O365_SubLink_SuiteMenu_LibrarySettings"));
                Browser.Click(settings);
                Browser.Wait(By.LinkText("Advanced settings"));
                settings = Browser.webDriver.FindElement(By.LinkText("Advanced settings"));
                Browser.Click(settings);
                Browser.Wait(By.Id("ctl00_PlaceHolderMain_ContentTypeSection_ctl02_RadEnableContentTypesYes"));
                ((IJavaScriptExecutor)Browser.webDriver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                var classicMode = Browser.webDriver.FindElement(By.Id("ctl00_PlaceHolderMain_ListExperienceSection_ctl02_RadDisplayOnClassicExperience"));
                Browser.Click(classicMode);
                var ok = Browser.webDriver.FindElement(By.Id("ctl00_PlaceHolderMain_ctl00_RptControls_BtnSaveAsTemplate"));
                Browser.Click(ok);
                Browser.Goto(Browser.DocumentAddress);
            }
        }

        /// <summary>
        /// Goto method
        /// </summary>
        /// <param name="url">Url to goes to</param>
        public static void Goto(string url)
        {
            try
            {
                webDriver.Navigate().GoToUrl(url);
            }
            catch (WebDriverException)
            {
                webDriver.Navigate().Refresh();
                Wait(TimeSpan.FromSeconds(5));
            }
        }

        /// <summary>
        /// Webdriver page Title
        /// </summary>
        public static string Title
        {
            get { return webDriver.Title; }
        }

        /// <summary>
        /// Weddriver url
        /// </summary>
        public static string Url
        {
            get { return webDriver.Url; }
        }
        /// <summary>
        /// Close webdriver
        /// </summary>
        public static void Close()
        {
            webDriver.Quit();
        }

        /// <summary>
        /// default wait method
        /// </summary>
        /// <param name="timeSpan"></param>
        public static void Wait(TimeSpan timeSpan)
        {
            Thread.Sleep((int)timeSpan.TotalSeconds * 1000);
        }

        /// <summary>
        /// Condition wait method
        /// </summary>
        /// <param name="by">Indicate which is used to find element</param>
        public static bool Wait(By by)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(defaultWaitTime * 3));
                wait.Until(ExpectedConditions.ElementExists(by));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Set Wait time
        /// </summary>
        /// <param name="timeSpan">TimeSpan type time</param>
        public static void SetWaitTime(TimeSpan timeSpan)
        {
            webDriver.Manage().Timeouts().ImplicitlyWait(timeSpan);
            webDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(timeSpan.TotalSeconds * 3));
            webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(timeSpan.TotalSeconds * 3));
       
        }

        /// <summary>
        /// Find Element
        /// </summary>
        /// <param name="by">Indicate which is used to find element</param>
        /// <param name="isRootElement">Indicate whether the element is a root element</param>
        /// <returns></returns>
        public static IWebElement FindElement(By by, bool isRootElement = true)
        {
            try
            {
                return webDriver.FindElement(by);
            }
            catch
            {
                IList<IWebElement> frames = webDriver.FindElements(By.TagName("iframe"));
                IWebElement element = null;
                if (frames.Count > 0)
                {
                    for (int i = 0; i < frames.Count; i++)
                    {
                        webDriver.SwitchTo().Frame(frames[i]);
                        element = FindElement(by, false);
                        if (element != null)
                        {
                            if (isRootElement)
                            {
                                webDriver.SwitchTo().DefaultContent();
                            }

                            return element;
                        }
                    }
                }
                
                if (element == null)
                {
                    webDriver.SwitchTo().ParentFrame();
                }

                return element;
            }
        }

        /// <summary>
        /// Click method
        /// </summary>
        /// <param name="element">element which is do click</param>
        internal static void Click(IWebElement element)
        {
            try
            {
                OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(webDriver);
                action.Click(element).Perform();
            }
            catch (WebDriverException)
            {
                Wait(TimeSpan.FromSeconds(15));
            }
        }

        /// <summary>
        /// Right click method
        /// </summary>
        /// <param name="element">element which is do right click</param>
        internal static void RClick(IWebElement element)
        {
            try
            {
                OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(webDriver);
                action.ContextClick(element).Perform();
            }
            catch (WebDriverException)
            {
                Wait(TimeSpan.FromSeconds(60));
            }
        }


        internal static void MovetoElement(IWebElement element)
        {
            try
            {
                OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(webDriver);
                action.MoveToElement(element).Perform();
            }
            catch (WebDriverException)
            {
                Wait(TimeSpan.FromSeconds(15));
            }
        }
        /// <summary>
        /// Find an iframe element
        /// </summary>
        /// <param name="frameIdOrName">Id or name of the iframe</param>
        /// <returns>The found iframe</returns>
        private static IWebElement FindFrame(string frameIdOrName)
        {
            IList<IWebElement> frames = webDriver.FindElements(By.TagName("iframe"));
            IWebElement frame = null;
            if (frames.Count > 0)
            {
                for (int i = 0; i < frames.Count; i++)
                {
                    if (frames[i].GetAttribute("id") == frameIdOrName || frames[i].GetAttribute("name") == frameIdOrName)
                    {
                        frame = frames[i];
                        return frame;
                    }
                    else
                    {
                        webDriver.SwitchTo().Frame(frames[i]);
                        frame = FindFrame(frameIdOrName);
                        if (frame != null)
                        {
                            return frame;
                        }
                    }
                }
            }

            if (frame == null)
            {
                webDriver.SwitchTo().ParentFrame();
            }

            return frame;
        }

        /// <summary>
        /// Check alert method
        /// </summary>
        public static void CheckAlert()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
                wait.Until(ExpectedConditions.AlertIsPresent());//ExpectedConditions.AlertIsPresent() 
                IAlert alert = webDriver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
       

        /// <summary>
        /// Check a Alert with sign in
        /// </summary>
        public static void SignIncheckAlert()
        {
            try
            {
                string username = ConfigurationManager.AppSettings["UserName"];
                string password = ConfigurationManager.AppSettings["Password"];
                Utility.SigninWindowsSecurity(username, password);
            }
            catch (Exception e)
            {
                throw e;
            }            
        }
    }
}