using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;

namespace MegaDiscoverWeekly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly string TOKEN;

        public PlaylistController(IConfiguration configuration)
        {
            TOKEN = configuration.GetValue<string>("Spotify:Token");
        }
        [HttpGet("{playlistId}")]
        public async Task<FullPlaylist> Get(string playlistId)
        {
            var spotify = new SpotifyClient(TOKEN);

            var playlist = await spotify.Playlists.Get(playlistId);

            return playlist;
        }
    }
}