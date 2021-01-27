using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFlight.Models;
using System.Data.Entity;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net;
using Microsoft.Owin.Security;

namespace TestFlight.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ApplicationDbContext _DBdata;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public CustomerController()
        {
            _DBdata = new ApplicationDbContext();
        }
        public CustomerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _DBdata = new ApplicationDbContext();
            UserManager = userManager;
            SignInManager = signInManager;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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

        public ActionResult CustomerPage()
        {
            var CustomerUp = new CustomerPageUpload();
            if (User.Identity.IsAuthenticated)
            {
                var Customer = _DBdata.Customer.SingleOrDefault(c => c.UserName == User.Identity.Name);
                if (Customer == null)
                {
                    var CustomerNew = new Customer();
                    CustomerNew.UserName = User.Identity.Name;
                    CustomerNew.Name = "";
                    CustomerNew.SurName = "";
                    CustomerNew.Email = User.Identity.Name;
                    CustomerNew.Phone = "";
                    CustomerNew.Photo = "nophoto.jpg";
                    CustomerNew.Street = "";
                    CustomerNew.HomeNr = "";
                    CustomerNew.FlatNr = "";
                    CustomerNew.RegDate = DateTime.Now;
                    _DBdata.Customer.Add(CustomerNew);
                    _DBdata.SaveChanges();

                    CustomerUp.Customer = CustomerNew;
                    CustomerUp.NewPassword = "";
                }
                else
                {
                    CustomerUp.Customer = Customer;
                    var Orders = _DBdata.Order.Where(o => o.CustomerId == Customer.Id)
                                        .Where(o => o.Done == true)
                                        .Include(o => o.OrderContent)
                                        .ToList();
                    if (Orders != null) CustomerUp.Orders = Orders;
                }
            return View(CustomerUp);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCustomer(CustomerPageUpload CustomerUp)
        {
            if ((ModelState.IsValid) && (User.Identity.IsAuthenticated))
            {
                var Customer = _DBdata.Customer.SingleOrDefault(c => c.UserName == User.Identity.Name);
                if (Customer != null)
                { 
                    Customer.Name = CustomerUp.Customer.Name;
                    Customer.SurName = CustomerUp.Customer.SurName;
                    Customer.Phone = CustomerUp.Customer.Phone;
                    Customer.Street = CustomerUp.Customer.Street;
                    Customer.HomeNr = CustomerUp.Customer.HomeNr;
                    Customer.FlatNr = CustomerUp.Customer.FlatNr;

                    if (CustomerUp.PhotoUpload != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("/Images/Customers/") + Customer.Photo) && (Customer.Photo != "nophoto.jpg"))
                            System.IO.File.Delete(Server.MapPath("/Images/Customers/") + Customer.Photo);
                        string photoName = Path.GetFileName(CustomerUp.PhotoUpload.FileName);
                        CustomerUp.PhotoUpload.SaveAs(Server.MapPath("/Images/Customers/" + photoName));
                        Customer.Photo = photoName;
                    }
                    else Customer.Photo = CustomerUp.Customer.Photo;

                    if (!String.IsNullOrEmpty(CustomerUp.NewPassword))
                    {
                        ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                        if (user != null)
                        {
                            var result = UserManager.ChangePassword(User.Identity.GetUserId(), CustomerUp.OldPassword, CustomerUp.NewPassword);
                            if (result.Succeeded)
                            {
                                _DBdata.SaveChanges();
                                return RedirectToAction("CustomerPage", "Customer", new { customer = true });
                            }
                        }
                    }
                    else 
                    {
                        _DBdata.SaveChanges();
                        return RedirectToAction("CustomerPage", "Customer", new { customer = true });
                    }
                }
            }
            return RedirectToAction("CustomerPage", "Customer", new { customer = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReview(CustomerPageUpload CustomerUp)
        {
            if ((ModelState.IsValid) && (!String.IsNullOrEmpty(CustomerUp.Review.Body)))
            {
                var ReviewNew = new Review();
                ReviewNew.Date = DateTime.Now;
                ReviewNew.Body = CustomerUp.Review.Body;
                ReviewNew.Score = CustomerUp.Review.Score;
                if (User.Identity.IsAuthenticated)
                {
                    var Customer = _DBdata.Customer.SingleOrDefault(c => c.UserName == User.Identity.Name);
                    if (Customer != null)
                    {
                        ReviewNew.CustomerId = Customer.Id;
                        ReviewNew.CustomerName = Customer.Name;
                        ReviewNew.CustomerSurName = Customer.SurName;
                        ReviewNew.CustomerPhoto = Customer.Photo;
                        _DBdata.Review.Add(ReviewNew);
                        _DBdata.SaveChanges();
                        return RedirectToAction("CustomerPage", "Customer", new { review = true });
                    }
                }
            }
            return RedirectToAction("CustomerPage", "Customer", new { review = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            if ((ModelState.IsValid) && (User.Identity.IsAuthenticated))
            {
                var customer = _DBdata.Customer.SingleOrDefault(c => c.UserName == User.Identity.Name);
                var user = UserManager.FindByName(User.Identity.Name);
                if ((user != null) && (customer != null))
                {
                    var result = UserManager.Delete(user);
                    if (result.Succeeded)
                    {
                        _DBdata.Customer.Remove(customer);
                        _DBdata.SaveChanges();
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return RedirectToAction("Index", "Home", new { delete = true });
                    }
                }
            }
            return RedirectToAction("CustomerPage", "Customer", new { delete = false });
        }


        // GET: Customer
        [AllowAnonymous]
        public ActionResult OrderList(Int32? Id)
        {
            var Order = new Order();
            var Basket = _DBdata.Basket.SingleOrDefault(b => b.Id == Id);
            if (Basket == null) return HttpNotFound();
            else if (Basket.Content.Count() > 0)
            {
                Order.BasketId = Basket.Id;
                foreach (var BasketEl in Basket.Content.ToList()) Order.OrderContent.Add(BasketEl);
            }

            if (User.Identity.IsAuthenticated) 
            {
                Order.IsAutontificated = 1;
                var Customer = _DBdata.Customer.SingleOrDefault(c => c.UserName == User.Identity.Name);
                if (Customer == null) return HttpNotFound();

                Order.CustomerId = Customer.Id;
                Order.ActualName = Customer.Name;
                Order.ActualSurName = Customer.SurName;
                Order.ActualPhone = Customer.Phone;
                Order.ActualEmail = Customer.Email;
                Order.ActualStreet = Customer.Street;
                Order.ActualHomeNr = Customer.HomeNr;
                Order.ActualFlatNr = Customer.FlatNr;
            }
            return View(Order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(Order OrderIn)
        {
            if (ModelState.IsValid)
            {
                var newOrder = new Order();
                newOrder.OrderTime = DateTime.Now;
                newOrder.DelivaryToTime = DateTime.Now;
                newOrder.CustomerId = OrderIn.CustomerId;
                newOrder.ActualName = OrderIn.ActualName;
                newOrder.ActualSurName = OrderIn.ActualSurName;
                newOrder.ActualPhone = OrderIn.ActualPhone;
                newOrder.ActualEmail = OrderIn.ActualEmail;
                newOrder.ActualStreet = OrderIn.ActualStreet;
                newOrder.ActualHomeNr = OrderIn.ActualHomeNr;
                newOrder.ActualFlatNr = OrderIn.ActualFlatNr;
                if (OrderIn.DeliveryASAP) newOrder.DeliveryASAP = true; else newOrder.DelivaryToTime = OrderIn.DelivaryToTime;
                if (OrderIn.PayByCash) newOrder.PayByCash = true; else newOrder.PayByCard = true;
                newOrder.OrderDetail = OrderIn.OrderDetail;
                newOrder.Done = false;

                float total = 0;
                foreach (var BasketEl in OrderIn.OrderContent.ToList())
                {
                    if ((BasketEl != null) && (BasketEl.Quant != 0) && (BasketEl.ProductId != 0))
                    {
                        var Product = _DBdata.Product.SingleOrDefault(p => p.Id == BasketEl.ProductId);
                        if (Product != null)
                        {
                            var newBasketElement = new BasketElement();
                            newBasketElement.ProductId = Product.Id;
                            newBasketElement.ProductName = Product.Name;
                            newBasketElement.Price = Product.Price;
                            newBasketElement.ProductPhoto = Product.Photo;
                            newBasketElement.Quant = BasketEl.Quant;

                            newOrder.OrderContent.Add(newBasketElement);
                            total = total + Product.Price * BasketEl.Quant;
                        }
                    }
                }

                newOrder.Total = total;
                _DBdata.Order.Add(newOrder);

                var BasketId = OrderIn.BasketId;
                _DBdata.Basket.SingleOrDefault(b => b.Id == BasketId).CustomerId = OrderIn.CustomerId;
                _DBdata.SaveChanges();

                return RedirectToAction("Index", "Home", new { order = true });
            }
            else return View("OrderList", "Customer", new { id = OrderIn.BasketId });
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