using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Net;


namespace MetaMoodAWSAPI.Services
{
    internal static class Response
    {
        private readonly static IDictionary<string, string> CORSHeader = new Dictionary<string, string>()
        {
            { "Access-Control-Allow-Origin", @"https://ec2-3-238-222-13.compute-1.amazonaws.com" }
        };
        
        


        /// <summary>
        /// A simple lambda that returns HttpStatus code 400 for AWS API Gateway HTTP response.
        /// The purpose of this method is to clean up the code in Function.cs.
        /// </summary>
        /// <param name="errorMessage">The error message returned by the exception that was thrown (probably during data
        /// retrieval or validation).</param>
        /// <returns>HTTP error code 400 - Bad Request with error message</returns>
        public static APIGatewayHttpApiV2ProxyResponse BadRequest(string? errorMessage) => 
            new()
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Headers = CORSHeader,
                Body = errorMessage ?? "No error message provided"
            };

        /// <summary>
        /// A simple lambda that returns HttpStatus code 404 for AWS API Gateway HTTP response.
        /// The purpose of this method is to clean up the code in Function.cs.
        /// </summary>
        /// <returns>HTTP error code 404 - Not Found</returns>
        public static APIGatewayHttpApiV2ProxyResponse NotFound() =>
            new()
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Headers = CORSHeader,
                Body = "Item(s) not found."
            };

        /// <summary>
        /// A simple lambda that returns HttpStatus code 200 for AWS API Gateway HTTP response.
        /// The purpose of this method is to clean up the code in Function.cs.
        /// </summary>
        /// <typeparam name="T">The type for the list that will be serialized into the body of the HTTP response</typeparam>
        /// <param name="objs">The list that will be serialized into the body of the HTTP response</param>
        /// <returns>HTTP success code 200 - OK</returns>
        public static APIGatewayHttpApiV2ProxyResponse OK<T>(IList<T> objs) =>
            new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = CORSHeader,
                Body = JsonConvert.SerializeObject(objs)
            };

        public static APIGatewayHttpApiV2ProxyResponse OKCount(int count) =>
            new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Headers = CORSHeader,
                Body = count.ToString()
            };




    }
}
