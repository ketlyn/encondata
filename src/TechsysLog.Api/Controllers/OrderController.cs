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
    [Route("api/Order")]
    [Authorize]
    public class OrderController : MainController
    {
        private readonly IOrderRepository _OrderRepository;

        public OrderController(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

        [HttpGet("all")]
        public async Task<PagedResult<Order>> Get([FromQuery] int ps = 8,
                                                      [FromQuery] int page = 1,
                                                      [FromQuery] string q = null)
        {
            return await _OrderRepository.GetAllOrdersPaged(ps, page, q);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromServices] IMediator _mediator,
            AddOrderCommand command)
        {
            return CustomResponse(await _mediator.Send(command));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromServices] IMediator _mediator, Guid id)
        {
            return CustomResponse(await _mediator.Send(new DeleteOrderCommand { Id = id}));
        }


    }

}
