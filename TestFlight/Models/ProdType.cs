using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class ProdType
    {
        public Int32 Id { get; set; }

        [Required]
        [StringLength(255)]
        public String RusName { get; set; }

        [Required]
        [StringLength(255)]
        public String EngName { get; set; }

        [Required]
        [StringLength(255)]
        public String Icon { get; set; }

        public ProdType()
        {
            
        }

    }
}