using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class NewsUpload
    {
        public News News { get; set; }

        public HttpPostedFileBase File { get; set; }

        public NewsUpload()
        {
            this.News = new News();
        }
    }
}