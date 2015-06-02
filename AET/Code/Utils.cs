using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using MongoDB.Bson;
using System.Web.UI;

namespace AET.Code
{
    public static class Utils
    {
      

        public static bool ValidaCPF(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            value = value.Trim();
            value = value.Replace(".", "").Replace("-", "");

            if (value.Length != 11)
                return false;

            tempCpf = value.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return value.EndsWith(digito);
        }

        public static bool ValidaEmail(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(value, ("(?<user>[^@]+)@(?<host>.+)"));
        }


        public static string CompleteLength(string input, string caracter, int length, bool left)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            for (int i = input.Length; i < length; i++)
                input = (left ? caracter : "") + input + (left ? "" : caracter);

            return input;
        }


    }
}