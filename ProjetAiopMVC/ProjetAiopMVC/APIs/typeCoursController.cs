using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjetAiopMVC.Models;

namespace ProjetAiopMVC.APIs
{
    public class typeCoursController : ApiController
    {
        private AIOPContext db = new AIOPContext();

        [HttpGet]
        public HttpResponseMessage GetAllTypeCours()
        {
            HttpResponseMessage response = null;
            List<API_TYPE_COURS> liste_api_tc = new List<API_TYPE_COURS>();
            List<TYPECOUR> liste_tc = db.TYPECOURS.ToList<TYPECOUR>();

            foreach (var tc in liste_tc)
            {
                API_TYPE_COURS api_tc = new API_TYPE_COURS();
                api_tc.id_type_cour = tc.ID_TYPE_DE_COURS;
                api_tc.libelle_type_cour = tc.LIBELLE_TYPE_DE_COURS;

                liste_api_tc.Add(api_tc);
            }



            response = Request.CreateResponse(HttpStatusCode.OK, liste_api_tc);

            return response;
        }

    }
}
