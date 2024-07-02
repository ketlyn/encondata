using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TechsysLog.Core.Communications;

namespace TechsysLog.Web.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response != null && response.errors.Messages.Any())
            {
                response.errors.Messages.ForEach(x => ModelState.AddModelError(key: string.Empty, errorMessage: x));
                return true;
            }
            return false;
        }
    }
}
