using System;
using System.Linq;
using System.Text;

namespace CMDB
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Col_Attribute :Attribute
    {
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}
