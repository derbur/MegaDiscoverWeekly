using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MegaDiscoverWeekly.Interfaces;
using SpotifyAPI.Web;

namespace MegaDiscoverWeekly.Services
{
    class PlaylistService : IPlaylistService
    {
        private readonly string _token;
        private readonly SpotifyClient spotifyClient;

        public PlaylistService(IConfiguration config)
        {
            _token = config.GetValue<string>("Spotify:Token");
            spotifyClient = new SpotifyClient(_token);
        }

        public async Task<SimplePlaylist> GetUsersDiscoverWeekly(string userId)
        {
            var playlistPage = await spotifyClient.Playlists.GetUsers(userId);
            SimplePlaylist discoverWeekly = new SimplePlaylist();
            
            await foreach(var playlist in spotifyClient.Paginate(playlistPage))
            {
                if(playlist.Name.ToLower() == "discover weekly" && playlist.Owner.Id.ToLower() == "spotify")
                {
                    discoverWeekly = playlist;
                    break;
                }
            }
            
            return discoverWeekly;
        }

        private async Task<FullPlaylist> Create(string userId, string name)
        {
            var createRequest = new PlaylistCreateRequest(name);

            return await spotifyClient.Playlists.Create(userId, createRequest);
        }

        private void BuildMegaDiscoverWeekly(string userId)
        {
            // build the mega discover weekly tracklist
        }
    }
}