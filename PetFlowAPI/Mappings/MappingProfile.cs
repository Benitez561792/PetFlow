using AutoMapper;
using PetFlowAPI.Models;
using PetFlowAPI.DTOs;

namespace PetFlowAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tutor, TutorResponseDTO>();
            CreateMap<TutorRequestDTO, Tutor>();

            CreateMap<Pet, PetResponseDTO>();
            CreateMap<PetRequestDTO, Pet>();

            CreateMap<Clinic, ClinicResponseDTO>();
            CreateMap<ClinicRequestDTO, Clinic>();

            CreateMap<Plan, PlanResponseDTO>();
            CreateMap<PlanRequestDTO, Plan>();

            CreateMap<HealthEvent, HealthEventResponseDTO>();
            CreateMap<HealthEventRequestDTO, HealthEvent>();

            CreateMap<Subscription, SubscriptionResponseDTO>();
            CreateMap<SubscriptionRequestDTO, Subscription>();

            CreateMap<Coupon, CouponResponseDTO>();
            CreateMap<CouponRequestDTO, Coupon>();

            CreateMap<Redeem, RedeemResponseDTO>();
            CreateMap<RedeemRequestDTO, Redeem>();
        }
    }
}
