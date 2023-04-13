namespace MetaMoodAWSAPI.QueryParameterModels
{
    internal class SpotifyParameters
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string SortBy { get; set; } = "Name";

        public string? Name { get; set; } = null;

    }
}
