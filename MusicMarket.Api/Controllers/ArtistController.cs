using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicMarket.Api.DTO;
using MusicMarket.Api.Validator;
using MusicMarket.Core.Model;
using MusicMarket.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace MusicMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;
        ILogger<ArtistController> _logger;
        public ArtistController(IArtistService artistService, IMapper mapper, ILogger<ArtistController> logger)
        {
            _mapper = mapper;
            _artistService = artistService;
            _logger = logger;
        }
        /// <summary>
        /// Bütün artistleri getirmektedir
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetAllArtists()
        {
            _logger.LogInformation("Test");

            try
            {
                throw new Exception("Some bad code was executed");

                var artists = await _artistService.GetAllArtists();

                var artistResources = _mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistDTO>>(artists);

                return Ok(artistResources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Index action of the Artist Controller");
            }

            return BadRequest("Error Artist Controller");
        }

        /// <summary>
        /// Id'si verilen artisti getirmektedir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDTO>> GetArtistById(int id)
        {
            var artist = await _artistService.GetArtistById(id);

            var artistResource = _mapper.Map<Artist, ArtistDTO>(artist);

            return Ok(artistResource);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistDTO>> CreateArtist(SaveArtistDTO saveArtistResource)
        {
            var validator = new SaveArtistResourceValidator();
            var validationResult = await validator.ValidateAsync(saveArtistResource);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors); // validationResult.Errors içerisinde hataları gösterecektir
            }

            var artistToCreate = _mapper.Map<SaveArtistDTO, Artist>(saveArtistResource);

            var newArtist = await _artistService.CreateArtist(artistToCreate);

            var artist = await _artistService.GetArtistById(newArtist.Id);

            var artistResource = _mapper.Map<Artist, ArtistDTO>(artist);

            return Ok(artistResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistDTO>> UpdateArtist(int id, SaveArtistDTO saveArtistResource)
        {
            var validator = new SaveArtistResourceValidator();
            var validationResult = await validator.ValidateAsync(saveArtistResource);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var artistToBeUpdated = await _artistService.GetArtistById(id);

            if (artistToBeUpdated == null)
            {
                return NotFound();
            }

            var artist = _mapper.Map<SaveArtistDTO, Artist>(saveArtistResource);

            await _artistService.UpdateArtist(artistToBeUpdated, artist);

            var updatedArtist = await _artistService.GetArtistById(id);

            var updatedArtistResource = _mapper.Map<Artist, ArtistDTO>(updatedArtist);

            return Ok(updatedArtistResource);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var artist = await _artistService.GetArtistById(id);

            if (artist == null)
            {
                return NotFound();
            }

            await _artistService.DeleteArtist(artist);

            return NoContent();
        }
    }
}
