using System.Web.Mvc;
using PhotoGallery.Repositories;
using System.Dynamic;
using System.Web.Helpers;

namespace PhotoGallery.Controllers
{
    public class TagController : Controller
    {
        public ActionResult Default()
        {
            ViewBag.Title = "View All Tags";

            var repository = new TagRepository();
            var tagsList = repository.GetTagList();

            return View(tagsList);
        }

        [OutputCache(Duration = 60)]
        public ActionResult Thumbnail(string id)
        {
            var repository = new TagRepository();
            var tag = repository.GetTag(id);
            var photoList = repository.GetPhotoList(id);

            if (tag != null && photoList.Count > 0)
            {
                using (MultiThumbnailGenerator generator = new MultiThumbnailGenerator())
                {
                    foreach (var photo in photoList)
                    {
                        using (var imageStream = new System.IO.MemoryStream(photo.FileContents))
                        {
                            using (var image = System.Drawing.Image.FromStream(imageStream))
                            {
                                generator.AddImage(image);
                            }
                        }
                    }

                    using (var outStream = new System.IO.MemoryStream())
                    {
                        generator.WritePngToStream(outStream);
                        var image = new WebImage(outStream);
                        image.Write();
                    }
                }

                return null;
            }
            
            return Redirect("~/Content/Images/gallery-empty.png");
        }

        public ActionResult Get(string id)
        {
            ViewBag.Title = "View Tag - " + id;

            dynamic model = new ExpandoObject();
            model.TagName = id;

            var repository = new TagRepository();
            model.Photos = repository.GetPhotos(id);
            model.SimilarTags = repository.GetSimilarTags(id);
            
            return View("View", model);
        }
    }
}
