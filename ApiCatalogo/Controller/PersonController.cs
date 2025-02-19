using AutoMapper;
using MainBlog.DTOs.Request;
using MainBlog.IService;
using MainBlog.Models;
using MainBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace MainBlog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public PersonController(
            IMapper mapper,
            IPersonService personService)
        {
            _mapper = mapper;
            _personService = personService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonAsync(PersonRequestDTO personDTO) {
            var person = _mapper.Map<Person>(personDTO);
            var newPerson = await _personService.CreatePersonAsync(person);
            if (newPerson != null)
                return Ok(newPerson);
            return BadRequest();
        }
    }
}
