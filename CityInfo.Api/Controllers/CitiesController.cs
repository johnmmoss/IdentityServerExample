using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Model;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository citiesInfoRepository)
        {
            _cityInfoRepository = citiesInfoRepository ?? throw new ArgumentNullException(nameof(citiesInfoRepository));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();

            var results = AutoMapper.Mapper.Map<IEnumerable<CityNoPointsOfInterest>>(cityEntities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var cityEntity = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (cityEntity == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = AutoMapper.Mapper.Map<CityDto>(cityEntity);
                return Ok(cityResult);
            }

            var cityNoPointsOfInterest = AutoMapper.Mapper.Map<CityNoPointsOfInterest>(cityEntity);

            return Ok(cityNoPointsOfInterest);
        }
    }
}
