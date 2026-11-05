using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class ComposicaoCarga : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public ComposicaoCarga(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var municipios = _inf.InfDoc?.InfMunDescarga;
            if (municipios == null || municipios.Count == 0)
                return;

            container.PaddingTop(8).Column(column =>
            {
                column.Item().Text("Documentos vinculados").Bold().FontSize(11);

                foreach (var mun in municipios)
                {
                    var documentos = Documentos(mun);
                    if (documentos.Count == 0)
                        continue;

                    var municipio = string.IsNullOrWhiteSpace(mun.XMunDescarga) ? mun.CMunDescarga : mun.XMunDescarga;
                    if (!string.IsNullOrWhiteSpace(mun.CMunDescarga))
                        municipio = $"{municipio} - {mun.CMunDescarga}";

                    column.Item().PaddingTop(3).Text(municipio).Bold().FontSize(8);
                    foreach (var documento in documentos)
                        column.Item().Text(documento).FontSize(8);
                }
            });
        }

        private static List<string> Documentos(MDFeInfMunDescarga mun)
        {
            var documentos = new List<string>();
            if (mun.InfCTe != null)
            {
                foreach (var cte in mun.InfCTe)
                    documentos.Add($"CT-e  {(cte.ChCTe ?? string.Empty).NormalizarChaveCTe().FormataChaveNFe()}");
            }
            if (mun.InfNFe != null)
            {
                foreach (var nfe in mun.InfNFe)
                    documentos.Add($"NF-e  {(nfe.ChNFe ?? string.Empty).NormalizarChaveNFe().FormataChaveNFe()}");
            }
            if (mun.InfMdFeTransps != null)
            {
                foreach (var mdfe in mun.InfMdFeTransps)
                    documentos.Add($"MDF-e  {(mdfe.ChMDFe ?? string.Empty).NormalizarChaveMDFe().FormataChaveNFe()}");
            }
            return documentos;
        }
    }
}
