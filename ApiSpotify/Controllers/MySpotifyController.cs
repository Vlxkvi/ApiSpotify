using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using TGbot.Constant;

namespace ApiSpotify.Controllers
{
    
    [ApiController]
    [Route("[controller]/[action]")]
    
    public class SpotifyController : ControllerBase
    {
        [HttpGet(Name = "PlaylistTracksID")]
        public async Task<Dictionary<int, string>> GetPlaylistTracksID(string PlaylistID)
        {
            var s_id = new Dictionary<int, string>();

            var spotify = new SpotifyClient(constants.token);
            var playlist = await spotify.Playlists.Get(PlaylistID);

            int i = 0;
            foreach (PlaylistTrack<IPlayableItem> item in playlist.Tracks.Items)
            {
                if (item.Track is FullTrack track)
                {
                    i++;
                    s_id[i] = track.Id;
                }
                if (item.Track is FullEpisode episode)
                {
                    i++;
                    s_id[i] = episode.Id;
                }
            }
            return s_id;
        }



        [HttpGet(Name = "PlaylistTracksNames")]
        public async Task<Dictionary<int, string>> GetPlaylistTracksNames(string PlaylistID)
        {
            var s_names = new Dictionary<int, string>();

            var spotify = new SpotifyClient(constants.token);
            var playlist = await spotify.Playlists.Get(PlaylistID);

            int i = 0;
            foreach (PlaylistTrack<IPlayableItem> item in playlist.Tracks.Items)
            {
                if (item.Track is FullTrack track)
                {
                    i++;
                    s_names[i] = track.Name;
                }
                if (item.Track is FullEpisode episode)
                {
                    i++;
                    s_names[i] = episode.Name;
                }
            }
            return s_names;
        }

        [HttpPost(Name = "CreatePlaylist")]
        public async Task<string> CreatePlaylist(string userID, string Name)
        {
            var spotify = new SpotifyClient(constants.token);
            var request = new PlaylistCreateRequest(Name);
            var playlist = await spotify.Playlists.Create(userID, request);

            return playlist.Id;
        }

        [HttpPut(Name = "AddToPlaylist")]
        public async Task<string> AddtoPlaylist(string PlaylistID, string TracksID)
        {
            string[] ids = TracksID.Split("$$");
            IList<string> names = new List<string>();
            foreach (var a in ids)
            {
                names.Add($"spotify:track:{a}");
            }

            var spotify = new SpotifyClient(constants.token);
            var request = new PlaylistAddItemsRequest(names);
            await spotify.Playlists.AddItems(PlaylistID, request);
            return PlaylistID;
        }
    }
}
