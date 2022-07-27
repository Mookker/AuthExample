using AuthExample.API.Requests;
using AuthExample.API.Responses;
using AuthExample.Infrastructure.Constants;
using AuthExample.Infrastructure.Features.UserFeatures;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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

        [HttpGet("{id}")]
        public async Task<UserResponse> GetUser(int id)
        {
            var user = await _mediator.Send(new GetUserQuery
            {
                Id = id
            });

            return _mapper.Map<UserResponse>(user);
        }
    }
}
