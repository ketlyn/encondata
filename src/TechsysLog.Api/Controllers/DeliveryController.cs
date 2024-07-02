using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechsysLog.Api.Commands;
using TechsysLog.Api.Entities;
using TechsysLog.Api.Repositories;
using TechsysLog.Core.Controllers;
using TechsysLog.Core.Extensions;

namespace TechsysLog.Api.Controllers
{
    [Route("api/Delivery")]
    [Authorize]
    public class DeliveryController : MainController
    {
        private readonly IDeliveryRepository _DeliveryRepository;

        public DeliveryController(IDeliveryRepository DeliveryRepository)
        {
            _DeliveryRepository = DeliveryRepository;
        }

        [HttpGet("all")]
        public async Task<PagedResult<Delivery>> Get([FromQuery] int ps = 8,
                                                      [FromQuery] int page = 1,
                                                      [FromQuery] string q = null)
        {
            return await _DeliveryRepository.GetAllDeliveriesPaged(ps, page, q);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromServices] IMediator _mediator,
            AddDeliveryCommand command)
        {
            return CustomResponse(await _mediator.Send(command));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromServices] IMediator _mediator, Guid id)
        {
            return CustomResponse(await _mediator.Send(new DeleteDeliveryCommand { Id = id}));
        }


    }

}
