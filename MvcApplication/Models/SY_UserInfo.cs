using System;
using CMDB;
namespace CMDB
{
     public class SY_UserInfo
      {
        /// <summary>
        /// 
        /// </summary>
        [Col_(IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int LoginType { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int UserType { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int IsDelete { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? AriSeq { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string RealName { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int UserState { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string Pwd { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int InternetPower { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string UserCode { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string AriName { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string AriDesc { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNo { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLogTime { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? DeptID { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? PostID { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? EmployeeID { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? Verify { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int AttDeptID { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? UserIdentity { get; set;}
        /// <summary>
        /// 
        /// </summary>
        public int? GetNotice { get; set;}
      }
}