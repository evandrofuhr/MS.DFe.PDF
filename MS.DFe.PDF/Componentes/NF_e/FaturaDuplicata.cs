using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Cobranca;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Linq;

namespace MS.DFe.PDF.Componentes.Nfe
{
    public class FaturaDuplicata : IComponent
    {
        private readonly cobr _cobr;

        public FaturaDuplicata(cobr cobr)
        {
            _cobr = cobr;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PadraoLabelGrupo(NFeResource.FATURA_DUPLICATA);

                const int maxPorLinha = 14;
                int total = _cobr.dup.Count;
                int blocos = (int)Math.Ceiling(total / (double)maxPorLinha);

                for (int b = 0; b < blocos; b++)
                {
                    var duplicatasBloco = _cobr.dup
                        .Skip(b * maxPorLinha)
                        .Take(maxPorLinha)
                        .ToList();

                    column.Item().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderLeft(ConstantsHelper.BORDA).BorderTop(ConstantsHelper.BORDA)
                                .Padding(ConstantsHelper.PADDING).Text(NFeResource.NUMERO_VALOR).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA).BorderTop(ConstantsHelper.BORDA)
                                    .Padding(2).AlignCenter().Text(item.nDup).SemiBold();
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderLeft(ConstantsHelper.BORDA)
                                .Padding(ConstantsHelper.PADDING).Text(NFeResource.VENCIMENTO).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA)
                                    .Padding(2).AlignCenter().Text(item.dVenc?.ToString("dd/MM/yyyy")).SemiBold();
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderBottom(ConstantsHelper.BORDA).BorderLeft(ConstantsHelper.BORDA)
                                .Padding(ConstantsHelper.PADDING).Text(NFeResource.VALOR).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA).BorderBottom(ConstantsHelper.BORDA)
                                    .Padding(2).AlignCenter().Text($"{NFeResource.CIFRAO} {item.vDup.ToString("N2")}").SemiBold();
                            }
                        });
                    });
                }
            });
        }
    }
}
