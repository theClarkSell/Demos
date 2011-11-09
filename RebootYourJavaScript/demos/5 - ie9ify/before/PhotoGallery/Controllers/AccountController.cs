using System.Web.Mvc;
using System.Web.WebPages;
using PhotoGallery.Repositories;
using WebMatrix.WebData;

namespace PhotoGallery.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, bool? rememberMe, string returnUrl)
        {
            if (email.IsEmpty())
                ModelState.AddModelError("email", "You must specify an email address.");
            if (password.IsEmpty())
                ModelState.AddModelError("password", "You must specify a password.");

            if (!ModelState.IsValid)
                return View();

            if (WebSecurity.Login(email, password, rememberMe.HasValue ? rememberMe.Value : false))
            {
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Default", "Gallery");
            }
            
            ModelState.AddModelError("_FORM", "The email or password provided is incorrect");
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Register";
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string confirmPassword)
        {
            if (email.IsEmpty())
                ModelState.AddModelError("email", "You must specify an email address.");
            if (password.IsEmpty())
                ModelState.AddModelError("password", "You must specify a password.");
            if (confirmPassword.IsEmpty())
                ModelState.AddModelError("confirmPassword", "You must specify a confirm password.");
            if (password != confirmPassword)
                ModelState.AddModelError("_Form", "The new password and confirmation password do not match.");
            
            if (!ModelState.IsValid)
                return View();

            var db = new AccountRepository();
            var user = db.GetAccountEmail(email);

            if (user == null)
            {
                db.CreateAccount(email);

                try
                {
                    WebSecurity.CreateAccount(email, password);
                    WebSecurity.Login(email, password);
                    
                    return RedirectToAction("Default", "Gallery");
                }
                catch (System.Web.Security.MembershipCreateUserException e)
                {
                    ModelState.AddModelError("_FORM", e.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("_FORM", "Email address is already in use.");
            }

            return View();
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Default", "Gallery");
        }
    }
}
