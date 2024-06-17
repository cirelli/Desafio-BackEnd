using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Services;

namespace Tests.Overrides.Services
{
    internal class RentServiceOverride(IRepositoryWrapper RepositoryWrapper,
                         IMapper mapper,
                         IValidator<RentDTO> dtoValidator)
        : RentService(RepositoryWrapper, mapper, dtoValidator)
    {
        public static void UpdateRentValuePublic(ref Rent rent)
            => UpdateRentValue(ref rent);
    }
}
