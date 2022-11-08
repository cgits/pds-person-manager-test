using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Services.Interfaces;

using EntityPhoneNumber = UKParliament.CodeTest.Data.Entities.PhoneNumber;

namespace UKParliament.CodeTest.Services;

public class PersonMapper : IPersonMapper
{
    public PersonDetail MapPerson(Person person)
    {
        return new PersonDetail(MapPersonSummary(person))
        {
            LineOne = person.Addresses?.Line1,
            LineTwo = person.Addresses?.Line2,
            City = person.Addresses?.City,
            Country = person.Addresses?.Country,
            PostCode = person.Addresses?.PostCode,
            PhoneNumbers = person.PhoneNumbers?.Select(MapPhoneNumbers).ToList()
        };
    }

    public PersonSummary MapPersonSummary(Person person)
    {
        return new PersonSummary(
            person.Id,
            person.Title,
            person.Name,
            person.DateOfBirth,
            person.Email,
            person.AccountEnabled);
    }

    public Person MapForSave(PersonDetail person)
    {
        var autoPrimary = person.PhoneNumbers?.Count() == 1;
        
        return new Person(person.Id,
            person.Title,
            person.Name,
            person.DateOfBirth,
            person.Email,
            person.AccountEnabled)
        {
            Addresses = MapEntities(person),
            PhoneNumbers = person.PhoneNumbers?.Select(x => MapPhoneNumberForSave(x, person.Id, autoPrimary)).Where(x => x != null).ToList()
        };
    }

    private Address? MapEntities(PersonDetail personDetail)
    {
        if (string.IsNullOrWhiteSpace(personDetail.LineOne) || string.IsNullOrWhiteSpace(personDetail.City) || string.IsNullOrWhiteSpace(personDetail.Country) || string.IsNullOrWhiteSpace(personDetail.PostCode))
        {
            return null;
        }
        
        return new Address(0, 
            personDetail.LineOne, 
            personDetail.LineTwo, 
            personDetail.City, 
            personDetail.Country,
            personDetail.PostCode, 
            personDetail.Id);
    }

    private EntityPhoneNumber? MapPhoneNumberForSave(PhoneNumbers phoneNumber, int personId, bool forcePrimary)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber.Number) || string.IsNullOrWhiteSpace(phoneNumber.Description))
        {
            return null;
        }
        
        return new EntityPhoneNumber(phoneNumber.Id, phoneNumber.Number, phoneNumber.Description, forcePrimary || phoneNumber.IsPrimary, personId);
    }
    
    private PhoneNumbers MapPhoneNumbers(EntityPhoneNumber phoneNumber)
    {
        return new PhoneNumbers(phoneNumber.Id, phoneNumber.Number, phoneNumber.Description, phoneNumber.IsPrimary);
    }
}