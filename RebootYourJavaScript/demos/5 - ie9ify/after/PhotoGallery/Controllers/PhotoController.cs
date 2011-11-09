using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using PhotoGallery.Repositories;
using WebMatrix.WebData;

namespace PhotoGallery.Controllers
{
    public class PhotoController : Controller
    {
        public ActionResult CommentsForUser()
        {
            if (WebSecurity.CurrentUserId <= 0)
                return Json(new []{ "error",  "you must be logged in to use this feature"}, JsonRequestBehavior.AllowGet);

            var db = new PhotoGalleryRepository();
            IList<dynamic> comments = db.GetCommentsByUser();

            var jsonComments = comments.Select(comment => new jsonComment
            {
                id = comment.Id,
                date = comment.CommentDate.ToString(),
                text = comment.CommentText
            }).ToList();
            
            return Json(jsonComments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(int? id)
        {
            if (!id.HasValue)
                return RedirectToRoute("Default");

            var photoDB = new PhotoGalleryRepository();
            var tagDB = new TagRepository();
            var userDB = new UserRepository();

            var photo = photoDB.GetPhoto(id.Value);
            if (photo == null)
                return HttpNotFound();

            ViewBag.Title = "Photo - " + photo.FileTitle;

            dynamic model = new ExpandoObject();
            model.Photo = photo;
            model.User = userDB.GetUserById(photo.UserId);
            model.Gallery = photoDB.GetGallery(photo.GalleryId);
            model.Comments = photoDB.GetCommentsByPhoto(photo.Id);
            model.TagList = tagDB.GetTagListByPhoto(photo.Id);

            return View("View", model);
        }

        [HttpPost]
        public ActionResult View(int? id, string newComment)
        {
            if (!id.HasValue)
                return RedirectToRoute("Default");

            if (newComment.IsEmpty())
                return RedirectToAction("View", new { id = id.Value });

            var db = new PhotoGalleryRepository();
            db.InsertComment(id.Value, newComment, WebSecurity.CurrentUserId);

            return RedirectToAction("View", new { id = id.Value });
        }

        [OutputCache(Duration = 60)]
        public ActionResult Thumbnail(int id)
        {
            var db = new PhotoGalleryRepository();
            var photo = db.GetPhoto(id);

            if (photo == null)
                return HttpNotFound();

            var size = Request["size"] ?? "";

            int width;
            int height;
            switch (size.ToUpperInvariant())
            {
                case "":
                case "SMALL":
                    width = 200;
                    height = 200;
                    break;
                case "MEDIUM":
                    width = 400;
                    height = 300;
                    break;
                case "LARGE":
                    width = 625;
                    height = 625;
                    break;
                default:
                    return new HttpStatusCodeResult(400);
            }

            var image = new WebImage(photo.FileContents);
            image.Resize(width, height).Write();

            return null;
        }

        [OutputCache(Duration = 60)]
        public ActionResult Full(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("View", new { id = id.Value });

            var db = new PhotoGalleryRepository();
            var photo = db.GetPhoto(id.Value);
            if (photo == null)
                return HttpNotFound();

            Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", HttpUtility.UrlPathEncode(photo.FileTitle + photo.FileExtension)));
            new WebImage(photo.FileContents).Write();

            return null;
        }

        public ActionResult Remove(int? id)
        {
            if (!id.HasValue)
                return RedirectToRoute("Default");

            var repository = new PhotoGalleryRepository();
            var photo = repository.GetPhoto(id.Value);

            if (photo == null)
                return HttpNotFound();

            ViewBag.Title = "Remove Photo - " + photo.FileTitle;

            return View(photo);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            var repository = new PhotoGalleryRepository();
            var photo = repository.GetPhoto(id);
            
            WebSecurity.RequireUser(photo.UserId);
            repository.RemovePhoto(id);

            return RedirectToAction("View", "Gallery", new {id = photo.GalleryId});
        }
        
        [Authorize]
        public ActionResult EditTags(int? id)
        {
            if (!id.HasValue)
                return RedirectToRoute("Default");

            var db = new TagRepository();
            var photoDB = new PhotoGalleryRepository();

            var tags = db.GetSortedTagListByPhoto(id.Value);
            var photo = photoDB.GetPhoto(id.Value);
            if (photo == null)
                return HttpNotFound();
            
            ViewBag.Title = "Photo Tags - " + photo.FileTitle;
            
            var tagsList = new List<string>();
            foreach (var tag in tags) {
                tagsList.Add(tag.TagName);
            }

            dynamic model = new ExpandoObject();
            model.Photo = photo;
            model.TagStringToDisplay = String.Join("; ", tagsList.ToArray());

            return View("EditTags", model);
        }

        [HttpPost]
        public ActionResult EditTags(int id, string newTags)
        {
            var photoRepository = new PhotoGalleryRepository();
            var tagRepository = new TagRepository();

            var photo = photoRepository.GetPhoto(id);
            if (photo == null)
                return HttpNotFound();

            var tagNames = newTags.Split(';');

            tagRepository.DeleteTagsForPhoto(id);
            foreach (string tagName in tagNames) {
                
                var trimmed = tagName.Trim();
                bool exists = tagRepository.GetTagCount(trimmed) > 0;
                if (!exists)
                    tagRepository.InsertTag(trimmed);
                
                photoRepository.InsertPhotoTag(id, trimmed);
            }

            return RedirectToAction("View", "Photo", new { id = id });
        }
    }

    public class jsonComment
    {
        public int id;
        public string date;
        public string text;
    }
}
