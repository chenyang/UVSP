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
    public class GreservationController : Controller
    {
        private AIOPContext db = new AIOPContext();

        //
        // GET: /Greservation/
        [InitializeSimpleMembership]

        [Authorize(Roles = "Super_user")]
        public ActionResult Index()
        {
           
            ReservationModels reservationModel = new ReservationModels();

            var query = from res in db.RESERVATIONs
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


            reservationModel.CheckBoxItems=new List<CheckBoxItem>();
            foreach (var ea in reservationModel.reservations_enattentes)
            {
                reservationModel.CheckBoxItems.Add(new CheckBoxItem { IsChecked=false});
            }

            return View(reservationModel);

        }

       
        [HttpPost, Authorize(Roles = "Super_user")]
        public ActionResult Accepter_OR_Refuser(string submitButton1, string submitButton2, ReservationModels reservationModel)
        {

            List<RESERVATION> liste_res_a_modifier = new List<RESERVATION>();
            foreach (var c in reservationModel.CheckBoxItems)
            {
                System.Diagnostics.Debug.WriteLine("HA" + c.IsChecked+"HA"+c.Code);

                if (c.IsChecked == true)
                {
                    liste_res_a_modifier.Add(db.RESERVATIONs.Find(c.Code));
                }
            }
            var button_name = submitButton1 ?? submitButton2;//NOTE THIS LINE!!
            if (button_name.Equals("flag_accepter"))//Decide wether can accept or not?
            {

                int count_fail = 0;

                for (int i = 0; i < liste_res_a_modifier.Count(); i++)
                {

                    RESERVATION res_a_decider = liste_res_a_modifier.ElementAt(i);

                    var vailde_res = from rs in db.RESERVATIONs
                                     where rs.STATUS == 1
                                     select rs;


                    List<RESERVATION> vailde_res_liste = vailde_res.ToList<RESERVATION>();

                    if (vailde_res_liste.Count() == 0)
                    {
                        res_a_decider.STATUS = 1;
                    }

                    for (int j = 0; j < vailde_res_liste.Count(); j++)
                    {

                        if (res_a_decider.DATE_RESERVATION == vailde_res_liste.ElementAt(j).DATE_RESERVATION)
                        {
                            if (res_a_decider.ID_CRENEAU == vailde_res_liste.ElementAt(j).ID_CRENEAU)
                            {
                                if (res_a_decider.ID_SALLE == vailde_res_liste.ElementAt(j).ID_SALLE)
                                {
                                    res_a_decider.STATUS = 0;
                                    count_fail++;
                                    break;
                                }

                            }

                        }
                        res_a_decider.STATUS = 1;  
                    }


                    try{
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

                System.Diagnostics.Debug.WriteLine("fails in total is " + count_fail);

            }
            else if (button_name.Equals("flag_refuser"))
            {
                foreach (var r in liste_res_a_modifier)
                {
                    r.STATUS = 2;

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
                   
                }
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}