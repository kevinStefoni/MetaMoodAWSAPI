using MetaMoodAWSAPI.QueryParameterModels;

namespace MetaMoodAWSAPI.Validation
{
    internal class SpotifyValidation
    {

        public static bool ValidateSpotifySortBy(string? sortby)
        {

            return sortby == "name"
                || sortby == "releasedate"
                || sortby == "popularity"
                || sortby == "acousticness"
                || sortby == "danceability"
                || sortby == "energy"
                || sortby == "liveness"
                || sortby == "loudness"
                || sortby == "speechiness"
                || sortby == "tempo"
                || sortby == "instrumentalness"
                || sortby == "valence";

        }
    }
}
