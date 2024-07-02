using System;
using TechsysLog.Core.DomainObjects;

namespace TechsysLog.Api.Entities
{
    public class Delivery : Entity
    {
        public string Number { get; private set; }
        public DateTime DeliveryDate { get; private set; }
        public Order Order { get; private set; }

        public Delivery(string number, DateTime deliveryDate, Order order)
        {
            Number = number;
            DeliveryDate = deliveryDate;
            Order = order;

        }
        protected Delivery() { }

    }
}
