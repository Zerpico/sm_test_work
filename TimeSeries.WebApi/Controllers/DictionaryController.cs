using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSeries.WebApi.Services;

namespace TimeSeries.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        IRepository _repository;

        public DictionaryController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{name}", Name = "GetDictionary")]
        public ActionResult GetDictionary(string name)
        {
            if (string.IsNullOrEmpty(name)) 
                return BadRequest();

            switch (name.ToLower())
            {
                case "country":
                    return Ok(_repository.GetCountries());
                case "indicators":
                    return Ok(_repository.GetIndicators());
                default:
                    return NotFound();
            }
        }
    }
}