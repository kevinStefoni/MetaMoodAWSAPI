namespace MetaMoodAWSAPI.DTOs
{
    public class SpotifyTrackDTO
    {

        public string? Name { get; set; }

        public string? ReleaseDate { get; set; }

        public int? Popularity { get; set; }

        public double? Acousticness { get; set; }

        public double? Danceability { get; set; }

        public double? Energy { get; set; }

        public double? Liveness { get; set; }

        public double? Loudness { get; set; }

        public double? Speechiness { get; set; }

        public double? Tempo { get; set; }

        public double? Instrumentalness { get; set; }

        public double? Valence { get; set; }

        public int Emotion { get; set; }

        public string? CoverImageUrl { get; set; }

    }
}
