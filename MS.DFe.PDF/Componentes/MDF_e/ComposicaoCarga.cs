using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

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
            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Padding(ConstantsHelper.PADDING).Text(MDFeResource.COMPOSICAO_CARGA).Bold().FontSize(6);

                var municipios = _inf.InfDoc?.InfMunDescarga;
                if (municipios == null || municipios.Count == 0)
                {
                    column.Item().Padding(ConstantsHelper.PADDING).Text(string.Empty).FontSize(7);
                    return;
                }

                foreach (var mun in municipios)
                {
                    column.Item().Padding(ConstantsHelper.PADDING)
                        .Text($"{MDFeResource.MUNICIPIO}: {mun.XMunDescarga} - {mun.CMunDescarga}")
                        .Bold().FontSize(7);

                    var chaves = new List<string>();
                    if (mun.InfNFe != null)
                    {
                        foreach (var nfe in mun.InfNFe)
                            chaves.Add($"{MDFeResource.NFE}  {(nfe.ChNFe ?? string.Empty).NormalizarChaveNFe().FormataChaveNFe()}");
                    }
                    if (mun.InfCTe != null)
                    {
                        foreach (var cte in mun.InfCTe)
                            chaves.Add($"{MDFeResource.CTE}  {(cte.ChCTe ?? string.Empty).NormalizarChaveCTe().FormataChaveNFe()}");
                    }
                    if (mun.InfMdFeTransps != null)
                    {
                        foreach (var mdfe in mun.InfMdFeTransps)
                            chaves.Add($"{MDFeResource.MDFE}  {(mdfe.ChMDFe ?? string.Empty).NormalizarChaveMDFe().FormataChaveNFe()}");
                    }

                    if (chaves.Count == 0) continue;

                    var metade = (chaves.Count + 1) / 2;
                    column.Item().Padding(ConstantsHelper.PADDING).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            for (var i = 0; i < metade && i < chaves.Count; i++)
                                col.Item().Text(chaves[i]).FontSize(6);
                        });
                        row.RelativeItem().Column(col =>
                        {
                            for (var i = metade; i < chaves.Count; i++)
                                col.Item().Text(chaves[i]).FontSize(6);
                        });
                    });
                }

                if (!string.IsNullOrEmpty(_inf.InfAdic?.InfCpl))
                {
                    column.Item().Padding(ConstantsHelper.PADDING).Column(c =>
                    {
                        c.Item().Text(MDFeResource.OBSERVACOES).Bold().FontSize(6);
                        c.Item().Text(_inf.InfAdic.InfCpl).FontSize(7);
                    });
                }
            });
        }
    }
}
