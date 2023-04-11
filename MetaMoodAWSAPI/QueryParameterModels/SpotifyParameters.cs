using System.ComponentModel.DataAnnotations;

namespace MetaMoodAWSAPI.QueryParameterModels
{
    internal class SpotifyParameters
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string SortBy { get; set; } = "Name";

        public string? Name { get; set; } = null;

        public string? LowerReleaseDate { get; set; } = null;

        public string? UpperReleaseDate { get; set; } = null;

        public int? LowerPopularity { get; set; } = null;

        public int? UpperPopularity { get; set; } = null;

        public double? LowerAcousticness { get; set; } = null;

        public double? UpperAcousticness { get; set; } = null;

        public double? LowerDanceability { get; set; } = null;

        public double? UpperDanceability { get; set; } = null;

        public double? LowerEnergy { get; set; } = null;

        public double? UpperEnergy { get; set; } = null;

        public double? LowerLiveness { get; set; } = null;

        public double? UpperLiveness { get; set; } = null;

        public double? LowerLoudness { get; set; } = null;

        public double? UpperLoudness { get; set; } = null;

        public double? LowerSpeechiness { get; set; } = null;

        public double? UpperSpeechiness { get; set; } = null;

        public double? LowerTempo { get; set; } = null;

        public double? UpperTempo { get; set; } = null;

        public double? LowerInstrumentalness { get; set; } = null;

        public double? UpperInstrumentalness { get; set; } = null;

        public double? LowerValence { get; set; } = null;

        public double? UpperValence { get; set; } = null;   

    }
}
