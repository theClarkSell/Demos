using System.Web.Mvc;
using System.Dynamic;
using System.Web.WebPages;
using PhotoGallery.Repositories;
using WebMatrix.WebData;

namespace PhotoGallery.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ActionResult Default()
        {
            var userRepository = new UserRepository();
            var user = userRepository.GetUserById(WebSecurity.CurrentUserId);

            ViewBag.Title = "Edit Profile - " + user.DisplayName;

            return View(user);
        }
        
        [HttpPost]
        public ActionResult Default(string displayName, string bio)
        {
            if (displayName.IsEmpty()) 
                ModelState.AddModelError("displayName", "You must specify a display name.");

            if (ModelState.IsValid)
            {
                var userRepository = new UserRepository();
                userRepository.UpdateUserProfile(displayName, bio, WebSecurity.CurrentUserId);
                
                return RedirectToAction("View", new { id = WebSecurity.CurrentUserId}); 
            }

            return View();
        }

        public ActionResult View(int id)
        {
            dynamic userModel = new ExpandoObject();

            var userRepository = new UserRepository();
            var user = userRepository.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "User - " + user.DisplayName;

            var photo = userRepository.GetPhotos(id);

            userModel.User = user;
            userModel.Photos = photo;

            return View(userModel);
        }
    }
}
