using UKParliament.CodeTest.Models.Delegates;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;
using UKParliament.CodeTest.Services.Interfaces;

namespace UKParliament.CodeTest.Services;

public class PersonValidator : IPersonValidator
{
    private readonly CurrentTime _currentTime;
    
    public PersonValidator(CurrentTime currentTime)
    {
        _currentTime = currentTime;
    }
    
    public ICollection<string> ValidateSearchQuery(PersonSearchQuery query)
    {
        var errors = new List<string>();
        
        if (query.AgeFrom > query.AgeTo)
        {
            errors.Add("Age to can not be before age from.");
        }

        return errors;
    }

    public ICollection<string> ValidatePerson(PersonDetail person)
    {
        var errors = new List<string>();
        
        errors.AddRange(ValidatePersonSummary(person));
        errors.AddRange(ValidateAddress(person));

        if (person.PhoneNumbers?.Any() ?? false)
        {
            errors.AddRange(person.PhoneNumbers?.SelectMany(ValidatePhoneNumber));
        }
        
        return errors;
    }

    public void Validate<T>(Func<T, ICollection<string>> validateMethod, T entityToValidate)
    {
        var validation = validateMethod(entityToValidate);

        if (validation?.Any() ?? false)
        {
            throw new ValidationException(validation);
        }
    }

    private IEnumerable<string> ValidatePersonSummary(PersonDetail person)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(person.Title))
        {
            errors.Add("Person must have a title.");
        }
        if (string.IsNullOrWhiteSpace(person.Name))
        {
            errors.Add("Person must have a name.");
        }
        if (string.IsNullOrWhiteSpace(person.Email))
        {
            errors.Add("Person must have an email.");
        }
        if (person.DateOfBirth > DateOnly.FromDateTime(_currentTime()))
        {
            errors.Add("Person's date of birth must be in the past.");
        }

        return errors;
    }
    
    private IEnumerable<string> ValidateAddress(PersonDetail person)
    {
        var errors = new List<string>();
        
        var addressValues = new List<string>(){person.LineOne, person.LineTwo, person.City, person.Country, person.PostCode};

        if (addressValues.All(string.IsNullOrWhiteSpace))
        {
            return errors;
        }
        
        if (string.IsNullOrWhiteSpace(person.LineOne))
        {
            errors.Add("Person's address must have line one.");
        }
        if (string.IsNullOrWhiteSpace(person.City))
        {
            errors.Add("Person's address must have city.");
        }
        if (string.IsNullOrWhiteSpace(person.Country))
        {
            errors.Add("Person's address must have country.");
        }
        if (string.IsNullOrWhiteSpace(person.PostCode))
        {
            errors.Add("Person's address must have a post code.");
        }

        return errors;
    }
    
    private IEnumerable<string> ValidatePhoneNumber(PhoneNumbers number)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(number.Number))
        {
            errors.Add("Phone number must have a number");
        }
        if (string.IsNullOrWhiteSpace(number.Description))
        {
            errors.Add("Phone number must have a description.");
        }

        return errors;
    }
}