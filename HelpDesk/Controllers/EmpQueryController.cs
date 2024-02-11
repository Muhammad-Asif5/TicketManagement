using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class EmpQueryController : Controller
    {
        // GET: EmpQuery
        Entities con = new Entities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EmpQuery()
        {
            string EMP_ID = Session["EMP_ID"] as string;
            string USER_NAME = Session["USER_NAME"] as string;
            string Email = Session["Email"] as string;
            string LoggedPaswrd = Session["LoggedPaswrd"] as string;

            var msg = Session["LoginMsg"];
            if (String.IsNullOrEmpty(Email))
            {
                //return View();
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }
        #region Load to Table
        public JsonResult GetAllQueryToTable()
        {
            DateTime dtt = DateTime.Now;
            DateTime dt = dtt.Date;

            var r = (from q in con.QUERies
                     join e in con.EMPLOYEEs on q.EMP_ID equals e.EMP_ID
                     where q.STATUS=="0"  && q.SOLVE_DATE.Value.Date<dt.Date
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
                         e.NAME
                     }).OrderBy(x => x.Q_ID).ToList();
            return Json(r);
        }

        #endregion

        #region Save Update
        public JsonResult SaveItem(string Issueno, string EmployeeNo, string Description, string Office)
        {
            con.Configuration.ProxyCreationEnabled = false;
            DateTime d = new DateTime();
            d = DateTime.Now;
            string dtDisplay = string.Empty;
            dtDisplay = d.ToString("hh:mm tt");
            string session = Session["LoggedID"] as string;

            if (session != null)
            {
                int id = Convert.ToInt32(Issueno);
                int emp = Convert.ToInt32(EmployeeNo);
                var item1 = con.QUERies.Where(x => x.Q_ID == id).FirstOrDefault();
                item1.DESCRIPTION = Description;
                item1.OFFICE = Office;
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