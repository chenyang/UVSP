using ProjetAiopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjetAiopMVC.APIs
{
    public class reservationController : ApiController
    {
         private AIOPContext db = new AIOPContext();

         [HttpGet]
         public HttpResponseMessage GetAllReservations()
         {
             HttpResponseMessage response = null;
             List<API_RESERVATION> liste_api_res = new List<API_RESERVATION>();
             List<RESERVATION> liste_res = db.RESERVATIONs.ToList<RESERVATION>();

             foreach (var res in liste_res)
             {
                 API_RESERVATION api_res = new API_RESERVATION();
                 api_res.id_reservation = res.ID_RESERVATION;
                 api_res.numero_salle = res.SALLE.NUMERO_SALLE;
                 api_res.heure_debut = res.CRENAUX.HEURE_DEBUT;
                 if (res.CRENAUX != null)
                 {
                     api_res.heure_fin = res.CRENAUX.HEURE_FIN;
                 }
                 api_res.status = res.STATUS;
                 api_res.date_reservation = res.DATE_RESERVATION;
                 if (res.ENSEIGNEMENT != null)
                 {
                     api_res.description_enseignement = res.ENSEIGNEMENT.DESCRIPTION_ENSEIGNEMENT;
                 }
                 liste_api_res.Add(api_res);

             }

             response = Request.CreateResponse(HttpStatusCode.OK, liste_api_res);
             return response;
         }


    }
}
