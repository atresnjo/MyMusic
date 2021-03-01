using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class NextSongIntentHandler : BaseIntentHandler
    {
        public override string Name => "NextSongIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            await SpotifyClient.SkipPlaybackToNextAsync();
            return ReturnEmptySkillResponse();
        }
    }
}