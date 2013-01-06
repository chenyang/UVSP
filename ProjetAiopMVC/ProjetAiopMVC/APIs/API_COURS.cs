using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAiopMVC.APIs
{
    public class API_COURS
    {
        public int id_cours { get; set; }
        public string libelle_matiere { get; set; }
        public string libelle_cours { get; set; }
        public string type_de_cours { get; set; }
    }
}