using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSeries.Common.Models;
using TimeSeries.WebApi.DAL;

namespace TimeSeries.WebApi.Services
{
    public class ContextRepository : IRepository
    {
        DbTimeSeriesContext _repository;
        public ContextRepository(DbTimeSeriesContext repository)
        {
            _repository = repository;

        }

        public IEnumerable<Country> GetCountries()
        {
            return  _repository.Countries;
        }

        public IEnumerable<Indicator> GetIndicators()
        {
            return  _repository.Indicators;
        }

        public async Task<Serie> GetSerie(int countryId, int indicatorId)
        {
            return await _repository.Series
                    .Include(x => x.Country)
                    .Include(d => d.Indicator)
                        .Include(o => o.Observables)
                    .FirstOrDefaultAsync(d => d.Country.CountryId == countryId &&
                        d.Indicator.IndicatorId == indicatorId);
        }

        public async Task<Serie> GetSerie(int serieId)
        {
            return await  _repository.Series
                .Include(x => x.Country)
                .Include(d => d.Indicator)
                .Include(o => o.Observables)
                .FirstOrDefaultAsync(d => d.SerieId == serieId);
        }

        public bool SetSerieComment(int id, string comment)
        {            
            var result = _repository.Series.SingleOrDefault(b => b.SerieId == id);
            if (result != null)
            {
                result.Comment = comment;
                _repository.SaveChanges();
                return true;
            }

            return false;
        }

        public bool SetSerieComment(int country, int indicator, string comment)
        {
            var result = _repository.Series.SingleOrDefault(
                b => b.Country.CountryId == country && b.Indicator.IndicatorId == indicator);
            if (result != null)
            {
                result.Comment = comment;
                _repository.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
