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

        /// <summary>
        ///  Browser initialize
        /// </summary>
        /// <param name="postfix">string value indicate address postfix</param>
        public static void Initialize(string postfix = "")
        {
            switch (ConfigurationManager.AppSettings["Browser"].ToLower())
            {
                case ("ie32"):
                    InternetExplorerOptions IEOption32 = new InternetExplorerOptions();
                    IEOption32.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    IEOption32.RequireWindowFocus = true;
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "") + @"\Drivers\IE32\", IEOption32);
                    break;
                case ("ie64"):
                    InternetExplorerOptions IEOption64 = new InternetExplorerOptions();
                    IEOption64.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    IEOption64.RequireWindowFocus = true;
                    webDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "") + @"\Drivers\IE64\", IEOption64);
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
            webDriver.Navigate().GoToUrl(address);
            checkAlert();
            signIncheckAlert();
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
        public static void Wait(By by)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(defaultWaitTime * 3));
            wait.Until(ExpectedConditions.ElementExists(by));
        }

        /// <summary>
        /// Set Wait time
        /// </summary>
        /// <param name="timeSpan">TimeSpan type time</param>
        public static void SetWaitTime(TimeSpan timeSpan)
        {
            webDriver.Manage().Timeouts().ImplicitlyWait(timeSpan);
            webDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(timeSpan.TotalSeconds * 2));
            webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(timeSpan.TotalSeconds * 2));
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
            catch (NoSuchElementException)
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
                (webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].click();", element);
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
                (webDriver as IJavaScriptExecutor).ExecuteScript("arguments[0].fireEvent('oncontextmenu');", element);
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
        public static void checkAlert()
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
        public static void signIncheckAlert()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = webDriver.SwitchTo().Alert();
                string username = ConfigurationManager.AppSettings["UserName"];
                string password = ConfigurationManager.AppSettings["Password"];
                alert.SetAuthenticationCredentials(username, password);
                alert.Accept();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}