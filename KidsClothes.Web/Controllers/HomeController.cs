using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KidsClothes.Core.Models;
using KidsClothes.Core.Utility;
using KidsClothes.Infrastructure.Repositories;
using KidsClothes.Infratructure.Repositories;
using KidsClothes.Infratructure.Services;
using KidsClothes.Web.ViewModels;

namespace KidsClothes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiscountsRepository _discountRepo;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly OffersRepository _offersRepo;
        private readonly ProductService _productService;
        private readonly TestimonialsRepository _testimonialRepo;
        private readonly PartnersRepository _partnersRepo;
        private readonly ArticlesRepository _articlesRepo;
        private readonly ContactFormsRepository _contactFormRepo;
        private readonly ProductGroupsRepository _productGroupRepo;
        private readonly OurTeamRepository _ourTeamRepo;
        private readonly FaqGroupsRepository _faqGroupsRepo;
        private readonly EmailSubscriptionRepository _emailSubscriptionRepo;
        private readonly CertificatesRepository _certificatesRepo;
        private readonly ProductsRepository _productsRepo;

        public HomeController(
            StaticContentDetailsRepository staticContentRepo,
            OffersRepository offersRepo, 
            ProductsRepository productsRepo,
            ProductService productService,
            TestimonialsRepository testimonialRepo, 
            PartnersRepository partnersRepo,
            ArticlesRepository articlesRepo,
            DiscountsRepository discountsRepo,
            EmailSubscriptionRepository emailSubscriptionRepo,
            ContactFormsRepository contactFormRepo,
            ProductGroupsRepository productGroupRepo,
            OurTeamRepository ourTeamRepo,
            FaqGroupsRepository faqGroupsRepo,
            CertificatesRepository certificatesRepository
            )
        {
            _discountRepo = discountsRepo;
            _staticContentRepo = staticContentRepo;
            _offersRepo = offersRepo;
            _productService = productService;
            _testimonialRepo = testimonialRepo;
            _partnersRepo = partnersRepo;
            _articlesRepo = articlesRepo;
            _contactFormRepo = contactFormRepo;
            _productGroupRepo = productGroupRepo;
            this._ourTeamRepo = ourTeamRepo;
            this._faqGroupsRepo = faqGroupsRepo;
            _emailSubscriptionRepo = emailSubscriptionRepo;
            _certificatesRepo = certificatesRepository;
            _productsRepo = productsRepo;
        }

        public ActionResult Index()
        {

            var popup = _staticContentRepo.GetSingleContentByTypeId((int)StaticContentTypes.Popup);

            ViewBag.HomeMiddleBanner = _staticContentRepo.GetStaticContentDetail((int)StaticContents.HomeMiddleBanner);

            ViewBag.NewsBackImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.NewsBackImage).Image;
            ViewBag.NewsTitle = _staticContentRepo.GetStaticContentDetail((int)StaticContents.NewsBackImage).Title;
            ViewBag.NewsShorDescription = _staticContentRepo.GetStaticContentDetail((int)StaticContents.NewsBackImage).ShortDescription;

            return View(popup);
        }

        public ActionResult HeaderSection()
        {
            
            var allMainGroups = _productGroupRepo.GetMainProductGroups();

            foreach (var group in allMainGroups)
            {
                group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
            }

            ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;

            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            if (wishListModel.WishListItems != null)
            {
                ViewBag.WishListCount = wishListModel.WishListItems.Count();
            }

            return PartialView(allMainGroups);
        }

        public ActionResult SharedHeaderSection()
        {
            var allMainGroups = _productGroupRepo.GetMainProductGroups();

            foreach (var group in allMainGroups)
            {
                group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
            }

            ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;

            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            if (wishListModel.WishListItems != null)
            {
                ViewBag.WishListCount = wishListModel.WishListItems.Count();
            }

            return PartialView(allMainGroups);
        }

        public ActionResult MobileMenuSection()
        {
            return PartialView();
        }

        public ActionResult NewsLetterPopupSection()
        {
            return PartialView();
        }

        public ActionResult MobileHeaderSection()
        {

            var allMainGroups = _productGroupRepo.GetMainProductGroups();

            foreach (var group in allMainGroups)
            {
                group.Children = _productGroupRepo.GetChildrenProductGroups(group.Id);
            }

            ViewBag.LogoImage = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo).Image;

            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            if (wishListModel.WishListItems != null)
            {
                ViewBag.WishListCount = wishListModel.WishListItems.Count();
            }

            return PartialView(allMainGroups);
        }

        public ActionResult FooterTopSection()
        {
            return PartialView();
        }

        public ActionResult FooterSection()
        {
            var vm = new FooterViewModel()
            {
                Phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone),
                Logo = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Logo),
                Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook),
                Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter),
                Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram),
                Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube),
                Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest),
            };

            return PartialView(vm);
        }

        public ActionResult CartSection()
        {
            var cartModel = new CartModel();

            HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

            if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
            {
                string cartJsonStr = cartCookie.Values["cart"];
                cartModel = new CartModel(cartJsonStr);
            }
            return PartialView(cartModel);
        }

        public ActionResult WishListSection()
        {
            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            return PartialView(wishListModel);
        }

        public ActionResult HomeTopSliderSection()
        {
            var content = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.HomeTopSlider);
            return PartialView(content);
        }

        public ActionResult OffersSection()
        {
            var offers = _offersRepo.GetAll();
            offers = offers.OrderBy(o => o.Id).ToList();
            return PartialView(offers);
        }

        public ActionResult TopSoldProductsSection(int take)
        {
            var products = _productService.GetTopSoldProductsWithPrice(take);
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                var tempVm = new ProductWithPriceViewModel(product);

                var group = _productGroupRepo.GetGroupByProductId(product.Id);

                tempVm.ProductGroupId = group.Id;

                tempVm.ProductGroupTitle = group.Title;

                vm.Add(tempVm);
            }

            return PartialView(vm);
        }

        public ActionResult TestimonialsSection()
        {
            var testimonials = _testimonialRepo.GetAll();
            var vm = testimonials.Select(testimonial => new TestimonialViewModel(testimonial)).ToList();

            return PartialView(vm);
        }
        public ActionResult LatestProductsSection(int take)
        {
            var products = _productService.GetLatestProductsWithPrice(take);
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
            {
                var tempVm = new ProductWithPriceViewModel(product);

                var group = _productGroupRepo.GetGroupByProductId(product.Id);

                tempVm.ProductGroupId = group.Id;

                tempVm.ProductGroupTitle = group.Title;

                vm.Add(tempVm);
            }

            
            return PartialView(vm);
        }

        public ActionResult HighRateProductsSection(int take)
        {
            var products = _productService.GetHighRatedProductsWithPrice(take);
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
                vm.Add(new ProductWithPriceViewModel(product));

            return PartialView(vm);
        }

        public ActionResult PartnerCompaniesSection(int take)
        {
            var allPartners = _partnersRepo.GetPartners(take);

            return PartialView(allPartners);
        }

        public ActionResult LatestArticlesSection(int take)
        {
            //var articles = _articlesRepo.GetLatestArticles(3);
            //var vm = articles.Select(item => new LatestArticlesViewModel(item)).ToList();

            //return PartialView(vm);

            var vm = new List<LatestArticlesViewModel>();
            var articles = _articlesRepo.GetTopArticles(take);
            foreach (var item in articles)
                vm.Add(new LatestArticlesViewModel(item));

            return PartialView(vm);
        }


        public ActionResult DiscountSection()
        {
            var discountItems = _discountRepo.GetProductsWithDiscount();
            var products = new List<DiscountProductViewModel>();
            foreach(var item in discountItems)
            {
                var product = new DiscountProductViewModel();
                product.Price = _productService.GetProductPrice(item.Product);
                product.PriceAfterDiscount = _productService.GetProductPriceAfterDiscount(item.Product);
                product.ProductId = item.ProductId.Value;
                product.Image = item.Product.Image;
                product.DiscountType = item.DiscountType;
                product.Amount = item.Amount;
                product.Title = item.Title;
                product.ShortTitle = item.Product.ShortTitle;
                product.DeadLine = item.DeadLine;

                products.Add(product);
            }

            return PartialView(products);
        }

        [Route("Faq")]
        public ActionResult Faq()
        {
            var model = _faqGroupsRepo.GetAllFaqGroupsWithFaqs();

            ViewBag.BanerTitle = _staticContentRepo.GetStaticContentDetail(3).Title;
            ViewBag.ShortDescription = _staticContentRepo.GetStaticContentDetail(3).ShortDescription;
            ViewBag.Link = _staticContentRepo.GetStaticContentDetail(3).Link;
            ViewBag.Image = _staticContentRepo.GetStaticContentDetail(3).Image;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(model);
        }

        [Route("About")]
        public ActionResult About()
        {
            var about = _staticContentRepo.GetStaticContentDetail((int)StaticContents.AboutDescription);

            var model = new AboutViewModel()
            {
                Title = about.Title,
                AboutDescription = about.ShortDescription,
                SignatureImage = about.Image,
            };

            model.Image = _staticContentRepo.GetStaticContentDetail((int)StaticContents.firstImageAboutPage).Image;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(model);
        }

        public ActionResult AboutOurPropertiesSection()
        {
            var model = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.AboutProperties);

            return PartialView(model);
        }

        public ActionResult AboutCustomerCounterSection()
        {
            var model = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.AboutCustomerCounter);

            return PartialView(model);
        }


        public ActionResult OurTeamsSection()
        {
            var ourTeam = _ourTeamRepo.GetAll();

            return PartialView(ourTeam);
        }

        public ActionResult PartnersSection()
        {
            var partners = _partnersRepo.GetAll();
            return PartialView(partners);
        }

        [Route("ContactUs")]
        public ActionResult Contact()
        {
            var map = _staticContentRepo.GetStaticContentDetail((int)StaticContents.ContactUsMap);
            var phone = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Phone);
            var email = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Email);
            var address = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Address);
            var workingHourseSaturdayWednesday = _staticContentRepo.GetStaticContentDetail((int)StaticContents.WorkingHoursSaturdayWednesday);
            var workingHourseTuesday = _staticContentRepo.GetStaticContentDetail((int)StaticContents.WorkingHoursTuesday);

            var vm = new ContactUsViewModel()
            { 
                Map = map,
                Phone = phone,
                Email = email,
                Address = address,
                WorkingHoursSaturdayWednesday = workingHourseSaturdayWednesday,
                WorkingHoursTuesday = workingHourseTuesday,
            };

            ViewBag.ContactUsDescriptionImg = _staticContentRepo.GetStaticContentDetail((int)StaticContents.ContactUsDescriptionImg);

            return View(vm);
        }

        public ActionResult ContactOurStoresSection()
        {
            var vm = new List<ContactStoreInfo>();

            //get and create storeInfo of store one
            var addressPhoneImageOne = _staticContentRepo.GetStaticContentDetail((int)StaticContents.StoreOneAddressPhone);

            var workingHoursOne = _staticContentRepo.GetStaticContentDetail((int)StaticContents.StoreOneWorkingHours);

            var storeInfoOne = new ContactStoreInfo()
            {
                Address = addressPhoneImageOne.Description,
                Phone = addressPhoneImageOne.ShortDescription,
                Image = addressPhoneImageOne.Image,
                WorkingHoursSaturdayWednesday = workingHoursOne.ShortDescription,
                WorkingHoursTuesday = workingHoursOne.Description,
            };

            vm.Add(storeInfoOne);

            //get and create storeInfo of store two
            var addressPhoneImageTwo = _staticContentRepo.GetStaticContentDetail((int)StaticContents.StoreTwoAddressPhone);

            var workingHoursTwo = _staticContentRepo.GetStaticContentDetail((int)StaticContents.StoreTwoWorkingHours);

            var storeInfoTwo = new ContactStoreInfo()
            {
                Address = addressPhoneImageTwo.Description,
                Phone = addressPhoneImageTwo.ShortDescription,
                Image = addressPhoneImageTwo.Image,
                WorkingHoursSaturdayWednesday = workingHoursTwo.ShortDescription,
                WorkingHoursTuesday = workingHoursTwo.Description,
            };

            vm.Add(storeInfoTwo);

            return View(vm);
        }

        public ActionResult ContactSocialsSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Facebook);
            model.Twitter = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Twitter);
            model.Pinterest = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Pinterest);
            model.Youtube = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Youtube);
            model.Instagram = _staticContentRepo.GetStaticContentDetail((int)StaticContents.Instagram);

            return PartialView(model);
        }

        public ActionResult ContactUsForm()
        {
            var model = new ContactForm();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ContactUsForm(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                _contactFormRepo.Add(contactForm);
                return RedirectToAction("ContactUsSummary");
            }
            return RedirectToAction("Contact");
        }

        public ActionResult ContactUsSummary()
        {
            return View();
        }

        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") +
                                    Path.GetExtension(upload.FileName).ToLower();
                    var vFolderPath = Server.MapPath("/Upload/");
                    if (!Directory.Exists(vFolderPath))
                    {
                        Directory.CreateDirectory(vFolderPath);
                    }
                    vFilePath = Path.Combine(vFolderPath, vFileName);
                    upload.SaveAs(vFilePath);
                    vImagePath = Url.Content("/Upload/" + vFileName);
                    vMessage = "Image was saved correctly";
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + vImagePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }

        [Route("NotFound")]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmailSubscription(string Email)
        {
            var email = "";
            var isValid = true;
            try
            {
                //email = collection["Email"];

                email = Email;
            }
            catch
            {

            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                isValid = addr.Address == email;
            }
            catch
            {
                isValid = false;
            }

            if(isValid)
            {
                EmailSubscription emailSubscription = new EmailSubscription();
                emailSubscription.Email = email;
                emailSubscription.IsDeleted = false;
                emailSubscription.InsertDate = DateTime.Now;

                _emailSubscriptionRepo.Create(emailSubscription);
            }

            ViewBag.Added = isValid;

            return View();
        }

        [Route("Certificates")]
        public ActionResult Certificates()
        {
            var certificates = _certificatesRepo.GetAll();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(certificates);
        }

        [Route("Gallery")]
        public ActionResult Gallery()
        {
            var model = _productsRepo.GetAllProductsWithGalleries();

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(model);
        }

        public ActionResult ServicesSection()
        {
            var model = _staticContentRepo.GetContentByTypeId((int)StaticContentTypes.OurServices);

            return PartialView(model);
        }
    }
}