using System.Collections.Generic;
using Paye.Domain.Entities;
using System.Threading.Tasks;

namespace Paye.Domain.Interfaces
{
    public interface IWeatherRepository
    {
        Task<IEnumerable<WeatherForecast>> GetForecastAsync();
    }
}