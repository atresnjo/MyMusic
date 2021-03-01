using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MoreLinq.Extensions;
using MyMusic.Extensions;
using SpotifyAPI.Web.Enums;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class SurpriseMeByGenreIntentHandler : BaseIntentHandler
    {
        public override string Name => "SurpriseMeByGenreIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var genreName = intentRequest.GetSlotValue("GenreNameSlot");
            var items = await SpotifyClient.SearchItemsAsync($"genre:{genreName}", SearchType.Track);
            if (items.HasError())
                return TellWithoutEnding("There was an error with searching for the genre");

            if (!items.Tracks.Items.Any())
                return TellWithoutEnding($"Sorry. Couldn't find any songs for the {genreName}, genre");

            var firstTrack = items.Tracks.Items.Shuffle().FirstOrDefault();
            if (firstTrack == null || firstTrack.HasError())
                return TellWithoutEnding("There was an error with shuffling the tracks");

            await SpotifyClient.ResumePlaybackAsync("", "", new List<string> {firstTrack.Uri}, "");
            return TellWithoutEnding(
                $"Surprising you with the following song: {firstTrack.Name}");

        }
    }
}