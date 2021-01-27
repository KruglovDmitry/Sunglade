using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using TestFlight.Models;

namespace TestFlight.Controllers.API
{
    public class ProductController : ApiController
    {
        private ApplicationDbContext _context;

        public ProductController()
        {
            _context = new ApplicationDbContext();
        }

        //GET/api/products
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _context.Product.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}
