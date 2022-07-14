using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using AuthExample.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthExample.Infrastructure.Features.UserFeatures
{
    public class BlockUserCommand : IRequest
    {
        public int Id { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class BlockUserCommandCommandHandler : AsyncRequestHandler<BlockUserCommand>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public BlockUserCommandCommandHandler(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        protected override async Task Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _usersRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new UserNotFoundException();
            await _usersRepository.UpdateBlockedAsync(request.Id, request.IsBlocked);
        }
    }
}