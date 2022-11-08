using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Models.Models;

namespace UKParliament.CodeTest.Services.Interfaces;

public interface IPersonMapper
{
    PersonDetail MapPerson(Person person);
    PersonSummary MapPersonSummary(Person person);
    Person MapForSave(PersonDetail person);
}