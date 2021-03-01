using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MyMusic.Extensions;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{

    public class PlayPlaylistIntentHandler : BaseIntentHandler
    {
        public override string Name => "PlayPlaylistIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var session = skillRequest.Session;
            var playlistUri = session.GetSessionValue<string>("PlaylistUri");
            if (string.IsNullOrEmpty(playlistUri))
                return TellWithoutEnding("Please specify a playlist first");

            await SpotifyClient.ResumePlaybackAsync("", playlistUri, null, "");
            return ReturnEmptySkillResponse();
        }
    }
}