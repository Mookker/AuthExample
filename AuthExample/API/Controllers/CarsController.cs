using AuthExample.Domain.Entities;
using AuthExample.Infrastructure.Features.CarFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Car>> Get([FromQuery] int offset = 0, [FromQuery] int limit = 10) => await _mediator.Send(new GetAllCarsQuery
        {
            Offset = offset,
            Limit = limit
        });

    }
}
