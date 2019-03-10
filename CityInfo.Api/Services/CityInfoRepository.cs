using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(x => x.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterst)
        {
            if (includePointsOfInterst)
            {
                return _context.Cities.FirstOrDefault(x => x.Id == cityId);
            }

            return _context.Cities.Include(x => x.PointOfInterest ).FirstOrDefault(x => x.Id == cityId);
        }

        public IEnumerable<PointOfInterest> GetPointOfInterests(int cityId)
        {
            return _context.PointOfInterests.Where(p => p.CityId == cityId).ToList();
        }

        public PointOfInterest GetPointOfInterestsForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterests.FirstOrDefault(p => p.CityId == cityId && p.Id == pointOfInterestId);
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(x => x.Id == cityId);
        }

        public void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointOfInterest.Add(pointOfInterest);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
