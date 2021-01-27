using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestFlight.Models
{
    public class Review
    {
        public Int32 Id { get; set; }

        public Int32 CustomerId { get; set; }

        [StringLength(255)]
        public String CustomerName { get; set; }

        [StringLength(255)]
        public String CustomerSurName { get; set; }

        [StringLength(255)]
        public String CustomerPhoto { get; set; }

        public DateTime Date { get; set; }

        [StringLength(255)]
        public String Body { get; set; }

        [Range(0, 5)]
        public byte Score { get; set; }

        public Boolean Valid { get; set; }

        public Review()
        {
            this.Date = new DateTime();
        }
    }
}