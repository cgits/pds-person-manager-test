using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UKParliament.CodeTest.Data.Entities.Configurations;

public class PhoneNumberEntityConfiguration : BaseEntityConfiguration<PhoneNumber>
{
    public override void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.Number).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.IsPrimary).IsRequired();

        builder.HasIndex(x => new {x.PersonId, x.Number});
        builder.HasIndex(x => new {x.PersonId, x.IsPrimary});
        
        builder.HasOne(x => x.Person).WithMany(x => x.PhoneNumbers);
    }
}