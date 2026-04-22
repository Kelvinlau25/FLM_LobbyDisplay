using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Library.Root.Control
{
    public class Convertion<T>
    {
        private static JavaScriptSerializer _ser;

        /// <summary>
        /// Convert List of T into String Format
        /// </summary>
        public static string Serializer(List<T> list)
        {
            _ser = new JavaScriptSerializer();
            string result = _ser.Serialize(list);
            _ser = null;
            return result;
        }

        /// <summary>
        /// Convert string into List of T
        /// </summary>
        public static List<T> Deserializer(string StringFormat)
        {
            _ser = new JavaScriptSerializer();
            List<T> result = _ser.Deserialize<List<T>>(StringFormat);
            _ser = null;
            return result;
        }
    }
}
