using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAiopMVC.Models
{
    public class ReservationModels
    {
        public RESERVATION RESERVATION { get; set; }

        public List<RESERVATION> reservations_valides { get; set; }
        public List<RESERVATION> reservations_refuses { get; set; }
        public List<RESERVATION> reservations_enattentes { get; set; }
        public List<CheckBoxItem> CheckBoxItems { get; set; }

        public int getIdEnseignant()
        {
            AIOPContext ap = new AIOPContext();
            string userName = HttpContext.Current.User.Identity.Name;
            List<ENSEIGNANT> listE = new List<ENSEIGNANT>();
            listE = ap.ENSEIGNANTs.ToList();
            foreach (ENSEIGNANT e in listE)
            {
                if (e.LOGIN != null)
                {
                    if (e.LOGIN.Equals(userName))
                    {
                        return e.ID_ENSEIGNANT;
                    }
                }
            }
            return 0;
        }
    }
}