using NUnit.Framework;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Services;

namespace UKParliament.CodeTest.Tests.UnitTests.ServiceTests
{
    public class PersonMapperTest
    {
        private PersonMapper mapper;
        
        [SetUp]
        public void Setup()
        {
            mapper = new PersonMapper();
        }
        
        [Test]
        public void MapPerson_MapsEntity_ToDetail()
        {
            var mapped = mapper.MapPerson(GetPersonEntity());
            Assert.AreEqual(GetValidDetail().ToString(), mapped.ToString());
        }
        
        [Test]
        public void MapPersonSummary_MapsEntity_ToSummary()
        {
            var mapped = mapper.MapPersonSummary(GetPersonEntity());
            Assert.AreEqual(GetPersonSummary().ToString(), mapped.ToString());
        }

        [Test]
        public void MapForSave_MapsDetail_ToEntity()
        {
            var mapped = mapper.MapForSave(GetValidDetail());
            Assert.AreEqual(GetPersonEntity().ToString(), mapped.ToString());
        }

        private Person GetPersonEntity() => new(0, "title", "name", DateOnly.FromDateTime(DateTime.Now), "test@test.com", true)
        {
            PhoneNumbers = new List<PhoneNumber>
            {
                new(0, "12", "description", true, 0), 
                new(0, "13", "description1", false, 0)
            },
            Addresses = new Address(0, "l1", "l2", "city", "country", "postcode", 0)
        };
        private PersonSummary GetPersonSummary() => new PersonSummary(0, "title", "name", DateOnly.FromDateTime(DateTime.Now), "test@test.com", true);
        private PersonDetail GetValidDetail() => new(0, "title", "name", DateOnly.FromDateTime(DateTime.Now), "test@test.com", true, "l1", "l2", "city", "country", "postcode", new List<PhoneNumbers> { new(0, "12", "description", true), new(0, "13", "description1", false) });
    }
}