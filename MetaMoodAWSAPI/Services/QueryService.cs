using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.QueryParameterModels;

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
        /// criteria. Method assumes that sort criteria has been validated.
        /// </summary>
        /// <typeparam name="T">This generic type should only actually receive SpotifyTrackDTO</typeparam>
        /// <param name="query">The query that needs to be further refined with sorting</param>
        /// <param name="sortBy">The criteria on which the tracks will be sorted</param>
        /// <returns>A query that also includes sorting based on a criteria</returns>
        public static IQueryable<T> SpotifyTrackSortBy<T>(this IQueryable<SpotifyTrackDTO> query, string sortBy)
        {
            return sortBy switch
            {
                "name" => (IQueryable<T>)query.OrderBy(t => t.Name),
                "releasedate" => (IQueryable<T>)query.OrderBy(t => t.ReleaseDate),
                "popularity" => (IQueryable<T>)query.OrderBy(t => t.Popularity),
                "acousticness" => (IQueryable<T>)query.OrderBy(t => t.Acousticness),
                "danceability" => (IQueryable<T>)query.OrderBy(t => t.Danceability),
                "energy" => (IQueryable<T>)query.OrderBy(t => t.Energy),
                "liveness" => (IQueryable<T>)query.OrderBy(t => t.Liveness),
                "loudness" => (IQueryable<T>)query.OrderBy(t => t.Loudness),
                "speechiness" => (IQueryable<T>)query.OrderBy(t => t.Speechiness),
                "tempo" => (IQueryable<T>)query.OrderBy(t => t.Tempo),
                "instrumentalness" => (IQueryable<T>)query.OrderBy(t => t.Instrumentalness),
                "valence" => (IQueryable<T>)query.OrderBy(t => t.Valence),
                _ => (IQueryable<T>)query.OrderBy(t => t.Name),
            };
        }

        /// <summary>
        /// This adds all of the search criteria given in the URL query parameters, including ranges and names
        /// </summary>
        /// <typeparam name="T">This should always be SpotifyTrackDTO</typeparam>
        /// <param name="query">The sorted, but unfiltered query</param>
        /// <param name="spotifyParameters">The object containing all criteria given by client</param>
        /// <returns>A query that has all the WHERE conditions applied</returns>
        public static IQueryable<T> SpotifyTrackSearchBy<T>(this IQueryable<SpotifyTrackDTO> query, SpotifyParameters spotifyParameters)
        {

            if(spotifyParameters.Name is not null)
                query = query.Where(t => t.Name == spotifyParameters.Name);

            if (spotifyParameters.LowerReleaseDate is not null)
                query = query.Where(t => String.Compare(t.ReleaseDate, spotifyParameters.LowerReleaseDate) > 0);

            if(spotifyParameters.UpperReleaseDate is not null)
                query = query.Where(t => String.Compare(t.ReleaseDate, spotifyParameters.UpperReleaseDate) < 0);

            return (IQueryable<T>) query;
        }

    }
}
