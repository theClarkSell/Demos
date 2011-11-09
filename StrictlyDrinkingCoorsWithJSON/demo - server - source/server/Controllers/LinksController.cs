using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotDelicious.Models;

namespace NotDelicious.Controllers
{   
    public class LinksController : Controller
    {
		private readonly ILinkRepository linkRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public LinksController() : this(new LinkRepository())
        {
        }

        public LinksController(ILinkRepository linkRepository)
        {
			this.linkRepository = linkRepository;
        }

        //
        // GET: /Links/

        public ViewResult Index()
        {
            return View(linkRepository.AllIncluding(link => link.Tags));
        }

        //
        // GET: /Links/Details/5

        public ViewResult Details(int id)
        {
            return View(linkRepository.Find(id));
        }

        //
        // GET: /Links/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Links/Create

        [HttpPost]
        public ActionResult Create(Link link)
        {
            if (ModelState.IsValid) {
                linkRepository.InsertOrUpdate(link);
                linkRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /Links/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(linkRepository.Find(id));
        }

        //
        // POST: /Links/Edit/5

        [HttpPost]
        public ActionResult Edit(Link link)
        {
            if (ModelState.IsValid) {
                linkRepository.InsertOrUpdate(link);
                linkRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /Links/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(linkRepository.Find(id));
        }

        //
        // POST: /Links/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            linkRepository.Delete(id);
            linkRepository.Save();

            return RedirectToAction("Index");
        }
    }
}

