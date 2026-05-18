using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetFlowAPI.Enums;

namespace PetFlowAPI.Models
{
    [Table("TUTOR")]
    public class Tutor
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NAME")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("EMAIL")]
        public string Email { get; set; }

        [MaxLength(20)]
        [Column("PHONE")]
        public string Phone { get; set; }

        [Required]
        [Column("PASSWORD_HASH")]
        public string PasswordHash { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Pet> Pets { get; set; } = new List<Pet>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Redeem> Redeems { get; set; } = new List<Redeem>();
        public ICollection<RewardPoint> RewardPoints { get; set; } = new List<RewardPoint>();
    }

    [Table("ADDRESS")]
    public class Address
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("TUTOR_ID")]
        public long TutorId { get; set; }

        [ForeignKey("TutorId")]
        public Tutor Tutor { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("STREET")]
        public string Street { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("CITY")]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("STATE")]
        public string State { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("ZIP_CODE")]
        public string ZipCode { get; set; }
    }

    [Table("SPECIES")]
    public class Species
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }

    [Table("PET")]
    public class Pet
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("TUTOR_ID")]
        public long TutorId { get; set; }

        [ForeignKey("TutorId")]
        public Tutor Tutor { get; set; }

        [Required]
        [Column("SPECIES_ID")]
        public long SpeciesId { get; set; }

        [ForeignKey("SpeciesId")]
        public Species Species { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(50)]
        [Column("BREED")]
        public string Breed { get; set; }

        [Column("BIRTH_DATE")]
        public DateTime? BirthDate { get; set; }

        [Column("WEIGHT")]
        public decimal? Weight { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<HealthEvent> HealthEvents { get; set; } = new List<HealthEvent>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<RiskScore> RiskScores { get; set; } = new List<RiskScore>();
    }

    [Table("CLINIC")]
    public class Clinic
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Column("ADDRESS")]
        public string Address { get; set; }

        [MaxLength(20)]
        [Column("PHONE")]
        public string Phone { get; set; }

        [MaxLength(18)]
        [Column("CNPJ")]
        public string Cnpj { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Plan> Plans { get; set; } = new List<Plan>();
    }

    [Table("PLAN")]
    public class Plan
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("CLINIC_ID")]
        public long ClinicId { get; set; }

        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Required]
        [Column("PRICE")]
        public decimal Price { get; set; }

        [Required]
        [Column("DURATION_DAYS")]
        public int DurationDays { get; set; }

        [Required]
        [Column("POINTS_PER_EVENT")]
        public int PointsPerEvent { get; set; }
    }

    [Table("EVENT_TYPE")]
    public class EventType
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("NAME")]
        public string Name { get; set; }

        [Required]
        [Column("POINTS_REWARD")]
        public int PointsReward { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("CATEGORY")]
        public string Category { get; set; }
    }

    [Table("HEALTH_EVENT")]
    public class HealthEvent
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("PET_ID")]
        public long PetId { get; set; }

        [ForeignKey("PetId")]
        public Pet Pet { get; set; }

        [Required]
        [Column("EVENT_TYPE_ID")]
        public long EventTypeId { get; set; }

        [ForeignKey("EventTypeId")]
        public EventType EventType { get; set; }

        [Column("CLINIC_ID")]
        public long? ClinicId { get; set; }

        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Required]
        [Column("EVENT_DATE")]
        public DateTime EventDate { get; set; }

        [Required]
        [Column("STATUS")]
        public HealthEventStatus Status { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("SUBSCRIPTION")]
    public class Subscription
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("PET_ID")]
        public long PetId { get; set; }

        [ForeignKey("PetId")]
        public Pet Pet { get; set; }

        [Required]
        [Column("PLAN_ID")]
        public long PlanId { get; set; }

        [ForeignKey("PlanId")]
        public Plan Plan { get; set; }

        [Required]
        [Column("START_DATE")]
        public DateTime StartDate { get; set; }

        [Column("END_DATE")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Column("STATUS")]
        public SubscriptionStatus Status { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("REWARD_ACTION")]
    public class RewardAction
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("NAME")]
        public string Name { get; set; }

        [Required]
        [Column("POINTS_VALUE")]
        public int PointsValue { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }

    [Table("REWARD_POINT")]
    public class RewardPoint
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("TUTOR_ID")]
        public long TutorId { get; set; }

        [ForeignKey("TutorId")]
        public Tutor Tutor { get; set; }

        [Required]
        [Column("REWARD_ACTION_ID")]
        public long RewardActionId { get; set; }

        [ForeignKey("RewardActionId")]
        public RewardAction RewardAction { get; set; }

        [Required]
        [Column("POINTS")]
        public int Points { get; set; }

        [MaxLength(50)]
        [Column("REFERENCE_TYPE")]
        public string ReferenceType { get; set; }

        [Column("REFERENCE_ID")]
        public long? ReferenceId { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("PARTNER_DISCOUNT")]
    public class PartnerDiscount
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("CLINIC_ID")]
        public long ClinicId { get; set; }

        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("PARTNER_NAME")]
        public string PartnerName { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("CATEGORY")]
        public string Category { get; set; }

        [Required]
        [Column("DISCOUNT_PERCENT")]
        public decimal DiscountPercent { get; set; }
    }

    [Table("COUPON_TEMPLATE")]
    public class CouponTemplate
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("PARTNER_DISCOUNT_ID")]
        public long PartnerDiscountId { get; set; }

        [ForeignKey("PartnerDiscountId")]
        public PartnerDiscount PartnerDiscount { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("TITLE")]
        public string Title { get; set; }

        [Required]
        [Column("DISCOUNT_VALUE")]
        public decimal DiscountValue { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("DISCOUNT_TYPE")]
        public string DiscountType { get; set; }

        [Required]
        [Column("POINTS_REQUIRED")]
        public int PointsRequired { get; set; }
    }

    [Table("COUPON")]
    public class Coupon
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("TEMPLATE_ID")]
        public long? TemplateId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("CODE")]
        public string Code { get; set; }

        [Required]
        [Column("STATUS")]
        public CouponStatus Status { get; set; }

        [Column("EXPIRATION_DATE")]
        public DateTime? ExpirationDate { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("REDEEM")]
    public class Redeem
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("TUTOR_ID")]
        public long TutorId { get; set; }

        [ForeignKey("TutorId")]
        public Tutor Tutor { get; set; }

        [Required]
        [Column("COUPON_ID")]
        public long CouponId { get; set; }

        [ForeignKey("CouponId")]
        public Coupon Coupon { get; set; }

        [Required]
        [Column("POINTS_USED")]
        public int PointsUsed { get; set; }

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("RISK_LEVEL")]
    public class RiskLevel
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("NAME")]
        public string Name { get; set; }

        [MaxLength(200)]
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Required]
        [Column("MIN_SCORE")]
        public int MinScore { get; set; }

        [Required]
        [Column("MAX_SCORE")]
        public int MaxScore { get; set; }
    }

    [Table("RISK_SCORE")]
    public class RiskScore
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("PET_ID")]
        public long PetId { get; set; }

        [ForeignKey("PetId")]
        public Pet Pet { get; set; }

        [Required]
        [Column("SCORE")]
        public int Score { get; set; }

        [Required]
        [Column("RISK_LEVEL_ID")]
        public long RiskLevelId { get; set; }

        [ForeignKey("RiskLevelId")]
        public RiskLevel RiskLevel { get; set; }

        [Column("CALCULATED_AT")]
        public DateTime CalculatedAt { get; set; } = DateTime.Now;
    }
}
