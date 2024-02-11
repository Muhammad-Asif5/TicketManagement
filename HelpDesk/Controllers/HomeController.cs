using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDesk.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Entities con = new Entities();
        List<modelEmployeeQuery> mlist = new List<modelEmployeeQuery>();
        public ActionResult Index()
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
                         q.SOLVE_DATE,
                         q.DESCRIPTION,
                         q.OFFICE,
                         q.STATUS,
                         q.EMP_ID,
                         q.CREATEDTIME,
                         q.CREATED_DATE,
                         q.USER_ID,
                         e.NAME,
                         q.COMMENTS
                     }).OrderBy(x => x.Q_ID).ToList();
            for (int i = 0; i < r.Count; i++)
            {
                modelEmployeeQuery obj = new modelEmployeeQuery();
                obj.Q_ID =(int)r[i].Q_ID;
                obj.ASSIGN_DATE = r[i].ASSIGN_DATE;
                obj.ASSIGN_TIME = r[i].ASSIGN_TIME;
                obj.SOLVE_TIME = r[i].SOLVE_TIME;
                obj.DESCRIPTION = r[i].DESCRIPTION;
                obj.OFFICE = r[i].OFFICE;
                obj.CREATEDTIME = r[i].CREATEDTIME;
                obj.CREATED_DATE = r[i].CREATED_DATE;
                obj.NAME = r[i].NAME;
                obj.SOLVE_DATE = r[i].SOLVE_DATE;
                obj.COMMENTS = r[i].COMMENTS;

                mlist.Add(obj);
            }

            return View(mlist);
        }
        public ActionResult Home()
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
                         q.COMMENTS,
                         q.SOLVE_DATE
                     }).OrderBy(x => x.Q_ID).ToList();
            return View(r);
        }
        IEnumerable<QUERY> GetQuery(string userid)
        {
            using (con)
            {
                return con.QUERies.Where(x => x.STATUS == "0" && x.USER_ID == userid).OrderBy(x => x.Q_ID).ToList();
                
            }
        }
    }
}