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
            ["Name"] = $"{trackName}"
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
            ["Name"] = $"{trackName}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Item(s) not found.", response.Body);

    }

}
