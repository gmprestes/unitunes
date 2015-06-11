using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using Models;

namespace SiteMVC.Helpers
{
    public class TransacaoCartao
    {
        #region Descricao Campos XML Cielo

        //        TAG Tipo Obrig. Tam. Descrição
        //dados-ec.numero N R 1..20 Número de credenciamento da loja com a Cielo.
        //dados-ec.chave AN R 1..100 Chave de acesso da loja atribuída pela Cielo.
        //dados-portador n/a O n/a Nó com os dados do cartão (somente Buy Page Loja).
        //dados-portador.numero N R 16 Número do cartão.
        //dados-portador.validade N R 6
        //Validade do cartão no formato aaaamm. Exemplo: 
        //201212 (dez/2012).
        //dados-portador.indicador N R 1
        //Indicador sobre o envio do Código de segurança: 
        //0 – não informado
        //1 – informado
        //2 – ilegível
        //9 – inexistenteCielo e-Commerce                                                   
        //dados-portador.codigoseguranca
        //N C 3..4 Obrigatório se indicador = 1.
        //dados-portador.nomeportador
        //AN O 0..50 Nome impresso no cartão.
        //dados-pedido n/a R n/a Nó com os dados do pedido.
        //dados-pedido.numero AN R 1..20
        //Número do pedido da loja. Recomenda-se que seja um 
        //valor único por pedido.
        //dados-pedido.valor N R 1..12
        //Valor a ser cobrado pelo pedido (já deve incluir valores 
        //de frete, embrulho e custos extras). Esse valor é o que 
        //será debitado do consumidor.
        //dados-pedido.moeda N R 3
        //Código numérico da moeda na norma ISO 4217. Para o 
        //Real, o código é 986.
        //dados-pedido.data-hora AN R 19 Data hora do pedido (verificar o formato no item 2.2.1).
        //dados-pedido.descricao AN O 0..1024 Descrição do pedido.
        //dados-pedido.idioma AN O 2
        //Idioma do pedido: PT (português), EN (inglês) ou ES 
        //(espanhol). Com base nessa informação é definida a 
        //língua a ser utilizada Lhtts telas da Cielo. Caso não seja 
        //enviado, o sistema assumirá “PT”.
        //dados-pedido.softdescriptor
        //AN O 0..13
        //Texto de até 13 caracteres que será exibido na fatura do 
        //portador, após o nome do Estabelecimento Comercial.
        //forma-pagamento n/a R n/a Nó com a forma de pagamento.
        //forma-pagamento.bandeira  AN R n/a
        //Nome da bandeira (minúsculo): 
        //“visa”
        //“mastercard”
        //“diners”
        //“discover”
        //“elo”
        //“amex”.
        //forma-pagamento.produto AN R 1
        //Código do produto: 
        //1 – Crédito à Vista.
        //2 – Parcelado loja.
        //3 – Parcelado administradora.
        //A – Débito.
        //forma-pagamento.parcelas N R 1..3
        //Número de parcelas. Para crédito à vista ou débito, 
        //utilizar “1”.
        //url-retorno AN C 1..1024
        //URL da página de retorno. É para essa página que a Cielo 
        //vai direcionar o browser ao fim da autenticação ou da
        //autorização. Não é obrigatório apenas para autorização 
        //direta.
        //autorizar N R 1
        //Indicador de autorização: 
        //0 – Não autorizar (somente autenticar).
        //1 – Autorizar somente se autenticada.
        //2 – Autorizar autenticada e não autenticada.
        //3 – Autorizar sem passar por autenticação (somente para 
        //crédito) – também conhecida como “Autorização Direta”.
        //Obs.: Para Diners, Discover, Elo e Amex o valor será 
        //sempre “3”, pois estas bandeiras não possuem programa 
        //de autenticação.
        //4 – Transação Recorrente.
        //capturar AN R n/a
        //[true|false]. Define se a transação será 
        //automaticamente capturada caso seja autorizada.
        //campo-livre AN O 0..128 Campo livre disponível para o Estabelecimento.
        //bin N O 6 Seis primeiros números do cartão. 
        //Legenda: 
        // AN (alfanumérico): campo que aceita caracteres e números
        // N: campo que aceita apenas números
        // R: campo requerido ou mandatório
        // O: campo opcional
        // C: campo requerido de acordo com uma condição

        #endregion


