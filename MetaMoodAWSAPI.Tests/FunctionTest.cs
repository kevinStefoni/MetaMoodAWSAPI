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
            ["pageSize"] = "10"
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
            ["pageSize"] = "abc",
            ["pageNumber"] = "2"
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
            ["pageSize"] = "20",
            ["pageNumber"] = "abc"
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
            ["pageSize"] = "20",
            ["pageNumber"] = "2",
            ["sortBy"] = "abc"
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
            ["pageSize"] = "20",
            ["pageNumber"] = "2",
            ["lowerPopularity"] = "abc"
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
            ["pageSize"] = "20",
            ["pageNumber"] = "2",
            ["upperValence"] = "abc"
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
            ["pageSize"] = "20",
            ["pageNumber"] = "2",
            ["lowerAcousticness"] = ""
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
            ["pageSize"] = "20",
            ["pageNumber"] = "2",
        };
        APIGatewayHttpApiV2ProxyResponse response = await function.GetTrackPageAsync(request, new TestLambdaContext());
        Assert.Equal(20, JsonConvert.DeserializeObject<List<SpotifyTrackDTO>>(response.Body)?.Count());

    }

}
