using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace AutoMapperProfiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<MotorbikeDTO, Motorbike>();
            CreateMap<Motorbike, MotorbikeViewModel>();

            CreateMap<DriverDTO, Driver>();
            CreateMap<Driver, DriverViewModel>();

            CreateMap<RentPlanDTO, RentPlan>();
            CreateMap<RentPlan, RentPlanViewModel>();

            CreateMap<Rent, RentViewModel>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => o.Value + o.Fee + o.AdditionalValue));

            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderViewModel>();
        }
    }
}
