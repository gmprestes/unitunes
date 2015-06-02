using AET.Code;
using GmpsMvcController;
using GmpsMvcController.Controller;
using Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OpenXMLExcel.ExcelUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AET.Controllers
{
    public class ControllerRelatorio : GmpsController
    {
        private Helpers _helpers;
        public ControllerRelatorio()
        {
            this._helpers = new Helpers();
        }

        [HttpGetMethod]
        public void RelatorioAlunosDia(string id)
        {

            id = "5492fddfc58e7f05e0d2f60e"; // 2015/1
            var itens = this._helpers.dbAcess._repositoryAuxilio.All(q => q.SemestreId == id);

            var data = new ExcelData();

            foreach (var iten in itens)
            {
                try
                {
                    var instituicao = this._helpers.dbAcess._repositoryInstituicao.GetSingle(q => q.Id == iten.InstituicaoId);
                    var pessoa = this._helpers.dbAcess._repositoryPessoa.GetSingle(q => q.UserId == iten.UserId);

                    if (pessoa != null)
                    {
                        var auxilio = this._helpers.dbAcess._repositoryAuxilio.GetSingle(q => q.SemestreId == id && q.UserId == iten.UserId);

                        string segIda, segVolta, tercaIda, tercaVolta, quartaIda, quartaVolta, quintaIda, quintaVolta, sextaIda, sextaVolta;
                        segIda = segVolta = tercaIda = tercaVolta = quartaIda = quartaVolta = quintaIda = quintaVolta = sextaIda = sextaVolta = "Não";
                        if (auxilio.Disciplinas != null)
                            foreach (var item in auxilio.Disciplinas.Where(q => q.Turno == "Noite"))
                            {
                                if (item.TransporteIda)
                                {
                                    if ("Não" == segIda && item.DiasSemana[0])
                                        segIda = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == tercaIda && item.DiasSemana[1])
                                        tercaIda = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == quartaIda && item.DiasSemana[2])
                                        quartaIda = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == quintaIda && item.DiasSemana[3])
                                        quintaIda = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == sextaIda && item.DiasSemana[4])
                                        sextaIda = item.EAD ? "EAD" : "Sim";
                                }

                                if (item.TransporteVolta)
                                {
                                    if ("Não" == segVolta && item.DiasSemana[0])
                                        segVolta = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == tercaVolta && item.DiasSemana[1])
                                        tercaVolta = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == quartaVolta && item.DiasSemana[2])
                                        quartaVolta = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == quintaVolta && item.DiasSemana[3])
                                        quintaVolta = item.EAD ? "EAD" : "Sim";

                                    if ("Não" == sextaVolta && item.DiasSemana[4])
                                        sextaVolta = item.EAD ? "EAD" : "Sim";
                                }
                            }

                        data.DataRows.Add(new List<object>()
                        {
                            pessoa.Codigo,
                            pessoa.Email,
                            pessoa.Nome + " "+pessoa.Sobrenome,
                            pessoa.CPF,
                            String.IsNullOrEmpty(pessoa.RG)?"Não informado":pessoa.RG,
                            segIda,
                            segVolta,
                            tercaIda,
                            tercaVolta,
                            quartaIda,
                            quartaVolta,
                            quintaIda,
                            quintaVolta,
                            sextaIda,
                            sextaVolta,
                            string.IsNullOrEmpty(iten.Curso) ? "Não informado" : iten.Curso,
                            instituicao == null ? "Não informado" : instituicao.Nome,
                            auxilio != null ? (!string.IsNullOrEmpty(auxilio.NumMatricula) ? auxilio.NumMatricula : "") : ""
                        });
                    }
                }
                catch
                {
                }
            }


            data.Headers = new List<ExcelHeader>()
                    {
                        new ExcelHeader(){Header = "Codigo"},
                        new ExcelHeader(){Header = "Email"},
                        new ExcelHeader(){Header = "Nome"},
                        new ExcelHeader(){Header = "CPF"},
                        new ExcelHeader(){Header = "RG"},
                        
                        new ExcelHeader(){Header = "Segunda Ida"},
                        new ExcelHeader(){Header = "Segunda Volta"},
                        new ExcelHeader(){Header = "Terça Ida"},
                        new ExcelHeader(){Header = "Terça Volta"},
                        new ExcelHeader(){Header = "Quarta Ida"},
                        new ExcelHeader(){Header = "Quarta Volta"},
                        new ExcelHeader(){Header = "Quinta Ida"},
                        new ExcelHeader(){Header = "Quinta Volta"},
                        new ExcelHeader(){Header = "Sexta Ida"},
                        new ExcelHeader(){Header = "Sexta Volta"},

                        new ExcelHeader(){Header = "Curso"},
                        new ExcelHeader(){Header = "Instituição"},
                        new ExcelHeader(){Header = "Nº Matricula"}
                    };

            var writer = new ExcelWriter();
            var file = writer.GenerateExcel(data);

            this.Response.Clear();
            this.Response.AddHeader("Content-Disposition", "application;filename=reportalunosdiatransporte.xlsx");
            this.Response.AddHeader("Content-Length", file.Length.ToString());
            this.Response.BinaryWrite(file);
            //this.Response.Flush();

        }

    }
}