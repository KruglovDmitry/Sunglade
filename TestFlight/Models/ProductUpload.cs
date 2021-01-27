using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestFlight.Models
{
    public class ProductUpload
    {
        public Product Product { get; set; }

        [ExistingFile]
        public IEnumerable<HttpPostedFileBase> PhotoUpload { get; set; }

        public ProductUpload()
        {
            this.Product = new Product();
        }
    }
}