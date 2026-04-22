using System.Collections.Generic;
using System.Text.Json;

namespace Library.Root.Control
{
    /// <summary>
    /// JSON convertion helper.
    ///
    /// NOTE (.NET 8 migration): the original Web Forms version used
    /// <c>System.Web.Script.Serialization.JavaScriptSerializer</c>, which is
    /// not available on .NET 8. This implementation now uses
    /// <c>System.Text.Json</c>. The on-the-wire JSON shape is compatible for
    /// plain DTOs (property names use camelCase to match the legacy output).
    /// </summary>
    public class Convertion<T>
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Convert List of T into String Format
        /// </summary>
        public static string Serializer(List<T> list)
        {
            return JsonSerializer.Serialize(list, _options);
        }

        /// <summary>
        /// Convert string into List of T
        /// </summary>
        public static List<T> Deserializer(string StringFormat)
        {
            return JsonSerializer.Deserialize<List<T>>(StringFormat, _options);
        }
    }
}
