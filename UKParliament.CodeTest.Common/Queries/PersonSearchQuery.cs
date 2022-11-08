namespace UKParliament.CodeTest.Models.Queries;

public record PersonSearchQuery(
    string? Name = null, 
    int? AgeFrom = null, 
    int? AgeTo = null, 
    string? Email = null, 
    bool? Enabled = null,
    int Skip = 0,
    int Take = 20) : BaseQuery(Skip, Take);