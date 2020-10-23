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
                using ( clientContext = GetClientContext())
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


                        string returnMessage;
                        double totalHours = GetDaysTotal(timesheet.Date) + timesheet.Hours;
                        if (totalHours <= 8)
                        {
                            oListItem["Status"] = "Approved";
                            returnMessage = "Your timesheet has been submitted and has been automatically approved.";
;
                        }
                        else
                        {
                            oListItem["Status"] = "Pending";
                            returnMessage = "Your timesheet has been submitted and overtime hours requires approval from Manager.";
                        }


                        oListItem.Update();

                        clientContext.ExecuteQuery();

                        return returnMessage;
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

        public string Update(Timesheet timesheet)
        {
            try
            {
                using (clientContext = GetClientContext())
                {

                    List list = clientContext.Site.RootWeb.GetListByTitle(TIMESHEET_LIST_NAME);
                    if (list != null)
                    {

                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        ListItem oListItem = list.GetItemById(timesheet.ID);
                        oListItem["Title"] = timesheet.Title;
                        oListItem["Description"] = timesheet.Description;
                        oListItem["Category"] = timesheet.Category;
                        oListItem["Date"] = timesheet.Date;
                        oListItem["Hours"] = timesheet.Hours;


                        string returnMessage;
                        double totalHours = GetDaysTotal(timesheet.Date) + timesheet.Hours;
                        if (totalHours <= 8)
                        {
                            oListItem["Status"] = "Approved";
                            returnMessage = "Your timesheet has been submitted and has been automatically approved.";
                            ;
                        }
                        else
                        {
                            oListItem["Status"] = "Pending";
                            returnMessage = "Your timesheet has been submitted and overtime hours requires approval from Manager.";
                        }


                        oListItem.Update();

                        clientContext.ExecuteQuery();

                        return returnMessage;
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

        public double GetDaysTotal(String date)
        {
            try
            {
                double daysHoursTotal = 0;
                using (clientContext = GetClientContext())
                {

                    List list = clientContext.Site.RootWeb.GetListByTitle(TIMESHEET_LIST_NAME);
                    if (list != null)
                    {

                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        ListItem oListItem = list.AddItem(itemCreateInfo);
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = "<View Scope=\"RecursiveAll\"><Where><Eq><FieldRef Name=\"Date\"/><Value Type=\"DateTime\" >" + date + "</Value></Eq></Where></View>";

                        ListItemCollection collListItem = list.GetItems(camlQuery);

                        clientContext.Load(collListItem, items => items.Include(
                         item => item.Id,
                         item => item["Title"],
                         item => item["Description"],
                         item => item["Category"],
                         item => item["Date"],
                         item => item["Hours"],
                         item => item["Status"]
                         ));

                        clientContext.ExecuteQuery();

                        foreach (ListItem listItem in collListItem)
                        {
                            daysHoursTotal += listItem["Hours"] != null ? Convert.ToDouble(listItem["Hours"]) : 0;
                        }

                        return daysHoursTotal;
                    }
                    else
                    {
                        Console.WriteLine("List is not available on the site");
                        return 0;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message: " + ex.Message);
                return 0;
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