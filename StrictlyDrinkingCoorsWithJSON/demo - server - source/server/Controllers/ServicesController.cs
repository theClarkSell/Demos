using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotDelicious.Models;

namespace NotDelicious.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ILinkRepository linkRepository;

        public ServicesController() : this(new LinkRepository())
        {
        }

        public ServicesController(ILinkRepository linkRepository)
        {
			this.linkRepository = linkRepository;
        }

        public JsonResult GetLinks()
        {
            return Json(linkRepository.All, JsonRequestBehavior.AllowGet);
        }
    }
}
