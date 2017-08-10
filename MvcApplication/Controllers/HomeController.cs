using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMDB;
using MvcApplication.Models;
using MvcApplication.Controllers;
using System.Collections;
using System.Linq.Expressions;
using System.Net.Http;

namespace MvcApplication.Controllers
{

    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public class Login
        {
            public string username { get; set; }

            public string pwd { get; set; }
        }

        public string JsonpTest(Login login)
        {
            if (login != null)
                return "successCallback(success)";
            else
                return "successCallback(login is null)";
        }


        public ActionResult Index()
        {
            OracleE();
            return View();

        }


        public void OracleE()
        {
            try
            {
                object aa = CM.DB.Select("select ITEM from TEST_CZ");

                List<TEST_CZ> list = CM.DB.Select<TEST_CZ>();

               // List<TEST_CZ> listss = CM.DB.Select<TEST_CZ>(t => t.ITEM == int.Parse("34")); // Guid 

                List<TEST_CZ> lists1s = CM.DB.Select<TEST_CZ>(t => t.RAWS == Guid.Parse("a4bc6a4c-e8d6-4c4f-ae66-edef5a7a2077")); // Guid 
            }
            catch (Exception ex) { }
        }

        public void SQLE()
        {

            // try
            //{

            int iss = CM.DB.Delete<TEST1_CZ>(t => t.dt <= DateTime.Now);
            List<TEST1_CZ> aa = CM.DB.Select<TEST1_CZ>();
            // a.OrderBy(t => t.id);


            //
            // catch (Exception e) { }
            TEST1_CZ aabc = new TEST1_CZ()
            {

            };

            //   CM.DB.Update(aabc);


            TEST1_CZ aab = new TEST1_CZ()
            {

                COLUMN1 = "内容1"
            };

            CM.DB.Update(aab);
            // }
            // catch (Exception e) { }
            List<Test> test = new List<Test>();
            for (int i = 2; i <= 5; i++)
            {
                Test tc = new Test();
                tc.Id = i;
                tc.Content = "更新更新更新" + i;
                tc.IsOk = true;
                tc.Item = 10 + i;
                tc.OperateTime = DateTime.Now;


                test.Add(tc);
            }
            Test tc1 = new Test();
            tc1.Content = "插入返回ID";
            tc1.IsOk = true;
            tc1.Item = 10;
            tc1.OperateTime = DateTime.Now;


            try
            {
                #region Oracle
                //bool b = CM.DB.Insert<TEST_CZ>(czz);
                //List<TEST1_CZ> tcz = CM.DB.Select<TEST1_CZ>();
                // bool aaa = CM.DB.Delete<TEST_CZ>(t => t.ISOK.Contains("OK"));
                //List<TEST_CZ> tcz1 = CM.DB.Select<TEST_CZ>();
                //TEST_CZ ttcz = CM.DB.Select<TEST_CZ>(2);
                //List<TEST_CZ> tcz11 = CM.DB.Select<TEST_CZ>(t=>t.ID==2);
                #endregion


                SY_UserInfo sui = new SY_UserInfo()
                {
                    AriDesc = "AriDesc",
                    // AriSeq = 1,
                    //AttDeptID = 1,
                    CreateTime = DateTime.Now,
                    // DeptID = 1,
                    Email = "Email",
                    //EmployeeID = 1,
                    GetNotice = 1,
                    InternetPower = 1,
                    IsDelete = 1,
                    LastLogTime = DateTime.Now,
                    //LoginType = 1,
                    PhoneNo = "Phone",
                    PostID = 1,
                    Pwd = "PWD",
                    RealName = "RealName",
                    //UserState = null,
                    UserCode = "UserCode",
                    UserType = 1,
                    Verify = 1
                };
                //bool aa = CM.DB.Insert(sui);

                int i = 1;
                List<SY_User> list = CM.DB.Select<SY_User>(m => m.LoginType == i);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
