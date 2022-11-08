using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository.Interfaces;
using UKParliament.CodeTest.Models.Delegates;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Interfaces;
using UKParliament.CodeTest.Web.Controllers;

namespace UKParliament.CodeTest.Tests.UnitTests.ServiceTests
{
    public class PersonServiceTest
    {
        private IPersonValidator _fakeValidator;
        private IPersonRepository _fakeReo;
        private IPersonMapper _fakeMapper;

        private PersonService service;
        
        [SetUp]
        public void Setup()
        {
            _fakeValidator = A.Fake<IPersonValidator>();
            _fakeReo = A.Fake<IPersonRepository>();
            _fakeMapper = A.Fake<IPersonMapper>();
            
            service = new PersonService(_fakeValidator, _fakeReo, _fakeMapper);
        }
        
        [Test]
        public async Task SearchPeople_ReturnsMappedSearchResult()
        {
            var dbSearchResult = new SearchResult<Person>(
                new List<Person> {GetPersonEntity(), GetPersonEntity()}, 
                1,
                2, 
                3);
            
            A.CallTo(() => _fakeReo.SearchPeople(A<PersonSearchQuery>.Ignored)).Returns(Task.FromResult<SearchResult<Person?>>(dbSearchResult));
            A.CallTo(() => _fakeMapper.MapPersonSummary(A<Person>.Ignored)).Returns(GetPersonSummary());

            var query = GetValidSearchQuery();
            var result = await service.SearchPeople(query);
            
            A.CallTo(() => _fakeReo.SearchPeople(A<PersonSearchQuery>.That.IsSameAs(query))).MustHaveHappened();
            
            A.CallTo(() => _fakeMapper.MapPersonSummary(A<Person>.That.IsSameAs(dbSearchResult.Results.First()))).MustHaveHappened();
            A.CallTo(() => _fakeMapper.MapPersonSummary(A<Person>.That.IsSameAs(dbSearchResult.Results.Skip(1).First()))).MustHaveHappened();

            Assert.AreEqual(dbSearchResult.Skip, result.Skip);
            Assert.AreEqual(dbSearchResult.Take, result.Take);
            Assert.AreEqual(dbSearchResult.TotalResults, result.TotalResults);
        }
        
        [Test]
        public async Task GetPersonDetail_ReturnsNull_IfNotFound()
        {
            A.CallTo(() => _fakeReo.GetPerson(A<int>.Ignored)).Returns(Task.FromResult<Person?>(null));

            var result = await service.GetPersonDetail(1);
            A.CallTo(() => _fakeReo.GetPerson(A<int>.That.IsEqualTo(1))).MustHaveHappened();
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task GetPersonDetail_ReturnsPersonDetail_IfFound()
        {
            var entity = GetPersonEntity();
            var mappedResult = GetValidDetail();
            
            A.CallTo(() => _fakeReo.GetPerson(A<int>.Ignored)).Returns(Task.FromResult<Person?>(entity));
            A.CallTo(() => _fakeMapper.MapPerson(A<Person>.Ignored)).Returns(mappedResult);

            var result = await service.GetPersonDetail(1);
            A.CallTo(() => _fakeReo.GetPerson(A<int>.That.IsEqualTo(1))).MustHaveHappened();
            A.CallTo(() => _fakeMapper.MapPerson(A<Person>.That.IsSameAs(entity))).MustHaveHappened();
            
            Assert.AreEqual(mappedResult, result);
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
        private PersonSearchQuery GetValidSearchQuery() => new("Name", 20, 29, "test", true);
    }
}