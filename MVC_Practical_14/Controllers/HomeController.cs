using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace MVC_Practical_14.Controllers
{
    public class HomeController : Controller
    {
        private UserDbContext db = new UserDbContext();

        // GET: Home
        public async Task<ActionResult> Index(int id =1)
        {
            return View(db.Users.OrderBy(e=> e.Name).ToPagedList(id,10));
        }

        // GET: Home/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return View("Error");
            }
            return View(user);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View(new User());
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,DOB,Age")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Home/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return View("Error");
            }
            return View(user);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DOB,Age")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Home/Delete/5

        public async Task<bool> Delete(int? id)
        {
            if (id == null)
            {
                return false;
            }
            User user = await db.Users.FindAsync(id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public JsonResult Search(string search = "")
        {
            if(search == "")
            {
                return Json(db.Users.ToList());
            }
           return Json(db.Users.Where(e => e.Name.ToLower().StartsWith(search.ToLower()) || search == null), JsonRequestBehavior.AllowGet);
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