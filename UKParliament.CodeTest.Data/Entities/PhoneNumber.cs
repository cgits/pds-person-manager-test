namespace UKParliament.CodeTest.Data.Entities;

public record PhoneNumber(
    int Id,
    string Number,
    string Description,
    bool IsPrimary,
    int PersonId) : BaseEntity(Id)
{
    public Person? Person { get; init; }
}