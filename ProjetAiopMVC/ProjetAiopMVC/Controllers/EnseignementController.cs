using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetAiopMVC.Models;

namespace ProjetAiopMVC.Controllers
{
    public class EnseignementController : Controller
    {
        private AIOPContext db = new AIOPContext();

        //
        // GET: /Enseignement/

        public ActionResult Index()
        {
            var enseignements = db.ENSEIGNEMENTs.Include(e => e.COUR).Include(e => e.ENSEIGNANT).Include(e => e.GROUPE);
            return View(enseignements.ToList());
        }

        //
        // GET: /Enseignement/Details/5

        public ActionResult Details(int id = 0)
        {
            ENSEIGNEMENT enseignement = db.ENSEIGNEMENTs.Find(id);
            if (enseignement == null)
            {
                return HttpNotFound();
            }
            return View(enseignement);
        }

        //
        // GET: /Enseignement/Create

        public ActionResult Create()
        {
            ViewBag.ID_COURS = new SelectList(db.COURS, "ID_COURS", "LIBELLE_COURS");
            ViewBag.ID_ENSEIGNANT = new SelectList(db.ENSEIGNANTs, "ID_ENSEIGNANT", "NOM");
            ViewBag.ID_GROUPE = new SelectList(db.GROUPEs, "ID_GROUPE", "LIBELLE_GROUPE");
            return View();
        }

        //
        // POST: /Enseignement/Create

        [HttpPost]
        public ActionResult Create(ENSEIGNEMENT enseignement)
        {
            if (ModelState.IsValid)
            {
                db.ENSEIGNEMENTs.Add(enseignement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_COURS = new SelectList(db.COURS, "ID_COURS", "LIBELLE_COURS", enseignement.ID_COURS);
            ViewBag.ID_ENSEIGNANT = new SelectList(db.ENSEIGNANTs, "ID_ENSEIGNANT", "NOM", enseignement.ID_ENSEIGNANT);
            ViewBag.ID_GROUPE = new SelectList(db.GROUPEs, "ID_GROUPE", "LIBELLE_GROUPE", enseignement.ID_GROUPE);
            return View(enseignement);
        }

        //
        // GET: /Enseignement/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ENSEIGNEMENT enseignement = db.ENSEIGNEMENTs.Find(id);
            if (enseignement == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_COURS = new SelectList(db.COURS, "ID_COURS", "LIBELLE_COURS", enseignement.ID_COURS);
            ViewBag.ID_ENSEIGNANT = new SelectList(db.ENSEIGNANTs, "ID_ENSEIGNANT", "NOM", enseignement.ID_ENSEIGNANT);
            ViewBag.ID_GROUPE = new SelectList(db.GROUPEs, "ID_GROUPE", "LIBELLE_GROUPE", enseignement.ID_GROUPE);
            return View(enseignement);
        }

        //
        // POST: /Enseignement/Edit/5

        [HttpPost]
        public ActionResult Edit(ENSEIGNEMENT enseignement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enseignement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_COURS = new SelectList(db.COURS, "ID_COURS", "LIBELLE_COURS", enseignement.ID_COURS);
            ViewBag.ID_ENSEIGNANT = new SelectList(db.ENSEIGNANTs, "ID_ENSEIGNANT", "NOM", enseignement.ID_ENSEIGNANT);
            ViewBag.ID_GROUPE = new SelectList(db.GROUPEs, "ID_GROUPE", "LIBELLE_GROUPE", enseignement.ID_GROUPE);
            return View(enseignement);
        }

        //
        // GET: /Enseignement/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ENSEIGNEMENT enseignement = db.ENSEIGNEMENTs.Find(id);
            if (enseignement == null)
            {
                return HttpNotFound();
            }
            return View(enseignement);
        }

        //
        // POST: /Enseignement/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ENSEIGNEMENT enseignement = db.ENSEIGNEMENTs.Find(id);
            db.ENSEIGNEMENTs.Remove(enseignement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}