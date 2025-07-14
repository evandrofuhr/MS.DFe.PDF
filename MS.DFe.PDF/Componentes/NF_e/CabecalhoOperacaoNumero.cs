using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Identificacao;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class CabecalhoOperacaoNumero : IComponent
    {
        private readonly ide _ide;

        public CabecalhoOperacaoNumero(ide ide)
        {
            _ide = ide;
        }

        private void ComposeEntradaSaida(IContainer container)
        {
            container.Row(
                row =>
                {
                    row.ConstantItem(4).ShrinkHorizontal();
                    row.RelativeItem()
                        .AlignCenter()
                        .Text(
                           text =>
                           {
                               text.Line(NFeResource.ENTRADA);
                               text.Span(NFeResource.SAIDA);
                           }
                        );

                    row.ConstantItem(13)
                    .PaddingTop(3)
                    .PaddingBottom(2)
                         .Border(ConstantsHelper.BORDA)
                         .AlignCenter()
                         .PaddingTop(1.4f)
                         .Text(((int)_ide.tpNF).ToString())
                         .AlignCenter()
                         .FontSize(12)
                         .Bold();

                    row.ConstantItem(4).ShrinkHorizontal();
                }
           );
        }

        public void Compose(IContainer container)
        {

            container
                .Border(ConstantsHelper.BORDA)
                .Padding(1)
                .AlignMiddle()
                .Column(
                    column =>
                        {
                            column.Item().Text(NFeResource.DANFE).FontSize(14).Bold().AlignCenter().LineHeight(1.5f);
                            column.Item().Text(NFCeResource.DOC_AUX).FontSize(6.6f).AlignCenter().LineHeight(1);
                            column.Item().Height(3);
                            column.Item().Element(ComposeEntradaSaida);
                            column.Item().Height(3);

                            column.Item().AlignCenter().Text(
                                text =>
                                {
                                    text.Line($"{NFeResource.NUMERO} {_ide.nNF.ToNumeroNfe()}").Bold();
                                    text.Span($"{NFeResource.SERIE} {_ide.serie.ToString()}").Bold();
                                }
                            );

                            column.Item()
                                .AlignCenter()
                                .Text(
                                    text =>
                                    {
                                        text.DefaultTextStyle(d => d.Bold());
                                        text.Span("Folha ");
                                        text.CurrentPageNumber();
                                        text.Span("/");
                                        text.TotalPages();
                                    }
                            );
                        }
                );
        }
    }
}





