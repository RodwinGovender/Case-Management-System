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
    public class ActionsTakensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ActionsTakens
        public ActionResult Index()
        {
            return View(db.ActionsTakens.ToList());
        }

        // GET: ActionsTakens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionsTaken actionsTaken = db.ActionsTakens.Find(id);
            if (actionsTaken == null)
            {
                return HttpNotFound();
            }
            return View(actionsTaken);
        }

        // GET: ActionsTakens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionsTakens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Action_ID,Action_Name")] ActionsTaken actionsTaken)
        {
            if (ModelState.IsValid)
            {
                db.ActionsTakens.Add(actionsTaken);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionsTaken);
        }

        // GET: ActionsTakens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionsTaken actionsTaken = db.ActionsTakens.Find(id);
            if (actionsTaken == null)
            {
                return HttpNotFound();
            }
            return View(actionsTaken);
        }

        // POST: ActionsTakens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Action_ID,Action_Name")] ActionsTaken actionsTaken)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionsTaken).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionsTaken);
        }

        // GET: ActionsTakens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionsTaken actionsTaken = db.ActionsTakens.Find(id);
            if (actionsTaken == null)
            {
                return HttpNotFound();
            }
            return View(actionsTaken);
        }

        // POST: ActionsTakens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionsTaken actionsTaken = db.ActionsTakens.Find(id);
            db.ActionsTakens.Remove(actionsTaken);
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
