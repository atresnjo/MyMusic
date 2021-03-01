using System.Threading.Tasks;
using MyMusic.Domain;

namespace MyMusic.Infrastructure
{
    public interface IUserStorageService
    {
        Task<SpotifyUser> GetAsync(string userId);
        Task SaveAsync(SpotifyUser user);
    }
}