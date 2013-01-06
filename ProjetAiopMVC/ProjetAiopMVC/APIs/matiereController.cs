using ProjetAiopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjetAiopMVC.APIs
{
    public class matiereController : ApiController
    {
        private AIOPContext db = new AIOPContext();

        [HttpGet]
        public HttpResponseMessage GetAllMatieres()
        {
            HttpResponseMessage response = null;
            List<API_MATIERE> liste_api_mat = new List<API_MATIERE>();
            List<MATIERE> liste_mat = db.MATIEREs.ToList<MATIERE>();

            foreach (var mat in liste_mat)
            {
                API_MATIERE api_mat = new API_MATIERE();
                api_mat.id_matiere = mat.ID_MATIERE;
                api_mat.libelle_matiere = mat.LIBELLE_MATIERE;
                if (mat.UE != null)
                {
                    api_mat.libelle_ue = mat.UE.LIBELLE_UE;
                }
                if (mat.ENSEIGNANT != null)
                {
                    api_mat.info_enseignant = mat.ENSEIGNANT.NOM + " " + mat.ENSEIGNANT.PRENOM;

                }

                liste_api_mat.Add(api_mat);
            }



            response = Request.CreateResponse(HttpStatusCode.OK, liste_api_mat);

            return response;
        }

    }
}
