using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFlight.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Hosting;

namespace TestFlight.Controllers
{
    [Authorize(Users = "dim4098@yandex.ru")]
    public class ManagerController : Controller
    {
        private ApplicationDbContext _DBdata;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManagerController()
        {
            _DBdata = new ApplicationDbContext();
        }

        public ManagerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _DBdata = new ApplicationDbContext();
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Main()
        {
            var Data = new int[4];
            Data[0] = _DBdata.Customer.Where(c => c.RegDate.Year == DateTime.Now.Year)
                                      .Where(c => c.RegDate.Month == DateTime.Now.Month)
                                      .Where(c => c.RegDate.Day == DateTime.Now.Day)
                                      .ToList().Count();
            Data[1] = _DBdata.Basket.Where(b => b.BasketDate.Year == DateTime.Now.Year)
                                    .Where(b => b.BasketDate.Month == DateTime.Now.Month)
                                    .Where(b => b.BasketDate.Day == DateTime.Now.Day)
                                    .ToList().Count();
            Data[2] = _DBdata.Order.Where(o => o.OrderTime.Year == DateTime.Now.Year)
                                   .Where(o => o.OrderTime.Month == DateTime.Now.Month)
                                   .Where(o => o.OrderTime.Day == DateTime.Now.Day)
                                   .ToList().Count();
            float TodayOrders = _DBdata.Order.Where(o => o.OrderTime.Year == DateTime.Now.Year)
                                             .Where(o => o.OrderTime.Month == DateTime.Now.Month)
                                             .Where(o => o.OrderTime.Day == DateTime.Now.Day)
                                             .ToList().Sum(o => o.Total);
            if (Data[2] != 0) Data[3] = (int)TodayOrders / Data[2]; else Data[3] = 0;
            return View(Data);
        }

        // GET: Meneger
        public ActionResult ProductList()
        {
            var ProductList = _DBdata.Product.ToList();
            return View(ProductList);
        }

        public ActionResult NewProduct()
        {
            var ProductUp = new ProductUpload();
            return View("ProductForm", ProductUp);
        }

        public ActionResult EditProduct(Int32 Id)
        {
            var dbProduct = _DBdata.Product.SingleOrDefault(p => p.Id == Id);
            if (dbProduct == null) return HttpNotFound();

            var productUp = new ProductUpload();
            productUp.Product = dbProduct;

            return View("ProductForm", productUp);
        }

        public ActionResult Delete(Int32 Id)
        {
            var dbProduct = _DBdata.Product.SingleOrDefault(p => p.Id == Id);
            if (dbProduct == null) return HttpNotFound();

            var Delete = dbProduct.Photo.Split('#');
            foreach (var delName in Delete)
            {
                if ((System.IO.File.Exists(Server.MapPath("/Images/Products/") + delName)) && (delName != "no_IMAGE.jpg"))
                {
                    System.IO.File.Delete(Server.MapPath("/Images/Products/") + delName);
                }
            }

            foreach (var BasketElement in _DBdata.BasketElement.ToList())
            {
                if (dbProduct.Id == BasketElement.ProductId) _DBdata.BasketElement.Remove(BasketElement);
            }

            _DBdata.Product.Remove(dbProduct);
            _DBdata.SaveChanges();

            return RedirectToAction("ProductList", "Manager");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProductUpload productUp)
        {
            if (!ModelState.IsValid) 
            {
                var productform = new ProductUpload();
                productform.Product = productUp.Product;
                return View("ProductForm", productform);
            }

            if (productUp.Product.Id == 0)
            {
                var newProduct = productUp.Product;
                newProduct.CrDate = DateTime.Now.Date;
                foreach (var file in productUp.PhotoUpload)
                {
                    if (file != null)
                    {
                        string photoName = Path.GetFileName(file.FileName);
                        if (System.IO.File.Exists(Server.MapPath("~/Images/Products/") + photoName))
                        {
                            System.IO.File.Delete(Server.MapPath("~/Images/Products/") + photoName);
                            file.SaveAs(Server.MapPath("~/Images/Products/") + photoName);
                            newProduct.Photo = newProduct.Photo + photoName + "#";
                        }
                        else 
                        {
                            file.SaveAs(Server.MapPath("~/Images/Products/") + photoName);
                            newProduct.Photo = newProduct.Photo + photoName + "#";
                        }
                    }
                }
                if (String.IsNullOrEmpty(newProduct.Photo)) newProduct.Photo = "no_IMAGE.jpg";
                else newProduct.Photo = newProduct.Photo.Substring(0, newProduct.Photo.Length - 1);
                _DBdata.Product.Add(newProduct);
            }
            else 
            {
                var dbProduct = _DBdata.Product.Single(p => p.Id == productUp.Product.Id);
                if (dbProduct == null) return HttpNotFound();

                dbProduct.Name = productUp.Product.Name;
                dbProduct.ProdTypeId = productUp.Product.ProdTypeId;
                dbProduct.SubTypeId = productUp.Product.SubTypeId;
                dbProduct.ThSubTypeId = productUp.Product.ThSubTypeId;
                dbProduct.ProdTypeName = productUp.Product.ProdTypeName;
                dbProduct.Price = productUp.Product.Price;
                dbProduct.PriceFor = productUp.Product.PriceFor;
                dbProduct.OldPrice = productUp.Product.OldPrice;
                dbProduct.Description = productUp.Product.Description;
                dbProduct.Finder = productUp.Product.Finder;
                dbProduct.DiscountProd = productUp.Product.DiscountProd;
                dbProduct.Avail = productUp.Product.Avail;
                dbProduct.Popularity = productUp.Product.Popularity;
                dbProduct.CrDate = productUp.Product.CrDate;
                dbProduct.Photo = "";

                foreach (var file in productUp.PhotoUpload)
                {
                    if (file != null) 
                    {
                        string photoName = Path.GetFileName(file.FileName);
                        if (System.IO.File.Exists(Server.MapPath("/Images/Products/") + photoName))
                        {
                            System.IO.File.Delete(Server.MapPath("/Images/Products/") + photoName);
                            file.SaveAs(Server.MapPath("/Images/Products/" + photoName));
                            dbProduct.Photo = dbProduct.Photo + photoName + "#";
                        }
                        else
                        {
                            file.SaveAs(Server.MapPath("/Images/Products/" + photoName));
                            dbProduct.Photo = dbProduct.Photo + photoName + "#";
                        }
                    }
                }
                if (dbProduct.Photo != "") dbProduct.Photo = dbProduct.Photo.Substring(0, dbProduct.Photo.Length - 1);
                if (dbProduct.Photo == "") dbProduct.Photo = productUp.Product.Photo;
            }
             _DBdata.SaveChanges();
            
            return RedirectToAction("ProductList", "Manager");
        }

