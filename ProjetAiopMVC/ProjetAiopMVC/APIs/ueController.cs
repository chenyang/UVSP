using ProjetAiopMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjetAiopMVC.APIs
{
    public class ueController : ApiController
    {

        private AIOPContext db = new AIOPContext();

        [HttpGet]
        public HttpResponseMessage GetAllUes()
        {
            HttpResponseMessage response = null;
            List<API_UE> liste_api_ue = new List<API_UE>();
            List<UE> liste_ue = db.UEs.ToList<UE>();

            foreach (var ue in liste_ue)
            {
                API_UE api_ue = new API_UE();
                api_ue.id_ue = ue.ID_UE;
                if (ue.ENSEIGNANT != null)
                {
                    api_ue.ens_responsable = ue.ENSEIGNANT.NOM + " " + ue.ENSEIGNANT.PRENOM;
                }
                api_ue.libelle_ue = ue.LIBELLE_UE;

                liste_api_ue.Add(api_ue);
            }

            response = Request.CreateResponse(HttpStatusCode.OK, liste_api_ue);

            return response;
        }


        [HttpPost]
        public HttpResponseMessage addUE(UE ue)
        {
            HttpResponseMessage response = null;
            //System.Diagnostics.Debug.WriteLine("ue=" + ue.LIBELLE_UE);
            //System.Diagnostics.Debug.WriteLine("!!" + ue.ID_ENSEIGNANT);
            if (ue.LIBELLE_UE!= null&&ue.ID_ENSEIGNANT!=0)
            {
                try
                {
                    
                    db.UEs.Add(ue);
                    ue.ENSEIGNANT = db.ENSEIGNANTs.Find(ue.ID_ENSEIGNANT);
                    db.SaveChanges();

                    
                    API_UE api_ue = new API_UE();
                    api_ue.id_ue = ue.ID_UE;
                    api_ue.libelle_ue = ue.LIBELLE_UE;
                    if (ue.ENSEIGNANT != null)
                    {
                        api_ue.ens_responsable = ue.ENSEIGNANT.NOM + " " + ue.ENSEIGNANT.PRENOM;
                    }
                     
                    response = Request.CreateResponse(HttpStatusCode.Created, api_ue);
                }
                catch (Exception dbEx)
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, dbEx.ToString()); 
                }

            }
            else
            {
                response=Request.CreateResponse(HttpStatusCode.BadRequest,"You should provide correct information");
            }

            return response; 
        }

    }
}
