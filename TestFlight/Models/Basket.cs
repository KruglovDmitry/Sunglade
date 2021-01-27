using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class Basket
    {
        public Int32 Id { get; set; }

        public Int32 CustomerId { get; set; }

        public DateTime BasketDate { get; set; }

        public virtual List<BasketElement> Content { get; set; }

        public Basket()
        {
            this.BasketDate = new DateTime();
            this.Content = new List<BasketElement>();
        }

    }
}