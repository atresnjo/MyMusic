using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class LikeCurrentSongIntentHandler : BaseIntentHandler
    {
        public override string Name => "LikeCurrentSongIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var actualTrack = await SpotifyClient.GetPlayingTrackAsync();
            if (actualTrack.HasError())
                return TellWithoutEnding("There was an error with getting the current playing track");

            await SpotifyClient.SaveTrackAsync(actualTrack.Item.Id);
            return TellWithoutEnding($"I have just saved the song: {actualTrack.Item.Name}");
        }
    }
}