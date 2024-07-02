using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechsysLog.Web.Models
{
    public class AddDeliveryModel
    {
        [Required(ErrorMessage = "The field {0} is mandatory")]
        public string Number { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory")]
        public string DeliveryDate { get; set; }

    }
}
