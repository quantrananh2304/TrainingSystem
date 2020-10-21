using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingProject.Models;

namespace TrainingProject.Controllers
{
    public class TopicsController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        // GET: Topics
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.TopicSortParm = sortOrder == "Topic" ? "topic_desc" : "Topic";
            ViewBag.CourseSortParm = sortOrder == "Course" ? "course_desc" : "Course";
            ViewBag.TrainerSortParm = sortOrder == "Trainer" ? "trainer_desc" : "Trainer";
            var topics = db.Topics.Include(t => t.Course).Include(t => t.Trainer);

            switch (sortOrder) {
                case "id_desc":
                    topics = topics.OrderByDescending(t => t.TopicID);
                    break;
                case "Topic":
                    topics = topics.OrderBy(t => t.TopicName);
                    break;
                case "topic_desc":
                    topics = topics.OrderByDescending(t => t.TopicName);
                    break;
                case "Course":
                    topics = topics.OrderBy(t => t.Course.CourseName);
                    break;
                case "course_desc":
                    topics = topics.OrderByDescending(t => t.Course.CourseName);
                    break;
                case "Trainer":
                    topics = topics.OrderBy(t => t.Trainer.TrainerName);
                    break;
                case "trainer_desc":
                    topics = topics.OrderByDescending(t => t.Trainer.TrainerName);
                    break;
                default:
                    topics = topics.OrderBy(t => t.TopicID);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.TopicName.Contains(searchString)
                || t.TopicDescription.Contains(searchString)
                || t.Course.CourseName.Contains(searchString)
                || t.Trainer.TrainerName.Contains(searchString));
            }
            return View(topics.ToList());
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        [Authorize(Roles = "Trainer, Admin")]
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerName");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trainer, Admin")]
        public ActionResult Create([Bind(Include = "TopicID,TopicName,TopicDescription,CourseID,TrainerID")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Topics.Add(topic);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                //db.Topics.Add(topic);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", topic.CourseID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerName", topic.TrainerID);
            return View(topic);
        }

        // GET: Topics/Edit/5
        [Authorize(Roles = "Admin, Trainer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", topic.CourseID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerName", topic.TrainerID);
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public ActionResult Edit([Bind(Include = "TopicID,TopicName,TopicDescription,CourseID,TrainerID")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", topic.CourseID);
            ViewBag.TrainerID = new SelectList(db.Trainers, "TrainerID", "TrainerName", topic.TrainerID);
            return View(topic);
        }

        // GET: Topics/Delete/5
        [Authorize(Roles = "Admin, Trainer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Trainer")]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
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
