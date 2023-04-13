using MetaMoodAWSAPI.QueryParameterModels;
using MetaMoodAWSAPI.Validation;

namespace MetaMoodAWSAPI.Services
{
    internal class QueryParameterService
    {

        /// <summary>
        /// This method retrieves and calls validators for all query parameters that were provided in the GET request.
        /// It will populate the SpotifyParameters object that has all the search and sort criteria. 
        /// </summary>
        /// <param name="spotifyParameters"></param>
        /// <param name="queryParameters"></param>
        /// <returns>An object with all search criteria provided by URL query parameters</returns>
        /// <exception cref="Exception">Thrown when there is missing or invalid input</exception>
        public static SpotifyParameters GetSpotifyQueryParameters(SpotifyParameters spotifyParameters, IDictionary<string, string> queryParameters)
        {
            if (queryParameters.ContainsKey("PageSize"))
            {
                try
                {
                    spotifyParameters.PageSize = Convert.ToInt32(queryParameters["PageSize"]);
                }
                catch
                {
                    throw new Exception("Page size must be an integer.");
                }

            }
            else
            {
                throw new Exception("Page size is a required parameter.");
            }

            if (queryParameters.ContainsKey("PageNumber"))
            {
                try
                {
                    spotifyParameters.PageNumber = Convert.ToInt32(queryParameters["PageNumber"]);
                }
                catch
                {
                    throw new Exception("Page number must be an integer.");
                }
            }
            else
            {
                throw new Exception("Page number is a required parameter.");
            }

            if (queryParameters.ContainsKey("SortBy"))
            {
                spotifyParameters.SortBy = queryParameters["SortBy"];

                if (!SpotifyValidation.ValidateSpotifySortBy(spotifyParameters.SortBy))
                    throw new Exception("Invalid sort criteria provided.");

            }

            if (queryParameters.ContainsKey("Name"))
            {
                spotifyParameters.Name = queryParameters["Name"].SanitizeString();
            }

            return spotifyParameters;
        }

        /// <summary>
        /// This method simply extracts the table name from the path parameter.
        /// </summary>
        /// <param name="pathParameters"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetCountParameters(IDictionary<string, string> pathParameters)
        {
            if (pathParameters.ContainsKey("table"))
            {
                return pathParameters["table"];
            }
            else
            {
                throw new Exception("No table provided to find the number of records in.");
            }
        }
    }
}