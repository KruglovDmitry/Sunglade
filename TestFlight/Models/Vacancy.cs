using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class Vacancy
    {
        public Int32 Id { get; set; }

        [Required]
        [StringLength(255)]
        public String Title { get; set; }

        [Required]
        [StringLength(255)]
        public String Shedule { get; set; }

        [Required]
        [StringLength(255)]
        public String Requirements { get; set; }

        [Required]
        public short Salary { get; set; }
        
        public DateTime Date { get; set; }

        public Vacancy()
        {
            this.Date = new DateTime();
        }
    }
}