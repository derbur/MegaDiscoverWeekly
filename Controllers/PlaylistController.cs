using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace MegaDiscoverWeekly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        [HttpGet("{playlistId}")]
        public async Task<FullPlaylist> Get(string playlistId)
        {
            var spotify = new SpotifyClient("");

            var playlist = await spotify.Playlists.Get(playlistId);

            return playlist;
        }
    }
}