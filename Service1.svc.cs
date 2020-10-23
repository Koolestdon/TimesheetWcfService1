using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TimesheetWcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        TimesheetDAO timesheetDAO = new TimesheetDAO();
        public string SaveTimesheet(string title, string description, string category, string date, int hours, string status)
        {
            Timesheet timesheet = new Timesheet(title, description, category, date, hours, status);


            return timesheetDAO.Create(timesheet);
        }
        public string VerifyList(string listName)
        {
            if(timesheetDAO.ListExist(listName))
            {
                return "List is available : " + listName;
            }
            else
            {
                return "List is not available on the site";
            }
        }


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


    }
}
