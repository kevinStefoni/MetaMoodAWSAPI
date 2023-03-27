using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.Entities;
using MetaMoodAWSAPI.QueryParameterModels;
using MetaMoodAWSAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    /// Constructor that allows injection of DBContext. This method is to allow unit tests to inject their own DB context.
    /// </summary>
    /// <param name="dbContext">The database context created in the test project</param>
    public Function(MetaMoodContext dbContext)
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.RegisterServices().BuildServiceProvider();
        _DBContext = dbContext;
    }



    /// <summary>
    /// This function makes an asynchronous request to the database to retrieve and return a page of tracks that
    /// fits the given criteria.
    /// </summary>
    /// <param name="request">request contains a dictionary that has all the query parameters necessary to
    /// determine any search and sort criteria and paging parameters.</param>
    /// <param name="context"></param>
    /// <returns>A selected page of tracks from the spotify tracks table</returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> GetTrackPageAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        SpotifyParameters spotifyParameters = new ();

        try
        {
            spotifyParameters = QueryParameterService.GetSpotifyQueryParameters(spotifyParameters, request.QueryStringParameters);
        }
        catch (Exception ex)
        {
            return Response.BadRequest(ex.Message);
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
        ).SpotifyTrackSortBy<SpotifyTrackDTO>(spotifyParameters.SortBy)
        .SpotifyTrackSearchBy<SpotifyTrackDTO>(spotifyParameters)
        .GetPage(spotifyParameters.PageSize, spotifyParameters.PageNumber).ToListAsync();

        if (tracks.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(tracks);
        }

    }

    /// <summary>
    /// This function returns the number of records in a given table. 
    /// </summary>
    /// <remarks>This function will mostly be used to determine how many pages of data there should be.</remarks>
    /// <param name="request">request contains a path parameter that says which table to get the count of</param>
    /// <param name="context"></param>
    /// <returns>The number of records in a given table.</returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> GetCountAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        string table = string.Empty;
        int count = 0;

        try
        {
            table = QueryParameterService.GetCountParameters(request.PathParameters);
        }
        catch (Exception ex)
        {
            Response.BadRequest(ex.Message);
        }

        switch (table)
        {
            case "spotify-tracks":
                count = await _DBContext.SpotifyTracks.Select(
                t => new SpotifyTrackDTO
                {
                    Name = t.Name,
                }
                ).CountAsync();
                break;
            default:
                Response.NotFound();
                break;
        }


        if (count < 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OKCount(count);
        }

    }

}
