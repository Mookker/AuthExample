using AuthExample.Domain.Entities;
using AuthExample.Domain.Interfaces;
using MediatR;

namespace AuthExample.Infrastructure.Features.CarFeatures
{
    public class GetAllCarsQuery : IRequest<List<Car>>
    {
        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 10;
    }

    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, List<Car>>
    {
        private readonly ICarsRepository _carsRepository;

        public GetAllCarsQueryHandler(ICarsRepository carsRepository)
        {
            _carsRepository = carsRepository;
        }

        public Task<List<Car>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            return _carsRepository.GetAll(request.Offset, request.Limit);
        }
    }

}
