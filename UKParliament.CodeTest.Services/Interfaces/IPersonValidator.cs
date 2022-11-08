using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;

namespace UKParliament.CodeTest.Services.Interfaces;

public interface IPersonValidator
{
    ICollection<string> ValidateSearchQuery(PersonSearchQuery query);
    ICollection<string> ValidatePerson(PersonDetail person);

    public void Validate<T>(Func<T, ICollection<string>> validateMethod, T entityToValidate);
}