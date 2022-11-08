using System.Text.Json.Serialization;

namespace UKParliament.CodeTest.Models.Models;

public record PersonDetail : PersonSummary
{
    [JsonConstructor]
    public PersonDetail(
        int id,
        string title,
        string name,
        DateOnly dateOfBirth,
        string email,
        bool accountEnabled = true,
        string lineOne = null,
        string lineTwo = null,
        string city = null,
        string country = null,
        string postCode = null,
        IEnumerable<PhoneNumbers>? phoneNumbers = null) : base(id, title, name, dateOfBirth, email, accountEnabled)
    {
        LineOne = lineOne;
        LineTwo = lineTwo;
        City = city;
        Country = country;
        PostCode = postCode;
        PhoneNumbers = phoneNumbers ?? new List<PhoneNumbers>();
    }
    
    public PersonDetail(PersonSummary summary): base(summary.Id, summary.Title, summary.Name, summary.DateOfBirth, summary.Email, summary.AccountEnabled)
    {
    }
    
    public string? LineOne { get; init; }
    public string? LineTwo { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public string? PostCode { get; init; }
    public IEnumerable<PhoneNumbers>? PhoneNumbers { get; init; }
}