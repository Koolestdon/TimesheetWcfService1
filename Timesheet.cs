using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimesheetWcfService1
{
    public class Timesheet
    {
        public Timesheet(string title, string description, string category, string date, int hours, string status)
        {
            Title = title;
            Description = description;
            Category = category;
            Date = date;
            Hours = hours;
            Status = status;
        }

        public int ID
        { get; set; }

        public string Title
        { get; set; }

        public string Description
        { get; set; }

        public string Category
        { get; set; }

        public string Date
        { get; set; }

        public int Hours
        { get; set; }

        public string Status
        { get; set; }
    }
}