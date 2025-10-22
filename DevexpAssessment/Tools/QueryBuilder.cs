using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DevexpAssessment.Tools
{
    internal class QueryBuilder
    {
        internal static string Build(Dictionary<string, object?>? parameters)
        {
            if (parameters == null || parameters.Count == 0) return string.Empty;

            var query = string.Join("&",
                parameters
                    .Where(kv => kv.Value != null)
                    .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!.ToString()!)}")
            );

            return $"?{query}";
        }
    }
}
