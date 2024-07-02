using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechsysLog.Web.Models
{
    public class AddOrderModel
    {
        [Required(ErrorMessage = "The field {0} is mandatory")]
        public string Number { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory")]
        public double Value { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory")]
        public DateTime UpdateDate { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory")]
        public Address Address { get; set; }

    }
}