        public ActionResult TypeList()
        {
            var TypeCont = new TypeConteiner();
            TypeCont.Main = _DBdata.ProdType.ToList();
            TypeCont.SubList = _DBdata.SubType.ToList();
            TypeCont.ThList = _DBdata.ThSubType.ToList();
            return View(TypeCont);
        }

        public ActionResult NewType(Int32 id, Int32 sid, Int32 tid)
        {

            var TypeCont = new TypeConteiner();
            if (id == 0)
            {
                var Main = new ProdType() { Id = -1 };
                TypeCont.Main.Add(Main);
            }
            else if (sid == 0)
            {
                var Sub = new SubType() { Id = -1 };
                TypeCont.SubList.Add(Sub);
            }
            else if (tid == 0)
            {
                var Th = new ThSubType() { Id = -1 };
                TypeCont.ThList.Add(Th);
            }
            return View("TypeForm", TypeCont);
        }


        public ActionResult EditType(Int32 id, Int32 sid, Int32 tid)
        {
            var TypeCont = new TypeConteiner();
            if ((sid == -1) && (tid == -1)) 
            {
                var Main = _DBdata.ProdType.SingleOrDefault(m => m.Id == id);
                if (Main != null) TypeCont.Main.Add(Main);
                else return HttpNotFound();
            }
            else if ((id == -1) && (tid == -1)) 
            {
                var Sub = _DBdata.SubType.SingleOrDefault(s => s.Id == sid);
                if (Sub != null) TypeCont.SubList.Add(Sub);
                else return HttpNotFound();
            }
            else if ((id == -1) && (sid == -1)) 
            {
                var Th = _DBdata.ThSubType.SingleOrDefault(t => t.Id == tid);
                if (Th != null) TypeCont.ThList.Add(Th);
                else return HttpNotFound();
            }
            return View("TypeForm", TypeCont);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveType(TypeConteiner TypeCont)
        {
            if (!ModelState.IsValid) return View("TypeForm", TypeCont);

            if (TypeCont.Main.ToList().Count() > 0)
            { 
                if (TypeCont.Main[0].Id == -1)
                {
                    var Main = new ProdType();
                    Main.RusName = TypeCont.Main[0].RusName;
                    Main.EngName = TypeCont.Main[0].EngName;
                    Main.Icon = TypeCont.Main[0].Icon;
                    _DBdata.ProdType.Add(Main);
                    _DBdata.SaveChanges();
                }
                else
                {
                    var index = TypeCont.Main[0].Id;
                    var Main = _DBdata.ProdType.SingleOrDefault(m => m.Id == index);
                    if (Main == null) return HttpNotFound();
                    Main.RusName = TypeCont.Main[0].RusName;
                    Main.EngName = TypeCont.Main[0].EngName;
                    Main.Icon = TypeCont.Main[0].Icon;
                    _DBdata.SaveChanges();
                }
            }
            else if (TypeCont.SubList.ToList().Count() > 0)
            {
                if (TypeCont.SubList[0].Id == -1)
                {
                    var Sub = new SubType();
                    Sub.ProdTypeId = TypeCont.SubList[0].ProdTypeId;
                    Sub.SubTypeName = TypeCont.SubList[0].SubTypeName;
                    _DBdata.SubType.Add(Sub);
                    _DBdata.SaveChanges();
                }
                else
                {
                    var index = TypeCont.SubList[0].Id;
                    var Sub = _DBdata.SubType.SingleOrDefault(s => s.Id == index);
                    if (Sub == null) return HttpNotFound();
                    Sub.ProdTypeId = TypeCont.SubList[0].ProdTypeId;
                    Sub.SubTypeName = TypeCont.SubList[0].SubTypeName;
                    _DBdata.SaveChanges();
                }
            }
            else if (TypeCont.ThList.ToList().Count() > 0)
            {
                if (TypeCont.ThList[0].Id == -1)
                {
                    var Th = new ThSubType();
                    Th.SubTypeId = TypeCont.ThList[0].SubTypeId;
                    Th.SubTypeName = TypeCont.ThList[0].SubTypeName;
                    _DBdata.ThSubType.Add(Th);
                    _DBdata.SaveChanges();
                }
                else
                {
                    var index = TypeCont.ThList[0].Id;
                    var Th = _DBdata.ThSubType.SingleOrDefault(t => t.Id == index);
                    if (Th == null) return HttpNotFound();
                    Th.SubTypeId = TypeCont.ThList[0].SubTypeId;
                    Th.SubTypeName = TypeCont.ThList[0].SubTypeName;
                    _DBdata.SaveChanges();
                }
            }
            return RedirectToAction("TypeList", "Manager");
        }

        public ActionResult DeleteType(Int32 id, Int32 sid, Int32 tid)
        {
            if ((id == -1) && (sid == -1))
            {
                var Th = _DBdata.ThSubType.SingleOrDefault(t => t.Id == tid);
                if (Th == null) return HttpNotFound();
                _DBdata.ThSubType.Remove(Th);
                _DBdata.SaveChanges();
            }
            else if ((id == -1) && (tid == -1))
            {
                var Sub = _DBdata.SubType.SingleOrDefault(t => t.Id == sid);
                if (Sub == null) return HttpNotFound();
                var ThList = _DBdata.ThSubType.Where(t => t.SubTypeId == sid).ToList();
                if (ThList.ToList().Count() > 0)
                    foreach (var Th in ThList) _DBdata.ThSubType.Remove(Th);
                _DBdata.SubType.Remove(Sub);
                _DBdata.SaveChanges();
            }
            else if ((sid == -1) && (tid == -1))
            {
                var Main = _DBdata.ProdType.SingleOrDefault(t => t.Id == id);
                if (Main == null) return HttpNotFound();
                var SubList = _DBdata.SubType.Where(t => t.ProdTypeId == id).ToList();

                if (SubList.ToList().Count() > 0)
                {
                    foreach (var Sub in SubList)
                    {
                        var ThList = _DBdata.ThSubType.Where(t => t.SubTypeId == Sub.Id);
                        if (ThList.ToList().Count() > 0) 
                            foreach (var Th in ThList) _DBdata.ThSubType.Remove(Th);
                        _DBdata.SubType.Remove(Sub);
                    }
                }
                _DBdata.ProdType.Remove(Main);
                _DBdata.SaveChanges();
            }
                return RedirectToAction("TypeList", "Manager");
        }

        public ActionResult CustomerList()
        {
            var Customers = _DBdata.Customer.ToList();
            return View("CustomerList", Customers);
        }

        public ActionResult AddOperator(Int32 id)
        {
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var customer = _DBdata.Customer.SingleOrDefault(c => c.Id == id);
            if (customer == null) return HttpNotFound();

            var user = UserManager.FindByName(customer.UserName);
            if (user != null)
            {
                if (!roleManager.RoleExists("Operator"))
                {
                    roleManager.Create(new IdentityRole("Operator"));
                    var Result = UserManager.AddToRole(user.Id, "Operator");
                    if (Result.Succeeded)
                    {
                        customer.Role = "Operator";
                        _DBdata.SaveChanges();
                    }
                }
                else
                {
                    var Result = UserManager.AddToRole(user.Id, "Operator");
                    if (Result.Succeeded)
                    {
                        customer.Role = "Operator";
                        _DBdata.SaveChanges();
                    }
                }

            }
            return RedirectToAction("CustomerList", "Manager");
        }

        public ActionResult DeleteCustomer(Int32 id)
        {
            var customer = _DBdata.Customer.SingleOrDefault(c => c.Id == id);
            if (customer == null) return HttpNotFound();

            ApplicationUser user = UserManager.FindByName(customer.UserName);
            if (user != null)
            {
                IdentityResult result = UserManager.Delete(user);
                if (result.Succeeded)
                {
                    _DBdata.Customer.Remove(customer);
                    _DBdata.SaveChanges();
                }
            }
            return RedirectToAction("CustomerList", "Manager");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            _DBdata.Dispose();
            base.Dispose(disposing);
        }
       
    }
}