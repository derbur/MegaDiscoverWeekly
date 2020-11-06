using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MegaDiscoverWeekly.Interfaces;
using SpotifyAPI.Web;

namespace MegaDiscoverWeekly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService playlistService;

        public PlaylistController(IConfiguration configuration, IPlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        [HttpGet("/discoverweekly/{userId}")]
        public async Task<bool> GetUsersDiscoverWeekly(string userId, string[] userIds)
        {
            return await playlistService.CreateMegaDiscoverWeekly(userId, userIds);
        }
        // [HttpPost]
        // public async Task<FullPlaylist> Create()
        // {
        //     PlaylistCreateRequest playlistCreateRequest = new PlaylistCreateRequest("Mega Discover Weekly");

        //     var playlist = await spotifyClient.Playlists.Create("", playlistCreateRequest);
        //     return playlist;
        // }
    }
}