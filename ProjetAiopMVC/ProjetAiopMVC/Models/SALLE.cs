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
    
    public partial class SALLE
    {
        public SALLE()
        {
            this.RESERVATIONs = new HashSet<RESERVATION>();
            this.CARACTERISTIQUEs = new HashSet<CARACTERISTIQUE>();
        }
    
        public int ID_SALLE { get; set; }
        public int ID_BATIMENT { get; set; }
        public string NUMERO_SALLE { get; set; }
    
        public virtual BATIMENT BATIMENT { get; set; }
        public virtual ICollection<RESERVATION> RESERVATIONs { get; set; }
        public virtual ICollection<CARACTERISTIQUE> CARACTERISTIQUEs { get; set; }
    }
}
