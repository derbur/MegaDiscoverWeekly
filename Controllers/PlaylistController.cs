using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MegaDiscoverWeekly.Interfaces;
using MegaDiscoverWeekly.Models;

namespace MegaDiscoverWeekly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscoverWeeklyController : ControllerBase
    {
        private readonly IPlaylistService playlistService;
        private const string megaDiscoverWeeklyPlaylistName = "Mega Discover Weekly";

        public DiscoverWeeklyController(IConfiguration configuration, IPlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        [HttpPost("/create")]
        public async Task<bool> CreateMegaDiscoverWeekly(CreateMegaDiscoverWeeklyRequest request)
        {
            // create discover weekly playlist
            var playlistIds = await this.GetAllDiscoveryWeeklyPlaylists(request.DiscoverWeeklyUserIds);
            var trackUris = await this.GetPlaylistTrackUris(playlistIds);
            var megaDiscoverWeekly = await playlistService.CreatePlaylist(request.UserId, megaDiscoverWeeklyPlaylistName);
            return await playlistService.UpdatePlaylist(request.UserId, megaDiscoverWeeklyPlaylistName, trackUris);
        }

        private async Task<List<string>> GetAllDiscoveryWeeklyPlaylists(IEnumerable<string> userIds)
        {
            List<string> playlistIds = new List<string>();

            // Get Discover Weekly playlists for provided users
            foreach(var id in userIds)
            {
                var playlist = await playlistService.GetDiscoverWeeklyPlaylist(id);
                playlistIds.Add(playlist.Id);
            }

            return playlistIds;
        }

        private async Task<List<string>> GetPlaylistTrackUris(IEnumerable<string> playlistIds)
        {
            List<string> trackUris = new List<string>();

            trackUris = await playlistService.GetPlaylistTrackUris(playlistIds);

            return trackUris;
        }
    }
}