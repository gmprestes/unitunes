using AET.Code;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace AET.Ajax
{
    /// <summary>
    /// Summary description for uploadarquivos
    /// </summary>
    public class uploadarquivos : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var metodo = string.Empty;
            var route = context.Request.Url.PathAndQuery;

            if (!string.IsNullOrEmpty(route))
                metodo = route.Split('/')[2].Split('?')[0].ToLower();

            var helpers = new Helpers();

            if (metodo == "savearquivoperfil")
            {
                try
                {

                    foreach (string file in context.Request.Files)
                    {
                        HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                        if (hpf.ContentType == "image/jpeg" || hpf.ContentType == "application/pdf")
                        {
                            string FileName = string.Empty;
                            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                            {
                                string[] files = hpf.FileName.Split(new char[] { '\\' });
                                FileName = files[files.Length - 1];
                            }
                            else
                            {
                                FileName = hpf.FileName;
                            }
                            if (hpf.ContentLength == 0)
                                continue;

                            byte[] buffer = new byte[hpf.ContentLength];
                            hpf.InputStream.Read(buffer, 0, hpf.ContentLength);

                            string tipo = string.Empty;
                            if (context.Request.QueryString.ToString().Contains("identidade"))
                                tipo = TipoArquivo.ComprovanteIdentidadePerfil.ToString();
                            else if (context.Request.QueryString.ToString().Contains("eleitor"))
                                tipo = TipoArquivo.ComprovanteTituloEleitor.ToString();
                            else if (context.Request.QueryString.ToString().Contains("cpf"))
                                tipo = TipoArquivo.ComprovanteCPF.ToString();
                            else if (context.Request.QueryString.ToString().Contains("certidaonascimento"))
                                tipo = TipoArquivo.ComprovanteCertidaoNascimento.ToString();
                            else
                                tipo = TipoArquivo.ComprovanteEnderecoPerfil.ToString();

                            var user = Membership.GetUser();

                            var arquivo = helpers.dbAcess._repositoryArquivo.GetSingle(q => q.UserId == user.ProviderUserKey.ToString() & q.TipoArquivo == tipo);
                            if (arquivo == null)
                                arquivo = new DtoArquivo();

                            arquivo.TipoArquivo = tipo;
                            arquivo.Nome = FileName;
                            arquivo.Tamanho = buffer.Length / 1048576;
                            arquivo.Aceito = arquivo.Verificado = false;


                            arquivo.File = buffer;
                            arquivo.ExtencaoArquivo = hpf.ContentType;
                            arquivo.DataUpload = DateTime.Now;
                            arquivo.Verificado = false;
                            arquivo.UserId = user.ProviderUserKey.ToString();

                            if (string.IsNullOrEmpty(arquivo.Id))
                                helpers.dbAcess._repositoryArquivo.Add(arquivo);
                            else
                                helpers.dbAcess._repositoryArquivo.Update(arquivo);

                            context.Response.Write(new JavaScriptSerializer().Serialize(true));
                        }
                        else
                            context.Response.Write(new JavaScriptSerializer().Serialize("O arquivo deve ser do tipo .jpeg ou .pdf"));

                    }
                }
                catch
                {
                    return;
                }

            }
            else if (metodo == "savearquivosemestre")
            {
                try
                {
                    foreach (string file in context.Request.Files)
                    {
                        HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                        if (hpf.ContentType == "image/jpeg" || hpf.ContentType == "application/pdf")
                        {
                            string FileName = string.Empty;
                            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                            {
                                string[] files = hpf.FileName.Split(new char[] { '\\' });
                                FileName = files[files.Length - 1];
                            }
                            else
                            {
                                FileName = hpf.FileName;
                            }
                            if (hpf.ContentLength == 0)
                                continue;

                            byte[] buffer = new byte[hpf.ContentLength];
                            hpf.InputStream.Read(buffer, 0, hpf.ContentLength);

                            var semestreid = context.Request.QueryString["semestreid"].ToString();

                            string tipo = string.Empty;
                            if (context.Request.QueryString.ToString().Contains("matricula"))
                                tipo = TipoArquivo.ComprovanteMatriculaSemestre.ToString();
                            else
                                tipo = TipoArquivo.ComprovanteNotasUltimoSemestre.ToString();

                            var user = Membership.GetUser();

                            var auxilio = helpers.dbAcess._repositoryAuxilio.GetSingle(q => q.UserId == user.ProviderUserKey.ToString() && q.SemestreId == semestreid);
                            if (auxilio == null)
                            {
                                auxilio = new DtoAuxilio();
                                auxilio.UserId = user.ProviderUserKey.ToString();
                                auxilio.SemestreId = semestreid;

                                helpers.dbAcess._repositoryAuxilio.Add(auxilio);
                            }

                            var arquivo = helpers.dbAcess._repositoryArquivo.GetSingle(q => q.FKId == auxilio.Id && q.UserId == user.ProviderUserKey.ToString() & q.TipoArquivo == tipo);
                            if (arquivo == null)
                                arquivo = new DtoArquivo();

                            arquivo.TipoArquivo = tipo;
                            arquivo.Nome = FileName;
                            arquivo.Tamanho = buffer.Length / 1048576;
                            arquivo.Aceito = arquivo.Verificado = false;


                            arquivo.File = buffer;
                            arquivo.ExtencaoArquivo = hpf.ContentType;
                            arquivo.DataUpload = DateTime.Now;
                            arquivo.Verificado = false;
                            arquivo.UserId = user.ProviderUserKey.ToString();
                            arquivo.FKId = auxilio.Id;

                            if (string.IsNullOrEmpty(arquivo.Id))
                                helpers.dbAcess._repositoryArquivo.Add(arquivo);
                            else
                                helpers.dbAcess._repositoryArquivo.Update(arquivo);

                            context.Response.Write(new JavaScriptSerializer().Serialize(true));
                        }
                        else
                            context.Response.Write(new JavaScriptSerializer().Serialize("O arquivo deve ser do tipo .jpeg ou .pdf"));

                    }
                }
                catch
                {
                    return;
                }

            }
            else if (metodo == "savearquivosemestreassociado")
            {
                try
                {
                    foreach (string file in context.Request.Files)
                    {
                        HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                        if (hpf.ContentType == "image/jpeg" || hpf.ContentType == "application/pdf")
                        {
                            string FileName = string.Empty;
                            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                            {
                                string[] files = hpf.FileName.Split(new char[] { '\\' });
                                FileName = files[files.Length - 1];
                            }
                            else
                            {
                                FileName = hpf.FileName;
                            }
                            if (hpf.ContentLength == 0)
                                continue;

                            byte[] buffer = new byte[hpf.ContentLength];
                            hpf.InputStream.Read(buffer, 0, hpf.ContentLength);

                            var semestreid = context.Request.QueryString["semestreid"].ToString();
                            var pessoaid = context.Request.QueryString["pessoaid"].ToString();


                            string tipo = string.Empty;
                            if (context.Request.QueryString.ToString().Contains("matricula"))
                                tipo = TipoArquivo.ComprovanteMatriculaSemestre.ToString();
                            else
                                tipo = TipoArquivo.ComprovanteNotasUltimoSemestre.ToString();

                            var pessoa = helpers.dbAcess._repositoryPessoa.GetById(pessoaid);

                            var auxilio = helpers.dbAcess._repositoryAuxilio.GetSingle(q => q.UserId == pessoa.UserId && q.SemestreId == semestreid);
                            if (auxilio == null)
                            {
                                auxilio = new DtoAuxilio();
                                auxilio.UserId = pessoa.UserId;
                                auxilio.SemestreId = semestreid;

                                helpers.dbAcess._repositoryAuxilio.Add(auxilio);
                            }

                            var arquivo = helpers.dbAcess._repositoryArquivo.GetSingle(q => q.FKId == auxilio.Id && q.UserId == pessoa.UserId & q.TipoArquivo == tipo);
                            if (arquivo == null)
                                arquivo = new DtoArquivo();

                            arquivo.TipoArquivo = tipo;
                            arquivo.Nome = FileName;
                            arquivo.Tamanho = buffer.Length / 1048576;
                            arquivo.Aceito = arquivo.Verificado = false;


                            arquivo.File = buffer;
                            arquivo.ExtencaoArquivo = hpf.ContentType;
                            arquivo.DataUpload = DateTime.Now;
                            arquivo.Verificado = false;
                            arquivo.UserId = pessoa.UserId;
                            arquivo.FKId = auxilio.Id;

                            if (string.IsNullOrEmpty(arquivo.Id))
                                helpers.dbAcess._repositoryArquivo.Add(arquivo);
                            else
                                helpers.dbAcess._repositoryArquivo.Update(arquivo);

                            context.Response.Write(new JavaScriptSerializer().Serialize(true));
                        }
                        else
                            context.Response.Write(new JavaScriptSerializer().Serialize("O arquivo deve ser do tipo .jpeg ou .pdf"));

                    }
                }
                catch
                {
                    return;
                }

            }
            else if (metodo == "getfile")
            {
                var id = context.Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    var file = helpers.dbAcess._repositoryArquivo.GetById(id);

                    context.Response.Clear();
                    context.Response.AddHeader("Content-Disposition", string.Format("application;filename={0}", file.Nome));
                    context.Response.AddHeader("Content-Length", file.File.Length.ToString());
                    context.Response.Flush();
                    context.Response.BinaryWrite(file.File);
                    context.Response.End();
                }
            }
            if (metodo == "savearquivoassociado")
            {
                try
                {
                    foreach (string file in context.Request.Files)
                    {
                        HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                        if (hpf.ContentType == "image/jpeg" || hpf.ContentType == "application/pdf")
                        {
                            string FileName = string.Empty;
                            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                            {
                                string[] files = hpf.FileName.Split(new char[] { '\\' });
                                FileName = files[files.Length - 1];
                            }
                            else
                            {
                                FileName = hpf.FileName;
                            }
                            if (hpf.ContentLength == 0)
                                continue;

                            byte[] buffer = new byte[hpf.ContentLength];
                            hpf.InputStream.Read(buffer, 0, hpf.ContentLength);

                            string tipo = string.Empty;
                            if (context.Request.QueryString.ToString().Contains("identidade"))
                                tipo = TipoArquivo.ComprovanteIdentidadePerfil.ToString();
                            else if (context.Request.QueryString.ToString().Contains("eleitor"))
                                tipo = TipoArquivo.ComprovanteTituloEleitor.ToString();
                            else if (context.Request.QueryString.ToString().Contains("cpf"))
                                tipo = TipoArquivo.ComprovanteCPF.ToString();
                            else if (context.Request.QueryString.ToString().Contains("certidaonascimento"))
                                tipo = TipoArquivo.ComprovanteCertidaoNascimento.ToString();
                            else
                                tipo = TipoArquivo.ComprovanteEnderecoPerfil.ToString();

                            var pessoa = new DtoPessoa();

                            if (!string.IsNullOrEmpty(context.Request.QueryString["id"]) && helpers.IsObjectId(context.Request.QueryString["id"]))
                                pessoa = helpers.dbAcess._repositoryPessoa.GetById(context.Request.QueryString["id"]);
                            else
                                context.Response.Write(new JavaScriptSerializer().Serialize(false));

                            var arquivo = helpers.dbAcess._repositoryArquivo.GetSingle(q => q.UserId == pessoa.UserId & q.TipoArquivo == tipo);
                            if (arquivo == null)
                                arquivo = new DtoArquivo();

                            arquivo.TipoArquivo = tipo;
                            arquivo.Nome = FileName;
                            arquivo.Tamanho = buffer.Length / 1048576;
                            arquivo.Aceito = arquivo.Verificado = false;

                            arquivo.File = buffer;
                            arquivo.ExtencaoArquivo = hpf.ContentType;
                            arquivo.DataUpload = DateTime.Now;
                            arquivo.Verificado = false;
                            arquivo.UserId = pessoa.UserId;

                            if (string.IsNullOrEmpty(arquivo.Id))
                                helpers.dbAcess._repositoryArquivo.Add(arquivo);
                            else
                                helpers.dbAcess._repositoryArquivo.Update(arquivo);

                            context.Response.Write(new JavaScriptSerializer().Serialize(true));
                        }
                        else
                            context.Response.Write(new JavaScriptSerializer().Serialize("O arquivo deve ser do tipo .jpeg ou .pdf"));

                    }
                }
                catch
                {
                    return;
                }

            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}