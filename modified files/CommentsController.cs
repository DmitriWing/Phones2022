using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Phones2022.Models;

namespace Phones2022.Controllers
{
    public class CommentsController : Controller
    {
        private PhonesEntities db = new PhonesEntities();

        // GET: Comments
        [ChildActionOnly]   // if dougher view, than can't be calling from address bar
        // to show comment for specified product, Id needed - transfer to the method Index from the link on the page Phones/Details
        public PartialViewResult Index(int PhoneID)
        {
            var comments = db.Comments.Where(c => c.PhonesId == PhoneID).OrderByDescending(m => m.DatePublish);
            ViewBag.PhoneId = PhoneID;
            return PartialView(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public PartialViewResult Create(int PhoneID)
        {
            ViewBag.PhoneId = PhoneID;
            return PartialView();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public PartialViewResult Index(Comment comment, int PhoneID)
        {
            // the comment comes from currently authentificated user
            comment.UserName = User.Identity.Name;
            comment.DatePublish = DateTime.Now;
            comment.PhonesId = PhoneID;

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            var comments = db.Comments.Where(c => c.PhonesId == PhoneID).OrderByDescending(m => m.DatePublish);
            ViewBag.PhoneId = PhoneID;
            return PartialView("Index", comments.ToList());
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhonesId = new SelectList(db.Phones, "Id", "Name", comment.PhonesId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PhonesId,UserName,Comments,DatePublish")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhonesId = new SelectList(db.Phones, "Id", "Name", comment.PhonesId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
