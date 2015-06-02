using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace GmpsMvcController.Utils
{
    internal static class Extensions
    {
        public static string ToJson(this Object obj)
        {
            var jsonString = JsonHelper.JsonSerializer(obj);

            return jsonString;
        }

        public static DynamicJsonObject FromJson(this string input)
        {


            JavaScriptSerializer serializador = new JavaScriptSerializer();
            //serializador.MaxJsonLength = Int32.MaxValue;

            var x = serializador.Deserialize<Dictionary<string, object>>(input);
            return new DynamicJsonObject(x);
        }

        public static object FromJson(this object input, Type type)
        {
            return JsonHelper.JsonDeserialize(input.ToString(), type);
        }
    }
}
