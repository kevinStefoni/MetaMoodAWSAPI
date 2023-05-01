using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.Entities;
using MetaMoodAWSAPI.QueryParameterModels;
using MetaMoodAWSAPI.Services;
using MetaMoodAWSAPI.Validation;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System.Data;
using System.Reflection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MetaMoodAWSAPI;

public class Function
{
    private readonly IServiceCollection _serviceCollection;
    private readonly MetaMoodContext _DBContext;
    private readonly string Conn = System.Environment.GetEnvironmentVariable("ConnectionString") ?? string.Empty;

    /// <summary>
    /// This is the constructor for the Lambda function that sets up the collection of services, calls RegisterServices(),
    /// and provides a database context for this class. 
    /// </summary>
    public Function()
    {
        _serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = _serviceCollection.RegisterServices(Conn).BuildServiceProvider();
        _DBContext = serviceProvider.GetRequiredService<MetaMoodContext>();
    }

    /// <summary>
    /// Constructor that allows injection of DBContext. This method is to allow unit tests to inject their own DB context.
    /// </summary>
    /// <param name="dbContext">The database context created in the test project</param>
    public Function(MetaMoodContext dbContext, string connectionString)
    {
        Conn = connectionString;
        _serviceCollection = new ServiceCollection();
        _serviceCollection.RegisterServices(Conn).BuildServiceProvider();
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
        SQLParameters spotifyParameters = new()
        {
            SortBy = "Name"
        };

        try
        {
            spotifyParameters = QueryParameterService.GetSQLParameters(spotifyParameters, request.QueryStringParameters);
        }
        catch (Exception ex)
        {
            return Response.BadRequest(ex.Message);
        }

        IList<SpotifyTrackDTO> tracks = new List<SpotifyTrackDTO>();
        using (MySqlConnection conn = new(Conn))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new("GET_SPOTIFY_TRACKS", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageSize", spotifyParameters.PageSize);
            cmd.Parameters.AddWithValue("@PageOffset", (spotifyParameters.PageNumber - 1) * spotifyParameters.PageSize);
            cmd.Parameters.AddWithValue("@Search", spotifyParameters.Search);
            cmd.Parameters.AddWithValue("@SortBy", spotifyParameters.SortBy);

            MySqlDataAdapter adapter = new(cmd);
            DataTable dtTracks = new();
            adapter.Fill(dtTracks);
            foreach (DataRow r in dtTracks.Rows)
            {
                tracks.Add(new()
                {
                    Name = r["name"].ToString(),
                    ReleaseDate = r["releasedate"].ToString(),
                    Popularity = Convert.ToInt32(r["popularity"]),
                    Acousticness = Convert.ToDouble(r["acousticness"]),
                    Danceability = Convert.ToDouble(r["danceability"]),
                    Energy = Convert.ToDouble(r["energy"]),
                    Liveness = Convert.ToDouble(r["liveness"]),
                    Loudness = Convert.ToDouble(r["loudness"]),
                    Speechiness = Convert.ToDouble(r["speechiness"]),
                    Tempo = Convert.ToDouble(r["tempo"]),
                    Instrumentalness = Convert.ToDouble(r["instrumentalness"]),
                    Valence = Convert.ToDouble(r["valence"]),
                    Emotion = Convert.ToInt32(r[12]),
                    CoverImageUrl = r[13].ToString()
                });
                
            }
        }


        if (tracks.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(tracks);
        }

    }

    public async Task<APIGatewayHttpApiV2ProxyResponse> GetCommentPageAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        SQLParameters redditParameters = new()
        {
            SortBy = "Body"
        };
        try
        {
            redditParameters = QueryParameterService.GetSQLParameters(redditParameters, request.QueryStringParameters);
        }
        catch (Exception ex)
        {
            return Response.BadRequest(ex.Message);
        }

        IList<RedditCommentDTO> comments = new List<RedditCommentDTO>();
        using (MySqlConnection conn = new(Conn))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new("GET_REDDIT_COMMENTS", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageSize", redditParameters.PageSize);
            cmd.Parameters.AddWithValue("@PageOffset", (redditParameters.PageNumber - 1) * redditParameters.PageSize);

            try
            {
                cmd.Parameters.AddWithValue("@Search", redditParameters.Search?.ToIntCluster());
            }
            catch { return Response.BadRequest("Invalid emotion in search."); }

            cmd.Parameters.AddWithValue("@SortBy", redditParameters.SortBy);

            MySqlDataAdapter adapter = new(cmd);
            DataTable dtComments = new();
            adapter.Fill(dtComments);
            foreach (DataRow r in dtComments.Rows)
            {
                comments.Add(new()
                {
                    Body = r["body"].ToString(),
                    Emotion = Convert.ToInt32(r["emotion"])

                });
            }
        }


