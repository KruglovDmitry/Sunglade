using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class CatalogConteiner
    {
        public List<CatalogElement> Content { get; set; }

        public CatalogConteiner()
        {
            this.Content = new List<CatalogElement>();
        }
    }
}