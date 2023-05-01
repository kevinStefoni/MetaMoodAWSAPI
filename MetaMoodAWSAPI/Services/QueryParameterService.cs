﻿using MetaMoodAWSAPI.QueryParameterModels;
using MetaMoodAWSAPI.Validation;
using System.Runtime.CompilerServices;

namespace MetaMoodAWSAPI.Services
{
    internal static class QueryParameterService
    {

        /// <summary>
        /// This method retrieves and calls validators for all query parameters that were provided in the GET request.
        /// It will populate the SQLParameters object that has all the search and sort criteria. 
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <param name="queryParameters"></param>
        /// <returns>An object with all search criteria provided by URL query parameters</returns>
        /// <exception cref="Exception">Thrown when there is missing or invalid input</exception>
        public static SQLParameters GetSQLParameters(SQLParameters sqlParameters, IDictionary<string, string> queryParameters)
        {
            if (queryParameters.ContainsKey("PageSize"))
            {
                try
                {
                    sqlParameters.PageSize = Convert.ToInt32(queryParameters["PageSize"]);
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
                    sqlParameters.PageNumber = Convert.ToInt32(queryParameters["PageNumber"]);
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
                sqlParameters.SortBy = queryParameters["SortBy"];

                if (!DataValidation.ValidateSortBy(sqlParameters.SortBy))
                    throw new Exception("Invalid sort criteria provided.");

            }

            if (queryParameters.ContainsKey("Search"))
            {
                sqlParameters.Search = queryParameters["Search"].SanitizeString();
            }

            return sqlParameters;
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

        public static int ToIntCluster(this string clusterName)
        {

            return clusterName switch
            {
                "anger" => 0,
                "angry" => 0,
                "fearful" => 1,
                "fear" => 1,
                "happy" => 2,
                "happiness" => 2,
                "love" => 3,
                "sadness" => 4,
                "sad" => 4,
                "surprise" => 5,
                "surprised" => 5,
                _ => throw new Exception("Invalid emotion search criteria."),
            };
        }
    }
}