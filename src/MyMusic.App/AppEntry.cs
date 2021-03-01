using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MyMusic.Domain;
using MyMusic.Domain.Exceptions;
using MyMusic.Infrastructure;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.App
{
    public class AppEntry
    {
        private readonly IEnumerable<IIntentHandler> _handlers;
        private readonly IUserStorageService _userStorageService;

        public AppEntry(IEnumerable<IIntentHandler> handlers, IUserStorageService userStorageService)
        {
            _handlers = handlers;
            _userStorageService = userStorageService;
        }

        public async Task<SkillResponse> Run(SkillRequest skillRequest, ILambdaContext context)
        {
            var userId = skillRequest.Context.System.User.UserId;
            if (skillRequest.GetRequestType() == typeof(LaunchRequest))
            {
                const string msg = "Hey. What can I do for you?";
                return ResponseBuilder.Ask(msg, new Reprompt("Sorry. I don't know what you mean."));
            }

            if (skillRequest.GetRequestType() == typeof(SessionEndedRequest))
            {
                const string msg = "See you later";
                return ResponseBuilder.Tell(msg);
            }

            if (skillRequest.GetRequestType() == typeof(SkillEventRequest))
            {
                // todo: check enable / disable
                var eventRequest = (SkillEventRequest) skillRequest.Request;
                if (eventRequest.Type == "AlexaSkillEvent.SkillDisabled")
                {

                }
            }

            var actualIntentRequest = (IntentRequest) skillRequest.Request;
            var handler = _handlers.FirstOrDefault(x => x.Name == actualIntentRequest.Intent.Name);
            if (handler == null)
                return TellWithoutEnding("Sorry. I don't know what you mean.");

            var user = await _userStorageService.GetAsync(userId);
            if (user == null)
            {
                user = new SpotifyUser(userId);
                await _userStorageService.SaveAsync(user);
            }

            try
            {
                await handler.VerifySession(skillRequest);
                await handler.BuildSpotifySession(skillRequest);
                return await handler.HandleIntent(skillRequest, actualIntentRequest, context);
            }
            catch (Exception ex)
            {
                if (ex is AccessTokenMissingException)
                    return ResponseBuilder.Tell("AccessToken is missing. Please link your account");

                return ResponseBuilder.Tell(ex.Message);
            }
        }
    }
}