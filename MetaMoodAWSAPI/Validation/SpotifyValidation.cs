namespace MetaMoodAWSAPI.Validation
{
    internal class SpotifyValidation
    {
        private static string GetInvalidSortByMessage() => "Invalid sorting criteria provided.";
        public static void ValidateSpotifySortBy(ref string sortby)
        {
            sortby ??= "name";

            if (sortby != "name"
                && sortby != "releasedate"
                && sortby != "popularity"
                && sortby != "acousticness"
                && sortby != "danceability"
                && sortby != "energy"
                && sortby != "liveness"
                && sortby != "loudness"
                && sortby != "speechiness"
                && sortby != "tempo"
                && sortby != "instrumentalness"
                && sortby != "valence")
            {
                throw new Exception(GetInvalidSortByMessage());
            }

        }
    }
}
