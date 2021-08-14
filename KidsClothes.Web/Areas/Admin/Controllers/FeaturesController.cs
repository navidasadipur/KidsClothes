﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using drugStore7.Core.Models;
using drugStore7.Infrastructure.Repositories;

namespace drugStore7.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class FeaturesController : Controller
    {
        private readonly FeaturesRepository _repo;
        public FeaturesController(FeaturesRepository repo)
        {
            _repo = repo;
        }
        // GET: Admin/Features
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: Admin/Features/Create
        public ActionResult Create()
        {
            ViewBag.Count = _repo.GetCount();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,OrderPriority")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(feature);
                return RedirectToAction("Index");
            }
            ViewBag.Count = _repo.GetCount();

            return View(feature);
        }

        // GET: Admin/Features/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = _repo.Get(id.Value);
            if (feature == null)
            {
                return HttpNotFound();
            }
            ViewBag.Count = _repo.GetCount();
            return PartialView(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,OrderPriority")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(feature);
                return RedirectToAction("Index");
            }
            ViewBag.Count = _repo.GetCount();
            return View(feature);
        }

        // GET: Admin/Features/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = _repo.Get(id.Value);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return PartialView(feature);
        }

        // POST: Admin/Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}