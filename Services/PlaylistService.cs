using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MegaDiscoverWeekly.Interfaces;
using SpotifyAPI.Web;
using System.Linq;

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

        # region Playlist Operations

        public async Task<FullPlaylist> CreatePlaylist(string userId, string name)
        {
            var createRequest = new PlaylistCreateRequest(name);

            return await spotifyClient.Playlists.Create(userId, createRequest);
        }
        
        public async Task<bool> UpdatePlaylist(string userId, string playlistName, IEnumerable<string> playlistItems)
        {
            
            var playlistPage = await spotifyClient.Playlists.GetUsers(userId);
            SimplePlaylist playlistToUpdate = new SimplePlaylist();
            
            await foreach(var playlist in spotifyClient.Paginate(playlistPage))
            {
                if(playlist.Name.ToLower() == playlistName.ToLower())
                {
                    playlistToUpdate = playlist;
                    break;
                }
            }

            return await UpdatePlaylistItems(playlistToUpdate.Id, playlistItems);
        }

        public async Task<SimplePlaylist> GetDiscoverWeeklyPlaylist(string userId)
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
            
        #endregion
        

        #region Playlist Tracks Operations

        private async Task<List<PlaylistTrack<IPlayableItem>>> GetPlaylistTracks(string playlistId)
        {
            var playlist = await spotifyClient.Playlists.Get(playlistId);
            List<PlaylistTrack<IPlayableItem>> tracks = new List<PlaylistTrack<IPlayableItem>>();
            // Use the paging before returning?
            tracks = playlist.Tracks.Items;

            return tracks;
        }

        public async Task<List<string>> GetPlaylistTrackUris(IEnumerable<string> playlistIds)
        {
            List<string> trackUris = new List<string>();
            
            // Get uris for all tracks to create Mega Playlist
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

            return trackUris;
        }

        private async Task<bool> UpdatePlaylistItems(string playlistId, IEnumerable<string> playlistItems)
        {
            PlaylistReplaceItemsRequest replaceItemsRequest = new PlaylistReplaceItemsRequest(playlistItems.ToList());

            var replaceResponse = await spotifyClient.Playlists.ReplaceItems(playlistId, replaceItemsRequest);

            return replaceResponse;
        }
            
        #endregion

    }
}