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
    
    public partial class UE
    {
        public UE()
        {
            this.MATIEREs = new HashSet<MATIERE>();
        }
    
        public int ID_UE { get; set; }
        public string LIBELLE_UE { get; set; }
        public Nullable<int> ID_ENSEIGNANT { get; set; }
        public Nullable<int> ID_SEMESTRE { get; set; }
    
        public virtual ENSEIGNANT ENSEIGNANT { get; set; }
        public virtual ICollection<MATIERE> MATIEREs { get; set; }
    }
}
