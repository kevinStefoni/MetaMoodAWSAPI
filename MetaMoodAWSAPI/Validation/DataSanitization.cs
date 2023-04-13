using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaMoodAWSAPI.Validation
{
    internal static class DataSanitization
    {
        /// <summary>
        /// This is an extension method that removes '-' and ';' from the input that will be used as a parameter in an SQL
        /// string. Removing the '-' should prevent condition nullification attacks and otherwise invalidating the remaining part of
        /// a query using comments. Removing ';' should prevent batch attacks. 
        /// <remark>It is still possible to add another condition that is
        /// always true with OR, but that is not a problem, since all the records are shown anyway.
        /// </remark>
        /// </summary>
        /// <param name="input">The input into a SQL query from the search bar</param>
        /// <returns>The sanitized input of a search query</returns>
        public static string SanitizeString(this string input)
        {
            char[] invalidChars = { '-', ';' };
            return input.Trim(invalidChars);
        }
    }
}
