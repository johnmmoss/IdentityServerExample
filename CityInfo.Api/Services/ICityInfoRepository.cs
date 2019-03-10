using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityInfo.Api.Entities;

namespace CityInfo.Api.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool includePointsOfInterest);

        IEnumerable<PointOfInterest> GetPointOfInterests(int cityId);

        PointOfInterest GetPointOfInterestsForCity(int cityId, int pointOfInterestId);

        bool CityExists(int cityId);

        void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        bool Save();
    }
}
