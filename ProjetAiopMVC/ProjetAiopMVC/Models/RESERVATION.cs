//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjetAiopMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class RESERVATION
    {
        public RESERVATION()
        {
            this.CARACTERISTIQUEs = new HashSet<CARACTERISTIQUE>();
        }
    
        public int ID_RESERVATION { get; set; }

        public int ID_SALLE { get; set; }
        public int ID_CRENEAU { get; set; }
        public int ID_ENSEIGNEMENT { get; set; }
        public string DATE_STRING { get; set; }

        public System.DateTime DATE_RESERVATION { get; set; }

        public Nullable<int> STATUS { get; set; }
    
        public virtual CRENAUX CRENAUX { get; set; }
        public virtual ENSEIGNEMENT ENSEIGNEMENT { get; set; }
        public virtual SALLE SALLE { get; set; }
        public virtual ICollection<CARACTERISTIQUE> CARACTERISTIQUEs { get; set; }
    }
}
