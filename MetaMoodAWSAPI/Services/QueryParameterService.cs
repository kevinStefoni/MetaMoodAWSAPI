using MetaMoodAWSAPI.QueryParameterModels;
using MetaMoodAWSAPI.Validation;

namespace MetaMoodAWSAPI.Services
{
    internal class QueryParameterService
    {

        public static SpotifyParameters GetSpotifyQueryParameters(SpotifyParameters spotifyParameters, IDictionary<string, string> queryParameters)
        {
            if(queryParameters.ContainsKey("pageSize"))
            {
                spotifyParameters.PageSize = Convert.ToInt32(queryParameters["pageSize"]);
            }
            else
            {
                throw new Exception("Page size is a required parameter");
            }

            if(queryParameters.ContainsKey("pageNumber"))
            {
                spotifyParameters.PageNumber = Convert.ToInt32(queryParameters["pageNumber"]);
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
