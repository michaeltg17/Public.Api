using SimMetrics.Net.Metric;
using System;

namespace Application.Services
{
    public class CityService
    {
        public static bool IsSameCity(string city, string candidate)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(city, nameof(city));
            ArgumentException.ThrowIfNullOrWhiteSpace(candidate, nameof(candidate));

            if (city == candidate) return true;

            var similarity = new Levenstein().GetSimilarity(city, candidate);

            return similarity > 0.70;
        }
    }
}