        if (comments.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(comments);
        }

    }

    public async Task<APIGatewayHttpApiV2ProxyResponse> GetTweetPageAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        SQLParameters tweetParameters = new()
        {
            SortBy = "Tweet"
        };

        try
        {
            tweetParameters = QueryParameterService.GetSQLParameters(tweetParameters, request.QueryStringParameters);
        }
        catch (Exception ex)
        {
            return Response.BadRequest(ex.Message);
        }

        IList<TweetDTO> comments = new List<TweetDTO>();
        using (MySqlConnection conn = new(Conn))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new("GET_TWEETS", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PageSize", tweetParameters.PageSize);
            cmd.Parameters.AddWithValue("@PageOffset", (tweetParameters.PageNumber - 1) * tweetParameters.PageSize);
            cmd.Parameters.AddWithValue("@Search", tweetParameters.Search);
            cmd.Parameters.AddWithValue("@SortBy", tweetParameters.SortBy);

            MySqlDataAdapter adapter = new(cmd);
            DataTable dtComments = new();
            adapter.Fill(dtComments);
            foreach (DataRow r in dtComments.Rows)
            {
                comments.Add(new()
                {

                    Tweet = r["tweet"].ToString(),
                    Emotion = Convert.ToInt32(r["emotion"])

                });
            }
        }


        if (comments.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(comments);
        }

    }

    /// <summary>
    /// This function returns the number of records in a given table. 
    /// </summary>
    /// <remarks>This function will mostly be used to determine how many pages of data there should be.</remarks>
    /// <param name="request">request contains a path parameter that says which table to get the count of</param>
    /// <param name="context"></param>
    /// <returns>The number of records in a given table.</returns>
    public APIGatewayHttpApiV2ProxyResponse GetCount(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        int count = 0;

        string table;
        try
        {
            table = QueryParameterService.GetCountParameters(request.PathParameters);
        }
        catch (Exception ex)
        {
            return Response.BadRequest(ex.Message);
        }

        switch (table)
        {
            case "spotify-tracks":
                using (var conn = new MySqlConnection(Conn))
                {
                    using var cmd = new MySqlCommand()
                    {
                        CommandText = $"SELECT `spotify-tracks` FROM counts",
                        CommandType = CommandType.Text,
                        Connection = conn
                    };
                    conn.Open();

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
                break;

            case "reddit-comments":
                using (var conn = new MySqlConnection(Conn))
                {
                    using var cmd = new MySqlCommand()
                    {
                        CommandText = $"SELECT `reddit-comments` FROM counts",
                        CommandType = CommandType.Text,
                        Connection = conn
                    };
                    conn.Open();

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
                break;

            case "tweets":
                using (var conn = new MySqlConnection(Conn))
                {
                    using var cmd = new MySqlCommand()
                    {
                        CommandText = $"SELECT `tweets` FROM counts",
                        CommandType = CommandType.Text,
                        Connection = conn
                    };
                    conn.Open();

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        count = (int)reader[0];
                    }
                }
                break;

            default:
                return Response.NotFound();
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

    /// <summary>
    /// This function returns the number of records that are in a result set after a search criteria has been applied.
    /// <remark>
    /// The performance of this function should be slower than that of the generic GetCount() that uses a separate table that
    /// keeps track of counts to retrieve the number of records in a table. 
    /// </remark>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>The number of records in the result set of a search query</returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> GetSpotifySearchCountAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        int count = 0;
        string Name;

        if (request.QueryStringParameters is not null && request.QueryStringParameters.ContainsKey("Search"))
        {
            Name = request.QueryStringParameters["Search"].SanitizeString();
        }
        else
        {
            return Response.BadRequest("Search criteria has to be provided.");
        }


        using (MySqlConnection conn = new(Conn))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new("GET_SPOTIFY_TRACKS_COUNT", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Search", Name);
        
            count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        return Response.OKCount(count);
        
    }

    public async Task<APIGatewayHttpApiV2ProxyResponse> GetRedditSearchCountAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        int count = 0;
        int Emotion;

        if (request.QueryStringParameters is not null && request.QueryStringParameters.ContainsKey("Search"))
        {
            try
            { 
                Emotion = request.QueryStringParameters["Search"].ToLower().SanitizeString().ToIntCluster();

            }catch(Exception e) { return Response.BadRequest(e.Message); }
        }
        else
        {
            return Response.BadRequest("Search criteria has to be provided.");
        }


        using (MySqlConnection conn = new(Conn))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new("GET_REDDIT_COMMENTS_COUNT", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Search", Emotion);

            count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        return Response.OKCount(count);

    }

    public async Task<APIGatewayHttpApiV2ProxyResponse> GetTweetSearchCountAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        int count = 0;
        string Search;

        if (request.QueryStringParameters is not null && request.QueryStringParameters.ContainsKey("Search"))
        {
            Search = request.QueryStringParameters["Search"].SanitizeString();
        }
        else
        {
            return Response.BadRequest("Search criteria has to be provided.");
        }


        using (MySqlConnection conn = new(Conn))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new("GET_TWEETS_COUNT", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Search", Search);

            count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        return Response.OKCount(count);

    }

    /// <summary>
    /// This function returns a list of strings containing the data for the bar graph that has the average values for each category
    /// with a floating point value (besides loudness). 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns>A list of doubles converted to strings that will be used for the Chart.js bargraph with Spotify metric averages</returns>
    public APIGatewayHttpApiV2ProxyResponse GetSpotifyAverages(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        IList<string> data = new List<string>();

        using (var conn = new MySqlConnection(Conn))
        {
            using var cmd = new MySqlCommand()
            {
                CommandText = $"SELECT * FROM spotify_metric_averages",
                CommandType = CommandType.Text,
                Connection = conn
            };
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                data.Add(((double)reader[0]).ToString() ?? "0.00");
                data.Add(((double)reader[1]).ToString() ?? "0.00");
                data.Add(((double)reader[2]).ToString() ?? "0.00");
                data.Add(((double)reader[3]).ToString() ?? "0.00");
                data.Add(((double)reader[4]).ToString() ?? "0.00");
                data.Add(((double)reader[5]).ToString() ?? "0.00");
                data.Add(((double)reader[6]).ToString() ?? "0.00");
            }
        }

        if (data.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(data);
        }

    }

    public APIGatewayHttpApiV2ProxyResponse GetRedditEmotionSum(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        IList<string> data = new List<string>();

        using (var conn = new MySqlConnection(Conn))
        {
            using var cmd = new MySqlCommand()
            {
                CommandText = $"SELECT anger, fear, happy, love, sad, surprise FROM reddit_emotion_sum",
                CommandType = CommandType.Text,
                Connection = conn
            };
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                data.Add(((int)reader[0]).ToString() ?? "0.00");
                data.Add(((int)reader[1]).ToString() ?? "0.00");
                data.Add(((int)reader[2]).ToString() ?? "0.00");
                data.Add(((int)reader[3]).ToString() ?? "0.00");
                data.Add(((int)reader[4]).ToString() ?? "0.00");
                data.Add(((int)reader[5]).ToString() ?? "0.00");
            }
        }

        if (data.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(data);
        }

    }

    public APIGatewayHttpApiV2ProxyResponse GetTwitterEmotionSum(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        IList<string> data = new List<string>();

        using (var conn = new MySqlConnection(Conn))
        {
            using var cmd = new MySqlCommand()
            {
                CommandText = $"SELECT anger, fear, happy, love, sad, surprise FROM twitter_emotion_sum",
                CommandType = CommandType.Text,
                Connection = conn
            };
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                data.Add(((int)reader[0]).ToString() ?? "0.00");
                data.Add(((int)reader[1]).ToString() ?? "0.00");
                data.Add(((int)reader[2]).ToString() ?? "0.00");
                data.Add(((int)reader[3]).ToString() ?? "0.00");
                data.Add(((int)reader[4]).ToString() ?? "0.00");
                data.Add(((int)reader[5]).ToString() ?? "0.00");
            }
        }

        if (data.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(data);
        }

    }

    public APIGatewayHttpApiV2ProxyResponse GetSpotifyEmotionSum(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        IList<string> data = new List<string>();

        using (var conn = new MySqlConnection(Conn))
        {
            using var cmd = new MySqlCommand()
            {
                CommandText = $"SELECT anger, fear, happy, love, sad, surprise FROM spotify_emotion_sum",
                CommandType = CommandType.Text,
                Connection = conn
            };
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                data.Add(((int)reader[0]).ToString() ?? "0.00");
                data.Add(((int)reader[1]).ToString() ?? "0.00");
                data.Add(((int)reader[2]).ToString() ?? "0.00");
                data.Add(((int)reader[3]).ToString() ?? "0.00");
                data.Add(((int)reader[4]).ToString() ?? "0.00");
                data.Add(((int)reader[5]).ToString() ?? "0.00");
            }
        }

        if (data.Count <= 0)
        {
            return Response.NotFound();
        }
        else
        {
            return Response.OK(data);
        }

    }

}
