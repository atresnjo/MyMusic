using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MoreLinq;
using MyMusic.Extensions;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class PlayCategoryIntentHandler : BaseIntentHandler
    {
        public override string Name => "PlayCategoryIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {

            var session = skillRequest.Session;
            var categoryId = session.GetSessionValue<string>("CategoryId");
            if (string.IsNullOrEmpty(categoryId))
                return TellWithoutEnding("Please specify a category first");

            var playlists = await SpotifyClient.GetCategoryPlaylistsAsync(categoryId);
            var shuffled = playlists.Playlists.Items.Shuffle();
            var shuffledPlaylist = shuffled.FirstOrDefault();
            if (shuffledPlaylist == null)
                return TellWithoutEnding("There was an error with shuffling the playlist");

            await SpotifyClient.ResumePlaybackAsync("", shuffledPlaylist.Uri, null, "");
            return TellWithoutEnding($"Playing playlist named: {shuffledPlaylist.Name}");
        }
    }
}