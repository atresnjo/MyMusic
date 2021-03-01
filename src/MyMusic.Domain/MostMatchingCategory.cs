using SpotifyAPI.Web.Models;

namespace MyMusic.Domain
{
    public class MostMatchingCategory
    {
        public MostMatchingCategory(Category category, double score)
        {
            Category = category;
            Score = score;
        }

        public Category Category { get; set; }
        public double Score { get; set; }
    }
}