using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class SelefQueryController : Controller
    {
        // GET: SelefQuery
        Entities con = new Entities();
        public ActionResult SelefQuery()
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
                //string sessionId = Session["LoggedID"] as string;
                //string StuOrUser = Session["StuOrUser"] as string;
                //string email = Session["Email"] as string;
                //string Admin = Session["Admin"] as string;

                if (Usertype == "Admin" || Usertype == "SuperUser")
                {
                    GetAllUsers();
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
        }


        public ActionResult AddQuery(string IssueNo, decimal EmployeeNo, string Description, string Office)
        {
            con.Configuration.ProxyCreationEnabled = false;
            DateTime d = new DateTime();
            d = DateTime.Now;
            string dtDisplay = string.Empty;
            dtDisplay = d.ToString("hh:mm tt");

            var checkInfo = (from q in con.USERS
                             where q.EMP_ID == EmployeeNo
                             select q).ToList();
            if (checkInfo.Count > 0)
            {
                decimal EMP_ID = checkInfo[0].EMP_ID;
                string Name = checkInfo[0].USER_NAME as string;
                string Email = checkInfo[0].EMAIL as string;
                string LoggedPaswrd = checkInfo[0].USER_PASSWORD as string;
                string Usertype = checkInfo[0].USER_TYPE as string;


                int EMPID = Convert.ToInt32(EMP_ID);

                var checkEmail = (from u in con.USERS
                                  join q in con.QUERies
                                      on u.EMP_ID equals q.EMP_ID
                                  where u.EMP_ID == EMPID
                                  select new { q.USER_EMAIL }).ToList();
                if (Email == "")
                {
                    return Json(new { Success = true, message = "Email Not Found" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var email = Email;
                    int SNO = 0;

                    var check = (from q in con.QUERies select q.Q_ID).ToList();
                    if (check.Count == 0)
                    {
                        SNO = 1;
                    }
                    else
                    {
                        var check_QID = (from q in con.QUERies
                                         select q.Q_ID).Max();

                        if (check_QID > 0)
                        {
                            SNO = (int)check_QID;
                            SNO++;
                        }
                    }
                    QUERY obj = new QUERY();
                    obj.Q_ID = SNO;
                    obj.CREATEDTIME = dtDisplay;
                    obj.ASSIGN_DATE = DateTime.Now;
                    obj.CREATED_DATE = DateTime.Now;
                    obj.DESCRIPTION = Description;
                    obj.OFFICE = Office;
                    obj.USER_EMAIL = email;
                    obj.STATUS = "0";
                    obj.USER_ID = Name;

                    con.QUERies.Add(obj);
                    con.SaveChanges();

                    return Json(new { Success = true, message = "Query has been added Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }else
                return Json(new { Success = true, message = "Account / Employee Not Fount" }, JsonRequestBehavior.AllowGet);
            
        }

        void GetAllUsers()
        {
            var viewData = (from q in con.USERS
                            orderby q.EMP_ID
                            select new
                            {
                                q.USER_NAME,
                                q.EMP_ID
                            }).ToList();

            var data = ViewBag.allUsers = viewData.Select(x => new SelectListItem
            {
                Value = x.EMP_ID.ToString(),

                Text = x.EMP_ID.ToString() + " : " + x.USER_NAME.ToString()

            }).Distinct().ToList();
        }
    }
}