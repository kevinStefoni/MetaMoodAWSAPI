namespace MetaMoodAWSAPI.Validation
{
    internal class SpotifyValidation
    {

        public static bool ValidateSpotifySortBy(ref string sortby)
        {
            if (sortby.Equals(string.Empty))
                sortby = "name";

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
