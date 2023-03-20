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
            if(queryParameters.ContainsKey("pageSize"))
            {
                try
                {
                    spotifyParameters.PageSize = Convert.ToInt32(queryParameters["pageSize"]);
                }
                catch
                {
                    throw new Exception("Page size must be an integer.");
                }
                
            }
            else
            {
                throw new Exception("Page size is a required parameter");
            }

            if(queryParameters.ContainsKey("pageNumber"))
            {
                try
                {
                    spotifyParameters.PageNumber = Convert.ToInt32(queryParameters["pageNumber"]);
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

            if (queryParameters.ContainsKey("sortBy"))
            {
                spotifyParameters.SortBy = queryParameters["sortBy"];

                if (!SpotifyValidation.ValidateSpotifySortBy(spotifyParameters.SortBy))
                    throw new Exception("Invalid sort criteria provided.");

            }

            if (queryParameters.ContainsKey("name"))
            {
                spotifyParameters.Name = queryParameters["name"];
            }

            if (queryParameters.ContainsKey("lowerReleaseDate"))
            {
                spotifyParameters.LowerReleaseDate = queryParameters["lowerReleaseDate"];
            }

            if (queryParameters.ContainsKey("upperReleaseDate"))
            {
                spotifyParameters.UpperReleaseDate = queryParameters["upperReleaseDate"];
            }

            return spotifyParameters;
        }
    }
}
