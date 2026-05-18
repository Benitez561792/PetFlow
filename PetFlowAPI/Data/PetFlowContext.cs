using Microsoft.EntityFrameworkCore;
using PetFlowAPI.Models;
using PetFlowAPI.Enums;

namespace PetFlowAPI.Data
{
    public class PetFlowContext : DbContext
    {
        public PetFlowContext(DbContextOptions<PetFlowContext> options) : base(options) { }

        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<HealthEvent> HealthEvents { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<RewardAction> RewardActions { get; set; }
        public DbSet<RewardPoint> RewardPoints { get; set; }
        public DbSet<PartnerDiscount> PartnerDiscounts { get; set; }
        public DbSet<CouponTemplate> CouponTemplates { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Redeem> Redeems { get; set; }
        public DbSet<RiskLevel> RiskLevels { get; set; }
        public DbSet<RiskScore> RiskScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar Enums para serem salvos como strings no banco (VARCHAR2)
            modelBuilder.Entity<HealthEvent>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (HealthEventStatus)Enum.Parse(typeof(HealthEventStatus), v, true));

            modelBuilder.Entity<Subscription>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), v, true));

            modelBuilder.Entity<Coupon>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (CouponStatus)Enum.Parse(typeof(CouponStatus), v, true));

            // Configurar precisão decimal para o Oracle
            modelBuilder.Entity<Pet>().Property(p => p.Weight).HasPrecision(5, 2);
            modelBuilder.Entity<Plan>().Property(p => p.Price).HasPrecision(10, 2);
            modelBuilder.Entity<PartnerDiscount>().Property(p => p.DiscountPercent).HasPrecision(5, 2);
            modelBuilder.Entity<CouponTemplate>().Property(p => p.DiscountValue).HasPrecision(10, 2);
        }
    }
}
