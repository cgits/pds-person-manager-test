namespace UKParliament.CodeTest.Models.Models;

public record PersonSummary(
    int Id,
    string Title,
    string Name,
    DateOnly DateOfBirth,
    string Email,
    bool AccountEnabled = true
);