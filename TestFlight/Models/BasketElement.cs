using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestFlight.Models
{
    public class BasketElement
    {
        public Int32 Id { get; set; }

        public Int32 ProductId { get; set; }

        [StringLength(255)]
        public String ProductPhoto { get; set; }

        [StringLength(255)]
        public String ProductName { get; set; }

        public float Price { get; set; }

        public short Quant { get; set; }

        public BasketElement()
        {
       
        }
    }
}