using AuthExample.API.Requests;
using AuthExample.Infrastructure.Constants;
using AuthExample.Infrastructure.Features.UserFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthExample.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(Roles = AuthRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}/password")]
        public async Task UpdateUserPassword([FromRoute] int id, [FromBody] UpdateUserPasswordRequest request) => 
            await _mediator.Send(new UpdateUserPasswordCommand
            {
                Id = id,
                Password = request.Password,
            });

        [HttpPut("{id}/block")]
        public async Task UpdateUserPassword([FromRoute] int id, [FromBody] BlockUserRequest request) => 
            await _mediator.Send(new BlockUserCommand
            {
                Id = id,
                IsBlocked = request.IsBlocked,
            });
    }
}
