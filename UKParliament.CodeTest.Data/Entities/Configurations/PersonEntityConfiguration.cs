using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UKParliament.CodeTest.Data.Entities.Configurations;

public class PersonEntityConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);
        
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.AccountEnabled).IsRequired();
        builder.Property(x => x.DateOfBirth).IsRequired();
    }
}