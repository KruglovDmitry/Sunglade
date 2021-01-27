using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class TypeConteiner
    {
        public List<ProdType> Main { get; set; }

        public List<SubType> SubList { get; set; }

        public List<ThSubType> ThList { get; set; }

        public TypeConteiner()
        {
            this.Main = new List<ProdType>();
            this.SubList = new List<SubType>();
            this.ThList = new List<ThSubType>();
        }
        
    }
}