using AutoMapper;
using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.Entities;

namespace MetaMoodAWSAPI.Services
{
    internal class AllMappersProfile : Profile
    {

        /// <summary>
        /// This method maps DTOs to their entity model counterparts, ignoring attributes that will not be directly displayed
        /// to users.
        /// </summary>
        public AllMappersProfile() {

            CreateMap<SpotifyTrack, SpotifyTrackDTO>();
            CreateMap<SpotifyTrackDTO, SpotifyTrack>().ForMember(t => t.TrackId, opt => opt.Ignore())
                                                      .ForMember(t => t.AlbumId, opt => opt.Ignore())
                                                      .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                                                      .ForMember(t => t.TrackHref, opt => opt.Ignore())
                                                      .ForMember(t => t.CoverImageUrl, opt => opt.Ignore())
                                                      .ForMember(t => t.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                                                      .ForMember(t => t.PreviewUrl, opt => opt.Ignore())
                                                      .ForMember(t => t.Popularity, opt => opt.MapFrom(src => src.Popularity))
                                                      .ForMember(t => t.Acousticness, opt => opt.MapFrom(src => src.Acousticness))
                                                      .ForMember(t => t.Danceability, opt => opt.MapFrom(src => src.Danceability))
                                                      .ForMember(t => t.Energy, opt => opt.MapFrom(src => src.Energy))
                                                      .ForMember(t => t.Liveness, opt => opt.MapFrom(src => src.Liveness))
                                                      .ForMember(t => t.Loudness, opt => opt.MapFrom(src => src.Loudness))
                                                      .ForMember(t => t.Speechiness, opt => opt.MapFrom(src => src.Speechiness))
                                                      .ForMember(t => t.Tempo, opt => opt.MapFrom(src => src.Tempo))
                                                      .ForMember(t => t.Instrumentalness, opt => opt.MapFrom(src => src.Instrumentalness))
                                                      .ForMember(t => t.Valence, opt => opt.MapFrom(src => src.Valence));


        }
    }
}
