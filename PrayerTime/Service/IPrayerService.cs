using System;
using System.Threading.Tasks;
using PrayerTime.Domain;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace PrayerTime.Service
{
    public interface IPrayerService
    {
        /// <summary>
        /// Return adhan times in the specified location after calling an API.
        /// </summary>
        /// <param name="adhanDate">Date in this format: DD-MM-YYYY</param>
        /// <param name="latitude">Latitude of the location</param>
        /// <param name="longitude">Longitude of the location</param>
        Task<Timings> GetAdhanTime(DateOnly adhanDate, double latitude, double longitude);

        /// <summary>
        /// Return adhan times in the specified country and city after calling an API.
        /// </summary>
        /// <param name="adhanDate">Date in this format: DD-MM-YYYY</param>
        /// <param name="country">Country</param>
        /// <param name="city">City</param>
        Task<Timings> GetAdhanTime(DateOnly adhanDate, string country, string city);

        /// <summary>
        /// Return Alexa speech response
        /// </summary>
        Task<SsmlOutputSpeech> PrayerTimeByCoordinate(SkillRequest input);

        /// <summary>
        /// Return Alexa speech response
        /// </summary>
        Task<SsmlOutputSpeech> PrayerTimeByCity(SkillRequest input);
    }
}