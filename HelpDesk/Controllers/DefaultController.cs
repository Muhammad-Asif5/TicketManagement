using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default.
        Entities con = new Entities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Default()
        {
            string Name = Session["Name"] as string;
            string USER_ID = Session["USER_ID"] as string;
            string Email = Session["Email"] as string;
            string Usertype = Session["Usertype"] as string;


            Session["Msg"] = ViewBag.CheckSession;

            var msg = Session["LoginMsg"];
            if (String.IsNullOrEmpty(Email))
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {

                if (Usertype == "Admin" || Usertype == "SuperUser")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("User", "User");
                }
            }
        }

        public JsonResult GetAllQueryToTable()
        {
            DateTime dtt = DateTime.Now;
            DateTime dt = dtt.Date;

            var r = (from q in con.QUERies
                     join e in con.EMPLOYEEs on q.EMP_ID equals e.EMP_ID
                     select new
                     {
                         q.Q_ID,
                         q.ASSIGN_DATE,
                         q.ASSIGN_TIME,
                         q.SOLVE_TIME,
                         q.DESCRIPTION,
                         q.OFFICE,
                         q.STATUS,
                         q.EMP_ID,
                         q.CREATEDTIME,
                         q.USER_ID,
                         e.NAME,
                         q.COMMENTS
                     }).OrderBy(x => x.Q_ID).ToList();
            return Json(r);
        }
        
    }
}