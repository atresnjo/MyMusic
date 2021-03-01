using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;


namespace MyMusic.Handlers
{
    public class FindSongIntentHandler : BaseIntentHandler
    {
        public override string Name => "FindSongIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            /*
            var songName = intentRequest.GetSlotValue("SongNameSlot");
            var items = await SpotifyClient.SearchItemsAsync(songName, SearchType.Track);
            if (items.HasError())
                return TellWithoutEnding("There was an error searching for the song");

            var firstItem = items.Tracks.Items.FirstOrDefault();
            return ResponseBuilder.Tell(
                $"Found {firstItem.Name} by {firstItem.Artists.FirstOrDefault().Name}, is this the correct one? ");
                */

            return ResponseBuilder.Tell("Sorry, this isn't supported yet");
        }
    }
}