using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository.Interfaces;
using UKParliament.CodeTest.Models.Delegates;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;

namespace UKParliament.CodeTest.Data.Repository;

public class PersonRepository : BaseRepository, IPersonRepository
{
    private readonly PersonManagerContext _context;
    private readonly CurrentTime _currentTime;
    
    public PersonRepository(PersonManagerContext context, CurrentTime currentTime) : base(context, currentTime)
    {
        _context = context;
        _currentTime = currentTime;
    }

    public async Task<Person?> GetPerson(int personId)
    {
        return await _context.People
            .Include(x => x.Addresses)
            .Include(x => x.PhoneNumbers)
            .FirstOrDefaultAsync(x => x.Id == personId);
    }

    public async Task<int> Save(Person person)
    { 
        var entity = await GetPerson(person.Id);
        if (entity != null)
        {
            if (person.Addresses == null && entity.Addresses != null)
            {
                //delete existing address if no new address exists
                Delete(entity.Addresses);
            }

            if (person.Addresses != null && entity.Addresses != null)
            {
                //set id of new address based on existing data if it exists
                person = person with {Addresses = person.Addresses with {Id = entity.Addresses.Id}};
            }

            var deleteNos = entity.PhoneNumbers?.Where(x => person.PhoneNumbers?.All(y => y.Id != x.Id) ?? true);
            Delete(deleteNos);
        }

        base.Save(person);
        await _context.SaveChangesAsync();
        return person.Id;
    }

    public async Task Delete(int personId)
    {
        var person = await GetPerson(personId);

        if(person is null) return;

        Delete(person.PhoneNumbers);
        Delete(person.Addresses);
        Delete(person);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<SearchResult<Person>> SearchPeople(PersonSearchQuery searchQuery)
    {
        var query = _context.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery.Name))
        {
            query = query.Where(x => x.Name.Contains(searchQuery.Name, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(searchQuery.Email))
        {
            query = query.Where(x => x.Email.Contains(searchQuery.Email, StringComparison.OrdinalIgnoreCase));
        }
        if (searchQuery.AgeFrom.HasValue || searchQuery.AgeTo.HasValue)
        {
            var date = _currentTime();

            if (searchQuery.AgeFrom.HasValue)
            {
                query = query.Where(x => x.DateOfBirth < DateOnly.FromDateTime(date.AddYears(-searchQuery.AgeFrom.Value)));
            }

            if (searchQuery.AgeTo.HasValue)
            {
                query = query.Where(x => x.DateOfBirth < DateOnly.FromDateTime(date.AddYears(searchQuery.AgeTo.Value)));
            }
        }
        if (searchQuery.Enabled.HasValue)
        {
            query = query.Where(x => x.AccountEnabled == searchQuery.Enabled);
        }

        var count = await query.CountAsync();
        var searchResult = await query.Skip(searchQuery.Skip).Take(searchQuery.Take).ToListAsync();

        return new SearchResult<Person>(searchResult, count, searchQuery.Skip, searchQuery.Take);
    }
}