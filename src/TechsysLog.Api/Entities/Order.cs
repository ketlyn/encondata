using System;
using TechsysLog.Core.DomainObjects;

namespace TechsysLog.Api.Entities
{
    public class Order : Entity
    {
        protected Order() { }

        public string Number { get; private set; }
        public string Description { get; private set; }
        public double Value { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public Address Address { get; private set; }

        public Order(string number, string description, double value, DateTime updateDate, Address address)
        {
            Number = number;
            Description = description;  
            Value = value;
            UpdateDate = updateDate;
            Address = address;
        }
    }
}
