using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class IndexConteiner
    {
        public Int32 Id { get; set; }

        public List<Product> DiscountList { get; set; }

        public List<Product> BestSellerList { get; set; }

        public List<Review> ReviewList { get; set; }

        public IndexConteiner()
        {
            this.BestSellerList = new List<Product>();
            this.DiscountList = new List<Product>();
            this.ReviewList = new List<Review>();
        }
    }
}