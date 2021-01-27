using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class CustomerPageUpload
    {
        public HttpPostedFileBase PhotoUpload { get; set; }

        public Customer Customer { get; set; }

        [StringLength(255)]
        public String OldPassword { get; set; }

        [StringLength(255)]
        public String NewPassword { get; set; }

        public Review Review { get; set; }

        public List<Order> Orders { get; set; }

        public CustomerPageUpload()
        {
            this.Customer = new Customer();
            this.Review = new Review();
            this.Orders = new List<Order>();
        }

    }
}