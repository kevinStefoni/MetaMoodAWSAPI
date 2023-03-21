using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

namespace MetaMoodAWSAPI.Tests;

public class FunctionTest
{
    public Function function = new();

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
        Assert.Equal("Page size is a required parameter.", response.Body);

    }

    [Fact]
    public async void TestGetTrackPageAsyncInvalidPageSizeException()
    {

        APIGatewayHttpApiV2ProxyRequest request = new();
        request.QueryStringParameters = new Dictionary<string, string>
        {
            ["PageSize"] = "abc"
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

}
