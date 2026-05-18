using System;
using PetFlowAPI.Enums;

namespace PetFlowAPI.DTOs
{
    // Tutor DTOs
    public class TutorRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

    public class TutorResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Pet DTOs
    public class PetRequestDTO
    {
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Weight { get; set; }
        public long SpeciesId { get; set; }
        public long TutorId { get; set; }
    }

    public class PetResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Weight { get; set; }
        public long SpeciesId { get; set; }
        public long TutorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Clinic DTOs
    public class ClinicRequestDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Cnpj { get; set; }
    }

    public class ClinicResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Cnpj { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Plan DTOs
    public class PlanRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int PointsPerEvent { get; set; }
        public long ClinicId { get; set; }
    }

    public class PlanResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int PointsPerEvent { get; set; }
        public long ClinicId { get; set; }
    }

    // HealthEvent DTOs
    public class HealthEventRequestDTO
    {
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public HealthEventStatus Status { get; set; }
        public long EventTypeId { get; set; }
        public long PetId { get; set; }
        public long? ClinicId { get; set; }
    }

    public class HealthEventResponseDTO
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public HealthEventStatus Status { get; set; }
        public long EventTypeId { get; set; }
        public long PetId { get; set; }
        public long? ClinicId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Subscription DTOs
    public class SubscriptionRequestDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public long PetId { get; set; }
        public long PlanId { get; set; }
    }

    public class SubscriptionResponseDTO
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }
        public long PetId { get; set; }
        public long PlanId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Coupon DTOs
    public class CouponRequestDTO
    {
        public string Code { get; set; }
        public CouponStatus Status { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long? TemplateId { get; set; }
    }

    public class CouponResponseDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public CouponStatus Status { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public long? TemplateId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Redeem DTOs
    public class RedeemRequestDTO
    {
        public int PointsUsed { get; set; }
        public long TutorId { get; set; }
        public long CouponId { get; set; }
    }

    public class RedeemResponseDTO
    {
        public long Id { get; set; }
        public int PointsUsed { get; set; }
        public long TutorId { get; set; }
        public long CouponId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
