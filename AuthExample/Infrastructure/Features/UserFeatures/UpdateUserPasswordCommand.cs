using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using AuthExample.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthExample.Infrastructure.Features.UserFeatures
{
    public record UpdateUserPasswordCommand : IRequest
    {
        public int Id { get; init; }
        public string Password { get; init; }
    }

    public class UpdateUserPasswordCommandHandler : AsyncRequestHandler<UpdateUserPasswordCommand>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UpdateUserPasswordCommandHandler(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        protected override async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new UserNotFoundException();
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _usersRepository.UpdatePasswordAsync(user.Id, user.PasswordHash);
        }
    }
}