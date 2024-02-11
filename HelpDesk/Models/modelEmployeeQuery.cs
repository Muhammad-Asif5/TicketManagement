using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models
{
    public class modelEmployeeQuery
    {
        public int Q_ID { get; set; }
        public string NAME { get; set; }
        public string ASSIGN_TIME { get; set; }
        public string SOLVE_TIME { get; set; }
        public string DESCRIPTION { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyy}")]
        public Nullable<System.DateTime> ASSIGN_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyy}")]
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyy}")]
        public Nullable<System.DateTime> SOLVE_DATE { get; set; }
        public string CREATEDTIME { get; set; }
        public string COMMENTS { get; set; }
        public string OFFICE { get; set; }
        
        
        //public Nullable<System.DateTime> ASSIGN_DATE { get; set; }
        
        
       
        //public string CREATEDTIME { get; set; }
        //public string USER_EMAIL { get; set; }

        //public decimal EMP_ID { get; set; }
       
        //public string USER_ID { get; set; }
        //public string USER_PASSWORD { get; set; }
        //public string USER_TYPE { get; set; }
        //public string EMAIL { get; set; }
        
    }
}