using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using MediatR;

namespace AuthExample.Infrastructure.Features.UserFeatures
{
    public class GetUserQuery: IRequest<User>
    {
        public int Id { get; set; }
    }

    public class GetUserQueryHanlder : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUserQueryHanlder(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return _usersRepository.GetByIdAsync(request.Id);
        }
    }
}
