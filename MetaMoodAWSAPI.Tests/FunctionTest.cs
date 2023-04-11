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
                                 Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")).Options));
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
    public async void TestGetTrackPageAsyncInvalidLowerPopularityException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
            ["LowerPopularity"] = "abc"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Lower bound for popularity must be an integer.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncInvalidUpperValenceException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
            ["UpperValence"] = "abc"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Upper bound for valence must be an integer.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncEmptyNumericCriteriaException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "20",
            ["PageNumber"] = "2",
            ["LowerAcousticness"] = ""
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal("Lower bound for acousticness must be an integer.", response.Body);

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
    public async void TestGetTrackPageAsyncGetPageSortByValenceSearchByLowerLiveness()
    {
        double livenessAmt = 0.75;

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "Valence",
            ["LowerLiveness"] = $"{livenessAmt}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<SpotifyTrackDTO> tracks = JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body) ?? new List<SpotifyTrackDTO>();
        IList<SpotifyTrackDTO> expectedTracks = tracks.OrderBy(t => t.Valence).Where(t => t.Liveness > livenessAmt).ToList();
        Assert.Equal(50, tracks.Count);
        Assert.Equal(expectedTracks, tracks);

    }

    [Fact]
    public async void TestGetTrackPageAsyncGetPageSortByReleaseDateSearchByLowerReleaseDateUpperReleaseDateLowerEnergy()
    {
        string lowerReleaseDate = "2001";
        string upperReleaseDate = "2010";
        double lowerEnergy = 0.5;

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "50",
            ["PageNumber"] = "1",
            ["SortBy"] = "ReleaseDate",
            ["LowerReleaseDate"] = $"{lowerReleaseDate}",
            ["UpperReleaseDate"] = $"{upperReleaseDate}",
            ["LowerEnergy"] = $"{lowerEnergy}"
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        IList<SpotifyTrackDTO> tracks = JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body) ?? new List<SpotifyTrackDTO>();
        IList<SpotifyTrackDTO> expectedTracks = tracks.OrderBy(t => t.ReleaseDate)
            .Where(t => String.Compare(t.ReleaseDate, lowerReleaseDate) > 0)
            .Where(t => String.Compare(t.ReleaseDate, upperReleaseDate) < 0)
            .Where(t => t.Energy > lowerEnergy)
            .ToList();
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
    [InlineData("It's a New Thing (It's Your Thing) - D-Nat & ONDA feat. De La Soul")]
    [InlineData("Baião De Ubá")]
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
            Assert.Equal(trackName.ToLower(), track.Name?.ToLower());
        }

    }

    [Fact]
    public async void TestGetTrackPageAsyncGetPageSearchByNameNotFound()
    {
        string trackName = "abvwuhoh2190u1 11p891ip1j1j 1j j1j8 88j 1j8p91 p89";

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
