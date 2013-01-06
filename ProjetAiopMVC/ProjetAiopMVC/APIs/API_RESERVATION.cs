using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAiopMVC.APIs
{
    public class API_RESERVATION
    {
        public int id_reservation { get; set; }
        public string numero_salle { get; set; }
        public string heure_debut { get; set; }
        public string heure_fin { get; set; }
        public string description_enseignement { get; set; }
        public DateTime date_reservation { get; set; }
        public Nullable <int> status { get; set; }
    }
}