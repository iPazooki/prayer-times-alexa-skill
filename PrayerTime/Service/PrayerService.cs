using System;
using System.Web;
using System.Threading.Tasks;
using Flurl.Http;
using PrayerTime.Domain;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;

namespace PrayerTime.Service
{
    public class PrayerService : IPrayerService
    {
        private const string GET_BY_CITY = "https://vdr4aoq5zi.execute-api.eu-west-2.amazonaws.com/production/city";

        private const string GET_BY_LOCATION = "https://vdr4aoq5zi.execute-api.eu-west-2.amazonaws.com/production/location";

        private const string SSML = @"<speak>
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

        public async Task<SsmlOutputSpeech> PrayerTimeByCoordinate(SkillRequest input)
        {
            if (input.Context?.Geolocation == null)
            {
                var ssml = @"<speak>
                                <amazon:emotion name=""disappointed"" intensity=""low"">
                                    Sorry, I don't have access to your device location, please give me access from the application setting.
                                </amazon:emotion>
                                <p>But for now, please tell me where do you live. It should be like:</p>
                                <break time=""1s""/>
                                <p>My country is the United Kingdom and my city is London</p>
                                <break time=""1s""/>
                                <p>Now please tell me where do you live?</p>
                            </speak>";

                return new SsmlOutputSpeech(ssml);
            }

            var dateTime = input.Request?.Timestamp ?? DateTime.Now;

            var lat = input.Context.Geolocation.Coordinate.Latitude;

            var lon = input.Context.Geolocation.Coordinate.Longitude;

            var adhanTime = await GetAdhanTime(DateOnly.FromDateTime(dateTime), lat, lon);

            var speech = new SsmlOutputSpeech
            {
                Ssml = SSML.Replace("#FAJR#", adhanTime.Fajr)
                    .Replace("#DHUHR#", adhanTime.Dhuhr)
                    .Replace("#MAGHRIB#", adhanTime.Maghrib)
            };

            return speech;
        }

        public async Task<SsmlOutputSpeech> PrayerTimeByCity(SkillRequest input)
        {
            var intentRequest = (IntentRequest) input.Request;

            var userCountry = intentRequest.Intent.Slots["Country"].Value;

            var userCity = intentRequest.Intent.Slots["City"].Value;

            var speech = new SsmlOutputSpeech();

            if (string.IsNullOrEmpty(userCountry) || string.IsNullOrEmpty(userCity))
            {
                speech.Ssml = "<speak>Sorry, I didn't get your country and city! Could you please repeat again?</speak>";

                return speech;
            }

            var dateTime = input.Request?.Timestamp ?? DateTime.Now;

            var timings = await GetAdhanTime(DateOnly.FromDateTime(dateTime), userCountry, userCity);

            speech.Ssml = SSML.Replace("#FAJR#", timings.Fajr)
                .Replace("#DHUHR#", timings.Dhuhr)
                .Replace("#MAGHRIB#", timings.Maghrib);

            return speech;
        }

        public async Task<Timings> GetAdhanTime(DateOnly adhanDate, double latitude, double longitude)
        {
            // Send an API call to retrieve prayer time
            var prayerResponse = await $"{GET_BY_LOCATION}?date={adhanDate.ToString("dd-MM-yyyy")}&latitude={latitude}&longitude={longitude}&method=7".GetJsonAsync<PrayerResponse>();

            return prayerResponse.Data.Timings;
        }

        public async Task<Timings> GetAdhanTime(DateOnly adhanDate, string country, string city)
        {
            // Send an API call to retrieve prayer time
            var prayerResponse = await $"{GET_BY_CITY}?date={adhanDate.ToString("dd-MM-yyyy")}&city={HttpUtility.UrlEncode(city)}&country={HttpUtility.UrlEncode(country)}&method=7".GetJsonAsync<PrayerResponse>();

            return prayerResponse.Data.Timings;
        }
    }
}