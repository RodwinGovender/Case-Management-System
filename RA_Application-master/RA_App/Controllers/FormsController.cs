using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RA_App.Models;

namespace RA_App.Controllers
{
    public class FormsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Forms
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string searchBy, string search)
        {

            if (searchBy == "Received")
            {
                return View(db.Forms.Where(x => x.StudNo.StartsWith(search) && x.Status.Contains("Received") || search == null).ToList());
            }
            else if (searchBy == "Declined")
            {
                return View(db.Forms.Where(x => x.StudNo.StartsWith(search) && x.Status.Contains("Declined") || search == null).ToList());
            }
            else if (searchBy == "Pending")
            {
                return View(db.Forms.Where(x => x.StudNo.StartsWith(search) && x.Status.Contains("Pending") || search == null).ToList());
            }
            else
            {
                return View(db.Forms.Where(x => x.StudNo.StartsWith(search) || search == null).ToList());
            }

           
        }

        [Authorize(Roles = "Admin, RA")]
        public ActionResult MyIndex(string searchBy, string search)
        {
            string currentUserName = User.Identity.Name;


            var all = (from p in db.Forms
                       where p.UserName == currentUserName
                       select p);

            var MyName = (from p in db.Users
                       where p.UserName == currentUserName
                       select p.Emp_Name).FirstOrDefault();
            if (MyName != null)
            {
               ViewBag.MyName = MyName.ToString();
            }
            

            if (searchBy == "Received")
            {
                return View(all.Where(x => x.StudNo.StartsWith(search) && x.Status.Contains("Received") || search == null).ToList());
            }
            else if (searchBy == "Declined")
            {
                return View(all.Where(x => x.StudNo.StartsWith(search) && x.Status.Contains("Declined") || search == null).ToList());
            }
            else if (searchBy == "Pending")
            {
                return View(all.Where(x => x.StudNo.StartsWith(search) && x.Status.Contains("Pending") || search == null).ToList());
            }
            else
            {
                return View(all.Where(x => x.StudNo.StartsWith(search) || search == null).ToList());
            }

        }

        public ActionResult HomePage()
        { 
          return View();
        }

        // GET: Forms/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }

            ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name");
            ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name");
            return View(form);
        }

        [Authorize(Roles = "Admin, RA")]
        public ActionResult MyDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }

            ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name");
            ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name");
            return View(form);
        }

        // GET: Forms/Create
        [Authorize(Roles = "Admin, RA")]
        public ActionResult Create()
        {
            var Inci = (from i in db.ReportTypes
                        where i.Category_Type == "Incident"
                        select i.Name);

            var Ill = (from l in db.ReportTypes
                       where l.Category_Type == "Illness"
                       select l.Name);

            ViewBag.Inci = new SelectList(Inci, "Report_ID", "Name");
            ViewBag.Illn = new SelectList(Ill, "Report_ID", "Name");



            ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name");
            ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name");
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, RA")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Res_ID,TOR,Report_ID,StudName,StudSurname,StudNo,YOS,ContactNo,NOKContactNo,NatureOf_II,Dateof_II,DateReported_II,Details_II,ActionsTakens,Action_ID,Recommendations,UserName,Emp_Name,Status")] Form form)
        {
            if (ModelState.IsValid)
            {
                if (form.Dateof_II > form.DateReported_II || form.DateReported_II > DateTime.Today || form.Dateof_II > DateTime.Today)
                {
                    ViewBag.ErrorDate = "Please check if your dates are correct and try again.";



            ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name", form.Res_ID);
            ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name", form.Action_ID);
                    return View(form);
                }

                string currentUserName = User.Identity.Name;
                

                var currentUser = (from u in db.Users
                                   where u.UserName.Equals(currentUserName)
                                   select u).FirstOrDefault();

                form.UserName = currentUser.UserName;
                form.Emp_Name = currentUser.Emp_Name + " " + currentUser.Emp_Surname;

                var TORName = (from j in db.ReportTypes
                                  where j.Report_ID == form.Report_ID
                                  select j).FirstOrDefault();

                form.TOR = TORName.Category_Type;
                form.NatureOf_II = TORName.Name;
            
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("MyIndex");
            }

           

            ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name", form.Res_ID);
            ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name", form.Action_ID);


            var TORName1 = (from j in db.ReportTypes
                           where j.Report_ID == form.Report_ID
                           select j).FirstOrDefault();

            if (TORName1==null)
            {
                if (form.Dateof_II > form.DateReported_II || form.DateReported_II > DateTime.Today || form.Dateof_II > DateTime.Today)
                {
                    ViewBag.ErrorDate = "Please check if your dates are correct and try again.";



                }

                ViewBag.ErrorName = "Please select an Illness/Incident from the List Provided";



                ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name", form.Res_ID);
                ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name", form.Action_ID);
                return View(form);
            }

            return View(form);
        }

        // GET: Forms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Res_ID,TOR,Report_ID,StudName,StudSurname,StudNo,YOS,ContactNo,NOKContactNo,NatureOf_II,Dateof_II,DateReported_II,Details_II,ActionsTakens,Action_ID,Recommendations,UserName,Emp_Name,Status")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(form);
        }

        [Authorize(Roles = "Admin")]
        // GET: Forms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        public ActionResult Statistics()
        {
            //Shows total number of Reports in Db
            int total = (from p in db.Forms
                         select p).Count();

            ViewBag.total = total.ToString();


            //Shows Total number of RAs 

            var roles = db.Roles.Where(r => r.Name == "RA");
            if (roles.Any())
            {


                var roleId = roles.First().Id;
                int totRA = (from r in db.Users
                             where r.Roles.Any(k => k.RoleId == roleId)
                             select r).Count();

                ViewBag.totRA = totRA.ToString();

                //Gets array of all RAs in the database

                var RAA = roles.First().Id;
                var RAs = (from a in db.Users
                           where a.Roles.Any(k => k.RoleId == RAA)
                           select a).ToList();

                ViewBag.RAs = RAs.Count();


                //Shows number of reports per RA

                List<RA> NumRepRA = new List<RA>();


                foreach (var RA in RAs)
                {
                    RA ra = new RA();

                    ra.UserName = RA.Emp_Name + " " + RA.Emp_Surname;

                    int totReportsPerPA = (from k in db.Forms
                                           where k.UserName == RA.UserName
                                           select k).Count();

                    ra.NumberOfReports = totReportsPerPA;

                    NumRepRA.Add(ra);

                }

                ViewBag.NumRepRA = NumRepRA;


            }



            //Shows Total number of Types of Reports (Incident)
            int totTorInc = (from k in db.Forms
                             where k.TOR == "Incident"
                             select k).Count();

            ViewBag.totTorInc = totTorInc;

            //Shows Total number of Types of Reports (Illness)
            int totTorIll = (from k in db.Forms
                             where k.TOR == "Illness"
                             select k).Count();

            ViewBag.totTorIll = totTorIll;


            //Shows Total number of Reports for first year students
            int totFirstyear = (from n in db.Forms
                                where n.YOS == 1
                                select n).Count();

            ViewBag.totFirstyear = totFirstyear.ToString();

            //Shows Total number of Reports for second year students
            int totSecondyear = (from n in db.Forms
                                 where n.YOS == 2
                                 select n).Count();

            ViewBag.totSecondyear = totSecondyear.ToString();

            //Shows Total number of Reports for third year students
            int totThirdyear = (from n in db.Forms
                                where n.YOS == 3
                                select n).Count();

            ViewBag.totThirdyear = totThirdyear.ToString();


            //Shows Total number of Reports for Fourth year students
            int totFourthyear = (from n in db.Forms
                                 where n.YOS == 4
                                 select n).Count();

            ViewBag.totFourthyear = totFourthyear.ToString();

            //Shows Total number of Reports for Fifth year students
            int totFifthyear = (from n in db.Forms
                                where n.YOS == 5
                                select n).Count();

            ViewBag.totFifthyear = totFifthyear.ToString();


            return View();
        }


        //public ActionResult ChangeStatus(int? id)
        //{

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Form form = db.Forms.Find(id);
        //    if (form == null)
        //    {
        //        return HttpNotFound();
        //    }


        //    //ViewBag.Res_ID = new SelectList(db.ResidenceNames, "Res_ID", "Res_Name");
        //    //ViewBag.Action_ID = new SelectList(db.ActionsTakens, "Action_ID", "Action_Name");

        //    return View(form);
        //}


       

        public ActionResult getTOR(string term)
        {
            return Json(db.ReportTypes.Where(c => c.Name.StartsWith(term)).Select(a => new { label = a.Name, id = a.Report_ID }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus([Bind(Include = "ID,Res_ID,TOR,Report_ID,StudName,StudSurname,StudNo,YOS,ContactNo,NOKContactNo,NatureOf_II,Dateof_II,DateReported_II,Details_II,ActionsTakens,Action_ID,Recommendations,UserName,Emp_Name,Status")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(form);
        }


        public ActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(Form form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }








            return View(form);
        }



        public ActionResult EditRA(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRA(Form form)
        {

            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyIndex");
            }
            return View(form);
        }








        [Authorize(Roles = "Admin, RA")]
        public ActionResult FeedbackDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }



        // POST: Forms/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Form form = db.Forms.Find(id);
            db.Forms.Remove(form);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
