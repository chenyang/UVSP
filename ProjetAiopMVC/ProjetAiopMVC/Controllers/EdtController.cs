using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetAiopMVC.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace ProjetAiopMVC.Controllers
{
    public class EdtController : Controller
    {
        //
        // GET: /Edt/
        private AIOPContext db = new AIOPContext();

        public ActionResult Index(int id =0)
        {
            //
            bool ensSelectionEstNul = true;
            ENSEIGNANT enseignant = new ENSEIGNANT();
            ReservationModels reservationModel = new ReservationModels(); // permet d'effectuer une requete pour avoir l'emploi du temps

            // On charge l'utilisateur choisi par le select enseignant
            ViewBag.listeEnseignant = db.ENSEIGNANTs.ToList();
            if (id != 0)
            {
                enseignant = db.ENSEIGNANTs.Find(id);
                if (enseignant != null)
                {
                    ensSelectionEstNul = false;
                }
            }

            // Si l'enseignant n'est pas nul alors on fait la requête pour avoir son emploi du temps
            // Sinon on charge l'emploi du temps de l'utilisateur courant
            if (!ensSelectionEstNul)
            {
                // requete permettant de récupérer l'emploi du temps de l'utilisateur sélectionné
                var query = from res in db.RESERVATIONs
                            where res.ENSEIGNEMENT.ENSEIGNANT.LOGIN == enseignant.LOGIN
                            select res;

                var liste_va = from r in query
                               where r.STATUS == 1
                               select r;
                reservationModel.reservations_valides = liste_va.ToList<RESERVATION>();
            }
            else
            {
                // requete permettant de récupérer l'emploi du temps de l'utilisateur connecté
                string username = WebSecurity.CurrentUserName;
                
                var query = from res in db.RESERVATIONs
                            where res.ENSEIGNEMENT.ENSEIGNANT.LOGIN == username
                            select res;

                var liste_va = from r in query
                               where r.STATUS == 1
                               select r;
                reservationModel.reservations_valides = liste_va.ToList<RESERVATION>();
            } 
      

            // Permet de structurer le code que l'on transmettra au Jquery pour l'affichage

            string boucle_nom_du_cours;
            string[] boucle_heure_minute_debut;
            string[] boucle_heure_minute_fin;
            string boucle_date_debut_annee;
            string boucle_date_debut_mois;
            string boucle_date_debut_jour;

            string ensemble_reservation = "{ header: {left: 'prev,next today', center: 'title',right: 'month,agendaWeek,agendaDay' },editable: false, events: [";

            int nombre_iteration = reservationModel.reservations_valides.Count;
            int iteration_courant = 0;

            foreach (RESERVATION i in reservationModel.reservations_valides)
            {
                iteration_courant++;
                boucle_nom_du_cours = i.ENSEIGNEMENT.COUR.LIBELLE_COURS;
                boucle_date_debut_annee = i.DATE_RESERVATION.Year.ToString();
                boucle_date_debut_mois = (i.DATE_RESERVATION.Month-1).ToString();
                boucle_date_debut_jour = i.DATE_RESERVATION.Day.ToString();
                boucle_heure_minute_debut = i.CRENAUX.HEURE_DEBUT.Split(new string[] { "h" }, StringSplitOptions.None);
                boucle_heure_minute_fin = i.CRENAUX.HEURE_FIN.Split(new string[] { "h" }, StringSplitOptions.None);

                if (iteration_courant < nombre_iteration)
                {

                    ensemble_reservation += "{ title: '" + boucle_nom_du_cours + "', " +
                        "start : new Date(" +
                        boucle_date_debut_annee + "," + boucle_date_debut_mois + "," + boucle_date_debut_jour + "," + boucle_heure_minute_debut[0] + "," + boucle_heure_minute_debut[1] + ")," +
                        "end : new Date(" +
                        boucle_date_debut_annee + "," + boucle_date_debut_mois + "," + boucle_date_debut_jour + "," + boucle_heure_minute_fin[0] + "," + boucle_heure_minute_fin[1] + ")," +
                        "allDay:false },";

                }
                else
                {
                    ensemble_reservation += "{ title: '" + boucle_nom_du_cours + "', " +
                                            "start : new Date(" +
                                            boucle_date_debut_annee + "," + boucle_date_debut_mois + "," + boucle_date_debut_jour + "," + boucle_heure_minute_debut[0] + "," + boucle_heure_minute_debut[1] + ")," +
                                            "end : new Date(" +
                                            boucle_date_debut_annee + "," + boucle_date_debut_mois + "," + boucle_date_debut_jour + "," + boucle_heure_minute_fin[0] + "," + boucle_heure_minute_fin[1] + ")," +
                                            "allDay:false }";
                }

            }
            ensemble_reservation = ensemble_reservation + "]}";
            ViewData["EDT"] = ensemble_reservation;
            //ViewBag.EDT = ensemble_reservation;


            ViewData["Enseignantss"] = new SelectList(db.ENSEIGNANTs.ToList(),"ID_ENSEIGNANT", "NOM" );
            ViewBag.listeenseignant1 = new SelectList(db.ENSEIGNANTs.ToList(), "NOM", "LOGIN");
            // fin de la fonction, on retourne la vue
            return View();
        }

        //
        // GET: /Edt/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Edt/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Edt/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Edt/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Edt/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Edt/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Edt/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
