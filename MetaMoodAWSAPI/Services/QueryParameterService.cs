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
            if (queryParameters.ContainsKey("pageSize"))
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
                throw new Exception("Page size is a required parameter.");
            }

            if (queryParameters.ContainsKey("pageNumber"))
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

            if (queryParameters.ContainsKey("lowerPopularity"))
            {
                try
                {
                    spotifyParameters.LowerPopularity = Convert.ToInt32(queryParameters["lowerPopularity"]);
                }
                catch
                {
                    throw new Exception("Lower bound for popularity must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperPopularity"))
            {
                try
                {
                    spotifyParameters.UpperPopularity = Convert.ToInt32(queryParameters["upperPopularity"]);
                }
                catch
                {
                    throw new Exception("Upper bound for popularity must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerAcousticness"))
            {
                try
                {
                    spotifyParameters.LowerAcousticness = Convert.ToDouble(queryParameters["lowerAcousticness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for acousticness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperAcousticness"))
            {
                try
                {
                    spotifyParameters.UpperAcousticness = Convert.ToDouble(queryParameters["upperAcousticness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for acousticness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerDanceability"))
            {
                try
                {
                    spotifyParameters.LowerDanceability = Convert.ToDouble(queryParameters["lowerDanceability"]);
                }
                catch
                {
                    throw new Exception("Lower bound for danceability must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperDanceability"))
            {
                try
                {
                    spotifyParameters.UpperDanceability = Convert.ToDouble(queryParameters["upperDanceability"]);
                }
                catch
                {
                    throw new Exception("Upper bound for danceability must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerEnergy"))
            {
                try
                {
                    spotifyParameters.LowerEnergy = Convert.ToDouble(queryParameters["lowerEnergy"]);
                }
                catch
                {
                    throw new Exception("Lower bound for energy must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperEnergy"))
            {
                try
                {
                    spotifyParameters.UpperEnergy = Convert.ToDouble(queryParameters["upperEnergy"]);
                }
                catch
                {
                    throw new Exception("Upper bound for energy must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerLiveness"))
            {
                try
                {
                    spotifyParameters.LowerLiveness = Convert.ToDouble(queryParameters["lowerLiveness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for liveness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperLiveness"))
            {
                try
                {
                    spotifyParameters.UpperLiveness = Convert.ToDouble(queryParameters["upperLiveness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for liveness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerLoudness"))
            {
                try
                {
                    spotifyParameters.LowerLoudness = Convert.ToDouble(queryParameters["lowerLoudness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for loudness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperLoudness"))
            {
                try
                {
                    spotifyParameters.UpperLoudness = Convert.ToDouble(queryParameters["upperLoudness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for loudness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerSpeechiness"))
            {
                try
                {
                    spotifyParameters.LowerSpeechiness = Convert.ToDouble(queryParameters["lowerSpeechiness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for speechiness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperSpeechiness"))
            {
                try
                {
                    spotifyParameters.UpperSpeechiness = Convert.ToDouble(queryParameters["upperSpeechiness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for speechiness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerTempo"))
            {
                try
                {
                    spotifyParameters.LowerTempo = Convert.ToDouble(queryParameters["lowerTempo"]);
                }
                catch
                {
                    throw new Exception("Lower bound for tempo must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperTempo"))
            {
                try
                {
                    spotifyParameters.UpperTempo = Convert.ToDouble(queryParameters["upperTempo"]);
                }
                catch
                {
                    throw new Exception("Upper bound for tempo must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerInstrumentalness"))
            {
                try
                {
                    spotifyParameters.LowerInstrumentalness = Convert.ToDouble(queryParameters["lowerInstrumentalness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for instrumentalness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperInstrumentalness"))
            {
                try
                {
                    spotifyParameters.UpperInstrumentalness = Convert.ToDouble(queryParameters["upperInstrumentalness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for instrumentalness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("lowerValence"))
            {
                try
                {
                    spotifyParameters.LowerValence = Convert.ToDouble(queryParameters["lowerValence"]);
                }
                catch
                {
                    throw new Exception("Lower bound for valence must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("upperValence"))
            {
                try
                {
                    spotifyParameters.UpperValence = Convert.ToDouble(queryParameters["upperValence"]);
                }
                catch
                {
                    throw new Exception("Upper bound for valence must be an integer.");
                }
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