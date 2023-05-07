using Hackathon.Application.Commands;
using Hackathon.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreditDto credit)
        {
            return Ok(_mediator.Send(new PostCreditOrderCommand(credit)).Result);
        }
    }
}
