using SpotifyAPI.Web.Models;

namespace MyMusic.Domain
{
    public class MostMatchingPlaylist
    {
        public MostMatchingPlaylist(SimplePlaylist playlist, double score)
        {
            Playlist = playlist;
            Score = score;
        }
        public SimplePlaylist Playlist { get; set; }
        public double Score { get; set; }
    }
}