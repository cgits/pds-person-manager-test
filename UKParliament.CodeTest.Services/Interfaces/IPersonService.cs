using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;

namespace UKParliament.CodeTest.Services.Interfaces;

public interface IPersonService
{
    public Task<SearchResult<PersonSummary>> SearchPeople(PersonSearchQuery query);
    public Task<PersonDetail?> GetPersonDetail(int personId);
    public Task DeletePerson(int personId);
    public Task<int> UpdatePerson(PersonDetail person);
}