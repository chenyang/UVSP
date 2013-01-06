using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetAiopMVC.Models;
using ProjetAiopMVC.Filters;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace ProjetAiopMVC.Controllers
{
    /*
     * Note: How to handle [Httppost] if dont use <using form>?
     * Use actionLink or httplink in the View, by passing parameters to Action
     * 
    */
    [InitializeSimpleMembership]
    [Authorize]

    public class SalleController : Controller
    {
        private AIOPContext db = new AIOPContext();


        public ActionResult Index()
        {
            return View();
        }


        /****Consultation des Salles*****/
        [Authorize]
        public ActionResult Detaille(int search =1)//default search =1
            {

                //Default search =1, even we dont pass it if we just type ./Salle/Detaille/
                //Here we dont use id because if we use keyword "id", we will get the following URL automatically to run the request:/Salle/Detaille/10. Not good for loading images paths..
                //What we are now using is: /Salle/Detaille?search=10. We can also use Html Helper @Html.ActionLink in the view
                /*For other details information, even model, we can attach them to ViewBag in the view Page.
                 Note that I'm not sure whether this is the best way but it works. The disadvantage is that we can not debug it at the compilation stage.
                 If For the view it has just one model to return, we should better attach it as "strongly-typed" view. (By associating a model)
                */

                var salles = db.SALLEs.Include(s => s.BATIMENT);

                SALLE salle = db.SALLEs.Find(search);

                if (salle == null)
                {
                    return HttpNotFound();
                }

                List<string> list_libelle_cars = new List<string>();
                foreach (var car in salle.CARACTERISTIQUEs)
                {
                    list_libelle_cars.Add(car.LIBELLE_CARACTERISTIQUE);
                }

                ViewBag.MY_NUMERO_SALLE = salle.NUMERO_SALLE;
                ViewBag.MY_ID_SALLE = salle.ID_SALLE;
                ViewBag.MY_LIBELLE_BATIMENT = salle.BATIMENT.LIBELLE_BATIMENT;
                ViewBag.MY_LIBELLE_CARS = list_libelle_cars;

                return View(salles.ToList());
            }



        /***SUPPRIMER LES SALLES*****/
        [Authorize(Roles = "Super_user")]
        public ActionResult Supprimer(int ID_SALLE = 0)//ID_SALLE default 0
        {
            ViewBag.listeSalles = db.SALLEs;

            if (ID_SALLE != 0)
            {
                SALLE salle = db.SALLEs.Find(ID_SALLE);
                SalleModels salleModel = new SalleModels();
                salleModel.SALLE = salle;

                List<CARACTERISTIQUE> liste_cars = new List<CARACTERISTIQUE>();
                liste_cars = salle.CARACTERISTIQUEs.ToList();
                salleModel.LISTE_CARACTERISTIQUE_DEFAULT = liste_cars;

                return View(salleModel);
            }

            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Super_user")]
        public ActionResult Supprimer(SalleModels salleModel)
        {

            int id_salle = salleModel.SALLE.ID_SALLE;
            SALLE salle = db.SALLEs.Find(id_salle);
            if (salle != null)
            {
                //Remove relations..: cars and enseignements..
                var cars = salle.CARACTERISTIQUEs;
                foreach (var item in cars.ToList())
                {
                    salle.CARACTERISTIQUEs.Remove(item);
                }

                var query = from res in db.RESERVATIONs
                            where res.ID_SALLE == salle.ID_SALLE
                            select res;
                foreach (var item in query.ToList<RESERVATION>())
                {
                    if (item.STATUS != 1)//IF not occupied..
                    {
                        db.RESERVATIONs.Remove(item);
                    }
                    else
                    {
                        return RedirectToAction("Supprimer");
                    }
                }

                try
                {
                    db.SALLEs.Remove(salle);
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
            }

            return RedirectToAction("Supprimer");
        }



        /*****AJOUTER UNE SALLE*****/
        [Authorize(Roles = "Super_user")]
        public ActionResult Ajouter()
        {
            //Set Viewbags list
            ViewBag.SELECTLIST_BATIMENT = new SelectList(db.BATIMENTs, "ID_BATIMENT", "LIBELLE_BATIMENT");//"ID_XX" in selectlist is a must coloum in db.BATIMENTs
            ViewBag.SELECTLIST_CARACTERISTIQUE = new SelectList(db.CARACTERISTIQUEs, "ID_CARACTERISTIQUE", "LIBELLE_CARACTERISTIQUE");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super_user")]
        public ActionResult Ajouter(SalleModels salleModel)
        {
            if (ModelState.IsValid)
            {
                SALLE salle = salleModel.SALLE;
                salle.ID_BATIMENT = salleModel.SALLE.ID_BATIMENT;
                salle.NUMERO_SALLE = salleModel.SALLE.NUMERO_SALLE;

                db.SALLEs.Add(salle);

                //assoicate caracteristiques to SALLE. Note here is many-to-many relationship
                if (salleModel.ID_CAR_1 != 0)
                    salle.CARACTERISTIQUEs.Add(db.CARACTERISTIQUEs.Find(salleModel.ID_CAR_1));

                if (salleModel.ID_CAR_2 != 0)
                    salle.CARACTERISTIQUEs.Add(db.CARACTERISTIQUEs.Find(salleModel.ID_CAR_2));

                if (salleModel.ID_CAR_3 != 0)
                    salle.CARACTERISTIQUEs.Add(db.CARACTERISTIQUEs.Find(salleModel.ID_CAR_3));


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

                return RedirectToAction("Detaille");

            }
                return RedirectToAction("Ajouter");
        }


        [HttpPost, Authorize(Roles = "Super_user")]
        public ActionResult AjouterCaracteristique(SalleModels salleModel)
        {
            
            List<CARACTERISTIQUE> liste_cars_apres = new List<CARACTERISTIQUE>();
            for (int i = 0; i < ((List<CARACTERISTIQUE>)Session["liste_cars_temp"]).Count(); i++)
            {
                liste_cars_apres.Add(((List<CARACTERISTIQUE>)Session["liste_cars_temp"]).ElementAt(i));
            }

            ((List<CARACTERISTIQUE>)Session["liste_cars_temp"]).Add(db.CARACTERISTIQUEs.Find(salleModel.ID_CARACTERISTIQUE_AJOUTE));
            return RedirectToAction("Modifier", new {ID_SALLE=salleModel.SALLE.ID_SALLE});
        }


        [Authorize(Roles = "Super_user")]
        public ActionResult Modifier(int ID_SALLE = 0)
        {

            //Viewbags..
            ViewBag.LISTE_BD_CARACTERISTIQUE = new SelectList(db.CARACTERISTIQUEs, "ID_CARACTERISTIQUE", "LIBELLE_CARACTERISTIQUE", string.Empty);
            ViewBag.LISTE_BD_BATIMENT = new SelectList(db.BATIMENTs, "ID_BATIMENT", "LIBELLE_BATIMENT");
            ViewBag.LISTE_BD_SALLE = db.SALLEs.ToList();


            if (ID_SALLE != 0)
            {
                SalleModels salleModel = new SalleModels();
                SALLE salle = db.SALLEs.Find(ID_SALLE);

                salleModel.SALLE = salle;


                if (Session["liste_cars_temp"] == null)
                {
                    salleModel.LISTE_CARACTERISTIQUE_DEFAULT = salle.CARACTERISTIQUEs.ToList();
                }
                else
                {
                    salleModel.LISTE_CARACTERISTIQUE_DEFAULT = ((List<CARACTERISTIQUE>)Session["liste_cars_temp"]).ToList();
                }
                List<SelectList> temp_selectlist = new List<SelectList>();
                for (int i = 0; i < salleModel.LISTE_CARACTERISTIQUE_DEFAULT.Count(); i++)
                {
                    SelectList temp = new SelectList(db.CARACTERISTIQUEs, "ID_CARACTERISTIQUE", "LIBELLE_CARACTERISTIQUE", salleModel.LISTE_CARACTERISTIQUE_DEFAULT.ElementAt(i).ID_CARACTERISTIQUE);
                    temp_selectlist.Add(temp);
                }
                salleModel.SELECTLIST_CARS = temp_selectlist;


                return View(salleModel);
            }

            return View();
        }


        [Authorize(Roles = "Super_user")]
        [HttpPost]
        public ActionResult Modifier_Confirm(SalleModels salleModel)
        {
            
                SALLE salle = db.SALLEs.Find(salleModel.SALLE.ID_SALLE);

            if (salle != null)
            {
                salle.ID_BATIMENT = salleModel.SALLE.ID_BATIMENT;
                salle.NUMERO_SALLE = salleModel.SALLE.NUMERO_SALLE;

                for (int i = 0; i < salleModel.LISTE_CARACTERISTIQUE_DEFAULT.Count(); i++)
                {
                    System.Diagnostics.Debug.WriteLine("!!!!!!!" + salleModel.LISTE_CARACTERISTIQUE_DEFAULT[i].ID_CARACTERISTIQUE);
                }

                foreach (var car in salle.CARACTERISTIQUEs.ToList())
                {
                    salle.CARACTERISTIQUEs.Remove(car);
                }

                foreach (var car in salleModel.LISTE_CARACTERISTIQUE_DEFAULT.ToList())
                {
                    salle.CARACTERISTIQUEs.Add(db.CARACTERISTIQUEs.Find(car.ID_CARACTERISTIQUE));
                }


                try
                {
                    db.Entry(salle).State = EntityState.Modified;
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


                Session.Abandon();
            }

            return RedirectToAction("Modifier", new {ID_SALLE=0 });
        }


        [Authorize]
        public ActionResult Erreur()
        {
            return View();
        }


        [Authorize(Roles = "Super_user")]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}