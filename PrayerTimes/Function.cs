using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Directive;
using Amazon.Lambda.Core;
using PrayerTimes.Service;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace PrayerTimes
{
    public class Function
    {
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            ILambdaLogger log = context.Logger;

            log.LogLine($"Skill Request Object:" + JsonConvert.SerializeObject(input));

            Session session = input.Session;

            IPrayerService namazService = new PrayerService();

            if (input.Request is LaunchRequest)
            {
                var speech = await namazService.PerayerTimeByCoordinate(input);

                Reprompt rp = new Reprompt("Prayer start");

                return ResponseBuilder.Ask(speech, rp, session);
            }
            else if (input.Request is SessionEndedRequest)
            {
                return ResponseBuilder.Tell("Don't forget me in your prayers, Goodbye!");
            }
            else if (input.Request is IntentRequest intentRequest)
            {
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                    case "AMAZON.StopIntent":
                        return ResponseBuilder.Tell("Don't forget me in your prayers, Goodbye!");

                    case "AMAZON.HelpIntent":
                        {
                            return ResponseBuilder.Tell("This skill needs to have access to your device location, so please give location access from Alexa setting.");
                        }
                    case "PrayerByInputIntent":
                        {
                            var speech = await namazService.PerayerTimeByCoordinate(input);

                            return ResponseBuilder.Tell(speech);
                        }
                    case "PrayerByCityIntent":
                        {
                            var speech = await namazService.PerayerTimeByCity(input);

                            return ResponseBuilder.Tell(speech);
                        }
                    default:
                        {
                            log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                            string speech = "I didn't understand - try again, please!";
                            Reprompt rp = new Reprompt(speech);
                            return ResponseBuilder.Ask(speech, rp, session);
                        }
                }
            }

            return ResponseBuilder.Tell("Don't forget me in your prayers, Goodbye!");
        }
    }
}