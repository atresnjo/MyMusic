using System.Text;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class GetCurrentTrackIntentHandler : BaseIntentHandler
    {
        public override string Name => "GetCurrentTrackIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var track = await SpotifyClient.GetPlayingTrackAsync();
            if (track.HasError())
                return TellWithoutEnding("There was an error getting the current playing track");

            var response = new StringBuilder();
            response.Append($"Name of the track is {track.Item.Name},");
            response.Append("by:");

            foreach (var simpleArtist in track.Item.Artists)
            {
                response.Append(simpleArtist.Name);
                response.Append(",");
            }

            return TellWithoutEnding(response.ToString());
        }
    }
}