using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Models.Exceptions;
using UKParliament.CodeTest.Models.Models;
using UKParliament.CodeTest.Models.Queries;
using UKParliament.CodeTest.Services.Interfaces;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<PersonDetail>> GetById(int id)
        {
            var personDetail = await _personService.GetPersonDetail(id);

            if (personDetail is null)
            {
                return NotFound($"No person with Id {id}");
            }
            
            return Ok(personDetail);
        }
        
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<SearchResult<PersonSummary>>> Search([FromQuery]PersonSearchQuery query)
        {
            var searchResult = await _personService.SearchPeople(query);
            return Ok(searchResult);
        }
        
        [Route("{id:int}")]
        [HttpPost]
        public async Task<ActionResult<int>> Update(int id, [FromBody]PersonDetail person)
        {
            if (id != person.Id)
            {
                throw new ValidationException($"Request to update person with id ${id} was sent with details for Person with id ${id}");
            }
            
            var personId = await _personService.UpdatePerson(person);
            return Ok(personId);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            await _personService.DeletePerson(id);
            return Ok();
        }
    }
}