        public string GetTemplateTransacao(TransacaoPagamento transacao)
        {
            StringBuilder template = new StringBuilder();
            template.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            template.AppendFormat("<requisicao-transacao id=\"{0}\" versao=\"1.2.0\">", transacao.Id);

            template.Append("<dados-ec>");

            template.Append("<numero>1006993069</numero>");
            template.Append("<chave>25fbb99741c739dd84d7b06ec78c9bac718838630f30b112d033ce2e621b34f3</chave>"); // Chave de testes

            template.Append("</dados-ec>");

            template.Append("<dados-portador>");
            template.AppendFormat("<numero>{0}</numero>", transacao.Cartao.Numero);
            template.AppendFormat("<validade>{0}</validade>", transacao.Cartao.ValidadeAno + transacao.Cartao.ValidadeMes);
            template.Append("<indicador>1</indicador>"); // INDICADOR DO CODIGO DE SEGURNAÇA DO CARTAO (1 = presente)
            template.AppendFormat("<codigo-seguranca>{0}</codigo-seguranca>", transacao.Cartao.CVC);
            template.Append("</dados-portador>");

            template.Append("<dados-pedido>");
            template.AppendFormat("<numero>{0}</numero>", new Random().Next(1, 100));
            template.AppendFormat("<valor>{0}</valor>", string.Format("{0:0.00}", Math.Round(transacao.ValorTransacao, 0)).Replace(",", ""));
            template.Append("<moeda>986</moeda>"); // 986 = REAL
            template.AppendFormat("<data-hora>{0:yyyy-MM-dd}T{0:HH:mm:ss}</data-hora>", transacao.DataTransacao); //EX.: 2011-12-07T11:43:37
            template.AppendFormat("<descricao>Pedido {0}</descricao>", transacao.Id);
            template.AppendFormat("<soft-descriptor>sige</soft-descriptor>"); // DESCRICAO QUE APARECE NA FATURA DO CARTAO JUNTO AO NOME DA LOJA
            template.Append("</dados-pedido>");

            template.Append("<forma-pagamento>");
            template.AppendFormat("<bandeira>{0}</bandeira>", transacao.Cartao.Bandeira); //BANDEIRA SUPORTADA PELA CIELO EM LETRAS MINUSCULAS
            template.AppendFormat("<produto>{0}</produto>", 1); // 2 = Credito loja
            template.AppendFormat("<parcelas>{0}</parcelas>", 1); // Numero de parcelas
            template.Append("</forma-pagamento>");

            template.Append("<autorizar>3</autorizar>"); // Suportados pela maioria dos cartões
            template.Append("<capturar>true</capturar>"); //para obter o retorno da transação na hora TRUE
            template.AppendFormat("<bin>{0}</bin>", transacao.Cartao.Numero.Substring(0, 6));

            template.Append("</requisicao-transacao>");

            return template.ToString();

            # region xml exemplo
            //"<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>" +
            //"<requisicao-transacao id=\"a96ab62a-7956-41ea-b03f-c2e9f612c293\" versao=\"1.2.0\">" +
            //"<dados-ec>" +
            //"<numero>1006993069</numero>" +
            //"<chave>25fbb99741c739dd84d7b06ec78c9bac718838630f30b112d033ce2e621b34f3</chave>" +
            //"</dados-ec>" +
            //"<dados-portador>" +
            //"<numero>4012001038443335</numero>" +
            //"<validade>201805</validade>" +
            //"<indicador>1</indicador>" +
            //"<codigo-seguranca>123</codigo-seguranca>" +
            //"</dados-portador>" +
            //"<dados-pedido>" +
            //"<numero>142536</numero>" +
            //"<valor>100</valor>" +
            //"<moeda>986</moeda>" +
            //"<data-hora>2012-11-10T20:52:34</data-hora>" +
            //"<descricao>a</descricao>" +
            //"</dados-pedido>" +
            //"<forma-pagamento>" +
            //"<bandeira>visa</bandeira>" +
            //"<produto>1</produto>" +
            //"<parcelas>1</parcelas>" +
            //"</forma-pagamento>" +
            //"<url-retorno>null</url-retorno>" +
            //"<autorizar>3</autorizar>" +
            //"<capturar>false</capturar>" +
            //"</requisicao-transacao>";
            #endregion
        }

        public string RealizaTransacao(string xml)
        {

            System.Net.WebRequest req = null;
            System.Net.WebResponse rsp = null;
            try
            {
                req = System.Net.WebRequest.Create("https://qasecommerce.cielo.com.br/servicos/ecommwsec.do"); //teste

                req.Method = "POST";

                req.Headers.Add("POST", "/servicos/ecommwsec.do HTTP/1.1");
                req.ContentType = "application/x-www-form-urlencoded";

                string x = req.Headers.ToString();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(req.GetRequestStream());
                writer.Write("mensagem=" + xml);
                writer.Flush();
                writer.Close();

                rsp = req.GetResponse();

                System.Text.Encoding encoding = System.Text.Encoding.Default;

                Stream dataStream = req.GetRequestStream();
                dataStream = rsp.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, encoding);

                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                reader.Close();
                rsp.Close();

                //return "10378946660000651001";

                if (responseFromServer.Contains("<erro"))
                    return 0.ToString();
                else if (responseFromServer.Contains("<mensagem>Transação autorizada</mensagem>"))
                {
                    if (responseFromServer.Contains("<mensagem>Transacao capturada com sucesso</mensagem>"))
                        return responseFromServer.extractTIDNode();
                    return 2.ToString();
                }
                else
                    return 1.ToString();
            }
            catch (Exception ex)
            {
                return 0.ToString();
            }
        }

    }



}