using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Flurl.Http;
using PrayerTimes.Domain;
using System;
using System.Threading.Tasks;
using System.Web;

namespace PrayerTimes.Service
{
    public class PrayerService : IPrayerService
    {
        private const string _ssml = @"<speak>
                                            <amazon:emotion name=""excited"" intensity=""high"">
                                                Today prayer time is
                                            </amazon:emotion>
                                            <break time=""1s""/>
                                                Fajr: <say-as interpret-as=""time"">#FAJR#</say-as>
                                            <break time=""1s""/>
                                                Dhuhr: <say-as interpret-as=""time"">#DHUHR#</say-as>
                                            <break time=""1s""/>
                                                Maghrib: <say-as interpret-as=""time"">#MAGHRIB#</say-as>
                                            <break time=""1s""/>
                                        </speak>";

        public async Task<SsmlOutputSpeech> PerayerTimeByCoordinate(SkillRequest input)
        {
            if (input.Context?.Geolocation == null)
            {
                var ssml = @"<speak>
                                <amazon:emotion name=""disappointed"" intensity=""high"">
                                    Sorry, I don't have access to your device location, please give me access from the application setting.
                                </amazon:emotion>
                                <break time=""1s""/>
                                <p>But for now, please tell me when do you live. It should be like:</p>
                                <break time=""1s""/>
                                <p>My country is the United Kingdom and my city is London</p>
                                <break time=""1s""/>
                                <p>Now please tell me where do you live?<p>
                            </speak>";

                return new SsmlOutputSpeech(ssml);
            }

            var time = input.Request?.Timestamp ?? DateTime.Now;

            var lat = input.Context.Geolocation.Coordinate.Latitude;
            var lon = input.Context.Geolocation.Coordinate.Longitude;

            var timings = await GetTimings(time, lat, lon);

            var speech = new SsmlOutputSpeech
            {
                Ssml = _ssml.Replace("#FAJR#", timings.Fajr)
                            .Replace("#DHUHR#", timings.Dhuhr)
                            .Replace("#MAGHRIB#", timings.Maghrib)
            };

            return speech;
        }

        public async Task<SsmlOutputSpeech> PerayerTimeByCity(SkillRequest input)
        {
            var intentRequest = (IntentRequest)input.Request;

            string userCountry = intentRequest.Intent.Slots["Coutry"].Value;
            string userCity = intentRequest.Intent.Slots["City"].Value;

            var time = input.Request?.Timestamp ?? DateTime.Now;

            var timings = await GetTimings(time, userCountry, userCity);

            var speech = new SsmlOutputSpeech
            {
                Ssml = _ssml.Replace("#FAJR#", timings.Fajr)
                            .Replace("#DHUHR#", timings.Dhuhr)
                            .Replace("#MAGHRIB#", timings.Maghrib)
            };

            return speech;
        }

        public async Task<Timings> GetTimings(DateTime adhanDate, double latitude, double longitude)
        {
            var prayerResponse = await $"https://api.aladhan.com/v1/timings/{adhanDate.ToString("dd-MM-yyyy")}?latitude={latitude}&longitude={longitude}&method=7".GetJsonAsync<PrayerResponse>();

            return prayerResponse.data.timings;
        }

        public async Task<Timings> GetTimings(DateTime adhanDate, string country, string city)
        {
            var prayerResponse = await $"https://api.aladhan.com/v1/timingsByCity/{adhanDate.ToString("dd-MM-yyyy")}?city={HttpUtility.UrlEncode(city)}&country={HttpUtility.UrlEncode(country)}&method=7".GetJsonAsync<PrayerResponse>();

            return prayerResponse.data.timings;
        }
    }
}