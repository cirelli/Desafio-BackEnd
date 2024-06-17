namespace Infraestructure.EntitiesConfiguration
{
    public class RentPlanConfiguration : IEntityTypeConfiguration<RentPlan>
    {
        public void Configure(EntityTypeBuilder<RentPlan> builder)
        {
            builder.Property(p => p.Days)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired();

            builder.Property(p => p.Fee)
                .IsRequired();
        }
    }
}
