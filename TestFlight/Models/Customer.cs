using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class Customer
    {
        public Int32 Id { get; set; }

        [StringLength(255)]
        public String UserName { get; set; }

        [StringLength(255)]
        public String Name { get; set; }

        [StringLength(255)]
        public String SurName { get; set; }

        [StringLength(255)]
        public String Email { get; set; }

        [StringLength(255)]
        public String Photo { get; set; }

        public String City { get; set; }

        [StringLength(255)]
        public String Street { get; set; }

        [StringLength(255)]
        public String HomeNr { get; set; }

        [StringLength(255)]
        public String FlatNr { get; set; }

        [StringLength(255)]
        public String Phone { get; set; }

        public DateTime RegDate { get; set; }

        public String Role { get; set; }

        public Customer()
        {
            this.City = "Самара";
            this.RegDate = new DateTime();
        }
    }
}