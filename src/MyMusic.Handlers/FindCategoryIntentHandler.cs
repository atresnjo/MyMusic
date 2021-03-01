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
    public class FindCategoryIntentHandler : BaseIntentHandler
    {
        public override string Name => "FindCategoryIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var categoryName = intentRequest.GetSlotValue("CategoryName");
            var categories = await SpotifyClient.GetCategoriesAsync(limit: 50);
            var list = (from playlist in categories.Categories.Items
                let score = new SmithWatermanGotoh().GetSimilarity(playlist.Name.ToLower(), categoryName.ToLower())
                select new MostMatchingCategory(playlist, score)).ToList();

            var mostMatching = list.MaxBy(x => x.Score).FirstOrDefault();
            if (mostMatching == null)
                return TellWithoutEnding("Sorry. Couldn't find the requested playlist");

            var speech = $"Found {mostMatching.Category.Name}, is this the correct one?";
            skillRequest.Session.SetSessionValue("CategoryId", mostMatching.Category.Id);
            return ResponseBuilder.Ask(speech, new Reprompt(speech), skillRequest.Session);
        }
    }
}