namespace UKParliament.CodeTest.Models.Models;

public record SearchResult<T>(
    IEnumerable<T> Results,
    int TotalResults,
    int Skip,
    int Take);