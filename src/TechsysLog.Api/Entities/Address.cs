using System;
using TechsysLog.Core.DomainObjects;

namespace TechsysLog.Api.Entities
{
    public class Address : Entity
    {
        protected Address() { }

        public string CEP { get; private set; }
        public string Street { get; private set; }
        public int Number { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        public Address(string Cep, string street, int number, string neighborhood, string city, string state)
        {
            CEP = Cep;
            Street = street;
            Number = number;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
}
