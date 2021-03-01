using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using MyMusic.Domain;
using MyMusic.Infrastructure;
using static MyMusic.Extensions.AlexaUtils;

namespace MyMusic.Handlers
{
    public class GetDevicesIntentHandler : BaseIntentHandler
    {
        private readonly IUserStorageService _userStorageService;
        public override string Name => "GetDevicesIntent";

        public GetDevicesIntentHandler(IUserStorageService userStorageService)
        {
            _userStorageService = userStorageService;
        }

        public override async Task<SkillResponse> HandleIntent(SkillRequest skillRequest, IntentRequest intentRequest,
            ILambdaContext context)
        {
            var user = await _userStorageService.GetAsync(skillRequest.Context.System.User.UserId);
            var devices = await SpotifyClient.GetDevicesAsync();
            if (devices.HasError())
                return TellWithoutEnding("There was an error getting your devices");

            var response = new StringBuilder();
            response.Append("You have the following devices available: \n");
            var index = 0;
            if (user.AvailableDevices.Any())
                index = user.AvailableDevices.Max(x => x.Index);

            foreach (var availableDevice in devices.Devices)
            {
                var device = user.AvailableDevices.FirstOrDefault(x => x.Id == availableDevice.Id);
                if (device != null)
                {
                    response.Append("\n");
                    response.Append($"Number: {device.Index} - {device.Name}");
                    response.Append("\n");
                    continue;
                }

                index++;
                response.Append("\n");
                response.Append($"Number: {index} - {availableDevice.Name}");
                response.Append("\n");

                var spotifyDevice =
                    new SpotifyDevice(availableDevice.Id, availableDevice.Name.ToLower(), index);
                user.AvailableDevices.Add(spotifyDevice);

            }

            await _userStorageService.SaveAsync(user);
            return TellWithoutEnding(response.ToString());
        }
    }
}