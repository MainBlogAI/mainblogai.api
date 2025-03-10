﻿using AutoMapper;
using MainBlog.DTOs.AuthenticationsDTO;
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
                return Ok(_mapper.Map<PersonCreateResponse>(newPerson));
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonByUserIdAsync(string userId)
        {
            var newPerson = await _personService.GetPersonByUserIdAsync(userId);
            if (newPerson != null)
                return Ok(_mapper.Map<PersonCreateResponse>(newPerson));
            return NotFound();
        }
    }
}
