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
    public class TraineesController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Trainees
        public ActionResult Index(string sortOrder)
        {
            ViewBag.TraineeSortParm = String.IsNullOrEmpty(sortOrder) ? "trainee_desc" : "";
            ViewBag.AgeSortParm = sortOrder == "Age" ? "age_desc" : "Age";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "addess_desc" : "Address";

            var trainees = from t in db.Trainees select t;

            switch(sortOrder)
            {
                case "trainee_desc":
                    trainees = trainees.OrderByDescending(t => t.TraineeName);
                    break;
                case "Age":
                    trainees = trainees.OrderBy(t => t.TraineeAge);
                    break;
                case "age_desc":
                    trainees = trainees.OrderByDescending(t => t.TraineeAge);
                    break;
                case "Address":
                    trainees = trainees.OrderBy(t => t.TraineeAddress);
                    break;
                case "address_desc":
                    trainees = trainees.OrderByDescending(t => t.TraineeAddress);
                    break;
                default:
                    trainees = trainees.OrderBy(t => t.TraineeName);
                    break;
            }
            return View(db.Trainees.ToList());
        }

        // GET: Trainees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            //ViewBag.EnrollmentID = new SelectList(db.Trainees, "CourseID", "CourseName", trainee.Enrollments);
            return View(trainee);
        }

        // GET: Trainees/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trainees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "TraineeID,TraineeName,TraineeAge,TraineeProgrammingLanguage,TraineeDoB,TraineePhone,TraineeAddress")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                db.Trainees.Add(trainee);
                db.SaveChanges();

                AuthenController.CreateAccount(trainee.TraineeName, "123456", "Trainee");
                return RedirectToAction("Index");
            }

            return View(trainee);
        }

        // GET: Trainees/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }

        // POST: Trainees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "TraineeID,TraineeName,TraineeAge,TraineeProgrammingLanguage,TraineeDoB,TraineePhone,TraineeAddress")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainee);
        }

        // GET: Trainees/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }

        // POST: Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainee trainee = db.Trainees.Find(id);
            db.Trainees.Remove(trainee);
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
