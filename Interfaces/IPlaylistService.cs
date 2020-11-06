using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace MegaDiscoverWeekly.Interfaces
{
    public interface IPlaylistService
    {
        Task<FullPlaylist> CreatePlaylist(string userId, string playlistName);
        Task<bool> UpdatePlaylist(string userId, string playlistName, IEnumerable<string> playlistItems);
        Task<SimplePlaylist> GetDiscoverWeeklyPlaylist(string userId);
        Task<List<string>> GetPlaylistTrackUris(IEnumerable<string> playlistIds);
    }
    
}