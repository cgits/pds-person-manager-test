namespace UKParliament.CodeTest.Models.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public ValidationException(string error)
    {
        Errors = Errors.Append(error);
    }

    public IEnumerable<string> Errors { get; } = new List<string>();
}