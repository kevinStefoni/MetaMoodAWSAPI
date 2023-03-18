using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Net;


namespace MetaMoodAWSAPI.Services
{
    internal static class Response
    {
        /// <summary>
        /// A simple lambda that returns HttpStatus code 400 for AWS API Gateway HTTP response.
        /// The purpose of this method is to clean up the code in Function.cs.
        /// </summary>
        /// <returns>HTTP error code 400 - Bad Request</returns>
        public static APIGatewayHttpApiV2ProxyResponse BadRequest() => 
            new()
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

        /// <summary>
        /// A simple lambda that returns HttpStatus code 404 for AWS API Gateway HTTP response.
        /// The purpose of this method is to clean up the code in Function.cs.
        /// </summary>
        /// <returns>HTTP error code 404 - Not Found</returns>
        public static APIGatewayHttpApiV2ProxyResponse NotFound() =>
            new()
            {
                StatusCode = (int)HttpStatusCode.NotFound
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
                Body = JsonConvert.SerializeObject(objs)
            };




    }
}
