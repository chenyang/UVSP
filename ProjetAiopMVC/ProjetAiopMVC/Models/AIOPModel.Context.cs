﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AIOPContext : DbContext
    {
        public AIOPContext()
            : base("name=AIOPContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ANNEEETUDE> ANNEEETUDEs { get; set; }
        public DbSet<BATIMENT> BATIMENTs { get; set; }
        public DbSet<CARACTERISTIQUE> CARACTERISTIQUEs { get; set; }
        public DbSet<COUR> COURS { get; set; }
        public DbSet<CRENAUX> CRENAUXes { get; set; }
        public DbSet<ENSEIGNANT> ENSEIGNANTs { get; set; }
        public DbSet<ENSEIGNEMENT> ENSEIGNEMENTs { get; set; }
        public DbSet<GROUPE> GROUPEs { get; set; }
        public DbSet<MATIERE> MATIEREs { get; set; }
        public DbSet<RESERVATION> RESERVATIONs { get; set; }
        public DbSet<SALLE> SALLEs { get; set; }
        public DbSet<SEMESTRE> SEMESTREs { get; set; }
        public DbSet<TYPECOUR> TYPECOURS { get; set; }
        public DbSet<UE> UEs { get; set; }
    }
}