namespace Infraestructure.EntitiesConfiguration
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.Cnpj)
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(p => p.BirthDate)
                .IsRequired();

            builder.Property(p => p.Cnh)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(p => p.CnhType)
                .IsRequired();

            builder.Property(p => p.CnhImage)
                .HasMaxLength(150);


            builder.HasIndex(p => p.Cnpj)
                .IsUnique();

            builder.HasIndex(p => p.Cnh)
                .IsUnique();
        }
    }
}
