using Alexa.NET.Request;
using Alexa.NET.Response;
using PrayerTime.Domain;
using System;
using System.Threading.Tasks;

namespace PrayerTime.Service
{
    public interface IPrayerService
    {
        /// <summary>
        /// Return adhan times in the specified location.
        /// </summary>
        /// <param name="adhanDate">Date in this format: DD-MM-YYYY</param>
        Task<Timings> GetTimings(DateTime adhanDate, double latitude, double longitude);

        /// <summary>
        /// Return adhan times in the specified location.
        /// </summary>
        /// <param name="adhanDate">Date in this format: DD-MM-YYYY</param>
        Task<Timings> GetTimings(DateTime adhanDate, string country, string city);

        /// <summary>
        /// Return Alexa speech response
        /// </summary>
        Task<SsmlOutputSpeech> PerayerTimeByCoordinate(SkillRequest input);

        /// <summary>
        /// Return Alexa speech response
        /// </summary>
        Task<SsmlOutputSpeech> PerayerTimeByCity(SkillRequest input);
    }
}