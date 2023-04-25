using MetaMoodAWSAPI.QueryParameterModels;

namespace MetaMoodAWSAPI.Validation
{
    internal class DataValidation
    {

        public static bool ValidateSortBy(string? sortby)
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
                || sortby == "Valence"
                || sortby == "Author"
                || sortby == "Body"
                || sortby == "User"
                || sortby == "Tweet"
                || sortby == "Emotion";

        }
    }
}
