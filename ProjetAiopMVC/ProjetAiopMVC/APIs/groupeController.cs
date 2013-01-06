using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjetAiopMVC.Models;

namespace ProjetAiopMVC.APIs
{
    public class groupeController : ApiController
    {
        private AIOPContext db = new AIOPContext();

        [HttpGet]
        public HttpResponseMessage GetAllGroupes()
        {
            HttpResponseMessage response = null;
            List<API_GROUPE> liste_api_groupe = new List<API_GROUPE>();
            List<GROUPE> liste_groupe = db.GROUPEs.ToList<GROUPE>();

            foreach (var groupe in liste_groupe)
            {
                API_GROUPE api_groupe = new API_GROUPE();
                api_groupe.id_groupe = groupe.ID_GROUPE;
                api_groupe.libelle_groupe = groupe.LIBELLE_GROUPE;

                liste_api_groupe.Add(api_groupe);
            }



            response = Request.CreateResponse(HttpStatusCode.OK, liste_api_groupe);

            return response;
        }
    }
}
