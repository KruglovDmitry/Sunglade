
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestFlight.Models
{
    public class Order
    {
        public Int32 Id { get; set; }

        public Int32 CustomerId { get; set; }

        [StringLength(255)]
        public String ActualName { get; set; }

        [StringLength(255)]
        public String ActualSurName { get; set; }

        [StringLength(255)]
        public String ActualPhone { get; set; }

        [StringLength(255)]
        public String ActualEmail { get; set; }

        [StringLength(255)]
        public String ActualStreet { get; set; }

        [StringLength(255)]
        public String ActualHomeNr { get; set; }

        [StringLength(255)]
        public String ActualFlatNr { get; set; }

        public Int32 BasketId { get; set; }

        public List<BasketElement> OrderContent { get; set; }

        public float Total { get; set; }

        [StringLength(255)]
        public String OrderDetail { get; set; }

        public DateTime OrderTime { get; set; }

        public Boolean DeliveryASAP { get; set; }

        public DateTime DelivaryToTime { get; set; }

        public Boolean PayByCash { get; set; }

        public Boolean PayByCard { get; set; }

        public Boolean Done { get; set; }

        public Byte IsAutontificated { get; set; }

        public Order()
        {
            this.OrderContent = new List<BasketElement>();
            this.OrderTime = new DateTime();
            this.DelivaryToTime = new DateTime();
            this.IsAutontificated = 0;
        }

    }
}