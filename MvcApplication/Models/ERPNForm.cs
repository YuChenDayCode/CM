using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.Models
{
    public class ERPNForm
    {
        // 摘要:
        //     排序码
        public int? ARISEQ { get; set; }
        //
        // 摘要:
        //     表单内容
        public string CONTENTSTR { get; set; }
        //
        // 摘要:
        //     允许使用部门
        public string DEPLISTOK { get; set; }
        //
        // 摘要:
        //     正式数据名
        public string FORMDATANAME { get; set; }
        //
        // 摘要:
        //     表单名称
        public string FORMNAME { get; set; }
        //
        // 摘要:
        //     主键
        public long ID { get; set; }
        //
        // 摘要:
        //     是否启用
        public string IFOK { get; set; }
        //
        // 摘要:
        //     是否为虚拟表单
        public int ISVIRTUAL { get; set; }
        //
        // 摘要:
        //     表单中数据列
        public string ItemsList { get; set; }
        //
        // 摘要:
        //     允许使用角色
        public string JIAOSELISTOK { get; set; }
        //
        // 摘要:
        //     排序字符
        public string PAIXUSTR { get; set; }
        //
        // 摘要:
        //     录入时间
        public DateTime? TIMESTR { get; set; }
        //
        // 摘要:
        //     待办工作数量
        public int? ToDoWorkNum { get; set; }
        //
        // 摘要:
        //     所属分类ID
        public long? TYPEID { get; set; }
        //
        // 摘要:
        //     允许使用人
        public string USERLISTOK { get; set; }
        //
        // 摘要:
        //     录入人
        public string USERNAME { get; set; }
    }
}