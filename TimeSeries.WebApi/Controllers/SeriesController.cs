using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSeries.Common.Models;
using TimeSeries.WebApi.Services;

namespace TimeSeries.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        IRepository _repository;

        public SeriesController(IRepository repository)
        {
            _repository = repository;
        }

        //Series?country&indicator
        [HttpGet]
        public async Task<ActionResult<Serie>> GetData(int country, int indicator)
        {
            //Ключ страны и ключ индикатора 
            var result = await _repository.GetSerie(country, indicator);

            if (result == null) return NotFound();

            return result;
        }

        //Series/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Serie>> GetData(int id)
        {
            //Ключ ряда
            var result = await _repository.GetSerie(id);

            if (result == null) return NotFound();

            return result;
        }

        [HttpPost]
        public ActionResult SetComment([FromForm]Serie serie)
        {
            if (serie == null)            
                return BadRequest();

            if (serie.SerieId != 0)
            {
                if (!_repository.SetSerieComment(serie.SerieId, serie.Comment))
                    return BadRequest();
            }
            else if (!_repository.SetSerieComment(serie.Country.CountryId, serie.Indicator.IndicatorId, serie.Comment))
                return BadRequest();


            return NoContent();
        }

      /*  [HttpPost]
        public ActionResult SetComment(int country, int indicator, string comment)
        {
            if (!_repository.SetSerieComment(country, indicator, comment))
                return BadRequest();

            return NoContent();
        }*/
    }
}