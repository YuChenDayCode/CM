using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMDB;

namespace MvcApplication.Models
{
    public class TEST1_CZ
    {
        [Col_(IsPrimaryKey = true)]
        public int ID { get; set; }

        public string COLUMN1 { get; set; }
    }
}