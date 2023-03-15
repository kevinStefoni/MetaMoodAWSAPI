using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using AutoMapper;
using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.Entities;
using MetaMoodAWSAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MetaMoodAWSAPI;

public class Function
{
    private readonly IServiceCollection _serviceCollection;
    private readonly MetaMoodContext _DBContext;
    //private readonly IMapper _Mapper;

    public Function()
    {
        _serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = _serviceCollection.RegisterServices().BuildServiceProvider();
        _DBContext = serviceProvider.GetRequiredService<MetaMoodContext>();
        //_Mapper = serviceProvider.GetRequiredService<IMapper>();
    }



    public async Task<List<SpotifyTrackDTO>> GetAllTracksAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {

        List<SpotifyTrackDTO> tracks = await _DBContext.SpotifyTracks.Select(
        t => new SpotifyTrackDTO
        {
            Name = t.Name,
            ReleaseDate = t.ReleaseDate,
            Popularity = t.Popularity,
            Acousticness = t.Acousticness,
            Danceability = t.Danceability,
            Energy = t.Energy,
            Liveness = t.Liveness,
            Loudness = t.Loudness,
            Speechiness = t.Speechiness,
            Tempo = t.Tempo,
            Instrumentalness = t.Instrumentalness,
            Valence = t.Valence
        }
        ).OrderBy(t => t.Name).ToListAsync();

        if (tracks.Count < 0)
        {
            throw new Exception("Unable to retrieve spotify tracks.");
        }
        else
        {
            return tracks;
        }
        
    }

    public async Task<SpotifyTrackDTO> GetSpotifyTrackByNameAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {

        string trackName = request.QueryStringParameters["name"];
        SpotifyTrackDTO? track = await _DBContext.SpotifyTracks.Select
        (
            t => new SpotifyTrackDTO
            {
                Name = t.Name,
                ReleaseDate = t.ReleaseDate,
                Popularity = t.Popularity,
                Acousticness = t.Acousticness,
                Danceability = t.Danceability,
                Energy = t.Energy,
                Liveness = t.Liveness,
                Loudness = t.Loudness,
                Speechiness = t.Speechiness,
                Tempo = t.Tempo,
                Instrumentalness = t.Instrumentalness,
                Valence = t.Valence
            }
        ).FirstOrDefaultAsync(s => s.Name == trackName);
        if (track == null)
        {
            throw new Exception($"Unable to find track with name {trackName}.");
        }
        else
        {
            return track;
        }
    }

}
