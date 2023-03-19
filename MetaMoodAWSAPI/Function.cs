using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.Entities;
using MetaMoodAWSAPI.QueryParameterModels;
using MetaMoodAWSAPI.Services;
using MetaMoodAWSAPI.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MetaMoodAWSAPI;

public class Function
{
    private readonly IServiceCollection _serviceCollection;
    private readonly MetaMoodContext _DBContext;

    /// <summary>
    /// This is the constructor for the Lambda function that sets up the collection of services, calls RegisterServices(),
    /// and provides a database context for this class. 
    /// </summary>
    public Function()
    {
        _serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = _serviceCollection.RegisterServices().BuildServiceProvider();
        _DBContext = serviceProvider.GetRequiredService<MetaMoodContext>();
    }



    /// <summary>
    /// This function makes an asynchronous request to the database to retrieve and return a page of tracks.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>A selected page of tracks from the spotify tracks table</returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> GetTrackPageAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        IDictionary<string, string> searchCriteria = new Dictionary<string, string>();
        string? lowerReleaseDate = null, upperReleaseDate = null;

        if(!(request.QueryStringParameters.TryGetValue("pageSize", out string? strPageSize)
            && request.QueryStringParameters.TryGetValue("pageNumber", out string? strPageNumber)))
        {
            return Response.BadRequest();
        }

        if (!request.QueryStringParameters.TryGetValue("sortby", out string? sortBy))
        {
            sortBy = "name";
        }

        if(request.QueryStringParameters.TryGetValue("name", out string? name))
        {
            searchCriteria["name"] = name;
        }
        
        if(request.QueryStringParameters.TryGetValue("lowerReleaseDate", out lowerReleaseDate)
            || request.QueryStringParameters.TryGetValue("upperReleaseDate", out upperReleaseDate))
        {
            lowerReleaseDate ??= "0000-00-00";
            upperReleaseDate ??= "9999-99-99";

            searchCriteria["lowerReleaseDate"] = lowerReleaseDate;
            searchCriteria["upperReleaseDate"] = upperReleaseDate;

        }
        

        if (!int.TryParse(strPageSize, out int iPageSize)
            || !int.TryParse(strPageNumber, out int iPageNumber)
            || !SpotifyValidation.ValidateSpotifySortBy(ref sortBy))
        {
            return Response.BadRequest();
        }

        IList<SpotifyTrackDTO> tracks = await _DBContext.SpotifyTracks.Select(
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
        ).SpotifyTrackSortBy<SpotifyTrackDTO>(sortBy).GetPage(iPageSize, iPageNumber).ToListAsync();

        if (tracks.Count < 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(tracks);
        }

    }
    /// <summary>
    /// This function makes an asynchronous request to the database to retrieve and a track, based on search criteria, from
    /// the database. 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>An individual track based on search criteria</returns>
    /// <exception cref="Exception">Thrown when track retrieval fails</exception>
    public async Task<SpotifyTrackDTO> GetSpotifyTrackByNameAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {

        string trackName = request.QueryStringParameters["name"];
        string releaseDate = request.QueryStringParameters["releasedate"];
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
