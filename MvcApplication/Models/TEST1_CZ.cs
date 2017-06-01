using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMDB;

namespace MvcApplication.Models
{
    public class TEST1_CZ
    {
        [Col_(IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        public string COLUMN1 { get; set; }

        public int? intss { get; set; }

        public int? intsss { get; set; }
    }
}