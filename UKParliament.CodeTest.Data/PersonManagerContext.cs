using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Converters;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Entities.Configurations;

namespace UKParliament.CodeTest.Data;
public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new PhoneNumberEntityConfiguration().Configure(modelBuilder.Entity<PhoneNumber>());
        new AddressEntityConfiguration().Configure(modelBuilder.Entity<Address>());
        new PersonEntityConfiguration().Configure(modelBuilder.Entity<Person>());
    }

    // We need this custom converter for now however this should be in native in future EF - https://github.com/dotnet/efcore/issues/24507
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        builder.Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter>()
            .HaveColumnType("date");
    }

    public DbSet<Person> People { get; set; }
}