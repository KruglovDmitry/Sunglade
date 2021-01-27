using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class ThSubType
    {
        public Int32 Id { get; set; }

        [Required]
        public Int32 SubTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public String SubTypeName { get; set; }

        public ThSubType()
        {
            
        }

    }
}