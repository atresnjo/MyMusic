using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace MyMusic.Infrastructure
{
    public interface IIntentHandler
    {
        Task VerifySession(SkillRequest request);
        Task BuildSpotifySession(SkillRequest request);

        Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context);

        string Name { get; }

    }
}