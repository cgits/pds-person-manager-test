namespace UKParliament.CodeTest.Data.Entities
{
    public record Person(
        int Id,
        string Title,
        string Name,
        DateOnly DateOfBirth,
        string Email,
        bool AccountEnabled = true) : BaseEntity(Id)
    {
        public List<PhoneNumber>? PhoneNumbers { get; init; }
        public Address? Addresses { get; init; }
    }
}