using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetAiopMVC.Models;
using System.Web.Security;
using WebMatrix.WebData;


namespace ProjetAiopMVC.Controllers
{
    public class ProfilController : Controller
    {
        //
        // GET: /Profil/  Affichage de la page de Mon Profil

        AIOPContext db = new AIOPContext();

        /**
         * Le message error peut être envoyé par l'action verification qui redirige vers cette action
         * 
         * */
        public ActionResult Index(String errorMessage = "")
        {

            ENSEIGNANT ens = new ENSEIGNANT(); // Permet de charger l'utilisateur courant
            ViewBag.PROFIL = ""; // Renseignera le profil de l'utilisateur
            ViewBag.ERROR = errorMessage;
            // On charge le login de l'utilisateur courant
            string userNameCurrentUser = WebSecurity.CurrentUserName;
            // On charge l'enseignant
            List<ENSEIGNANT> listE = new List<ENSEIGNANT>();
            listE = db.ENSEIGNANTs.ToList();

            foreach (ENSEIGNANT e in listE)
            {
                if (e.LOGIN != null)
                {
                    if (e.LOGIN.Equals(userNameCurrentUser))
                    {
                        ens = e;
                    }
                }
            }
            // l'enseignant se trouve dans la variable ens

            // On renseigne le profil de l'utilisateur
            if (isSuperUser(ens))
            {
                ViewBag.PROFIL = "Administrateur";
            }
            else
            {
                ViewBag.PROFIL = "Enseignant";
            }


            // Liste des enseignement que le professeur courant a
            SelectList Malist = new SelectList(db.ENSEIGNANTs.Find(ens.ID_ENSEIGNANT).ENSEIGNEMENTs.ToList(), "ID_COURS", "ID_ENSEIGNEMENT");
            List<SelectListItem> listCours = new List<SelectListItem>(); // la liste des cours
            SelectListItem element; // chaque element de la liste de cours
            foreach (var item in Malist)
            {
                element = new SelectListItem() { Text = db.COURS.Find(Convert.ToInt32(item.Value)).LIBELLE_COURS, Value = item.Value.ToString() };
                listCours.Add(element);
            }
            // envoi à la dropDownList la liste des cours
            ViewData["ListeCours"] = listCours;

            return View(ens);
        }


        /**
         * 
         * 
         * 
         **/
        [HttpPost]
        public ActionResult Verification()
        {
            string form_old_password = Request.Form["OldPassword"];
            string form_new_password = Request.Form["NewPassword"];
            string form_new_password_confirmation = Request.Form["NewPasswordConfirmation"];

            if (form_new_password == null || form_new_password_confirmation == null || form_old_password == null)
            {
                return RedirectToAction("Index", new { errorMessage = "Veuillez renseigner tous les champs mot de passe" });
            }

            if (form_old_password.Length < 6)
            {
                return RedirectToAction("Index", new { errorMessage = "Veuillez entrer votre ancien mot de passe" });
            }
            else
            {
                if (form_new_password != form_new_password_confirmation)
                {
                    return RedirectToAction("Index", new { errorMessage = "Vos mots de passe ne corresponent pas ou il faut entrer un mot de passe de plus de 6 caractères" });
                }
                else // Les nouveau mots de passe correspondent et l'ancien mot de passe est entrée
                {
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, form_old_password, form_new_password);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }
                    if (changePasswordSucceeded)
                    {
                        //return RedirectToAction("Manage");
                        //return Redirect("~/pwdChangeSucess");
                        return RedirectToAction("changePasswordResult", new {statut = true });
                    }
                    else
                    {
                        return RedirectToAction("changePasswordResult", new { statut = false });
                        //return Redirect("string url");
                    }

                }

            }

        }

        /**
         * permet de vérifier si un enseignant est un super user !
         * 
         **/
        private bool isSuperUser(ENSEIGNANT enseignant)
        {
            string[] users;
            users = Roles.FindUsersInRole("Super_user", enseignant.LOGIN);
            if (users != null)
            {
                if (users.Length <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Si statut vaut true = changement réussi
         * si statu vaut false = cahngement échec
         * **/
        public ActionResult changePasswordResult(bool statut)
        {
            ViewBag.STATUT = statut;
            return View();
        }


    }
}
