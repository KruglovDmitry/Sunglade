using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class CatalogElement
    {
        public Int32 ProductTypeId { get; set; }

        public String ProductTypeName { get; set; }

        public List<Product> Products { get; set; }

        public CatalogElement()
        {
            this.Products = new List<Product>();
        }
    }
}