using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetAiopMVC.Models
{
    public class SalleModels
    {
        
        public SALLE SALLE { get; set; }



        public List<CARACTERISTIQUE> LISTE_CARACTERISTIQUE_DEFAULT { get; set; }
        public List<SelectList> SELECTLIST_CARS {get; set; }

        [Required]
        public int ID_CARACTERISTIQUE_AJOUTE { get; set; }

        public Nullable<int> ID_CAR_1 { get; set; }
        public Nullable<int> ID_CAR_2 { get; set; }
        public Nullable<int> ID_CAR_3 { get; set; }

    }
}