using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Timers;
using System.Data;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace HelpDesk.Controllers
{
    public class QueryController : Controller
    {
        // GET: Query
        Entities con = new Entities();
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Query()
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
                    GetAllEmployee();
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
        }


        #region Load to Table
        public JsonResult GetAllQueryToTable()
        {
            DateTime dtt = DateTime.Now;
            DateTime dt = dtt.Date;
            //var rr = con.QUERies.Where(x => x.STATUS == "0").ToList();
            //if (rr[0].SOLVE_DATE.Value.Date.Day <= dt.Day)
            //{
            //    int b = 0;
            //}
           

            var r = (from q in con.QUERies
                     where q.STATUS == "0"
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
                         q.SOLVE_DATE,
                         q.USER_ID
                     }).OrderBy(x => x.Q_ID).ToList();
            return Json(r);
        }

        #endregion

        #region Save Update
        public JsonResult SaveItem(string Issueno, string EmployeeNo, string Description, string Office)
        {
            con.Configuration.ProxyCreationEnabled = false;
            string Name = Session["Name"] as string;
            string USER_ID = Session["USER_ID"] as string;
            string Email = Session["Email"] as string;
            string Usertype = Session["Usertype"] as string;

            decimal emailId = Convert.ToInt32(EmployeeNo);
            int qid = Convert.ToInt32(Issueno);

            var result = con.EMPLOYEEs.Where(m => m.EMP_ID == emailId).FirstOrDefault();
            var result1 = con.QUERies.Where(m => m.Q_ID == qid).FirstOrDefault();

            string ToEmail = result.EMAIL;
            string UserEmail = result1.USER_EMAIL;
            string ccName = result1.USER_ID;
            string ToName = result.NAME;

            DateTime d = new DateTime();
            d = DateTime.Now;
            string dtDisplay = string.Empty;
            dtDisplay = d.ToString("hh:mm tt");

            if (Email != null)
            {
                var check1 = (from q in con.QUERies select q).ToList();

                if (check1.Count > 0)
                {
                    //add here
                    int id;
                    int emp = Convert.ToInt32(EmployeeNo);

                    if (Issueno == "")
                    {
                        id = 0;
                    }
                    else
                    {
                        id = Convert.ToInt32(Issueno);
                        
                    }

                    var check2 = (from q in con.QUERies where q.Q_ID == id select q).ToList();
                    if (check2.Count > 0)
                    {
                        //string txtFrom = "mohammad.asif@nu.edu.pk";
                        //string txtTo = "mohammad.asif@nu.edu.pk";
                        //string txtSubject = "mohammad.asif@nu.edu.pk";
                        //string txtBody = "mohammad.asif@nu.edu.pk";
                         
                        //MailMessage message = new MailMessage(txtFrom, txtTo, txtSubject, txtBody);
                        //SmtpClient emailClient = new SmtpClient();
                        //emailClient.Credentials = new System.Net.NetworkCredential("mohammad.asif@nu.edu.pk", "Asif12345");
                        //emailClient.Port = 587;
                        //emailClient.Host = "smtp.gmail.com";
                        //emailClient.EnableSsl = true;
                        //emailClient.Send(message);

                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        mail.To.Add(ToEmail);
                        mail.CC.Add(UserEmail);
                        mail.From = new MailAddress(Email, "IT Help Desk", System.Text.Encoding.UTF8);
                        mail.Subject = "HelpDesk Query Portal";
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        //mail.Body = "Dear Mr. Muhammad Asif,\n This is to inform that your";
                        mail.Body += " <html>";
                        mail.Body += "<body>";
                        mail.Body += "<label style='font-weight:bolder'>Dear User,";
                        mail.Body += "</label>";
                        mail.Body += "</label><p>Your query has been assigned to " + ToName + ". </p>";
                        mail.Body += "</label><p>User ID: "+ccName+"</p>";
                        mail.Body += "</label><p>Office Location: " + Office + "</p>";
                        mail.Body += "</label><p>Problem: " + Description + "</p>";
                        mail.Body += "</body>";
                        mail.Body += "</html>";
                        mail.IsBodyHtml = true;

                        mail.BodyEncoding = System.Text.Encoding.UTF8;
                        mail.Priority = MailPriority.High;
                        SmtpClient client = new SmtpClient();
                        client.Credentials = new System.Net.NetworkCredential("support.isb@nu.edu.pk", "HQIM@email");
                        client.Port = 587;
                        client.Host = "smtp.gmail.com";
                        client.EnableSsl = true;
                        try
                        {
                            client.Send(mail);
                            //Update here
                            var item1 = con.QUERies.Where(x => x.Q_ID == id).FirstOrDefault();
                            item1.DESCRIPTION = Description;
                            item1.EMP_ID = emp;
                            item1.ASSIGN_TIME = dtDisplay;
                            item1.OFFICE = Office;
                            con.Entry(item1).State = EntityState.Modified;
                            con.SaveChanges();

                            return Json(new { Success = true, message = "Query Assigned Successfully" }, JsonRequestBehavior.AllowGet);

                        }
                        catch (Exception ex)
                        {
                            return new JsonResult
                            {
                                Data = new { message = ex.Message+" Internet is not Connected", Success = true },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.DenyGet
                            };
                        }
                        //return Json(new { Success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //add here
                        var checkSno = (from q in con.QUERies
                                        select q.Q_ID).Max();
                        int SNO = 0;
                        if (checkSno > 0)
                        {
                            SNO = (int)checkSno;
                            SNO++;
                        }
                        else SNO = 1;

                        QUERY obj = new QUERY();
                        obj.Q_ID = SNO;
                        //obj.ASSIGN_TIME = DateTime.Now.TimeOfDay; ;
                        obj.ASSIGN_TIME = dtDisplay;
                        //obj.SOLVE_TIME = dtDisplay
                        obj.ASSIGN_DATE = DateTime.Now;
                        obj.DESCRIPTION = Description;
                        obj.OFFICE = Office;
                        obj.EMP_ID = Convert.ToInt32(EmployeeNo);
                        obj.STATUS = "0";

                        try
                        {
                            con.QUERies.Add(obj);
                            con.SaveChanges();

                            return Json(new { Success = true, message = "Save Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return new JsonResult
                            {
                                Data = new { ErrorMessage = ex.Message, Success = false },
                                ContentEncoding = System.Text.Encoding.UTF8,
                                JsonRequestBehavior = JsonRequestBehavior.DenyGet
                            };
                        }
                    }
                }
                else
                {
                    //add here
                    int SNO = 1;

                    QUERY obj = new QUERY();
                    obj.Q_ID = SNO;
                    obj.ASSIGN_TIME = dtDisplay;
                    //obj.SOLVE_TIME = DateTime.Now.TimeOfDay;
                    obj.ASSIGN_DATE = DateTime.Now;
                    obj.DESCRIPTION = Description;
                    obj.OFFICE = Office;
                    obj.EMP_ID = Convert.ToInt32(EmployeeNo);
                    obj.STATUS = "0";

                    try
                    {
                        con.QUERies.Add(obj);
                        con.SaveChanges();

                        return Json(new { Success = true, message = "Save Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult
                        {
                            Data = new { ErrorMessage = ex.Message, Success = false },
                            ContentEncoding = System.Text.Encoding.UTF8,
                            JsonRequestBehavior = JsonRequestBehavior.DenyGet
                        };
                    }
                }
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

                       where q.Q_ID==id
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
                           q.CREATEDTIME
                       }).ToList();

            return Json(Get);
        }
        #endregion


        #region Save Solve
        public JsonResult SolveQuery(string Issueno)
        {
            con.Configuration.ProxyCreationEnabled = false;
            DateTime d = new DateTime();
            d = DateTime.Now;
            string dtDisplay = string.Empty;
            dtDisplay = d.ToString("hh:mm tt");

            if (Issueno != "")
            {
                int id = Convert.ToInt32(Issueno);
                //add here
                //Update here
                var item1 = con.QUERies.Where(x => x.Q_ID == id).FirstOrDefault();
                item1.COMMENTS = "This Query has been Modified by Admin";
                item1.STATUS = "1";
                item1.SOLVE_TIME = dtDisplay;
                item1.SOLVE_DATE = d;
                con.Entry(item1).State = EntityState.Modified;
                con.SaveChanges();
                return Json(new { Success = true, message = "Query Solved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false");
            }
        }
        #endregion
       
    }
}