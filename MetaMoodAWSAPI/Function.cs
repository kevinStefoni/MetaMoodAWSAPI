using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
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
    private MetaMoodContext _DBContext;

    public Function()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.RegisterServices();
        _DBContext = _serviceCollection.BuildServiceProvider().GetRequiredService<MetaMoodContext>();
    }



    public async Task<List<SpotifyTrack>> GetAllTracksAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {

        List<SpotifyTrack> tracks = await _DBContext.SpotifyTracks.Select(
        t => new SpotifyTrack
        {
            TrackId = t.TrackId,
            Name = t.Name
        }
        ).OrderBy(t => t.TrackId).ToListAsync();

        if (tracks.Count < 0)
        {
            throw new Exception("Unable to retrieve spotify tracks.");
        }
        else
        {
            return tracks;
        }
        
    }

    public async Task<SpotifyTrack> GetSpotifyTrackByNameAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {

        string trackName = request.QueryStringParameters["name"];
        SpotifyTrack? track = await _DBContext.SpotifyTracks.Select
        (
            t => new SpotifyTrack
            {
                TrackId= t.TrackId,
                Name = t.Name
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
