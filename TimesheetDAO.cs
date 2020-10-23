using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimesheetWcfService1
{
    public class TimesheetDAO
    {
        const string SITE_URL = "https://playgroundtech.sharepoint.com/sites/Timesheet/";
        const string TIMESHEET_LIST_NAME =  "Timesheet";
        ClientContext clientContext = null;
        private ClientContext GetClientContext()
        {
            AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();

            if (clientContext == null)
            {
                return authManager.GetWebLoginClientContext(SITE_URL);
            }
            else
            {
                return clientContext;
            }
        }

        public string Create(Timesheet timesheet)
        {
            try
            {
                using (var clientContext = GetClientContext())
                {

                    List list = clientContext.Site.RootWeb.GetListByTitle(TIMESHEET_LIST_NAME);
                    if (list != null)
                    {

                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        ListItem oListItem = list.AddItem(itemCreateInfo);
                        oListItem["Title"] = timesheet.Title;
                        oListItem["Description"] = timesheet.Description;
                        oListItem["Category"] = timesheet.Category;
                        oListItem["Date"] = timesheet.Date;
                        oListItem["Hours"] = timesheet.Hours;

                        oListItem.Update();

                        clientContext.ExecuteQuery();

                        return "Timesheet Item Created!";
                    }
                    else
                    {
                        Console.WriteLine("List is not available on the site");
                        return "Error Message: " + "List is not available on the site";

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
                return "Error Message: " + ex.Message;
            }

            
        }

        public Boolean ListExist(string listName)
        {
            try
            {
                using (var clientContext = GetClientContext())
                {

                    List list = clientContext.Site.RootWeb.GetListByTitle(listName);
                    if (list != null)
                    {

                        Console.WriteLine("List Title : " + list.Title);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("List is not available on the site");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
                return false;
            }

            
        }
    }
}