using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAiopMVC.APIs
{
    public class API_ENSEIGNEMENT
    {
        public int id_enseignement { get; set; }
        public string info_cours { get; set; }
        public string info_enseignant { get; set; }
        public string info_groupe { get; set; }
    }
}