using System.ComponentModel.DataAnnotations;

namespace MetaMoodAWSAPI.QueryParameterModels
{
    internal class SpotifyParameters
    {
        [Required]
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string? SortBy { get; set; }

        public string? Name { get; set; }

        public string? LowerReleaseDate { get; set; }

        public string? UpperReleaseDate { get; set; }

        public int? LowerPopularity { get; set; }

        public int? UpperPopularity { get; set; }

        public double? LowerAcousticness { get; set; }

        public double? UpperAcousticness { get; set; }

        public double? LowerDanceability { get; set; }

        public double? UpperDanceability { get; set; }

        public double? LowerEnergy { get; set; }

        public double? UpperEnergy { get; set; }

        public double? LowerLiveness { get; set; }

        public double? UpperLiveness { get; set; }

        public double? LowerSpeechiness { get; set; }

        public double? UpperSpeechiness { get; set; }

        public double? LowerTempo { get; set; }

        public double? UpperTempo { get; set; }

        public double? LowerInstrumentalness { get; set; }

        public double? UpperInstrumentalness { get; set; }

        public double? LowerValence { get; set; }

        public double? UpperValence { get; set; }

    }
}
