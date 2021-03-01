using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MyMusic.Extensions;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class SetVolumeIntentHandler : BaseIntentHandler
    {
        public override string Name => "SetVolumeIntent";

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {

            var volumeLevel = int.Parse(intentRequest.GetSlotValue("VolumeLevelSlot"));
            await SpotifyClient.SetVolumeAsync(volumeLevel);
            return ReturnEmptySkillResponse();
        }
    }
}