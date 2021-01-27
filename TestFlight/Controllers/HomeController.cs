using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFlight.Models;

namespace TestFlight.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _DBdata;

        public HomeController()
        {
            _DBdata = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _DBdata.Dispose();
        }

        [ChildActionOnly]
        public ActionResult HeaderPartial()
        {
            var TCont = new TypeConteiner();
            TCont.Main = _DBdata.ProdType.ToList();
            TCont.SubList = _DBdata.SubType.ToList();
            TCont.ThList = _DBdata.ThSubType.ToList();

            return PartialView("Header", TCont);
        }

        [ChildActionOnly]
        public ActionResult SideNavPartial()
        {
            var TCont = new TypeConteiner();
            TCont.Main = _DBdata.ProdType.ToList();
            TCont.SubList = _DBdata.SubType.ToList();
            TCont.ThList = _DBdata.ThSubType.ToList();

            return PartialView("SideNav",TCont);
        }

        [ChildActionOnly]
        public ActionResult FooterPartial()
        {
            return PartialView("Footer");
        }

        public ActionResult Index()
        {
            var IndexContent = new IndexConteiner();
            var DiscountList = new List<Product>();
            var BestSellerList = new List<Product>();
            var ReviewList = new List<Review>();
            
            foreach (var product in _DBdata.Product.ToList())
                if ((product != null) && (product.Avail == true))
                {
                    if (product.DiscountProd == true) DiscountList.Add(product);
                    if (product.Popularity > 6) BestSellerList.Add(product);
                }

            foreach (var review in _DBdata.Review.ToList())
                if ((review != null) && (review.Valid == true)) ReviewList.Add(review);
            

            var rand = new Random();
           
            for (var i = 0; i < 10; i++)
            {
                if (DiscountList.Count() > 0) IndexContent.DiscountList.Add(DiscountList[rand.Next(0, DiscountList.Count())]);
                if (BestSellerList.Count() > 0) IndexContent.BestSellerList.Add(BestSellerList[rand.Next(0, BestSellerList.Count())]);
                if (ReviewList.Count() > 0)  IndexContent.ReviewList.Add(ReviewList[rand.Next(0, ReviewList.Count())]);
            }

            return View(IndexContent);
        }

        public ActionResult Catalog(String Filter)
        {
            var OutputConteiner = new CatalogConteiner();
            if (String.IsNullOrEmpty(Filter)) {
                var i = 0;
                var Rand = new Random();
                var AllTypes = _DBdata.ProdType.ToList();
                var AllProducts = _DBdata.Product.Where(p => p.Avail == true).ToList();
                foreach (var MainType in AllTypes)
                {
                    var Element = new CatalogElement();
                    var ProductByType = AllProducts.Where(p => p.ProdTypeId == MainType.Id).ToList();
                    Element.ProductTypeId = MainType.Id;
                    Element.ProductTypeName = MainType.RusName;
                    for (var j = 0; j< 10; j++) if (ProductByType.Count() > 0) Element.Products.Add(ProductByType[Rand.Next(0, ProductByType.Count())]);
                    if (Element.Products.Count() > 0) OutputConteiner.Content.Add(Element);
                    i++;
                }
            }
            return View(OutputConteiner);
        }

        public ActionResult Product(Int32 Id)
        {
            var ProductConteiner = new List<Product>();

            var Product = _DBdata.Product.SingleOrDefault(p => p.Id == Id);
            if (Product == null) return HttpNotFound();
            else ProductConteiner.Add(Product);

            var ProductList = _DBdata.Product.ToList();
            if (ProductList.Count() > 0)
            {
                var rand = new Random();
                var i = 0;
                while (i < 10)
                {
                    var ProductIn = ProductList[rand.Next(0, ProductList.Count())];
                    if (ProductIn.Avail == true)
                    {
                        ProductConteiner.Add(ProductIn);
                        i++;
                    }
                }
            }
            return View(ProductConteiner);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult News()
        {
            var News = _DBdata.News.ToList().OrderBy(n => n.Date).ToList();
            return View(News);
        }

        public ActionResult Vacancies()
        {
            var Vacancies = _DBdata.Vacancies.ToList().OrderBy(v => v.Date).ToList();
            return View(Vacancies);
        }
    }
}