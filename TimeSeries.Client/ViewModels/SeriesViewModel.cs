using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeries.Client.ViewModels.Base;
using TimeSeries.Common.Models;
using OxyPlot;
using OxyPlot.Series;
using TimeSeries.Client.WebClient;
using System.Configuration;

namespace TimeSeries.Client.ViewModels
{
    public class SeriesViewModel : ViewModelBase
    {
        IRepository _repository;

        public SeriesViewModel()
        {
            string url = ConfigurationManager.AppSettings["api_url"].ToString();

            _repository = new WebRepository(url);
            GetDictionary();
            InitializeCommands();
        }

        private async void GetDictionary()
        {
            Indicators = new ObservableCollection<Indicator>(await _repository.GetIndicators());
            Countries = new ObservableCollection<Country>(await _repository.GetCountries());
        }

        ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries
        {
            get => countries;
            set { countries = value; OnPropertyChanged(nameof(Countries)); }
        }

        ObservableCollection<Indicator> indicators;
        public ObservableCollection<Indicator> Indicators
        {
            get => indicators;
            set { indicators = value; OnPropertyChanged(nameof(Indicators)); }
        }

        private Country selectedCountry;
        public Country SelectedCountry
        {
            get => selectedCountry;
            set { selectedCountry = value; OnPropertyChanged(nameof(SelectedCountry)); GetSeries(); }
        }

        private Indicator selectedIndicator;
        public Indicator SelectedIndicator
        {
            get => selectedIndicator;
            set { selectedIndicator = value; OnPropertyChanged(nameof(SelectedIndicator)); GetSeries(); }
        }

        public string Comment
        {
            get => serie?.Comment;
            set { if (serie != null) serie.Comment = value; UpdateComment(); }
        }

        private Serie serie;
        public Serie Serie 
        { 
            get { return serie; }
            private set { serie = value; OnPropertyChanged(nameof(Serie)); OnPropertyChanged(nameof(Comment)); }
        }

      
        public IList<DataPoint> Points { get; private set; }

        private async void GetSeries()
        {
            if (selectedIndicator != null && selectedCountry != null)
            {
                Serie = await _repository.GetSerie(selectedCountry.CountryId, selectedIndicator.IndicatorId);

                if (serie != null)
                {
                    Points = new List<DataPoint>();
                    foreach (var obs in Serie.Observables.OrderBy(d => d.Time))
                    {
                        Points.Add(new DataPoint(obs.Time.Year, obs.ObservableValue));
                    }
                }
                else Points = null;

                OnPropertyChanged(nameof(Points));
            }
        }

        async void UpdateComment()
        {
            await _repository.SetSerieComment(serie);
        }

        private void InitializeCommands()
        {
            
        }
    }
}
