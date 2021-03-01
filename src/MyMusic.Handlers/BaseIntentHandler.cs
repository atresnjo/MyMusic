using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MyMusic.Domain.Exceptions;
using MyMusic.Infrastructure;
using SpotifyAPI.Web;

namespace MyMusic.Handlers
{
    public abstract class BaseIntentHandler : IIntentHandler
    {
        public SpotifyWebAPI SpotifyClient { get; set; }
        public abstract string Name { get; }

        public Task VerifySession(SkillRequest request)
        {
            if (string.IsNullOrEmpty(request.Context.System.User.AccessToken))
                throw new AccessTokenMissingException();

            return Task.CompletedTask;
        }

        public Task BuildSpotifySession(SkillRequest request)
        {
            SpotifyClient = new SpotifyWebAPI
            {
                TokenType = "Bearer",
                AccessToken = request.Context.System.User.AccessToken
            };
            return Task.CompletedTask;
        }

        public abstract Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context);
    }
}