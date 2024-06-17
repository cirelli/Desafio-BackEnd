namespace Infraestructure.EntitiesConfiguration
{
    public class MotorbikeConfiguration : IEntityTypeConfiguration<Motorbike>
    {
        public void Configure(EntityTypeBuilder<Motorbike> builder)
        {
            builder.Property(p => p.Plate)
                .HasMaxLength(7)
                .IsRequired();

            builder.Property(p => p.Model)
                .HasMaxLength(100)
                .IsRequired();


            builder.HasIndex(p => p.Plate)
                .IsUnique();
        }
    }
}
