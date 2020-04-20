using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeSeries.Common.Models;

namespace TimeSeries.Client.WebClient
{
    public class WebRepository : IRepository
    {        
        private HttpClient client;
        private readonly string APP_URL ;

        public WebRepository(string api_url)
        {
            APP_URL = api_url;
            client = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {            
            //наши настройки
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
            httpClientHandler.AllowAutoRedirect = true;

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // создаем объект клиента HTTP
            return new HttpClient(handler: httpClientHandler, disposeHandler: true);
        }

      
        public async Task<IEnumerable<Country>> GetCountries()
        {
            try
            {
                var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, $"Dictionary/country"));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<Country>>();
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }

        public async Task<IEnumerable<Indicator>> GetIndicators()
        {
            try
            {
                var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, $"Dictionary/indicators"));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<Indicator>>();
                }
                return null;
            }
            catch(Exception ex)
            { return null; }
        }

        public async Task<Serie> GetSerie(int countryId, int indicatorId)
        {
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, $"Series?country={countryId}&indicator={indicatorId}"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<Serie>();
            }
            return null;
        }

        public async Task<Serie> GetSerie(int serieId)
        {
            var response = await client.GetAsync(System.IO.Path.Combine(APP_URL, $"Series", serieId.ToString()));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsAsync<Serie>();
            }
            return null;
        }

        public async Task<bool> SetSerieComment(Serie serie)
        {           
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(nameof(serie.SerieId), serie.SerieId.ToString()),
                new KeyValuePair<string, string>(nameof(serie.Comment), serie.Comment)
            });           

            var result = await client.PostAsync(System.IO.Path.Combine(APP_URL, $"Series"), content);

            if (result.StatusCode == HttpStatusCode.NoContent)
                return true;
            else return false;
        }

    }
}
