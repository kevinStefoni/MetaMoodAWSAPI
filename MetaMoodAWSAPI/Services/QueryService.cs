using MetaMoodAWSAPI.DTOs;
using MetaMoodAWSAPI.QueryParameterModels;
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
                "Name" => (IQueryable<T>)query.OrderBy(t => t.Name),
                "ReleaseDate" => (IQueryable<T>)query.OrderBy(t => t.ReleaseDate),
                "Popularity" => (IQueryable<T>)query.OrderBy(t => t.Popularity),
                "Acousticness" => (IQueryable<T>)query.OrderBy(t => t.Acousticness),
                "Danceability" => (IQueryable<T>)query.OrderBy(t => t.Danceability),
                "Energy" => (IQueryable<T>)query.OrderBy(t => t.Energy),
                "Liveness" => (IQueryable<T>)query.OrderBy(t => t.Liveness),
                "Loudness" => (IQueryable<T>)query.OrderBy(t => t.Loudness),
                "Speechiness" => (IQueryable<T>)query.OrderBy(t => t.Speechiness),
                "Tempo" => (IQueryable<T>)query.OrderBy(t => t.Tempo),
                "Instrumentalness" => (IQueryable<T>)query.OrderBy(t => t.Instrumentalness),
                "Valence" => (IQueryable<T>)query.OrderBy(t => t.Valence),
                _ => (IQueryable<T>)query.OrderBy(t => t.Name),
            };
        }

        /// <summary>
        /// This extension method adds all of the search criteria given in the URL query parameters, including ranges and names
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

            if (spotifyParameters.LowerPopularity is not null)
                query = query.Where(t => t.Popularity > spotifyParameters.LowerPopularity);

            if(spotifyParameters.UpperPopularity is not null)
                query = query.Where(t => t.Popularity < spotifyParameters.UpperPopularity);

            if (spotifyParameters.LowerAcousticness is not null)
                query = query.Where(t => t.Acousticness > spotifyParameters.LowerAcousticness);

            if (spotifyParameters.UpperAcousticness is not null)
                query = query.Where(t => t.Acousticness < spotifyParameters.UpperAcousticness);

            if(spotifyParameters.LowerDanceability is not null)
                query = query.Where(t => t.Danceability > spotifyParameters.LowerDanceability);

            if(spotifyParameters.UpperDanceability is not null)
                query = query.Where(t => t.Danceability < spotifyParameters.UpperDanceability);

            if(spotifyParameters.LowerEnergy is not null)
                query = query.Where(t => t.Energy > spotifyParameters.LowerEnergy);

            if(spotifyParameters.UpperEnergy is not null)
                query = query.Where(t => t.Energy < spotifyParameters.UpperEnergy);

            if(spotifyParameters.LowerLiveness is not null)
                query = query.Where(t => t.Liveness > spotifyParameters.LowerLiveness);

            if(spotifyParameters.UpperLiveness is not null)
                query = query.Where(t => t.Liveness < spotifyParameters.UpperLiveness);

            if(spotifyParameters.LowerLoudness is not null)
                query = query.Where(t => t.Loudness  > spotifyParameters.LowerLoudness);

            if(spotifyParameters.UpperLoudness is not null)
                query = query.Where(t => t.Loudness < spotifyParameters.UpperLoudness);

            if(spotifyParameters.LowerSpeechiness is not null)
                query = query.Where(t => t.Speechiness > spotifyParameters.LowerSpeechiness);

            if(spotifyParameters.UpperSpeechiness is not null)
                query = query.Where(t => t.Speechiness < spotifyParameters.UpperSpeechiness);
            
            if(spotifyParameters.LowerTempo is not null)
                query = query.Where(t => t.Tempo > spotifyParameters.LowerTempo);

            if(spotifyParameters.UpperTempo is not null)
                query = query.Where(t => t.Tempo < spotifyParameters.UpperTempo);

            if(spotifyParameters.LowerInstrumentalness is not null)
                query = query.Where(t => t.Instrumentalness > spotifyParameters.LowerInstrumentalness);

            if(spotifyParameters.UpperInstrumentalness is not null)
                query = query.Where(t => t.Instrumentalness < spotifyParameters.UpperInstrumentalness);

            if(spotifyParameters.LowerValence is not null)
                query = query.Where(t => t.Valence > spotifyParameters.LowerValence);

            if(spotifyParameters.UpperValence is not null)
                query = query.Where(t => t.Valence  < spotifyParameters.UpperValence);

            return (IQueryable<T>) query;
        }

    }
}
