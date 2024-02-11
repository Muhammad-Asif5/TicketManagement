using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        Entities con = new Entities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult User()
        {
            string session = Session["LoggedID"] as string;

            Session["Msg"] = ViewBag.CheckSession;

            var msg = Session["LoginMsg"];
            if (String.IsNullOrEmpty(session))
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string sessionId = Session["LoggedID"] as string;
                string StuOrUser = Session["StuOrUser"] as string;
                string Admin = Session["Admin"] as string;
                string Usertype = Session["Usertype"] as string;

                if (Usertype == "Admin")
                {
                    GetAllEmployee();
                    return RedirectToAction("Default", "Default");
                }
                else
                {
                    return View();
                }
            }

            
        }
        #region Load to Table
        public JsonResult GetAllQueryToTable()
        {
            DateTime dtt = DateTime.Now;
            DateTime dt = dtt.Date;
            var session = Session["empId"] as string;
            int id = Convert.ToInt32(session);

            var r = (from q in con.QUERies
                     join e in con.EMPLOYEEs on q.EMP_ID equals e.EMP_ID
                     where q.EMP_ID==id
                     //where si.ISSUE_DATE >= dt
                     select new
                     {
                         q.Q_ID,
                         q.ASSIGN_DATE,
                         //q.ASSIGN_TIME.Value.Hours,
                         q.ASSIGN_TIME,
                         q.SOLVE_TIME,
                         q.DESCRIPTION,
                         q.OFFICE,
                         q.STATUS,
                         q.EMP_ID,
                         e.NAME
                     }).OrderBy(x => x.Q_ID).ToList();
            return Json(r);
        }

        #endregion

        #region Save Update
        public JsonResult SaveItem(string Issueno, string Description)
        {
            con.Configuration.ProxyCreationEnabled = false;
            DateTime d = new DateTime();
            d = DateTime.Now;
            string dtDisplay = string.Empty;
            dtDisplay = d.ToString("hh:mm tt");
            string session = Session["LoggedID"] as string;
            string empId = Session["empId"] as string;

            int id = Convert.ToInt32(Issueno);
            int emp = Convert.ToInt32(empId);

            if (session != null)
            {
                //Update here
                var item1 = con.QUERies.Where(x => x.Q_ID == id && x.EMP_ID==emp).FirstOrDefault();
                item1.DESCRIPTION = Description;
                item1.SOLVE_TIME = dtDisplay;
                item1.STATUS = "1";
                con.Entry(item1).State = EntityState.Modified;
                con.SaveChanges();
                return Json(new { Success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false");
            }

        }
        #endregion

        #region All EMployee
        public void GetAllEmployee()
        {
            var AllEmployee = (from q in con.EMPLOYEEs
                               select new
                               {
                                   q.EMP_ID,
                                   q.NAME
                               }).ToList();

            if (AllEmployee.Count > 0)
            {
                var data = ViewBag.DDLItem = AllEmployee.Select(x => new SelectListItem
                {
                    Value = x.EMP_ID.ToString(),
                    Text = x.EMP_ID + " : " + x.NAME.ToString(),
                    //Selected = (x.STOCK_NO==""),
                    //Disabled=(x.STOCK_NO=="")
                }).Distinct().ToList();
            }

        }
        #endregion

        #region Load To TextBox Data
        public JsonResult LoadToTextBoxData(string IssueID)
        {
            int id = Convert.ToInt32(IssueID);
            //int emp = Convert.ToInt32(EmployeeNo);

            var Get = (from q in con.QUERies
                       join e in con.EMPLOYEEs
                       on q.EMP_ID equals e.EMP_ID
                       where q.Q_ID == id
                       select new
                       {
                           q.Q_ID,
                           q.ASSIGN_DATE,
                           //q.ASSIGN_TIME.Value.Hours,
                           q.ASSIGN_TIME,
                           q.SOLVE_TIME,
                           q.DESCRIPTION,
                           q.OFFICE,
                           q.STATUS,
                           q.EMP_ID,
                           e.NAME
                       }).ToList();

            return Json(Get);
        }
        #endregion
    }
}