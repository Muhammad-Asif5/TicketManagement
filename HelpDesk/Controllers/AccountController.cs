using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        Entities con = new Entities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Account()
        {
            string Name = Session["Name"] as string;
            string USER_ID = Session["USER_ID"] as string;
            string Email = Session["Email"] as string;
            string Usertype = Session["Usertype"] as string;
            var sessionID = Session["empId"] as string;

            if (String.IsNullOrEmpty(Email))
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                GetAllEmployee();
                return View();
            }           
        }


        public void GetAllEmployee()
        {
            var viewData = (from q in con.EMPLOYEEs
                            // where q.STATUS =""
                            select new
                            {
                                q.EMP_ID,
                                q.NAME,
                                q.USER_ID,
                                q.USER_PASSWORD,
                                q.USER_TYPE,
                                q.EMAIL,
                                q.STATUS
                            }).ToList();

            if (viewData.Count > 0)
            {
                var data = ViewBag.getEmployeeName = viewData.Select(x => new SelectListItem
                {
                    Value = x.EMP_ID.ToString(),
                    Text = x.EMP_ID + " : " + x.NAME.ToString(),
                    //Selected = (x.EMP_ID == viewData[0].EMP_ID)

                }).Distinct().ToList();
            }
        }

        public JsonResult GetAllEmployeeById(string empId)
        {
            int id = Convert.ToInt32(empId);
            var viewData = (from q in con.EMPLOYEEs
                             where q.EMP_ID == id
                            select new
                            {
                                q.EMP_ID,
                                q.NAME,
                                q.USER_ID,
                                q.USER_PASSWORD,
                                q.USER_TYPE,
                                q.EMAIL,
                                q.STATUS
                            }).ToList();

            return Json(viewData);
        }

        public JsonResult UpdateUser(string empId, string Password, string LoginID)
        {
            //con.Configuration.ProxyCreationEnabled = false;

            var sessionID = Session["empId"] as string;
            var type = Session["Usertype"] as string;

            var emp = (from q in con.EMPLOYEEs
                       select q).ToList();

            if (emp.Count > 0 && sessionID != null)
            {
                decimal eid = Convert.ToInt32(empId);
                int id = Convert.ToInt32(sessionID);

                 var checkAdmin = con.EMPLOYEEs.Where(x => x.EMP_ID == id).ToList();
               
                if(checkAdmin[0].USER_TYPE=="Admin" ){

                    var checkCurrrent = con.EMPLOYEEs.Where(x => x.EMP_ID == id).FirstOrDefault();

                    checkCurrrent.USER_TYPE = "user";
                    con.Entry(checkCurrrent).State = EntityState.Modified;
                    con.SaveChanges();

                    var checkNew = (from q in con.EMPLOYEEs
                                    where q.EMP_ID == eid
                                         select q).FirstOrDefault();
                    checkNew.USER_TYPE = "Admin";
                    checkNew.USER_ID = LoginID;
                    checkNew.USER_PASSWORD = Password;
                    con.Entry(checkNew).State = EntityState.Modified;
                    con.SaveChanges();

                    return Json("Updated Successfully", JsonRequestBehavior.AllowGet);
                }
                if (checkAdmin[0].USER_TYPE == "SuperUser")
                {
                    var checkCurrrent = con.EMPLOYEEs.Where(x => x.EMP_ID == eid).FirstOrDefault();
                    checkCurrrent.USER_ID = LoginID;
                    checkCurrrent.USER_PASSWORD = Password;
                    con.Entry(checkCurrrent).State = EntityState.Modified;
                    con.SaveChanges();
                    return Json("Updated Successfully", JsonRequestBehavior.AllowGet);
                }
                else{
                    return Json("Only Admin Can Change User");
                }
            }
            return Json("Error");
        }
    }
}