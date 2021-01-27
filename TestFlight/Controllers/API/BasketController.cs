using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using TestFlight.Models;


namespace TestFlight.Controllers.API
{
    public class BasketController : ApiController
    {
        private ApplicationDbContext _context;

        public BasketController()
        {
            _context = new ApplicationDbContext();
        }

        // /api/newbasket
        [HttpPost]
        public Int32 GetBasket(IEnumerable<BasketAPI> Basket)
        {
            var BasketForDB = new Basket() { BasketDate = DateTime.Now };
            var Product = new Product();

            foreach (var Element in Basket)
            {
                Product = _context.Product.SingleOrDefault(p => p.Id == Element.ProductId);
                if (Product != null)
                {
                    var BasketElement = new BasketElement();
                    BasketElement.ProductId = Product.Id;
                    BasketElement.ProductName = Product.Name;
                    BasketElement.ProductPhoto = Product.Photo;
                    BasketElement.Price = Product.Price;
                    BasketElement.Quant = (short)Element.Quantity;
                    BasketForDB.Content.Add(BasketElement);
                }
            }

            if (BasketForDB.Content.Count() == 0) return 0;
            else
            {
                _context.Basket.Add(BasketForDB);
                _context.SaveChanges();
                return BasketForDB.Id;
            }
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

    }
}
