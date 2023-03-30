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
                spotifyParameters.Name = queryParameters["Name"];
            }

            if (queryParameters.ContainsKey("LowerReleaseDate"))
            {
                spotifyParameters.LowerReleaseDate = queryParameters["LowerReleaseDate"];
            }

            if (queryParameters.ContainsKey("UpperReleaseDate"))
            {
                spotifyParameters.UpperReleaseDate = queryParameters["UpperReleaseDate"];
            }

            if (queryParameters.ContainsKey("LowerPopularity"))
            {
                try
                {
                    spotifyParameters.LowerPopularity = Convert.ToInt32(queryParameters["LowerPopularity"]);
                }
                catch
                {
                    throw new Exception("Lower bound for popularity must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperPopularity"))
            {
                try
                {
                    spotifyParameters.UpperPopularity = Convert.ToInt32(queryParameters["UpperPopularity"]);
                }
                catch
                {
                    throw new Exception("Upper bound for popularity must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerAcousticness"))
            {
                try
                {
                    spotifyParameters.LowerAcousticness = Convert.ToDouble(queryParameters["LowerAcousticness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for acousticness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperAcousticness"))
            {
                try
                {
                    spotifyParameters.UpperAcousticness = Convert.ToDouble(queryParameters["UpperAcousticness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for acousticness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerDanceability"))
            {
                try
                {
                    spotifyParameters.LowerDanceability = Convert.ToDouble(queryParameters["LowerDanceability"]);
                }
                catch
                {
                    throw new Exception("Lower bound for danceability must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperDanceability"))
            {
                try
                {
                    spotifyParameters.UpperDanceability = Convert.ToDouble(queryParameters["UpperDanceability"]);
                }
                catch
                {
                    throw new Exception("Upper bound for danceability must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerEnergy"))
            {
                try
                {
                    spotifyParameters.LowerEnergy = Convert.ToDouble(queryParameters["LowerEnergy"]);
                }
                catch
                {
                    throw new Exception("Lower bound for energy must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperEnergy"))
            {
                try
                {
                    spotifyParameters.UpperEnergy = Convert.ToDouble(queryParameters["UpperEnergy"]);
                }
                catch
                {
                    throw new Exception("Upper bound for energy must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerLiveness"))
            {
                try
                {
                    spotifyParameters.LowerLiveness = Convert.ToDouble(queryParameters["LowerLiveness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for liveness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperLiveness"))
            {
                try
                {
                    spotifyParameters.UpperLiveness = Convert.ToDouble(queryParameters["UpperLiveness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for liveness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerLoudness"))
            {
                try
                {
                    spotifyParameters.LowerLoudness = Convert.ToDouble(queryParameters["LowerLoudness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for loudness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperLoudness"))
            {
                try
                {
                    spotifyParameters.UpperLoudness = Convert.ToDouble(queryParameters["UpperLoudness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for loudness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerSpeechiness"))
            {
                try
                {
                    spotifyParameters.LowerSpeechiness = Convert.ToDouble(queryParameters["LowerSpeechiness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for speechiness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperSpeechiness"))
            {
                try
                {
                    spotifyParameters.UpperSpeechiness = Convert.ToDouble(queryParameters["UpperSpeechiness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for speechiness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerTempo"))
            {
                try
                {
                    spotifyParameters.LowerTempo = Convert.ToDouble(queryParameters["LowerTempo"]);
                }
                catch
                {
                    throw new Exception("Lower bound for tempo must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperTempo"))
            {
                try
                {
                    spotifyParameters.UpperTempo = Convert.ToDouble(queryParameters["UpperTempo"]);
                }
                catch
                {
                    throw new Exception("Upper bound for tempo must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerInstrumentalness"))
            {
                try
                {
                    spotifyParameters.LowerInstrumentalness = Convert.ToDouble(queryParameters["LowerInstrumentalness"]);
                }
                catch
                {
                    throw new Exception("Lower bound for instrumentalness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperInstrumentalness"))
            {
                try
                {
                    spotifyParameters.UpperInstrumentalness = Convert.ToDouble(queryParameters["UpperInstrumentalness"]);
                }
                catch
                {
                    throw new Exception("Upper bound for instrumentalness must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("LowerValence"))
            {
                try
                {
                    spotifyParameters.LowerValence = Convert.ToDouble(queryParameters["LowerValence"]);
                }
                catch
                {
                    throw new Exception("Lower bound for valence must be an integer.");
                }
            }

            if (queryParameters.ContainsKey("UpperValence"))
            {
                try
                {
                    spotifyParameters.UpperValence = Convert.ToDouble(queryParameters["UpperValence"]);
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