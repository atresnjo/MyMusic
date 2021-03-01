using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyMusic.App.Configuration;
using MyMusic.Domain.Configuration;
using MyMusic.Handlers;
using MyMusic.Infrastructure;
using MyMusic.Services;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MyMusic.App
{
    public class Function
    {
        private static ServiceProvider ServiceProvider { get; }

        static Function()
        {
            var services = new ServiceCollection();
            ConfigureServices(services, LambdaConfiguration.Configuration);
            ServiceProvider = services.BuildServiceProvider();
        }

        public async Task<SkillResponse> FunctionHandler(SkillRequest skillRequest, ILambdaContext context)
        {
            return await ServiceProvider.GetService<AppEntry>().Run(skillRequest, context);
        }

        private static void ConfigureServices(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.Configure<AwsDbConfig>(config => configurationRoot.GetSection("AwsDbConfig").Bind(config));
            services.AddTransient<AppEntry>();
            services.AddTransient<IUserStorageService, UserStorageService>();
            services.Scan(scan =>
                scan.FromAssemblyOf<BaseIntentHandler>()
                    .AddClasses(classes => classes.AssignableTo<BaseIntentHandler>()).AsImplementedInterfaces()
                    .WithTransientLifetime());
        }

        ~Function()
        {
            ServiceProvider.Dispose();
        }
    }
}