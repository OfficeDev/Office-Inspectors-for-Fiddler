using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Management.Automation;

namespace WOPIautomation
{
    public class SharepointClient
    {
        /// <summary>
        /// Lock a file
        /// </summary>
        /// <param name="name">file name</param>
        public static void LockItem(string filename)
        {
            ClientContext clientContext = new ClientContext(Browser.BaseAddress);
            List spList = clientContext.Web.Lists.GetByTitle("Documents");
            Microsoft.SharePoint.Client.CamlQuery query = new Microsoft.SharePoint.Client.CamlQuery();
            query.ViewXml = "<View>"
               + "<Query>"
               + "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>" + filename + "</Value></Eq></Where>"
               + "</Query>"
               + "</View>";
            // execute the query                
            ListItemCollection listItems = spList.GetItems(query);
            clientContext.Load(listItems);
            clientContext.ExecuteQuery();
            foreach (Microsoft.SharePoint.Client.ListItem listitem in listItems)
            {
                listitem.File.CheckOut();
                try { clientContext.ExecuteQuery(); }
                catch
                {
                    Thread.Sleep(4000);
                    clientContext.ExecuteQuery();
                }
            }
        }

        /// <summary>
        /// UnLock a file
        /// </summary>
        /// <param name="name">file name</param>
        public static void UnLockItem(string filename)
        {
            ClientContext clientContext = new ClientContext(Browser.BaseAddress);
            List spList = clientContext.Web.Lists.GetByTitle("Documents");
            Microsoft.SharePoint.Client.CamlQuery query = new Microsoft.SharePoint.Client.CamlQuery();
            query.ViewXml = "<View>"
               + "<Query>"
               + "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>" + filename + "</Value></Eq></Where>"
               + "</Query>"
               + "</View>";
            // execute the query                
            ListItemCollection listItems = spList.GetItems(query);
            clientContext.Load(listItems);
            clientContext.ExecuteQuery();
            foreach (Microsoft.SharePoint.Client.ListItem listitem in listItems)
            {
                listitem.File.UndoCheckOut();
                try { clientContext.ExecuteQuery(); }
                catch
                {
                    Thread.Sleep(4000);
                    clientContext.ExecuteQuery();
                }
            }
        }

        /// <summary>
        /// UnLock a file
        /// </summary>
        /// <param name="name">file name</param>
        public static void UpdateItem(string filename)
        {
            ClientContext clientContext = new ClientContext(Browser.BaseAddress);
            List spList = clientContext.Web.Lists.GetByTitle("Documents");
            Microsoft.SharePoint.Client.CamlQuery query = new Microsoft.SharePoint.Client.CamlQuery();
            query.ViewXml = "<View>"
               + "<Query>"
               + "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>" + filename + "</Value></Eq></Where>"
               + "</Query>"
               + "</View>";
            // execute the query                
            ListItemCollection listItems = spList.GetItems(query);
            clientContext.Load(listItems);
            clientContext.ExecuteQuery();
            foreach (Microsoft.SharePoint.Client.ListItem listitem in listItems)
            {
                listitem.File.Publish("up");
                clientContext.ExecuteQuery();
            }
        }

        /// <summary>
        /// Delete a file on site
        /// </summary>
        /// <param name="filename">file name</param>
        public static void DeleteFile(string filename)
        {
            ClientContext clientContext = new ClientContext(Browser.BaseAddress);
            List spList = clientContext.Web.Lists.GetByTitle("Documents");
            Microsoft.SharePoint.Client.CamlQuery query = new Microsoft.SharePoint.Client.CamlQuery();
            query.ViewXml = "<View>"
               + "<Query>"
               + "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>" + filename + "</Value></Eq></Where>"
               + "</Query>"
               + "</View>";
            // execute the query                
            ListItemCollection listItems = spList.GetItems(query);
            clientContext.Load(listItems);
            clientContext.ExecuteQuery();
            foreach (Microsoft.SharePoint.Client.ListItem listitem in listItems)
            {
                listitem.DeleteObject();
                try { clientContext.ExecuteQuery(); }
                catch
                {
                    Thread.Sleep(4000);
                    clientContext.ExecuteQuery();
                }
            }
        }

        /// <summary>
        /// Delete folder on site
        /// </summary>
        /// <param name="filename">folder name</param>
        public static void DeleteFolder(string foldername)
        {
            ClientContext clientContext = new ClientContext(Browser.BaseAddress);
            List spList = clientContext.Web.Lists.GetByTitle("Documents");
            Microsoft.SharePoint.Client.CamlQuery query = new Microsoft.SharePoint.Client.CamlQuery();
            query.ViewXml = "<View>"
               + "<Query>"
               + "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='Folder'>" + foldername + "</Value></Eq></Where>"
               + "</Query>"
               + "</View>";
            // execute the query                
            ListItemCollection listItems = spList.GetItems(query);
            clientContext.Load(listItems);
            clientContext.ExecuteQuery();
            foreach (Microsoft.SharePoint.Client.ListItem listitem in listItems)
            {
                listitem.DeleteObject();
                clientContext.ExecuteQuery();
            }
        }

        /// <summary>
        /// Upload a file on site
        /// </summary>
        /// <param name="path">file path</param>
        public static void UploadFile(string path)
        {
            ClientContext context = new ClientContext(Browser.BaseAddress);
            Web web = context.Web;

            // new file
            FileCreationInformation newFile = new FileCreationInformation();
            newFile.Content = System.IO.File.ReadAllBytes(path);
            newFile.Overwrite = true;
            newFile.Url = path.Split('\\').Last();

            List docs = web.Lists.GetByTitle("Documents");
            Folder currentRunFolder = docs.RootFolder;

            currentRunFolder.Files.Add(newFile);
            currentRunFolder.Update();
            context.ExecuteQuery();
        }
    }
}
