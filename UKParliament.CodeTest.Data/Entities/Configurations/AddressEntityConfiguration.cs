using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UKParliament.CodeTest.Data.Entities.Configurations;

public class AddressEntityConfiguration : BaseEntityConfiguration<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.Line1).IsRequired();
        builder.Property(x => x.City).IsRequired();
        builder.Property(x => x.Country).IsRequired();
        builder.Property(x => x.PostCode).IsRequired();

        builder.HasOne(x => x.Person).WithOne(x => x.Addresses).HasForeignKey<Address>(x => x.PersonId);
    }
}