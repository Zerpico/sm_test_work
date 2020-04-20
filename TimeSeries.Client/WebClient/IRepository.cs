using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSeries.Common.Models;

namespace TimeSeries.Client.WebClient
{
    public interface IRepository
    {
        /// <summary>
        /// Получить справочник стран
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Country>> GetCountries();

        /// <summary>
        /// Получить справочник индикаторов 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Indicator>> GetIndicators();

        /// <summary>
        /// Полученияе ряда и его точек 
        /// </summary>
        /// <param name="countryId">ключ страны</param>
        /// <param name="indicatorId">ключ индикатора</param>
        /// <returns></returns>
        Task<Serie> GetSerie(int countryId, int indicatorId);

        /// <summary>
        /// Полученияе ряда и его точек 
        /// </summary>
        /// <param name="serieId">ключ ряда</param>
        /// <returns></returns>
        Task<Serie> GetSerie(int serieId);

        /// <summary>
        /// Изменить комментарий ряда
        /// </summary>      
        Task<bool> SetSerieComment(Serie serie);

        
    }
}
