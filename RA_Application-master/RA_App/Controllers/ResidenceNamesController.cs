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
    public class ResidenceNamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResidenceNames
        public ActionResult Index()
        {
            return View(db.ResidenceNames.ToList());
        }

        // GET: ResidenceNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceName residenceName = db.ResidenceNames.Find(id);
            if (residenceName == null)
            {
                return HttpNotFound();
            }
            return View(residenceName);
        }

        // GET: ResidenceNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResidenceNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Res_ID,Res_Name")] ResidenceName residenceName)
        {
            if (ModelState.IsValid)
            {
                db.ResidenceNames.Add(residenceName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(residenceName);
        }

        // GET: ResidenceNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceName residenceName = db.ResidenceNames.Find(id);
            if (residenceName == null)
            {
                return HttpNotFound();
            }
            return View(residenceName);
        }

        // POST: ResidenceNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Res_ID,Res_Name")] ResidenceName residenceName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(residenceName).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(residenceName);
        }

        // GET: ResidenceNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResidenceName residenceName = db.ResidenceNames.Find(id);
            if (residenceName == null)
            {
                return HttpNotFound();
            }
            return View(residenceName);
        }

        // POST: ResidenceNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResidenceName residenceName = db.ResidenceNames.Find(id);
            db.ResidenceNames.Remove(residenceName);
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
