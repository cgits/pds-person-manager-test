using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;

namespace UKParliament.CodeTest.Data.Repository.Interfaces;

public interface IPersonRepository
{
    public Task<Person?> GetPerson(int personId);
    public Task<int> Save(Person person);
    public Task Delete(int personId);
    public Task<SearchResult<Person?>> SearchPeople(PersonSearchQuery query);
}