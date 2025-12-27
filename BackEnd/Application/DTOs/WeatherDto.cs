using System;

namespace Paye.Application.DTOs
{
    public class WeatherDto
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}