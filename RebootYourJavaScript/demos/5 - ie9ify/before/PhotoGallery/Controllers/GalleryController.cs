using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using PhotoGallery.Repositories;
using WebMatrix.WebData;
using System.Web.Helpers;

namespace PhotoGallery.Controllers
{
    public class GalleryController : Controller
    {
        public ActionResult Default()
        {
            ViewBag.Title = "Photo Gallery";

            var db = new PhotoGalleryRepository();
            var galleries = db.GetGalleryList();

            return View(galleries);
        }

        public ActionResult List()
        {
            var db = new PhotoGalleryRepository();
            var galleries = db.GetGalleryList();

            var jsonGalleries = galleries.Select(gallery => new jsonGallery
                                                                {
                                                                    id = gallery.Id, name = gallery.Name
                                                                }).ToList();

            return Json(jsonGalleries, JsonRequestBehavior.AllowGet);
        }
        
        [Authorize]
        public ActionResult New()
        {
            ViewBag.Title = "New Gallery";

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult New(string Name)
        {
            if (Name.IsEmpty()) 
                ModelState.AddModelError("Name", "You must specify a gallery name.");
            
            if (ModelState.IsValid)
            {
                var db = new PhotoGalleryRepository();
                var nameExists = db.GetGalleryCount(Name) > 0;
                
                if (nameExists)
                {
                    ModelState.AddModelError("Name", "A gallery with the supplied name already exists.");
                }
                else
                {
                    int galleryId = db.CreateGallery(Name);
                    return RedirectToAction("View", new { id = galleryId });
                }
            }

            return View();
        }

        [OutputCache(Duration = 60)]
        public ActionResult Thumbnail(int id)
        {
            var db = new PhotoGalleryRepository();

            var gallery = db.GetGallery(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }

            var photos = db.GetTopPhotosForGallery(id);
            if (photos.Count == 0)
            {
                return File(Server.MapPath("~/Content/Images/gallery-empty.png"), "image/jpeg");
            }

            using (var generator = new MultiThumbnailGenerator())
            {
                foreach (var photo in photos)
                {
                    using (var imageStream = new MemoryStream(photo.FileContents))
                    {
                        using (var image = System.Drawing.Image.FromStream(imageStream))
                        {
                            generator.AddImage(image);
                        }
                    }
                }
                using (var outStream = new MemoryStream())
                {
                    generator.WritePngToStream(outStream);
                    var image = new WebImage(outStream);
                    image.Write();
                    
                    return null;
                }
            }
        }

        public ActionResult View(int id)
        {
            var db = new PhotoGalleryRepository();

            var gallery = db.GetGallery(id);
            if (gallery == null)
                return HttpNotFound();

            var photos = db.GetPhotosForGallery(id);

            ViewBag.Title = "Gallery - " + gallery.Name;
            ViewBag.Name = gallery.Name;
            ViewBag.GalleryId = id;

            return View(photos);
        }

        public ActionResult Upload(int id)
        {
            WebSecurity.RequireAuthenticatedUser();

            var db = new PhotoGalleryRepository();
            var gallery = db.GetGallery(id);

            if (gallery == null)
                return HttpNotFound();

            ViewBag.Title = "Upload Photo to Gallery - " + gallery.Name;
            
            return View(gallery);
        }

        [HttpPost]
        public ActionResult Upload()
        {
            var db = new PhotoGalleryRepository();

            int id = Request.Url.Segments[3].AsInt();
            var numFiles = Request.Files.Count;
            int imageId = 0;
            
            for (int i = 0; i < numFiles; i++)
            {
                var file = Request.Files[i];
                if (file.ContentLength > 0)
                {
                    var fileUpload = new WebImage(file.InputStream);
                    var fileTitle = Path.GetFileNameWithoutExtension(file.FileName).Trim();
                    if (fileTitle.IsEmpty())
                    {
                        fileTitle = "Untitled";
                    }
                    var fileExtension = Path.GetExtension(file.FileName).Trim();
                    var fileBytes = fileUpload.GetBytes();

                    imageId = db.InsertImage(id, WebSecurity.CurrentUserId, fileTitle, 
                        fileExtension, fileUpload.ImageFormat, fileBytes.Length, fileBytes);
                }
            }

            return RedirectToAction("View", "Photo", new { id = imageId });
        }
    }

    public class jsonGallery
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
