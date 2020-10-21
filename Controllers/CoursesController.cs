using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingProject.Models;

namespace TrainingProject.Controllers
{
    public class CoursesController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Courses
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "courseId_desc" : "";
            ViewBag.CourseNameSortParm = sortOrder == "CourseName" ? "course_name_desc" : "CourseName";
            ViewBag.CatNameSortParm = sortOrder == "CatName" ? "cat_name_desc" : "CatName";
            var courses = db.Courses.Include(c => c.Category);
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.CourseName.Contains(searchString) || c.Category.CategoryName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "courseId_desc":
                    courses = courses.OrderByDescending(c => c.CourseID);
                    break;
                case "CourseName":
                    courses = courses.OrderBy(c => c.CourseName);
                    break;
                case "course_name_desc":
                    courses = courses.OrderByDescending(c => c.CourseName);
                    break;
                case "CatName":
                    courses = courses.OrderBy(c => c.Category.CategoryName);
                    break;
                case "cat_name_desc":
                    courses = courses.OrderByDescending(c => c.Category.CategoryName);
                    break;
                default:
                    courses = courses.OrderBy(c => c.CourseID);
                    break;
            }
            
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CourseID,CourseName,CategoryID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", course.CategoryID);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", course.CategoryID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,CategoryID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", course.CategoryID);
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
