using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace SysAdmin.Json
{
    public static class JSONHelper
    {
        public static string Serializar(object t)
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            serializador.MaxJsonLength = Int32.MaxValue;
            serializador.RegisterConverters(new[] { new JsonTimeSpanConverter() });

            var jsonString = serializador.Serialize(t);

            //Replace Json Date String                                         
            string p = @"\\/Date\(-?(\d+)\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            // trata o NaN do C# que da pau no js
            p = @"NaN";
            matchEvaluator = new MatchEvaluator(ConvertNaNToZero);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            // trata o Infinity do C# que da pau no js ++> Se for infinity é TIM, por que é mais facil o moguerço come alguem do que a TIM pega sinal
            p = @"Infinity";
            matchEvaluator = new MatchEvaluator(ConvertInfinityToZero);
            reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);

            return jsonString;
        }

        public static T Desserializar<T>(string jsonString)
        {
            try
            {
                //alguns otários devem usar um navegador pré-histórico onde a máscara não funciona direito
                jsonString = jsonString.Replace("__/__/____", string.Empty);

                //Replace Json DateTime String                                         
                string p = "\"(\\d{1,2})/(\\d{1,2})/(\\d{4}) - (\\d{1,2}):(\\d{1,2})\"";
                MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateTimeToDateTime);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);

                //Replace Json Date String                                         
                p = "\"(\\d{1,2})/(\\d{1,2})/(\\d{4})\"";
                matchEvaluator = new MatchEvaluator(ConvertJsonDateToDate);
                reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);

                //Replace Json Number String         
                p = "\"(\\-)?(\\d{1,3}\\.)?(\\d{1,3}\\.)?(\\d{1,3}\\.)?(\\-)?\\d{1,15}(\\,\\d{1,10})?\"";
                matchEvaluator = new MatchEvaluator(ConvertJsonNumberToDouble);
                reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);

                JavaScriptSerializer serializador = new JavaScriptSerializer();
                serializador.MaxJsonLength = Int32.MaxValue;
                serializador.RegisterConverters(new[] { new JsonTimeSpanConverter() });

                var dic = serializador.Deserialize<T>(jsonString);

                return dic;
            }
            catch (Exception ex)
            {
                //SendMail(string.Format("Usuário {0} em {1:dd/MM/yyyy HH:mm} página JsonDeserialize<br /><br />JsonString: {2}<br /><br />", HttpContext.Current.User.Identity.Name, DateTime.Now, jsonString) + ex.ToString());
                throw new Exception("Erro ao dessserializar Json no SIGE.Security", ex);
            }
        }

        #region
        private static string ConvertDateToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);

            var grupo = m.Value.Replace(@"\/Date(", string.Empty).Replace(@")\/", string.Empty);

            //long timeInicial = 62135589600000;

            ////// Assim as datas abaixo de 01/01/1970 entram ali
            //if (timeInicial > long.Parse(grupo))
            //    grupo = "-" + grupo;

            //if (m.Groups[1].Value == "62135589600000")
            //    return "";

            dt = dt.AddMilliseconds(long.Parse(grupo));
            dt = dt.ToLocalTime();

            if (dt.Date == DateTime.MinValue)
                result = "";
            else if (dt.Hour == 0 && dt.Minute == 0)
                result = dt.ToString("dd/MM/yyyy");
            else
                result = dt.ToString("dd/MM/yyyy - HH:mm");

            return result;
        }

        private static string ConvertJsonDateToDate(Match m)
        {
            string result = string.Empty;
            DateTime dtBase = new DateTime(1970, 1, 1);

            DateTime dt = DateTime.MinValue;

            if (DateTime.TryParse(m.Groups[0].Value.Replace("\"", string.Empty), out dt))
            {
                dt = dt.ToUniversalTime();
                result = string.Format("\"\\/Date({0})\\/\"", (dt - dtBase).TotalMilliseconds);
            }
            else
            {
                //se for valor inválido retorna ele e deixa o cara se virar com o valor
                result = m.Groups[0].Value;
            }

            return result;
        }

        private static string ConvertJsonDateTimeToDateTime(Match m)
        {
            string result = string.Empty;
            DateTime dtBase = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime dt = DateTime.ParseExact(m.Groups[0].Value.Replace("\"", string.Empty), "dd/MM/yyyy - HH:mm", CultureInfo.InvariantCulture).AddHours(3);

            if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                dt = dt.AddHours(-1);

            result = string.Format("\"\\/Date({0})\\/\"", (dt - dtBase).TotalMilliseconds);

            return result;
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
        #endregion
    }

    internal class JsonTimeSpanConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new[] { typeof(TimeSpan) };
            }
        }


        public override object Deserialize(IDictionary<string, object> dictionary, Type targetType, JavaScriptSerializer javaScriptSerializer)
        {
            TimeSpan interval = TimeSpan.MinValue;
            if (dictionary.ContainsKey("Value"))
            {
                TimeSpan.TryParseExact(dictionary["Value"].ToString(), @"hh\:mm", null, out interval);
            }

            return interval;
        }

        public override IDictionary<string, object> Serialize(Object timeSpan, JavaScriptSerializer javaScriptSerializer)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            if (timeSpan.GetType() == typeof(TimeSpan))
            {
                var timeString = ((TimeSpan)timeSpan).ToString(@"hh\:mm");
                dictionary.Add("Value", timeString);
            }

            return dictionary;
        }
    }

}