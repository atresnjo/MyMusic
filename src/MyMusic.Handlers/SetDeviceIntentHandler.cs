using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MyMusic.Infrastructure;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class SetDeviceIntentHandler : BaseIntentHandler
    {
        private readonly IUserStorageService _userStorageService;
        public override string Name => "SetDeviceIntent";

        public SetDeviceIntentHandler(IUserStorageService userStorageService)
        {
            _userStorageService = userStorageService;
        }

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var user = await _userStorageService.GetAsync(skillRequest.Context.System.User.UserId);
            var deviceSlotIndex = int.Parse(intentRequest.Intent.Slots["DeviceSlot"].Value);
            var foundDevice = user.AvailableDevices.FirstOrDefault(x => x.Index == deviceSlotIndex);
            if (foundDevice == null)
                return TellWithoutEnding($"Could not find device with index {deviceSlotIndex}");

            await SpotifyClient.TransferPlaybackAsync(foundDevice.Id, true);
            user.CurrentPlayingDevice = foundDevice;
            await _userStorageService.SaveAsync(user);
            return TellWithoutEnding($"Playing on {foundDevice.Name} now");
        }
    }
}