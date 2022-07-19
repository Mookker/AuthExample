using AuthExample.API.Requests;
using AuthExample.API.Responses;
using AuthExample.Infrastructure.Features.AuthFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthExample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("jwt")]
        public async Task<JwtGenerationResponse> GenerateJwtToken([FromBody] JwtGenerationRequest jwtGenerationRequest) =>
            await _mediator.Send(new JwtGenerationCommand
            {
                Login = jwtGenerationRequest.Login,
                Password = jwtGenerationRequest.Password
            });
    }
}
