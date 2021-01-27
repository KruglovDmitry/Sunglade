
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class Product
    {
        public Int32 Id { get; set; }

        [Required]
        public short ProdTypeId { get; set; }

        [Required]
        public short SubTypeId { get; set; }

        [Required]
        public short ThSubTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public String Name { get; set; }

        [Required]
        [StringLength(255)]
        public String ProdTypeName { get; set; }

        [Required]
        public Int32 Price { get; set; }

        [Required]
        [StringLength(255)]
        public String PriceFor { get; set; }

        public Int32? OldPrice { get; set; }

        [StringLength(255)]
        public String Photo { get; set; }

        [Required]
        [StringLength(255)]
        public String Description { get; set; }
        
        [Required]
        [StringLength(255)]
        public String Finder { get; set; }

        public Boolean DiscountProd { get; set; }

        [Range (0, 10)]
        public Byte Popularity { get; set; }

        public DateTime CrDate { get; set; }
              
        public Boolean Avail { get; set; }

        public Product()
        {
            this.CrDate = new DateTime();
        }
    }
}