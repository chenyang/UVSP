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
    public class enseignementsController : ApiController
    {
        private AIOPContext db = new AIOPContext();
        
        [HttpGet]
        public HttpResponseMessage GetEnseignements()
        {
            HttpResponseMessage response = null;
            List<API_ENSEIGNEMENT> liste_api_enseignements = new List<API_ENSEIGNEMENT>();

            List<ENSEIGNEMENT> liste_ensm = db.ENSEIGNEMENTs.ToList<ENSEIGNEMENT>();

            foreach (var ensm in liste_ensm)
            {
                API_ENSEIGNEMENT api_ensm = new API_ENSEIGNEMENT();
                api_ensm.id_enseignement = ensm.ID_ENSEIGNEMENT;
                if (ensm.COUR != null)
                {
                    api_ensm.info_cours = ensm.COUR.LIBELLE_COURS;
                }
                if (ensm.ENSEIGNANT != null)
                {
                    api_ensm.info_enseignant = ensm.ENSEIGNANT.NOM + " " + ensm.ENSEIGNANT.PRENOM;
                }
                if (ensm.GROUPE != null)
                {
                    api_ensm.info_groupe = ensm.GROUPE.LIBELLE_GROUPE;
                }

                liste_api_enseignements.Add(api_ensm);
            }

            response=Request.CreateResponse(HttpStatusCode.OK, liste_api_enseignements);

            return response;
        }



        [HttpPost]
        public HttpResponseMessage addEnseignement(ENSEIGNEMENT ensm)
        {
            HttpResponseMessage response = null;
            if (ensm.ID_COURS != 0 && ensm.ID_ENSEIGNANT != 0 && ensm.ID_GROUPE != 0 && ensm.NB_HEURE_PREVUE != 0)
            {
                //System.Diagnostics.Debug.WriteLine("ue=" + ue.LIBELLE_UE);

                
                try
                {
                    db.ENSEIGNEMENTs.Add(ensm);
                    ensm.ENSEIGNANT = db.ENSEIGNANTs.Find(ensm.ID_ENSEIGNANT);
                    ensm.GROUPE = db.GROUPEs.Find(ensm.ID_GROUPE);
                    ensm.COUR = db.COURS.Find(ensm.ID_COURS);
                    db.SaveChanges();
                    
                    API_ENSEIGNEMENT api_ensm = new API_ENSEIGNEMENT();
                    api_ensm.id_enseignement = ensm.ID_ENSEIGNEMENT;
                    if(ensm.ENSEIGNANT!=null)
                    api_ensm.info_enseignant = ensm.ENSEIGNANT.NOM + " " + ensm.ENSEIGNANT.PRENOM;
                    if (ensm.GROUPE != null)
                    api_ensm.info_groupe = ensm.GROUPE.LIBELLE_GROUPE;
                    if (ensm.COUR != null)
                    api_ensm.info_cours = ensm.COUR.LIBELLE_COURS;
                     
                    response = Request.CreateResponse(HttpStatusCode.Created, api_ensm);
                }
                catch (Exception dbEx)
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, dbEx.ToString());
                }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "You should complete the information");
            }

            return response;
        }



        [HttpDelete]
        public HttpResponseMessage deleteEnseignement(int ID_ENSEIGNEMENT)
        {
            HttpResponseMessage response = null;

            //Check if ID_ENSEIGNEMENT exists in db
            ENSEIGNEMENT ensm = db.ENSEIGNEMENTs.Find(ID_ENSEIGNEMENT);
            if (ensm == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else//IF Enseignement n'est pas null
            {
                List<RESERVATION> liste_res = ensm.RESERVATIONs.ToList<RESERVATION>();

                if (liste_res.Count == 0)//IF ensm has no reservation related
                {
                    try
                    {
                        db.ENSEIGNEMENTs.Remove(ensm);
                        db.SaveChanges();
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }
                    catch (Exception ex)
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
                    }

                }

                else//IF ensm has reservation related
                {

                    int flag = 0;
                    foreach (var res in liste_res)
                    {
                        if (res.STATUS == 1)
                        {
                            response = Request.CreateResponse(HttpStatusCode.Conflict, "L'enseignement est pri!");
                            flag = 1;
                            break;
                        }
                    }

                    if (flag == 0)//IF l'enseignement n'est pas pris..
                    {
                        try
                        {
                            foreach (var res in liste_res)
                            {
                                db.RESERVATIONs.Remove(res);
                                db.ENSEIGNEMENTs.Remove(ensm);
                                db.SaveChanges();
                                response = Request.CreateResponse(HttpStatusCode.OK);
                            }
                        }
                        catch (Exception ex)
                        {
                            response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
                        }
                    }

                }

          }

            return response;
        }



    }
}
