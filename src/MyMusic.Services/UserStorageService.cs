using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.Extensions.Options;
using MyMusic.Domain;
using MyMusic.Domain.Configuration;
using MyMusic.Infrastructure;

namespace MyMusic.Services
{
    public class UserStorageService : IUserStorageService
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public UserStorageService(IOptions<AwsDbConfig> awsConfig)
        {
            var dynamoDbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(awsConfig.Value.AccessKey, awsConfig.Value.SecretKey),
                new AmazonDynamoDBConfig
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(awsConfig.Value.Endpoint)
                });
            _dynamoDbContext = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<SpotifyUser> GetAsync(string userId)
        {
            return await _dynamoDbContext.LoadAsync<SpotifyUser>(userId);
        }

        public async Task SaveAsync(SpotifyUser user)
        {
            await _dynamoDbContext.SaveAsync(user);
        }
    }
}