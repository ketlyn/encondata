using System;
using System.ComponentModel.DataAnnotations;
using TechsysLog.Api.Entities;
using TechsysLog.Core.Extensions;
using TechsysLog.Core.Identity;

namespace TechsysLog.Web.Models
{
    public class ListDeliveryModel
    {
        public PagedResult<DeliveryModel> Deliverys { get; set; }
        public IAspNetUser aspNetUser { get; set; }
    }

    public class DeliveryModel
    {
        public Guid Id { get; set; }
        public string Number { get; private set; }
        public DateTime DeliveryDate { get; private set; }
    }

}
