namespace UKParliament.CodeTest.Data.Entities;

public record Address(
    int Id,
    string Line1,
    string? Line2,
    string City,
    string Country,
    string PostCode,
    int PersonId) : BaseEntity(Id)
{
    public Person? Person { get; init; }
}