using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SiteMVC.Helpers
{
    public static class StringExtensions
    {
        public static string FormatCasasNumero(this string num, int casas)
        {
            var aux = string.Empty;
            for (int i = 0; i < casas - num.Length; i++)
            {
                aux += "0";
            }

            return aux + num;
        }

        public static string extractTIDNode(this string xmlResponse)
        {
            try
            {
                string xml = xmlResponse.ToString().Replace("\"", "").Replace("\n", "").Replace("=", "").Replace(" ", "");
                string tidNode = "<tid>";
                string aux = "";
                for (int i = 0, currentIndex = 5; i < xml.Length; ++i, ++currentIndex)
                {
                    aux = "";
                    aux = xml.Substring(i, 5);
                    if (aux.Equals(tidNode))
                        return xml.Substring(currentIndex, 20);
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string WebNameFormat(this string name)
        {
            return name.Trim().RemoveSpecialCharacters().Replace("$", "-").SubstituteChars().Replace(".", "").Replace(",", "").Replace(" ", "").Replace("/", "").Replace("---", "-").Replace("--", "-").ToLower();
        }


        /// <summary>
        /// Method that limits the length of text to a defined length.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="maxLength">The maximum limit of the string to return.</param>
        public static string LimitLength(this string source, int maxLength)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "N/A";
            }

            if (source.Length <= maxLength)
            {
                return source;
            }

            return source.Substring(0, maxLength) + " ...";
        }


        public static string RemoveHtmlAndLimitString(this string source, int maxLength)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            var text = Regex.Replace(source, @"<(.|\n)*?>", string.Empty);

            if (string.IsNullOrEmpty(text))
                return "N/A";

            return text.LimitLength(maxLength);
        }

        public static string RemoveHtmlAndLimitString(this string source, int maxLength, params string[] caracters)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            var text = Regex.Replace(source, @"<(.|\n)*?>", string.Empty);

            foreach (var item in caracters)
            {
                text = text.Replace(item, string.Empty);
            }

            if (string.IsNullOrEmpty(text))
                return "N/A";

            return text.LimitLength(maxLength);
        }


        public static String RemoveSpecialCharacters(this String self)
        {
            if (string.IsNullOrEmpty(self))
                return string.Empty;

            var normalizedString = self.ToLower();

            normalizedString = normalizedString.Replace("-", "999999");
            normalizedString = normalizedString.Replace(",", "888888");
            normalizedString = normalizedString.Replace("/", "777777");
            normalizedString = normalizedString.Replace(".", "666666");
            normalizedString = normalizedString.Replace(" ", "555555");

            // Prepara a tabela de símbolos.
            var symbolTable = new Dictionary<char, char[]>();
            symbolTable.Add('a', new char[] { 'à', 'á', 'ä', 'â', 'ã' });
            symbolTable.Add('c', new char[] { 'ç' });
            symbolTable.Add('e', new char[] { 'è', 'é', 'ë', 'ê' });
            symbolTable.Add('i', new char[] { 'ì', 'í', 'ï', 'î' });
            symbolTable.Add('o', new char[] { 'ò', 'ó', 'ö', 'ô', 'õ' });
            symbolTable.Add('u', new char[] { 'ù', 'ú', 'ü', 'û' });

            // Substitui os símbolos.
            foreach (var key in symbolTable.Keys)
            {
                foreach (var symbol in symbolTable[key])
                {
                    normalizedString = normalizedString.Replace(symbol, key);
                }
            }

            // Remove os outros caracteres especiais.
            normalizedString = Regex.Replace(normalizedString, "[^0-9a-zA-Z._]+?", "");

            return normalizedString.Replace("999999", "-").Replace("888888", "_").Replace("777777", "~").Replace("666666", "^").Replace("555555", "$");
        }

        public static string SubstituteChars(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Replace("$", " ").Replace("_", ",").Replace("~", "/").Replace("^", ".");
        }


        public static string RemoveTags(this string text, bool useTable)
        {

            try
            {
                Regex reImg = new Regex(@"<img\s[^>]*>", RegexOptions.IgnoreCase);
                MatchCollection mc = reImg.Matches(text);

                string img = mc.Count == 0 ? "" : mc[0].Value;
                //img = Regex.Replace(img, @"(<img\b[^>]*?\b)(heigth=""(?:[^""]*)"")", string.Empty);
                //img = Regex.Replace(img, @"(<img\b[^>]*?\b)(width=""(?:[^""]*)"")", string.Empty);
                //img = Regex.Replace(img, @"(<img\b[^>]*?\b)(align=""(?:[^""]*)"")", string.Empty);

                if (img.Contains("align"))
                    img = img.Replace("align=", "");
                if (img.Contains("height"))
                    img = img.Replace("height=", "");
                if (img.Contains("width"))
                    img = img.Replace("width=", "");

                text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);


                if (text.Split(' ').Count() > 0)
                {
                    string texto = "";
                    for (int i = 0; i < text.Split(' ').Count(); i++)
                    {
                        if (!(text.Split(' ')[i].Length > 30))
                            texto += " " + text.Split(' ')[i];
                    }

                    text = texto.TrimStart(' ');
                }

                if (!string.IsNullOrEmpty(img))
                {
                    img = img.Replace("src", "¬");

                    if (img.Contains("¬"))
                    {
                        string image = img.Split('¬')[0] + " width=\"105px\" class=\"newsimage\" src" + img.Split('¬')[1];

                        image = image.Replace("¬", "src");

                        img = image.Contains(".jpg") && !image.Contains("?") ? image : "";
                    }
                }

                if (!img.Contains("http")) img = "";

                int limit = string.IsNullOrEmpty(img) ? 220 : 100;
                string textolimitado = LimitText(text, limit);

                return string.Format("{0}{1}",
                    string.IsNullOrEmpty(img) ? "" : "<span class=\"image-holder\">" + img + "</span>",
                    textolimitado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string LimitText(this string strInput, int max)
        {
            if (string.IsNullOrEmpty(strInput)) return null;

            strInput = strInput.Replace("'", "");

            int offset = 0;
            string text = Regex.Replace(strInput, @"\s{2,}", " ");
            List<string> lines = new List<string>();

            while (offset < strInput.Length)
            {
                int index = text.LastIndexOf(" ", Math.Min(text.Length, offset + max));

                string pontinhos = strInput.Length > 80 ? " ..." : "";
                return text.Substring(offset, (index - offset <= 0 ? text.Length : index) - offset) + pontinhos;
                break;
            }

            return strInput;
        }

        public static string extractDomaint(this string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return null;

                url = url.Contains("url=") ? url.Replace("url=", "¬").Split('¬')[1] : url;

                Match match = Regex.Match(url, @"^(?:\w+://)?([^/?]*)");

                url = match.Groups[1].Value.Replace("www.", "");
                url = url.Split('.').Count() > 2 ? url.Replace(url.Split('.')[0], "") : url;

                return url.TrimStart('.');
            }
            catch
            { }

            return url;
        }

        public static string GetImage(this string text)
        {
            try
            {

                if (string.IsNullOrEmpty(text)) return null;

                Regex reImg = new Regex(@"<img\s[^>]*>", RegexOptions.IgnoreCase);
                MatchCollection mc = reImg.Matches(text);
                string img = mc.Count == 0 ? "" : mc[0].Value;


                img = Regex.Match(img, @"src=\""(.*?)\""", RegexOptions.Singleline).Value;


                return img;
            }
            catch
            {
            }
            return null;

        }

        public static string RemoveAllTags(this string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) return null;

                text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);

                return text;
            }
            catch
            {
            }
            return null;

        }
    }

}
