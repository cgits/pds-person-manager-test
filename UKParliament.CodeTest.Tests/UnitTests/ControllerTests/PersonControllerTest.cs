using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Services.Interfaces;
using UKParliament.CodeTest.Web.Controllers;

namespace UKParliament.CodeTest.Tests.UnitTests.ControllerTests
{
    public class PersonControllerTest
    {
        private IPersonService _fakePersonService;

        private PersonController controller;
        
        [SetUp]
        public void Setup()
        {
            _fakePersonService = A.Fake<IPersonService>();
            controller = new PersonController(_fakePersonService);
        }
        
        [Test]
        public async Task GetById_ReturnsNotfound_IfNull()
        {
            var personDetail = Task.FromResult<PersonDetail?>(null);
            A.CallTo(() => _fakePersonService.GetPersonDetail(A<int>.That.IsEqualTo(1))).Returns(personDetail);
            var result = await controller.GetById(1);
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
        
        [Test]
        public async Task GetById_ReturnsPerson_IfFound()
        {
            var personDetail = new PersonDetail(1, "title", "name", DateOnly.MaxValue, "email");
            A.CallTo(() => _fakePersonService.GetPersonDetail(A<int>.That.IsEqualTo(1))).Returns(Task.FromResult(personDetail));
            var result = await controller.GetById(1);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.AreSame(personDetail, ((OkObjectResult)result.Result).Value);
        }

        [Test]
        public void Update_ThrowsValidationError_IfDifferentIds()
        {
            var personDetail = new PersonDetail(1, "title", "name", DateOnly.MaxValue, "email");
            Assert.ThrowsAsync<ValidationException>(() => controller.Update(2, personDetail));
        }

        [Test]
        public async Task Update_ReturnsPersonId_IfSameIds()
        {
            var personDetail = new PersonDetail(1, "title", "name", DateOnly.MaxValue, "email");
         
            A.CallTo(() => _fakePersonService.UpdatePerson(A<PersonDetail>.That.IsSameAs(personDetail))).Returns(Task.FromResult(personDetail.Id));

            var result = await controller.Update(1, personDetail);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.AreEqual(1, ((OkObjectResult)result.Result).Value);
        }
    }
}