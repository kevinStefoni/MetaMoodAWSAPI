using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using MetaMoodAWSAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MetaMoodAWSAPI;

public class Function
{
    private MetaMoodContext _DBContext = new MetaMoodContext();
    public async Task<List<SpotifyTrack>> GetAllTracksAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        List<SpotifyTrack> List = await _DBContext.SpotifyTracks.Select(
        t => new SpotifyTrack
        {
           TrackId = t.TrackId,
           Name = t.Name
        }
        ).OrderBy(t => t.TrackId).ToListAsync();

        if (List.Count < 0)
        {
            return null;
        }
        else
        {
            return List;
        }
    }
}
