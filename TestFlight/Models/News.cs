using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class News
    {
        public Int32 Id { get; set; }

        [StringLength(255)]
        public String Photo { get; set; }

        [Required]
        [StringLength(255)]
        public String Header { get; set; }

        [Required]
        [StringLength(255)]
        public String Body { get; set; }

        public DateTime Date { get; set; }

        public News()
        {
            this.Date = new DateTime();
        }

    }
}