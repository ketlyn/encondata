using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using TechsysLog.Api.Entities;
using TechsysLog.Core.Extensions;
using TechsysLog.Core.Identity;

namespace TechsysLog.Web.Models
{
    public class ListOrderModel
    {
       public PagedResult<OrderModel> orders { get; set; }
       public IAspNetUser aspNetUser { get; set; }
    }

    public class OrderModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public DateTime UpdateDate { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public Address(string cep, string street, int number, string neighborhood, string city, string state)
        {
            CEP = cep;
            Street = street;
            Neighborhood = neighborhood;
            Number = number;
            City = city;
            State = state;
        }

        public string CEP { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

}
