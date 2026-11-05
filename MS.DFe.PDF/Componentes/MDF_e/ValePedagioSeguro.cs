using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class ValePedagioSeguro : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public ValePedagioSeguro(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var rodo = _inf.InfModal?.Modal as MDFeRodo;
            var disps = rodo?.infANTT?.valePed?.Disp ?? rodo?.ValePed?.Disp;

            var responsaveis = new List<string>();
            var fornecedores = new List<string>();
            var comprovantes = new List<string>();

            if (disps != null)
            {
                foreach (var disp in disps)
                {
                    var responsavel = !string.IsNullOrEmpty(disp.CPFPg) ? disp.CPFPg : disp.CNPJPg;
                    responsaveis.Add((responsavel ?? string.Empty).FormataCNPJCPF());
                    fornecedores.Add((disp.CNPJForn ?? string.Empty).FormataCNPJCPF());
                    comprovantes.Add(disp.NCompra ?? string.Empty);
                }
            }

            container.PaddingTop(8).Column(column =>
            {
                column.Item().Text("Vale Pedágio").Bold().FontSize(11);
                column.Item().PaddingTop(3).Row(row =>
                {
                    row.RelativeItem().Text("Responsável CNPJ").FontSize(8);
                    row.RelativeItem().Text("Fornecedor CNPJ").FontSize(8);
                    row.RelativeItem().Text("Nº Comprovante").FontSize(8);
                });

                if (responsaveis.Count == 0)
                {
                    column.Item().PaddingTop(1).Text(" ").FontSize(8);
                    return;
                }

                for (var i = 0; i < responsaveis.Count; i++)
                {
                    var indice = i;
                    column.Item().PaddingTop(1).Row(row =>
                    {
                        row.RelativeItem().Text(responsaveis[indice]).FontSize(8);
                        row.RelativeItem().Text(fornecedores[indice]).FontSize(8);
                        row.RelativeItem().Text(comprovantes[indice]).FontSize(8);
                    });
                }
            });
        }
    }
}
