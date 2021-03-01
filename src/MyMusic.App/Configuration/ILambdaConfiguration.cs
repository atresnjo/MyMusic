using Microsoft.Extensions.Configuration;

namespace MyMusic.App.Configuration
{
    public interface ILambdaConfiguration
    {
        IConfigurationRoot Configuration { get; }
    }
}