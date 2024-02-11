using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HelpDesk.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        Entities db = new Entities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            string Name = Session["Name"] as string;
            string USER_ID = Session["USER_ID"] as string;
            string USER_PASSWORD = Session["USER_PASSWORD"] as string;
            string Email = Session["Email"] as string;
            string empId = Session["empId"] as string;



            if (String.IsNullOrEmpty(USER_ID))
            {
                return View();
            }
            else
            {
                //if (Usertype == "Admin")
                //{
                    return RedirectToAction("Default", "Default");
                //}
                //else
                //{
                //    return View();
                //}
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(EMPLOYEE menu_userreg)
        {
            db.Configuration.ProxyCreationEnabled = false;

            string time = DateTime.Now.AddMinutes(25).ToString("hh:mm:ss");
            string WarningTime = DateTime.Now.AddMinutes(20).ToString("hh:mm:ss");
            Session["SessTime"] = time;
            Session["SessTimeWarning"] = WarningTime;

            string UserId = menu_userreg.USER_ID;
            string UserPas = menu_userreg.USER_PASSWORD;

            var result = db.EMPLOYEEs.Where(m => m.USER_PASSWORD.Equals(UserPas) &&
                         m.USER_ID.Equals(UserId)).FirstOrDefault();

            if (result != null)
            {
                string empId = result.EMP_ID.ToString();
                string Usertype = result.USER_TYPE.ToString();

                Session["Name"] = result.NAME.ToString();  // User Name
                Session["USER_ID"] = result.USER_ID.ToString();  // User Id
                Session["USER_PASSWORD"] = result.USER_PASSWORD.ToString();   //Password of the User.
                Session["Email"] = result.EMAIL.ToString();   //Password of the User.
                Session["empId"] = empId;   //Password of the User.
                Session["Usertype"] = Usertype;   // User Type.


                if (Usertype == "Admin" || Usertype == "SuperUser")
                {
                    return RedirectToAction("Default", "Default");
                }
                else
                {
                    ViewBag.ErrorMessage = " Invalid User Id or Password.Try Again";
                    return View();
                }
            }
            else
            {
               ViewBag.ErrorMessage = " Invalid User Id or Password.Try Again";
                //return RedirectToAction("Login", "Login");
                return View();
            }
            // return View(menu_userreg);
        }

        public ActionResult SignOut()
        {
            Session["Name"] = null;

            Session["USER_ID"] = null;
            Session["USER_PASSWORD"] = null;
            Session["Email"] = null;
            Session["empId"] = null;
            Session["Usertype"] = null;

            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Session.Contents.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Login");

        }
    }
}