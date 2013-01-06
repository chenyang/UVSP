using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetAiopMVC.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ProjetAiopMVC.Controllers
{
    public class ReservationController : Controller
    {
        private AIOPContext db = new AIOPContext();

        //
        // GET: /Reservation/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MesReservations()
        {
            //Add logic to verify the view of enseignant..
            System.Diagnostics.Debug.WriteLine(User.Identity.Name);


            ReservationModels reservationModel = new ReservationModels();

            int MY_ID_ENS = reservationModel.getIdEnseignant();

            var query = from res in db.RESERVATIONs
                        where res.ENSEIGNEMENT.ENSEIGNANT.ID_ENSEIGNANT == MY_ID_ENS
                        select res;

            var liste_va = from r in query
                           where r.STATUS == 1
                           select r;
            reservationModel.reservations_valides = liste_va.ToList<RESERVATION>();

            var liste_ea = from r in query
                           where r.STATUS == 0
                           select r;
            reservationModel.reservations_enattentes = liste_ea.ToList<RESERVATION>();

            var liste_rf = from r in query
                           where r.STATUS == 2
                           select r;
            reservationModel.reservations_refuses = liste_rf.ToList<RESERVATION>();

            return View(reservationModel);
        }



        /***AJOUTER/SOUMETTRE UNE RESERVATION***/
        public ActionResult Ajouter()
        {

            ViewBag.liste_salles = new SelectList(db.SALLEs, "ID_SALLE", "NUMERO_SALLE");//"ID_XX" in selectlist is a must coloum in db.XX
            ViewBag.liste_creneaux = new SelectList(db.CRENAUXes, "ID_CRENEAU", "HEURE_DEBUT_AND_HEURE_FIN");

            ReservationModels reservationModel=new ReservationModels();
            int ID_ENSEIGANT_ACTUEL = reservationModel.getIdEnseignant();

            var query = from ensm in db.ENSEIGNEMENTs
                                where ensm.ENSEIGNANT.ID_ENSEIGNANT== ID_ENSEIGANT_ACTUEL
                                select ensm;

            List<ENSEIGNEMENT> liste_ensm = query.ToList<ENSEIGNEMENT>();

            ViewBag.liste_enseignements = new SelectList(liste_ensm, "ID_ENSEIGNEMENT", "DESCRIPTION_ENSEIGNEMENT");
            return View();
        }
        


        [HttpPost]
        public ActionResult Soumettre(ReservationModels reservationModel)
        {
           
            //if (ModelState.IsValid)
            if(reservationModel.RESERVATION.ID_CRENEAU!=0&&reservationModel.RESERVATION.ID_ENSEIGNEMENT!=0&&reservationModel.RESERVATION.DATE_STRING!=null&&reservationModel.RESERVATION.ID_SALLE!=0)
            {
                try
                {
                    reservationModel.RESERVATION.STATUS = 0;
                    DateTime temp = reservationModel.RESERVATION.DATE_RESERVATION;
                    reservationModel.RESERVATION.DATE_RESERVATION = DateTime.ParseExact(reservationModel.RESERVATION.DATE_STRING, "dd/MM/yyyy", null);
                    System.Diagnostics.Debug.WriteLine(reservationModel.RESERVATION.DATE_RESERVATION.ToString());


                    db.RESERVATIONs.Add(reservationModel.RESERVATION);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }

                    return RedirectToAction("Index");

                }catch(FormatException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    return RedirectToAction("Erreur", new { error_message = "Internal Server Error", REQUEST_PATH="/Reservation/Ajouter"});
                }
            }

            return RedirectToAction("Erreur", new { error_message = "vous devez fournir des informations correctes", REQUEST_PATH = "/Reservation/Ajouter" });
        }


        [Authorize]
        public ActionResult Erreur(string error_message, string REQUEST_PATH)
        {
            ViewBag.error_message = error_message;
            ViewBag.REQUEST_PATH=REQUEST_PATH;
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult test()
        {
            ReservationModels rs = new ReservationModels();
            ViewBag.model = rs;
            return View();
        }
    }
}