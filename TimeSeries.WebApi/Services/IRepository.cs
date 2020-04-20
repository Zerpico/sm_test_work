using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSeries.Common.Models;

namespace TimeSeries.WebApi.Services
{
    public interface IRepository
    {
        IEnumerable<Country> GetCountries();
        IEnumerable<Indicator> GetIndicators();

        Task<Serie> GetSerie(int countryId, int indicatorId);
        Task<Serie> GetSerie(int serieId);

        bool SetSerieComment(int id, string comment);
        bool SetSerieComment(int country, int indicator, string comment);
    }
}
