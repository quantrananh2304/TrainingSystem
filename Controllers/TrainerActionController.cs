using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingProject.Models;

namespace TrainingProject.Controllers
{
    public class TrainerActionController : Controller
    {
        private TrainingDBEntities db = new TrainingDBEntities();

        public ActionResult Profile()
        {
            var userName = User.Identity.Name;

            var trainer = (from t in db.Trainers where t.TrainerName.Equals(userName) select t).FirstOrDefault();
            return View(trainer);
        }

        [Authorize(Roles = "Trainer")]
        public ActionResult MyTopics()
        {
            var userName = User.Identity.Name;

            var mytopics = from t in db.Topics where t.Trainer.TrainerName.Equals(userName) select t;
            return View(mytopics.ToList());
        }

        [Authorize(Roles ="Trainer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainer trainer = db.Trainers.Find(id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Trainer")]
        public ActionResult Edit([Bind(Include = "TrainerID, TrainerName, WorkingPlace, Telephone, Email")] Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(trainer);
        }

        // GET: Trainer
        public ActionResult Index()
        {
            return View();
        }
    }
}