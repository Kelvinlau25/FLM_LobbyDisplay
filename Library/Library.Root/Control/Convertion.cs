using System.Collections.Generic;
using System.Text.Json;

namespace Library.Root.Control
{
    /// <summary>
    /// JSON serialization helpers (net8.0 migration — JavaScriptSerializer replaced with System.Text.Json)
    /// </summary>
    public class Convertion<T>
    {
        /// <summary>
        /// Convert List of T into String Format
        /// </summary>
        public static string Serializer(List<T> list)
        {
            return JsonSerializer.Serialize(list);
        }

        /// <summary>
        /// Convert string into List of T
        /// </summary>
        public static List<T> Deserializer(string StringFormat)
        {
            return JsonSerializer.Deserialize<List<T>>(StringFormat);
        }
    }
}
