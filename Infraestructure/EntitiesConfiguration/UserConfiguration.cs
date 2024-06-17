namespace Infraestructure.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.UserName)
                .HasMaxLength(14)
                .IsRequired();

            builder.HasOne(p => p.Driver)
                .WithOne(p => p.User);
        }
    }
}
