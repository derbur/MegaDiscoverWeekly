using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace MegaDiscoverWeekly.Interfaces
{
    public interface IPlaylistService
    {
        Task<SimplePlaylist> GetUsersDiscoverWeekly(string userId);
        Task<bool> CreateMegaDiscoverWeekly(string userId, string[] userIds);
    }
    
}