using MetaMoodAWSAPI.QueryParameterModels;

namespace MetaMoodAWSAPI.Validation
{
    internal class SpotifyValidation
    {

        public static bool ValidateSpotifySortBy(string? sortby)
        {

            return sortby == "Name"
                || sortby == "ReleaseDate"
                || sortby == "Popularity"
                || sortby == "Acousticness"
                || sortby == "Danceability"
                || sortby == "Energy"
                || sortby == "Liveness"
                || sortby == "Loudness"
                || sortby == "Speechiness"
                || sortby == "Tempo"
                || sortby == "Instrumentalness"
                || sortby == "Valence";

        }
    }
}
