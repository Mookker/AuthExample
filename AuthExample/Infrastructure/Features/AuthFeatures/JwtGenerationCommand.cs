using AuthExample.API.Responses;
using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using AuthExample.Infrastructure.Exceptions;
using AuthExample.Infrastructure.Helpers;
using AuthExample.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AuthExample.Infrastructure.Features.AuthFeatures
{
    public record JwtGenerationCommand : IRequest<JwtGenerationResponse>
    {
        public string Login { get; init; }
        public string Password { get; init; }
    }

    public class JwtGenerationCommandHandler : IRequestHandler<JwtGenerationCommand, JwtGenerationResponse>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        public JwtGenerationCommandHandler(IUsersRepository usersRepository, IOptions<JwtSettings> jwtSettings, IPasswordHasher<User> passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentException("JwtSettings not resolved");
        }
        public async Task<JwtGenerationResponse> Handle(JwtGenerationCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByLoginAsync(request.Login);
            if (user == null)
                throw new UserNotFoundException();

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
                throw new InvalidPasswordException();

            if(user.IsBlocked == true)
                throw new UserBlockedException();


            var result = JwtGenerator.GenerateToken(_jwtSettings.JwtSecret, user.Id, user.Login, _jwtSettings.Iss, user.IsAdmin);
            return new JwtGenerationResponse
            {
                Token = result.Token,
                Expiration = result.ExpirationDate,
            };
        }
    }
}
