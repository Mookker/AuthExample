using AuthExample.API.Requests;
using AuthExample.API.Responses;
using AuthExample.Infrastructure.Constants;
using AuthExample.Infrastructure.Features.UserFeatures;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthExample.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdateUserPassword([FromRoute] int id, [FromBody] UpdateUserPasswordRequest request)
        {
            if (IsNotAdminOrSameUser(id))
            {
                return new ForbidResult();
            }

            await _mediator.Send(new UpdateUserPasswordCommand
            {
                Id = id,
                Password = request.Password,
            });

            return Ok();
        }

        [Authorize(Roles = AuthRoles.Admin)]
        [HttpPut("{id}/block")]
        public async Task UpdateUserPassword([FromRoute] int id, [FromBody] BlockUserRequest request) => 
            await _mediator.Send(new BlockUserCommand
            {
                Id = id,
                IsBlocked = request.IsBlocked,
            });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (IsNotAdminOrSameUser(id))
            {
                return new ForbidResult();
            }

            var user = await _mediator.Send(new GetUserQuery
            {
                Id = id
            });

            return Ok(_mapper.Map<UserResponse>(user));
        }

        private bool IsNotAdminOrSameUser(int id) => !User.IsInRole(AuthRoles.Admin) &&
                id.ToString() != User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }
}
