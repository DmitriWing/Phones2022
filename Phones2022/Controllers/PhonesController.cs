using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Phones2022.Models;
using PagedList.Mvc;
using PagedList;

namespace Phones2022.Controllers
{
    public class PhonesController : Controller
    {
        private PhonesEntities db = new PhonesEntities();

        // GET: Phones
        public ActionResult Index(int? page)
        {
            // ---------- all items on page
            // var phones = db.Phones.Include(p => p.Company).OrderBy(p => p.Name);
            // return View(phones.ToList());

            // ---------- pagination. Some items per page
            
            int pageSize = 3;   // items on the page
            int pageNumber = (page ?? 1);
            var phones = db.Phones.Include(p => p.Company).OrderBy(p => p.Name).ToList();   // items
            return View(phones.ToPagedList(pageNumber, pageSize));
        }

        // GET: Phones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Include(p => p.Company).FirstOrDefault(t => t.Id == id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //------ search
        [HttpPost]
        public  ActionResult PhoneSearch (string PhoneName) { 
            // phones list by search condition, price desc sorted
            var allPhones = db.Phones.Include(a => a.Company).Where(b => b.Name.Contains(PhoneName)).OrderByDescending(t => t.Price).ToList();
            if (allPhones.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(allPhones);  
        }
    }
}
