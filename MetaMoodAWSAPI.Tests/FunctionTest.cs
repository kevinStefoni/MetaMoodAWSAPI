using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetaMoodAWSAPI.Tests;

public class FunctionTest
{
    public Function function;


    public FunctionTest()
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        function = new(new MetaMoodContext(new DbContextOptionsBuilder<MetaMoodContext>()
                                 .UseMySql(config["ConnectionString"],
                                 Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")).Options), config["ConnectionString"] ?? string.Empty);
    }

    [Fact]
    public async void TestGetTrackPageAsyncPageSizeException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>();
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Page size is a required parameter.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncPageNumberException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "10"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Page number is a required parameter.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncInvalidPageSizeException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "abc",
            ["PageNumber"] = "2"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Page size must be an integer.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncInvalidPageNumberException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "abc"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Page number must be an integer.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncInvalidSortByException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
            ["SortBy"] = "abc"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Invalid sort criteria provided.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncGetPage()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<SpotifyTrackDTO> tracks = JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body) ?? new List<SpotifyTrackDTO>();
        Assert.Equal(20, tracks.Count);

    }

    [Fact]
    public async void TestGetTrackPageAsyncGetPageSortByAcousticness()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "3",
            ["SortBy"] = "Acousticness"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<SpotifyTrackDTO> tracks = JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body) ?? new List<SpotifyTrackDTO>();
        IList<SpotifyTrackDTO> expectedTracks = tracks.OrderBy(t => t.Acousticness).ToList();
        Assert.Equal(50, tracks.Count);
        Assert.Equal(expectedTracks, tracks);

    }

    [Fact]
    public async void TestGetTrackPageAsyncGetPageSortByNameByDefault()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<SpotifyTrackDTO> tracks = JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body) ?? new List<SpotifyTrackDTO>();
        IList<SpotifyTrackDTO> expectedTracks = tracks.OrderBy(t => t.Name).ToList();
        Assert.Equal(50, tracks.Count);
        Assert.Equal(expectedTracks, tracks);

    }

    [Theory]
    [InlineData("Sunrise")]
    [InlineData("親愛的黑色")]
    public async void TestGetTrackPageAsyncGetPageSearchByName(string trackName)
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["Search"] = $"{trackName}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<SpotifyTrackDTO> tracks = JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body) ?? new List<SpotifyTrackDTO>();
        Assert.True(tracks.Count > 0);
        foreach(var track in tracks)
        {
            Assert.Contains(trackName.ToLower(), track.Name?.ToLower());
        }

    }

    [Fact]
    public async void TestGetTrackPageAsyncGetPageSearchByNameNotFound()
    {
        string trackName = "abvwuhoh2190u1qiuuqjquhqiuqvhuihu2h2h9ph9v298hiuhv2uh2v9hvui2hv982";

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["Search"] = $"{trackName}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Item(s) not found.", response.Body);

    }

    [Fact]
    public async void TestGetCommentPageAsync()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetCommentPageAsync(request, new TestLambdaContext());
        IList<RedditCommentDTO> comments = JsonConvert.DeserializeObject<List<RedditCommentDTO>>(response.Body) ?? new List<RedditCommentDTO>();
        Assert.Equal(20, comments.Count);

    }

    [Fact]
    public async void TestGetCommentPageAsyncSortByBodyByDefault()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<RedditCommentDTO> comments = JsonConvert.DeserializeObject<List<RedditCommentDTO>>(response.Body) ?? new List<RedditCommentDTO>();
        IList<RedditCommentDTO> expectedComments = comments.OrderBy(t => t.Body).ToList();
        Assert.Equal(50, comments.Count);
        Assert.Equal(expectedComments, comments);

    }

    [Fact]
    public async void TestGetCommentPageAsyncSortByBody()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Body"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<RedditCommentDTO> comments = JsonConvert.DeserializeObject<List<RedditCommentDTO>>(response.Body) ?? new List<RedditCommentDTO>();
        IList<RedditCommentDTO> expectedComments = comments.OrderBy(t => t.Body).ToList();
        Assert.Equal(50, comments.Count);
        Assert.Equal(expectedComments, comments);

    }

    [Fact]
    public async void TestGetCommentPageAsyncSortByEmotion()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Emotion"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<RedditCommentDTO> comments = JsonConvert.DeserializeObject<List<RedditCommentDTO>>(response.Body) ?? new List<RedditCommentDTO>();
        IList<RedditCommentDTO> expectedComments = comments.OrderBy(t => t.Emotion).ToList();
        Assert.Equal(50, comments.Count);
        Assert.Equal(expectedComments, comments);

    }

    [Theory]
    [InlineData (0)]
    [InlineData (1)]
    [InlineData (2)]
    [InlineData (3)]
    [InlineData (4)]
    [InlineData (5)]
    public async void TestGetCommentPageAsyncSearch(int Search)
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Body",
            ["Search"] = Search.ToString()
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<RedditCommentDTO> comments = JsonConvert.DeserializeObject<List<RedditCommentDTO>>(response.Body) ?? new List<RedditCommentDTO>();
        Assert.True(comments.Count > 0);
        foreach (var c in comments)
        {
            Assert.Equal(Search, c.Emotion);
        }

    }

    [Fact]
    public async void TestGetCommentPageAsyncGetPageSearchByEmotionNotFound()
    {
        string commentSearch = "abvwuhoh2190u1qiuuqjquhqiuqvhuihu2h2h9ph9v298hiuhv2uh2v9hvui2hv982";

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["Search"] = $"{commentSearch}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Item(s) not found.", response.Body);

    }

    [Fact]
    public async void TestGetTweetPageAsync()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTweetPageAsync(request, new TestLambdaContext());
        IList<TweetDTO> tweets = JsonConvert.DeserializeObject<List<TweetDTO>>(response.Body) ?? new List<TweetDTO>();
        Assert.Equal(20, tweets.Count);

    }

    [Fact]
    public async void TestGetTweetPageAsyncSortByTweetByDefault()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<TweetDTO> tweets = JsonConvert.DeserializeObject<List<TweetDTO>>(response.Body) ?? new List<TweetDTO>();
        IList<TweetDTO> expectedTweets = tweets.OrderBy(t => t.Tweet).ToList();
        Assert.Equal(50, tweets.Count);
        Assert.Equal(expectedTweets, tweets);

    }

    [Fact]
    public async void TestGetTweetPageAsyncSortByTweet()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Tweet"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<TweetDTO> tweets = JsonConvert.DeserializeObject<List<TweetDTO>>(response.Body) ?? new List<TweetDTO>();
        IList<TweetDTO> expectedTweets = tweets.OrderBy(t => t.Tweet).ToList();
        Assert.Equal(50, tweets.Count);
        Assert.Equal(expectedTweets, tweets);

    }

    [Fact]
    public async void TestGetTweetPageAsyncSortByEmotion()
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Emotion"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<TweetDTO> tweets = JsonConvert.DeserializeObject<List<TweetDTO>>(response.Body) ?? new List<TweetDTO>();
        IList<TweetDTO> expectedTweets = tweets.OrderBy(t => t.Emotion).ToList();
        Assert.Equal(50, tweets.Count);
        Assert.Equal(expectedTweets, tweets);

    }

    [Theory]
    [InlineData("test")]
    [InlineData("hello")]
    [InlineData("basketball")]
    public async void TestGetTweetPageAsyncSearch(string Search)
    {
        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Tweet",
            ["Search"] = Search
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<TweetDTO> comments = JsonConvert.DeserializeObject<List<TweetDTO>>(response.Body) ?? new List<TweetDTO>();
        Assert.True(comments.Count > 0);
        foreach (var c in comments)
        {
            Assert.Contains(Search.ToLower(), c.Tweet?.ToLower());
        }

    }

    [Fact]
    public async void TestGetTweetPageAsyncGetPageSearchByTweetNotFound()
    {
        string commentSearch = "abvwuhoh2190u1qiuuqjquhqiuqvhuihu2h2h9ph9v298hiuhv2uh2v9hvui2hv982";

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["Search"] = $"{commentSearch}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Item(s) not found.", response.Body);

    }

}
