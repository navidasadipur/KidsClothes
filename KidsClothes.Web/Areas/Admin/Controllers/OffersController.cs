using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KidsClothes.Core.Models;
using KidsClothes.Infrastructure.Helpers;
using KidsClothes.Infrastructure.Repositories;

namespace KidsClothes.Web.Areas.Admin.Controllers
{
    public class OffersController : Controller
    {
        private readonly OffersRepository _repo;
        public OffersController(OffersRepository repo)
        {
            _repo = repo;
        }
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Offer offer, HttpPostedFileBase OfferImage)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (OfferImage != null)
                {
                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(OfferImage.FileName);
                    OfferImage.SaveAs(Server.MapPath("/Files/OffersImages/Temp/" + newFileName));
                    // Resize Image

                    // Resizing Image
                    ImageResizer image = new ImageResizer();
                    if (offer.Id == 1 || offer.Id == 2)
                        image = new ImageResizer(470, 670, true);
                    if (offer.Id == 5 || offer.Id == 6)
                        image = new ImageResizer(470, 320, true);

                    image.Resize(Server.MapPath("/Files/OffersImages/Temp/" + newFileName),
                        Server.MapPath("/Files/OffersImages/Image/" + newFileName));

                    ImageResizer thumb = new ImageResizer(200, 200, true);
                    thumb.Resize(Server.MapPath("/Files/OffersImages/Temp/" + newFileName),
                        Server.MapPath("/Files/OffersImages/Thumb/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/OffersImages/Temp/" + newFileName));

                    offer.Image = newFileName;
                }
                #endregion

                _repo.Add(offer);
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer image = _repo.Get(id.Value);
            if (image == null)
            {
                return HttpNotFound();
            }
            return PartialView(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Offer offer, HttpPostedFileBase OfferImage)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (OfferImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Files/OffersImages/Image/" + offer.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/OffersImages/Image/" + offer.Image));

                    if (System.IO.File.Exists(Server.MapPath("/Files/OffersImages/Thumb/" + offer.Image)))
                        System.IO.File.Delete(Server.MapPath("/Files/OffersImages/Thumb/" + offer.Image));

                    // Saving Temp Image
                    var newFileName = Guid.NewGuid() + Path.GetExtension(OfferImage.FileName);
                    OfferImage.SaveAs(Server.MapPath("/Files/OffersImages/Temp/" + newFileName));

                    // Resize Image
                    ImageResizer image = new ImageResizer();
                    if (offer.Id == 1 || offer.Id == 2)
                        image = new ImageResizer(470, 670, true);
                    if (offer.Id == 5 || offer.Id == 6)
                        image = new ImageResizer(470, 320, true);

                    image.Resize(Server.MapPath("/Files/OffersImages/Temp/" + newFileName),
                        Server.MapPath("/Files/OffersImages/Image/" + newFileName));

                    ImageResizer thumb = new ImageResizer(200, 200, true);
                    thumb.Resize(Server.MapPath("/Files/OffersImages/Temp/" + newFileName),
                        Server.MapPath("/Files/OffersImages/Thumb/" + newFileName));

                    // Deleting Temp Image
                    System.IO.File.Delete(Server.MapPath("/Files/OffersImages/Temp/" + newFileName));
                    offer.Image = newFileName;
                }
                #endregion

                _repo.Update(offer);
                return RedirectToAction("Index");
            }
            return View(offer);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer image = _repo.Get(id.Value);
            if (image == null)
            {
                return HttpNotFound();
            }
            return PartialView(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var image = _repo.Get(id);

            //#region Delete Image
            //if (image.Image != null)
            //{
            //    if (System.IO.File.Exists(Server.MapPath("/Files/OffersImages/" + image.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/OffersImages/" + image.Image));

            //    if (System.IO.File.Exists(Server.MapPath("/Files/OffersImages/" + image.Image)))
            //        System.IO.File.Delete(Server.MapPath("/Files/OffersImages/" + image.Image));
            //}
            //#endregion

            _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}