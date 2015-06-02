using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace GmpsMvcController.Utils
{
    internal class JsonHelper
    {
        public static string JsonSerializer(object t)
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            serializador.MaxJsonLength = Int32.MaxValue;

            var jsonString = serializador.Serialize(t);

            ////Replace Json DateTime String                                         
            //string p = @"\\/Date\(-?(\d+)+\)\\/";
            //MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateTimeToDateTimeString);
            //Regex reg = new Regex(p);
            //jsonString = reg.Replace(jsonString, matchEvaluator);

            //Replace Json Date String                                         
            string p = @"\\/Date\(-?(\d+)+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            // trata o NaN do C# que da pau no js
            p = @"NaN";
            matchEvaluator = new MatchEvaluator(ConvertNaNToZero);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            // trata o Infinity do C# que da pau no js
            p = @"Infinity";
            matchEvaluator = new MatchEvaluator(ConvertInfinityToZero);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            return jsonString;
        }


        public static T JsonDeserialize<T>(string jsonString)
        {
            //Replace Json DateTime String                                         
            string p = "\"(\\d{1,2})/(\\d{1,2})/(\\d{4}) - (\\d{1,2}):(\\d{1,2})\"";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateTimeStringToJsonDateTime);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            //Replace Json Date String                                         
            p = "\"(-)?(\\d{1,2})/(\\d{1,2})/(\\d{4})\"";
            matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);


            //Replace Json Number String         
            p = "\"(\\d{1,3}\\.)?(\\d{1,3}\\.)?(\\d{1,3}\\.)?\\d{1,15}(\\,\\d{1,10})?\"";

            matchEvaluator = new MatchEvaluator(ConvertJsonNumberToDouble);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            //serializador.MaxJsonLength = Int32.MaxValue;

            return serializador.Deserialize<T>(jsonString);
        }

        public static object JsonDeserialize(string jsonString, Type typeTarget)
        {
            //Replace Json DateTime String                                         
            string p = "\"(\\d{1,2})/(\\d{1,2})/(\\d{4}) - (\\d{1,2}):(\\d{1,2})\"";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateTimeStringToJsonDateTime);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            //Replace Json Date String                                         
            p = "\"(-)?(\\d{1,2})/(\\d{1,2})/(\\d{4})\"";
            matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);


            //Replace Json Number String         
            p = "\"(\\-)?(\\d{1,3}\\.)?(\\d{1,3}\\.)?(\\d{1,3}\\.)?(\\-)?\\d{1,15}(\\,\\d{1,10})?\"";
            matchEvaluator = new MatchEvaluator(ConvertJsonNumberToDouble);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            //serializador.MaxJsonLength = Int32.MaxValue;

            return serializador.Deserialize(jsonString, typeTarget);
        }

        //public static Dictionary<string, string> JsonDeserialize(string jsonString)
        //{
        //    JavaScriptSerializer serializador = new JavaScriptSerializer();
        //    return serializador.DeserializeObject(jsonString);
        //}


        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);

            // Assim as datas abaixo de 01/01/1970 
            if (m.Groups[1].Value == "62135589600000")
                return "";

            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();

            if (dt.Hour == 0 && dt.Minute == 0)
                result = dt.ToString("dd/MM/yyyy");
            else
                result = dt.ToString("dd/MM/yyyy - HH:mm");

            return result;
        }

        private static string ConvertJsonDateTimeToDateTimeString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);

            // Assim as datas abaixo de 01/01/1970 - 00:00 
            if (m.Groups[1].Value == "62135589600000")
                return "";

            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("dd/MM/yyyy - HH:mm");
            return result;
        }

        /// <summary>
        /// Convert DateTime String as Json Time
        /// </summary>
        private static string ConvertDateTimeStringToJsonDateTime(Match m)
        {
            string result = string.Empty;
            DateTime dtBase = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime dt = DateTime.ParseExact(m.Groups[0].Value.Replace("\"", string.Empty), "dd/MM/yyyy - HH:mm", CultureInfo.InvariantCulture).AddHours(3);

            result = string.Format("\"\\/Date({0})\\/\"", (dt - dtBase).TotalMilliseconds);

            return result;
        }


        /// <summary>
        /// Convert Date String as Json Time
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dtBase = new DateTime(1970, 1, 1);

            DateTime dt = DateTime.MinValue;

            if (DateTime.TryParse(m.Groups[0].Value.Replace("\"", string.Empty), out dt))
            {
                dt = dt.AddHours(3);
                result = string.Format("\"\\/Date({0})\\/\"", (dt - dtBase).TotalMilliseconds);
            }
            else
            {
                //se for valor inválido retorna ele e deixa o cara se virar com o valor
                result = m.Groups[0].Value;
            }

            return result;
        }

        private static string ConvertDoubleToJsonNumber(Match m)
        {
            var mathString = m.Groups[0].Value;
            mathString = mathString.Replace(".", ",");
            return "\"" + mathString + "\"";


        }

        private static string ConvertJsonNumberToDouble(Match m)
        {
            var valor = 0d;
            var mathString = m.Groups[0].Value;

            if (mathString.Contains('.') && mathString.Contains(",") == false)
            {
                if (double.TryParse(mathString.Replace("\"", ""), out valor))
                    return mathString;
            }
            else
            {
                var aux = mathString.Replace("\"", "").Replace(".", "").Replace(",", ".");
                if (double.TryParse(aux, out valor))
                    return mathString.Replace(".", "").Replace(",", ".");
            }

            return m.Groups[0].Value;
        }

        private static string ConvertNaNToZero(Match m)
        {
            var mathString = m.Groups[0].Value;
            mathString = mathString.Replace("NaN", "0");
            return mathString;
        }

        private static string ConvertInfinityToZero(Match m)
        {
            var mathString = m.Groups[0].Value;
            mathString = mathString.Replace("Infinity", "0");
            return mathString;
        }
    }
}
