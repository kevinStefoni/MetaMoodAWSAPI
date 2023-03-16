namespace MetaMoodAWSAPI.Services
{
    internal static class PageService
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
        
    }
}
