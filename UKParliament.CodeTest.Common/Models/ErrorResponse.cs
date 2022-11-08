using UKParliament.CodeTest.Models.Exceptions;

namespace UKParliament.CodeTest.Models.Models;

public record ErrorResponse
{
    public ErrorResponse(ValidationException ex)
    {
        Errors = ex.Errors;
    }

    public ErrorResponse(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public ErrorResponse(string error)
    {
        Errors = Errors.Append(error);
    }

    public IEnumerable<string> Errors { get; init; } = new List<string>();
}
