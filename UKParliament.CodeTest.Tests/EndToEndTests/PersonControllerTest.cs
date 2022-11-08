using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Entities;
using UKParliament.CodeTest.Data.Repository;
using UKParliament.CodeTest.Models.Delegates;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Interfaces;
using UKParliament.CodeTest.Web.Controllers;

namespace UKParliament.CodeTest.Tests.EndToEndTests
{
    public class PersonApiTest
    {
        private PersonController controller;

        private PersonManagerContext _context;
        private CurrentTime _currentTime;

        [SetUp]
        public void Setup()
        {
            _currentTime = A.Fake<CurrentTime>();
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now);

            var option = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: "PersonManager")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging();
            
            _context = new PersonManagerContext(option.Options);
            
            var personMapper = new PersonMapper();
            var personValidator = new PersonValidator(_currentTime);
            var personRepo = new PersonRepository(_context, _currentTime);

            var personService = new PersonService(personValidator, personRepo, personMapper);
            
            controller = new PersonController(personService);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        
        [Test]
        [NonParallelizable]
        public async Task GetById_ReturnsPersonWithSpecifiedId_IfExists()
        {
            _context.People.RemoveRange(_context.People);
            _context.People.AddRange(
                GetPersonEntity(1),
                GetPersonEntity(2),
                GetPersonEntity(3),
                GetPersonEntity(4)
            );
            await _context.SaveChangesAsync();

            var response = await controller.GetById(1);
            var result = GetResponseObject(response);
            
            Assert.AreEqual(1, result.Id);
        }
        
        [Test]
        [NonParallelizable]
        public async Task GetById_ReturnsNotFound_IfNotExists()
        {
            _context.People.RemoveRange(_context.People);
            _context.People.AddRange(
                GetPersonEntity(1),
                GetPersonEntity(2),
                GetPersonEntity(3),
                GetPersonEntity(4)
            );
            await _context.SaveChangesAsync();

            var response = await controller.GetById(5);
            Assert.IsInstanceOf<NotFoundObjectResult>(response.Result);
        }
        
        [Test]
        [NonParallelizable]
        public async Task Search_ReturnsMatchingPeople()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(new DateTime(2020, 10, 10));

            _context.People.RemoveRange(_context.People);
            _context.People.AddRange(
                GetPersonEntity(1, name: "adam", dob: DateOnly.FromDateTime(new DateTime(2022, 02, 02)), email: "unmatching@email.com", accountEnabled: false),
                GetPersonEntity(2, name: "unknown", dob: DateOnly.FromDateTime(new DateTime(2022, 02, 02)), email: "unmatching@email.com", accountEnabled: true),
                GetPersonEntity(3, name: "unknown", dob: DateOnly.FromDateTime(new DateTime(2022, 02, 02)), email: "adam@email.com", accountEnabled: false),
                GetPersonEntity(4, name: "unknown", dob: DateOnly.FromDateTime(new DateTime(1990, 10, 10)), email: "unmatching@email.com", accountEnabled: false),
                GetPersonEntity(5, name: "unknown", dob: DateOnly.FromDateTime(new DateTime(2022, 02, 02)), email: "unmatching@email.com", accountEnabled: false)
            );
            await _context.SaveChangesAsync();

            var query = new PersonSearchQuery(Name: "ada");
            var response = await controller.Search(query);
            var result = GetResponseObject(response);
            Assert.AreEqual(new[]{1}, result.Results.Select(x => x.Id));

            query = new PersonSearchQuery(Enabled: true);
            response = await controller.Search(query);
            result = GetResponseObject(response);
            Assert.AreEqual(new[]{2}, result.Results.Select(x => x.Id));

            query = new PersonSearchQuery(Email: "ada");
            response = await controller.Search(query);
            result = GetResponseObject(response);
            Assert.AreEqual(new[]{3}, result.Results.Select(x => x.Id));

            query = new PersonSearchQuery(AgeFrom: 29, AgeTo: 31);
            response = await controller.Search(query);
            result = GetResponseObject(response);
            Assert.AreEqual(new[]{4}, result.Results.Select(x => x.Id));
        }

        [Test]
        [NonParallelizable]
        public async Task Update_InsertsPersonWithId0()
        {
            _context.People.RemoveRange(_context.People);
            _context.People.AddRange(
                GetPersonEntity(1),
                GetPersonEntity(2),
                GetPersonEntity(3),
                GetPersonEntity(4)
            );
            await _context.SaveChangesAsync();
            
            var response = await controller.Update(0, new PersonDetail(
                    0,
                    "title",
                    "updated name",
                    DateOnly.FromDateTime(DateTime.Now),
                    "updatedemail@email.com",
                    false,
                    "line 1 updated",
                    "line 2 updated",
                    "city updated",
                    "country updated",
                    "postcode updated",
                    new List<PhoneNumbers>()
                    {
                        new(0, "001", "updated", true)
                    }
                ));
            

            var result = GetResponseObject(response);
            Assert.AreEqual(5, result);
        }
        
        [Test]
        [NonParallelizable]
        public async Task Update_UpdatesPersonWithSameId()
        {
            _context.People.RemoveRange(_context.People);
            _context.People.AddRange(
                GetPersonEntity(1),
                GetPersonEntity(2),
                GetPersonEntity(3),
                GetPersonEntity(4)
            );
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var response = await controller.Update(2, new PersonDetail(
                2,
                "title",
                "updated name",
                DateOnly.FromDateTime(DateTime.Now),
                "updatedemail@email.com",
                false,
                "line 1 updated",
                "line 2 updated",
                "city updated",
                "country updated",
                "postcode updated",
                new List<PhoneNumbers>()
                {
                    new(0, "001", "updated", true)
                }
            ));
            

            var result = GetResponseObject(response);
            Assert.AreEqual(2, result);
            Assert.AreEqual("updated name", _context.People.First(x => x.Id == 2).Name);
            Assert.AreEqual("line 1 updated", _context.People.Include(x => x.Addresses).First(x => x.Id == 2).Addresses.Line1);
            Assert.AreEqual("updated", _context.People.Include(x => x.PhoneNumbers).First(x => x.Id == 2).PhoneNumbers.First().Description);
        }
        
        [Test]
        [NonParallelizable]
        public async Task Delete_DeletesPersonWithId()
        {
            _context.People.RemoveRange(_context.People);
            _context.People.AddRange(
                GetPersonEntity(1),
                GetPersonEntity(2),
                GetPersonEntity(3),
                GetPersonEntity(4)
            );
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var actionResult = await controller.Delete(2);
            Assert.IsInstanceOf<OkResult>(actionResult);
            Assert.IsNull(_context.People.FirstOrDefault(x => x.Id == 2));
        }

        private T GetResponseObject<T>(ActionResult<T> actionResult)
        {
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);

            var result = ((OkObjectResult) actionResult.Result).Value;
            Assert.IsInstanceOf<T>(result);

            return (T) result;
        }
        
        private Person GetPersonEntity(int id = 0, string title = "title", string name = "name", DateOnly? dob = null, string email = "test@test.com", bool accountEnabled = true) => 
            new(id, title, name, dob ?? DateOnly.FromDateTime(DateTime.Now), email, accountEnabled)
            {
                PhoneNumbers = new List<PhoneNumber>
                {
                    new(0, "12", "description", true, 0), 
                    new(0, "13", "description1", false, 0)
                },
                Addresses = new Address(0, "l1", "l2", "city", "country", "postcode", 0)
            };
    }
}