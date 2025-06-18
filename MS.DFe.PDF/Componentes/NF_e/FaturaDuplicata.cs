using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Cobranca;
using System.Linq;
using System;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;

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
                column.Item().Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.FATURA_DUPLICATA).SemiBold();

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
                            row.ConstantItem(37).BorderLeft(DadoPadraoExtensoes.BORDA).BorderTop(DadoPadraoExtensoes.BORDA)
                                .Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.NUMERO_VALOR).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).BorderTop(DadoPadraoExtensoes.BORDA)
                                    .Padding(2).AlignCenter().Text(item.nDup).SemiBold();
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderLeft(DadoPadraoExtensoes.BORDA)
                                .Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.VENCIMENTO).Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA)
                                    .Padding(2).AlignCenter().Text(item.dVenc?.ToString("dd/MM/yyyy")).SemiBold();
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderBottom(DadoPadraoExtensoes.BORDA).BorderLeft(DadoPadraoExtensoes.BORDA)
                                .Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.VALOR).Bold();

                        foreach (var item in duplicatasBloco)
                        {
                            row.ConstantItem(38.64f).BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).BorderBottom(DadoPadraoExtensoes.BORDA)
                                .Padding(2).AlignCenter().Text($"{NFeResource.CIFRAO} {item.vDup.ToString("N2")}").SemiBold();
                            }
                        });
                    });
                }
            });
        }
    }
}
