using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Cobranca;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Linq;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    public class FaturaRetrato : IComponent
    {
        private readonly cobr _cobr;

        public FaturaRetrato(cobr cobr)
        {
            _cobr = cobr;
        }

        public static bool TemConteudo(cobr cobr)
        {
            if (cobr == null)
                return false;
            return TemFatura(cobr.fat) || (cobr.dup != null && cobr.dup.Count > 0);
        }

        private static bool TemFatura(fat fat)
        {
            if (fat == null)
                return false;
            return !string.IsNullOrWhiteSpace(fat.nFat)
                || fat.vOrig != null
                || fat.vDesc != null
                || fat.vLiq != null;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PadraoLabelGrupo(NFeResource.FATURA_DUPLICATA);

                if (TemFatura(_cobr.fat))
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().PadraoInformacao(NFeResource.NUMERO_FATURA, _cobr.fat.nFat, true);
                        row.RelativeItem().PadraoInformacao(NFeResource.VALOR_ORIGINAL, _cobr.fat.vOrig);
                        row.RelativeItem().PadraoInformacao(NFeResource.VALOR_DESCONTO, _cobr.fat.vDesc);
                        row.RelativeItem().PadraoInformacao(NFeResource.VALOR_LIQUIDO, _cobr.fat.vLiq);
                    });
                }

                var duplicatas = _cobr.dup;
                if (duplicatas == null || duplicatas.Count == 0)
                    return;

                const int maxPorLinha = 9;
                int blocos = (int)Math.Ceiling(duplicatas.Count / (double)maxPorLinha);

                for (int b = 0; b < blocos; b++)
                {
                    var duplicatasBloco = duplicatas
                        .Skip(b * maxPorLinha)
                        .Take(maxPorLinha)
                        .ToList();

                    column.Item().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.ConstantItem(51.4f).BorderLeft(ConstantsHelper.BORDA).BorderTop(ConstantsHelper.BORDA)
                                .Padding(ConstantsHelper.PADDING).Text(NFeResource.NUMERO_VALOR).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(58.5f).BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA).BorderTop(ConstantsHelper.BORDA)
                                    .Padding(ConstantsHelper.PADDING).AlignCenter().Text(item.nDup);
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(51.4f).BorderLeft(ConstantsHelper.BORDA)
                                .Padding(ConstantsHelper.PADDING).Text(NFeResource.VENCIMENTO).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(58.5f).BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA)
                                    .Padding(ConstantsHelper.PADDING).AlignCenter().Text(item.dVenc?.ToString("dd/MM/yyyy"));
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(51.4f).BorderBottom(ConstantsHelper.BORDA).BorderLeft(ConstantsHelper.BORDA)
                                .Padding(ConstantsHelper.PADDING).Text(NFeResource.VALOR_INF).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(58.5f).BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA).BorderBottom(ConstantsHelper.BORDA)
                                    .Padding(ConstantsHelper.PADDING).AlignCenter().Text($"{NFeResource.CIFRAO} {item.vDup.ToString("N2")}");
                            }
                        });
                    });
                }
            });
        }
    }
}
