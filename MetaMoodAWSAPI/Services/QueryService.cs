using MetaMoodAWSAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MetaMoodAWSAPI.Services
{
    internal static class QueryService
    {
        /// <summary>
        /// This function is a generic extension method for the IQueryable class that essentially implements offset pagination.
        /// </summary>
        /// <typeparam name="T">The datatype that is going to be retrieved by the query</typeparam>
        /// <param name="query">The query prior to offset pagination</param>
        /// <param name="pageSize">The number of records in any given page</param>
        /// <param name="pageNumber">The specific page that is to be retrieved</param>
        /// <returns>Returns the desired page of records</returns>
        public static IQueryable<T> GetPage<T>(this IQueryable<T> query, int pageSize, int pageNumber) 
            => query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        

        /// <summary>
        /// This function is an extension method for IQueryable<SpotifyTrackDTO> that sorts by a given search
        /// criteria.
        /// </summary>
        /// <typeparam name="T">This generic type actually receives SpotifyTrackDTO</typeparam>
        /// <param name="query">The query that needs to be further refined with sorting</param>
        /// <param name="sortBy">The criteria on which the tracks will be sorted</param>
        /// <returns>A query that also includes sorting based on a criteria</returns>
        /// <exception cref="ArgumentException">Thrown when sorting criteria is invalid</exception>
        public static IQueryable<T> SpotifyTrackSortBy<T>(this IQueryable<SpotifyTrackDTO> query, string sortBy)
        {
            switch(sortBy)
            {
                case "name":
                    return (IQueryable<T>)query.OrderBy(t => t.Name);

                case "releasedate":
                    return (IQueryable<T>)query.OrderBy(t => t.ReleaseDate);

                case "popularity":
                    return (IQueryable<T>)query.OrderBy(t => t.Popularity);

                case "acousticness":
                    return (IQueryable<T>)query.OrderBy(t => t.Acousticness);

                case "danceability":
                    return (IQueryable<T>)query.OrderBy(t => t.Danceability);

                case "energy":
                    return (IQueryable<T>)query.OrderBy(t => t.Energy);

                case "liveness":
                    return (IQueryable<T>)query.OrderBy(t => t.Liveness);

                case "loudness":
                    return (IQueryable<T>)query.OrderBy(t => t.Loudness);

                case "speechiness":
                    return (IQueryable<T>)query.OrderBy(t => t.Speechiness);

                case "tempo":
                    return (IQueryable<T>)query.OrderBy(t => t.Tempo);

                case "instrumentalness":
                    return (IQueryable<T>)query.OrderBy(t => t.Instrumentalness);

                case "valence":
                    return (IQueryable<T>)query.OrderBy(t => t.Valence);

                default:
                    throw new ArgumentException("Invalid search criteria.");
            }
        }

    }
}
