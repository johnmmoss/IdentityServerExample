using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityInfo.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext _context;

        public DummyController(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
