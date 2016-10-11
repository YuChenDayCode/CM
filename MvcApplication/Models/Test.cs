using CMDB;
using System;

namespace MvcApplication.Models
{
    public class Test
    {
        [Col_(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string Content { get; set; }

        public int? Item { get; set; }

        public bool IsOk { get; set; }

        public DateTime? OperateTime { get; set; }
    }
}