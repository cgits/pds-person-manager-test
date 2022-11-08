using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UKParliament.CodeTest.Models.Delegates;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Interfaces;
using UKParliament.CodeTest.Web.Controllers;

namespace UKParliament.CodeTest.Tests.UnitTests.ServiceTests
{
    public class PersonValidatorTest
    {
        private CurrentTime _currentTime;

        private PersonValidator validator;
        
        [SetUp]
        public void Setup()
        {
            _currentTime = A.Fake<CurrentTime>();
            validator = new PersonValidator(_currentTime);
        }
        
        [Test]
        public void ValidateSearch_ReturnsEmpty_IfValid()
        {
            var errors = validator.ValidateSearchQuery(GetValidSearchQuery());
            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidateSearch_ReturnsError_IfAgeToLessThanAgeFrom()
        {
            var query = GetValidSearchQuery() with {AgeFrom = 29, AgeTo = 20};
            var errors = validator.ValidateSearchQuery(query);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ValidatePerson_ReturnsEmpty_IfValid()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now.AddDays(1));

            var person = GetValidDetailQuery();
            var errors = validator.ValidatePerson(person);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidatePerson_ReturnsError_IfEmptyTitle()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now.AddDays(1));

            var person = GetValidDetailQuery() with { Title = ""};
            var errors = validator.ValidatePerson(person);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ValidatePerson_ReturnsError_IfEmptyName()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now.AddDays(1));

            var person = GetValidDetailQuery() with { Name = ""};
            var errors = validator.ValidatePerson(person);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ValidatePerson_ReturnsError_IfEmptyEmail()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now.AddDays(1));

            var person = GetValidDetailQuery() with { Email = ""};
            var errors = validator.ValidatePerson(person);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ValidatePerson_ReturnsError_IfDobFuture()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now);
            
            var person = GetValidDetailQuery() with { DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(1))};
            var errors = validator.ValidatePerson(person);
            Assert.AreEqual(1, errors.Count);
        }
        
        [Test]
        public void ValidatePerson_ReturnsError_IfAnyAddressAndPartEmpty()
        {
            A.CallTo(() => _currentTime.Invoke()).Returns(DateTime.Now.AddDays(1));

            var invalidPeople = new List<PersonDetail>
            {
                GetValidDetailQuery() with { LineOne = ""},
                GetValidDetailQuery() with { City = ""},
                GetValidDetailQuery() with { PostCode = ""},
                GetValidDetailQuery() with { Country = ""},
            };

            Assert.IsTrue(invalidPeople.Select(people => validator.ValidatePerson(people)).All(x => x.Count == 1));
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void Validate_Throws_IfNotEmpty_ElseReturns(bool isEmpty)
        {
            Func<bool, ICollection<string>> validateMethod = x => isEmpty ? new List<string>() : new List<string>() { "test" };

            if (isEmpty)
            {
                Assert.DoesNotThrow(() => validator.Validate(validateMethod, isEmpty));
            }
            else
            {
                Assert.Throws<ValidationException>(() => validator.Validate(validateMethod, isEmpty));
            }
        }
        
        private PersonSearchQuery GetValidSearchQuery() => new("Name", 20, 29, "test", true);
        private PersonDetail GetValidDetailQuery() => new(0, "title", "name", DateOnly.FromDateTime(DateTime.Now), "test@test.com", true, "l1", "l2", "city", "country", "postcode", new List<PhoneNumbers> { new(0, "12", "description", true), new(0, "13", "description1", false) });
    }
}