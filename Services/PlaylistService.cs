using System.Collections.Generic;
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

        private async Task<List<PlaylistTrack<IPlayableItem>>> GetPlaylistTracks(string playlistId)
        {
            // Use the paging before returning?
            var playlist = await spotifyClient.Playlists.Get(playlistId);
            List<PlaylistTrack<IPlayableItem>> tracks = new List<PlaylistTrack<IPlayableItem>>();
            tracks = playlist.Tracks.Items;

            return tracks;
        }

        private async Task<FullPlaylist> CreatePlaylist(string userId, string name)
        {
            var createRequest = new PlaylistCreateRequest(name);

            return await spotifyClient.Playlists.Create(userId, createRequest);
        }

        // Need to break this big boi up
        public async Task<bool> CreateMegaDiscoverWeekly(string userId, string[] userIds)
        {
            List<string> playlistIds = new List<string>();
            List<string> trackUris = new List<string>();

            // Build the mega discover weekly playlist
            // 1. Get Discover Weekly playlists for provided users
            foreach(var id in userIds)
            {
                var playlist = await this.GetUsersDiscoverWeekly(id);
                playlistIds.Add(playlist.Id);
            }

            // 2. Get uris for all tracks to create Mega Playlist
            foreach(var id in playlistIds)
            {
                var items = await this.GetPlaylistTracks(id);
                foreach(var item in items)
                {
                    if(item.Track is FullTrack track)
                    {
                        trackUris.Add(track.Uri);
                    }
                }
            }

            // 3. Create/Update Mega Discover Weekly playlist
            // Update this logic to check if this playlist exists first
            var mdPlaylist = await this.CreatePlaylist(userId, "Mega Discover Weekly");

            PlaylistReplaceItemsRequest replaceItemsRequest = new PlaylistReplaceItemsRequest(trackUris);

            var replaceResponse = await spotifyClient.Playlists.ReplaceItems(mdPlaylist.Id, replaceItemsRequest);

            return replaceResponse;
        }
    }
}