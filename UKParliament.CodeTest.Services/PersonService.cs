using UKParliament.CodeTest.Data.Repository.Interfaces;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;
using UKParliament.CodeTest.Services.Interfaces;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly IPersonValidator _validator;
    private readonly IPersonRepository _repo;
    private readonly IPersonMapper _mapper;
    
    public PersonService(IPersonValidator validator, IPersonRepository repo, IPersonMapper mapper)
    {
        _validator = validator;
        _repo = repo;
        _mapper = mapper;
    }
    
    public async Task<SearchResult<PersonSummary>> SearchPeople(PersonSearchQuery query)
    {
        _validator.Validate(_validator.ValidateSearchQuery, query);

        var searchResult = await _repo.SearchPeople(query);
        return new SearchResult<PersonSummary>(searchResult.Results.Select(_mapper.MapPersonSummary).ToList(),
            searchResult.TotalResults, 
            searchResult.Skip, 
            searchResult.Take);
    }

    public async Task<PersonDetail?> GetPersonDetail(int personId)
    {
        var person = await _repo.GetPerson(personId);
        
        return person is null ? null : _mapper.MapPerson(person);
    }

    public async Task DeletePerson(int personId)
    {
        await _repo.Delete(personId);
    }

    public async Task<int> UpdatePerson(PersonDetail person)
    {
        _validator.Validate(_validator.ValidatePerson, person);
        
        var entity = _mapper.MapForSave(person);
        return await _repo.Save(entity);
    }
}