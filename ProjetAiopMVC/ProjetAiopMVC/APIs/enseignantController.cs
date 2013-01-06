using ProjetAiopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjetAiopMVC.APIs
{
    public class enseignantController : ApiController
    {
        private AIOPContext db = new AIOPContext();

        [HttpGet]
        public HttpResponseMessage GetAllEnseignants()
        {
            HttpResponseMessage response = null;
            List<API_ENSEIGANT> liste_api_ens = new List<API_ENSEIGANT>();
            List<ENSEIGNANT> liste_ens = db.ENSEIGNANTs.ToList<ENSEIGNANT>();

            foreach (var ens in liste_ens)
            {
                API_ENSEIGANT api_ens = new API_ENSEIGANT();
                api_ens.id_enseigant = ens.ID_ENSEIGNANT;
                api_ens.nom = ens.NOM;
                api_ens.prenom = ens.PRENOM;

                liste_api_ens.Add(api_ens);
            }

          

            response = Request.CreateResponse(HttpStatusCode.OK, liste_api_ens);

            return response;
        }

    }
}
