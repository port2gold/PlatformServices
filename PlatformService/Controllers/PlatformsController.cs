using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dto;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[Controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepos;
        private readonly IMapper _mapper;
        public PlatformsController(IPlatformRepository platformRepos, IMapper mapper)
        {
            _mapper = mapper;
            _platformRepos = platformRepos;
        }

        [HttpGet("GetAllPlatforms")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            Console.WriteLine("Getting platforms ....");

            var platformItems = _platformRepos.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));

        }

        [HttpGet("GetPlatformById/{id}")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _platformRepos.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpPost("CreatePlatform")]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _platformRepos.CreatePlatform(platformModel);
            _platformRepos.SaveChanges();

            var plaformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = plaformReadDto.Id, plaformReadDto });
        }
    }
}