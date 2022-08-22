using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;
using Amazon.Lambda.Core;
using PrayerTime.Service;
using Alexa.NET.ProactiveEvents;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace PrayerTime;

    public class Function
    {
    public Function()
    {
        new ProactiveSubscriptionChangedRequestHandler().AddToRequestHandler();
    }

    /// <summary>
    /// The main function for reacting to the user interaction.
    /// </summary>
    /// <param name="input">Skill input</param>
    /// <param name="context">Lambda context</param>
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var log = context.Logger;

            //log.LogLine($"Skill Request Object:" + JsonConvert.SerializeObject(input));

            var session = input.Session;

            var namazService = new PrayerService();

        if (input.Request is ProactiveSubscriptionChangedRequest request)
        {
            var remainingEventTypes = request.Body?.Subscriptions;

            if (remainingEventTypes!= null)
            {
                log.LogLine($"Notification Changed:" + JsonConvert.SerializeObject(remainingEventTypes));
            }

            log.LogLine($"Notification type:" + request.Type);
        }

            if (input.Request is LaunchRequest)
            {
                var speech = await namazService.PrayerTimeByCoordinate(input);

                var rp = new Reprompt("Prayer start");

                return ResponseBuilder.Ask(speech, rp, session);
            }

            if (input.Request is SessionEndedRequest)
            {
                return ResponseBuilder.Tell("Don't forget me in your prayers, Goodbye!");
            }
            
            if (input.Request is IntentRequest intentRequest)
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
                        var speech = await namazService.PrayerTimeByCoordinate(input);

                        var rp = new Reprompt("Prayer start");

                        return ResponseBuilder.Ask(speech, rp, session);
                    }
                    case "PrayerByCityIntent":
                    {
                        var speech = await namazService.PrayerTimeByCity(input);

                        var rp = new Reprompt("Prayer start");

                        return ResponseBuilder.Ask(speech, rp, session);
                    }
                    default:
                    {
                        log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        
                        var speech = "Sorry, I don't understand - try again, please!";
                        
                        var rp = new Reprompt(speech);
                        
                        return ResponseBuilder.Ask(speech, rp, session);
                    }
                }
            }

            return ResponseBuilder.Tell("Don't forget me in your prayers, Goodbye!");
        }
    }