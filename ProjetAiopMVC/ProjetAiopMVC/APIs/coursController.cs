using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjetAiopMVC.Models;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace ProjetAiopMVC.APIs
{
   public class coursController : ApiController
    {
        private AIOPContext db = new AIOPContext();

        [HttpPost]
        public HttpResponseMessage addCours(COUR cour)
        {

            HttpResponseMessage response = null;
            if (cour.ID_MATIERE!=0&&cour.ID_TYPE_DE_COURS!=0&&cour.LIBELLE_COURS!=null)
            {
                try
                {
                    db.COURS.Add(cour);
                    cour.MATIERE = db.MATIEREs.Find(cour.ID_MATIERE);
                    cour.TYPECOUR = db.TYPECOURS.Find(cour.ID_TYPE_DE_COURS);
                    db.SaveChanges();

                    
                    //var objectContext = ((IObjectContextAdapter)db).ObjectContext;
                    //objectContext.Refresh(RefreshMode.StoreWins, db.COURS);

                    API_COURS api_cours = new API_COURS();
                    api_cours.id_cours = cour.ID_COURS;
                    api_cours.libelle_cours = cour.LIBELLE_COURS;
                    if (cour.TYPECOUR != null)
                    {
                        api_cours.type_de_cours = cour.TYPECOUR.LIBELLE_TYPE_DE_COURS;
                    }
                    if (cour.MATIERE != null)
                    {
                        api_cours.libelle_matiere = cour.MATIERE.LIBELLE_MATIERE;
                    }


                    response = Request.CreateResponse(HttpStatusCode.Created, api_cours);
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
