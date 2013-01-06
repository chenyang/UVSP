using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetAiopMVC.Models;

using System.Transactions;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ProjetAiopMVC.Filters;

namespace ProjetAiopMVC.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class EnseignantController : Controller
    {
        private AIOPContext db = new AIOPContext();
        private AccountController dak = new AccountController();
        private UsersContext userC = new UsersContext();

        //
        // GET: /Enseignant/
        [Authorize(Roles = "Super_user")]
        public ActionResult Index()
        {
            return View(db.ENSEIGNANTs.ToList());
        }


        //
        // Get : /enseignant/Visaenseignant
        // If no parameter is passed, we put a 0 default !
        [Authorize(Roles = "Super_user")]
        public ActionResult Visaenseignant(int id = 0)
        {
            ViewBag.numeroEnseignant = id;

            return View(db.ENSEIGNANTs.ToList());
        }



        //
        // GET: /Enseignant/Details/5
        [Authorize(Roles = "Super_user")]
        public ActionResult Details(int id = 0)
        {
            ENSEIGNANT enseignant = db.ENSEIGNANTs.Find(id);
            if (enseignant == null)
            {
                return HttpNotFound();
            }
            return View(enseignant);
        }

        //
        // GET: /Enseignant/Create
        [Authorize(Roles = "Super_user")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Enseignant/Create

        [HttpPost]
        [Authorize(Roles = "Super_user")]
        public ActionResult Create(ENSEIGNANT enseignant)
        {
            enseignant.LOGIN = enseignant.NOM;
            RegisterModel rg = new RegisterModel();
            userC.isLoginAlreadyUsed(enseignant.NOM);

            string motDePasse = Request.Form["Mot de passe"];
            if (motDePasse.Length < 6)
            {
                ViewBag.ERROR = "Veuillez entrer un mot de passe de plus de 6 caractères.";
                return View(enseignant);
            }
            string profil = Request.Form["Profil"];

            string username = enseignant.NOM;
            string usernameBase = username;
            int valeurAutre = 1;

            if (ModelState.IsValid)
            {

                while (userC.isLoginAlreadyUsed(username))
                {
                    username = usernameBase + valeurAutre.ToString();
                }
                rg.UserName = username;
                rg.Password = motDePasse;
                rg.ConfirmPassword = motDePasse;
                // On enregistre la personne dans la BD !
                dak.Register(rg);

                if (profil.Equals("Super_user"))
                {
                    Roles.AddUserToRole(rg.UserName, "Super_user");
                    Roles.AddUserToRole(rg.UserName, "Normal_user");
                }
                else
                {
                    Roles.AddUserToRole(rg.UserName, "Normal_user");
                }
                enseignant.LOGIN = username;
                db.ENSEIGNANTs.Add(enseignant);
                db.SaveChanges();

                // Permettra de renvoyer à la vue le login effectivement enregistré dans la BD
                ViewBag.LOGIN = username;
                //return RedirectToAction("Index");
                return View(enseignant);
            }

            return View(enseignant);
        }

        //
        // GET: /Enseignant/Edit/5
        [Authorize(Roles = "Super_user")]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.numeroEnseignant = id;
            return View(db.ENSEIGNANTs.ToList());
            //ENSEIGNANT enseignant = db.ENSEIGNANTs.Find(id);
            //if (enseignant == null)
            //{
            //return HttpNotFound();
            //}
            //return View(enseignant);
        }

        //
        // POST: /Enseignant/Edit/5
        [Authorize(Roles = "Super_user")]
        [HttpPost]
        public ActionResult Edit(ENSEIGNANT enseignant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enseignant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enseignant);
        }

        //
        // GET: /Enseignant/Delete/5
        [Authorize(Roles = "Super_user")]
        public ActionResult Delete(int id = 0)
        {
            ENSEIGNANT enseignant = db.ENSEIGNANTs.Find(id);
            if (enseignant == null)
            {
                return HttpNotFound();
            }
            return View(enseignant);
        }



        //
        // POST: /Enseignant/Delete/5
        [Authorize(Roles = "Super_user")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ENSEIGNANT enseignant = db.ENSEIGNANTs.Find(id);
            db.ENSEIGNANTs.Remove(enseignant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        [Authorize(Roles = "Super_user")]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        // permet de vérifier si un enseignant est un super user !
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


        [Authorize(Roles = "Super_user")]
        public ActionResult Modifenseignant(int id = 0)
        {
            bool Super_user_state; // Détermine si l'uitlisateur est un super user

            ViewBag.numeroEnseignant = id;
            ViewBag.listeEnseignant = db.ENSEIGNANTs.ToList();
            if (id != 0)
            {
                ENSEIGNANT enseignant = db.ENSEIGNANTs.Find(id);
                if (enseignant == null)
                {
                    // on remet le viewbag a zéro, puisque rien dans la bd a cet id
                    ViewBag.numeroEnseignant = 0;
                    return View();
                }
                Super_user_state = this.isSuperUser(enseignant);
                if (Super_user_state)
                {
                    ViewBag.SUPER_USER = "Super_user";
                }
                else
                {
                    ViewBag.SUPER_USER = "Normal_user";
                }
                return View(enseignant);
            }
            return View();
        }


        //
        // POST: /Enseignant/Modifenseignant/5
        [Authorize(Roles = "Super_user")]
        [HttpPost]
        public ActionResult Modifenseignant(ENSEIGNANT enseignant)
        {
            if (ModelState.IsValid)
            {
                string form_mot_de_passe = Request.Form["Mot de passe"];
                string form_confirmation_mot_de_passe = Request.Form["Valider Mot de passe"];

                if (form_mot_de_passe == form_confirmation_mot_de_passe)
                {
                    if (form_mot_de_passe != "" && form_mot_de_passe.Length >= 6)
                    {
                        // On change le mot de passe 
                        string token = WebSecurity.GeneratePasswordResetToken(enseignant.LOGIN, 1440);
                        WebSecurity.ResetPassword(token, form_mot_de_passe);
                    }
                }

                string form_valeur_super_user = Request.Form["Profil"];
                if (form_valeur_super_user == "Super_user")
                {
                    if (!Roles.IsUserInRole(enseignant.LOGIN, "Super_user"))
                    {
                        Roles.AddUserToRole(enseignant.LOGIN, "Super_user");
                    }
                }
                else // On doit vérifier qu'on est pas passé de super user a user normal
                {
                    if (Roles.IsUserInRole(enseignant.LOGIN, "Super_user"))
                    {
                        Roles.RemoveUserFromRole(enseignant.LOGIN, "Super_user");
                    }
                }

                db.Entry(enseignant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Modifenseignant");
            }
            return View(enseignant);
        }


        [Authorize(Roles = "Super_user")]
        public ActionResult Supenseignant(int id = 0)
        {
            ViewBag.numeroEnseignant = id;
            ViewBag.listeEnseignant = db.ENSEIGNANTs.ToList();
            if (id != 0)
            {
                ENSEIGNANT enseignant = db.ENSEIGNANTs.Find(id);
                if (enseignant == null)
                {
                    // on remet le viewbag a zéro, puisque rien dans la bd a cet id
                    ViewBag.numeroEnseignant = 0;
                    return View();
                }
                return View(enseignant);
            }
            return View();

        }

        //
        // POST: /Enseignant/Modifenseignant/5
        [Authorize(Roles = "Super_user")]
        [HttpPost]
        public ActionResult Supenseignant(ENSEIGNANT enseignant)
        {
            int id = enseignant.ID_ENSEIGNANT;
            if (ModelState.IsValid)
            {
                ENSEIGNANT enseignantSupprimer = db.ENSEIGNANTs.Find(id);
                db.ENSEIGNANTs.Remove(enseignantSupprimer);
                db.SaveChanges();
                //int id_Account = WebSecurity.GetUserId(enseignant.LOGIN);
                if (Roles.IsUserInRole(enseignant.LOGIN, "Super_user"))
                {
                    Roles.RemoveUserFromRole(enseignant.LOGIN, "Super_user");
                    Roles.RemoveUserFromRole(enseignant.LOGIN, "Normal_user");
                }
                else
                {
                    Roles.RemoveUserFromRole(enseignant.LOGIN, "Normal_user");
                }
                
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(enseignant.LOGIN);
                return RedirectToAction("Index");
            }
            return View(enseignant);
        }

    }
}