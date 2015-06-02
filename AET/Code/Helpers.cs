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
    public class Helpers
    {
        public Helpers()
        {
        }

        private DBAcess _dbAcess;
        public DBAcess dbAcess
        {
            get
            {
                if (_dbAcess == null)
                    _dbAcess = new DBAcess(new MongoUrl("mongodb://aetadmin:64608099@177.55.99.170:27017/aet"));

                return _dbAcess;
            }
        }

        public bool IsObjectId(string Id)
        {
            try
            {
                new ObjectId(Id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Mensagem(string mensagem)
        {
            var page = HttpContext.Current.Handler as Page;

            ScriptManager.
               RegisterStartupScript(
               page,
               page.GetType(),
               "mailError",
               "alert('" + mensagem + "');",
               true);
        }

        public void Mensagem(string mensagem, Page page)
        {
            ScriptManager.
               RegisterStartupScript(
               page,
               page.GetType(),
               "mailError",
               "alert('" + mensagem + "');",
               true);
        }

        /// <summary>
        /// Retorna TRUE caso o CPF seja valido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CPFValido(string value)
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

        /// <summary>
        /// Retorna TRUE caso o email seja valido
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool EmailValido(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(value, ("(?<user>[^@]+)@(?<host>.+)"));
        }
    }

}