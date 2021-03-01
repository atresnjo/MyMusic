using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MoreLinq;
using MyMusic.Domain;
using MyMusic.Extensions;
using SimMetrics.Net.Metric;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class GetPlaylistIntentHandler : BaseIntentHandler
    {
        public override string Name => "GetPlaylistIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var profile = await SpotifyClient.GetPrivateProfileAsync();
            if (profile.HasError())
                return TellWithoutEnding("There was an error getting your profile");

            var playlists = await SpotifyClient.GetUserPlaylistsAsync(profile.Id);
            if (playlists.HasError())
                return TellWithoutEnding("There was an error getting your playlists");

            var playlistName = intentRequest.GetSlotValue("PlaylistName");
            var list = (from playlist in playlists.Items
                let score = new SmithWatermanGotoh().GetSimilarity(playlist.Name.ToLower(), playlistName.ToLower())
                select new MostMatchingPlaylist(playlist, score)).ToList();

            var mostMatching = list.MaxBy(x => x.Score).FirstOrDefault();
            if (mostMatching == null)
                return TellWithoutEnding("Sorry. Couldn't find the requested playlist");

            var speech = $"Found {mostMatching.Playlist.Name}, is this the correct one?";
            skillRequest.Session.SetSessionValue("PlaylistUri", mostMatching.Playlist.Uri);
            return ResponseBuilder.Ask(speech, new Reprompt(speech), skillRequest.Session);
        }
    }
}