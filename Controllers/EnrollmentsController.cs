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
    public class EnrollmentsController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Enrollments
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.CourseSortParm = String.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewBag.TraineeSortParm = sortOrder == "Trainee" ? "trainee_desc" : "Trainee";
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Trainee);

            if (!String.IsNullOrEmpty(searchString))
            {
                enrollments = enrollments.Where(e => e.Course.CourseName.Contains(searchString)
                || e.Trainee.TraineeName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "course_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Course.CourseName);
                    break;
                case "Trainee":
                    enrollments = enrollments.OrderBy(e => e.Trainee.TraineeName);
                    break;
                case "trainee_desc":
                    enrollments = enrollments.OrderByDescending(e => e.Trainee.TraineeName);
                    break;
                default:
                    enrollments = enrollments.OrderBy(e => e.Course.CourseName);
                    break;
            }
            return View(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,TraineeID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", enrollment.CourseID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeName", enrollment.TraineeID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", enrollment.CourseID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeName", enrollment.TraineeID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,TraineeID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", enrollment.CourseID);
            ViewBag.TraineeID = new SelectList(db.Trainees, "TraineeID", "TraineeName", enrollment.TraineeID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
