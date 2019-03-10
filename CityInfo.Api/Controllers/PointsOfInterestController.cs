using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Model;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            this._cityInfoRepository = cityInfoRepository;
            // To get out without constructor injection:
            //HttpContext.RequestServices.GetService(typeof(ILogger<PointsOfInterestController>));
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogWarning($"City with id {cityId} was not found.");
                    return NotFound();
                }

                var pointsOfInterestCity = _cityInfoRepository.GetPointOfInterests(cityId);

                var pointsOfInterestForCityResult =
                    AutoMapper.Mapper.Map<IEnumerable<PointOfInterest>>(pointsOfInterestCity);

                return Ok(pointsOfInterestForCityResult);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception while getting points of interest for city Id: " + cityId);
                return StatusCode(500, "A problem occured with your request");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestsForCity(cityId, id);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = AutoMapper.Mapper.Map<PointOfInterestDto>(pointOfInterestEntity);

            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "Description must not be the same as the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = AutoMapper.Mapper.Map<PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterest(cityId, finalPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem occured saving the data");
            }

            var returnPointOfInterest = AutoMapper.Mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = returnPointOfInterest.Id }, returnPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "Description must not be the same as the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = _cityInfoRepository.GetPointOfInterestsForCity(cityId, id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            AutoMapper.Mapper.Map(pointOfInterest, pointOfInterestFromStore);

            if (!_cityInfoRepository.Save()) // Entity still tracked by repository.
            {
                return StatusCode(500, "A problem occured saving the data");
            }

            return NoContent();
        }

        // Sample Patch Request using Json Patch 
        //[{
        //    "op" : "replace",
        //    "path" : "/name",
        //    "value": "Central Perk :)"
        //}]
        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestsForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = AutoMapper.Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) // Check is performed againest the JsonPatchDocument as a model
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "Description must not be the same as the name.");
            }

            TryValidateModel(pointOfInterestToPatch); // Force validation to occur againest the correct object

            if (!ModelState.IsValid)  // Check that the correct object is now valid.
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_cityInfoRepository.Save()) // Entity still tracked by repository.
            {
                return StatusCode(500, "A problem occured saving the data");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestsForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_cityInfoRepository.Save()) // Entity still tracked by repository.
            {
                return StatusCode(500, "A problem occured saving the data");
            }

            return NoContent();
        }
    }
}
