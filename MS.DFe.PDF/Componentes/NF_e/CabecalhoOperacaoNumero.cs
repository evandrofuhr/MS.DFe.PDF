using MS.DFe.PDF.Extensoes;
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
                               text.Line(NFeResource.ENTRADA).FontSize(8);
                               text.Span(NFeResource.SAIDA).FontSize(8);
                           }
                        );

                   row.ConstantItem(13)
                        .Border(DadoPadraoExtensoes.BORDA)
                        .AlignCenter()
                        .Text(((int)_ide.tpNF).ToString())
                        .FontSize(12)
                        .Bold();

                    row.ConstantItem(4).ShrinkHorizontal();
               }
           );
        }

        public void Compose(IContainer container)
        {

            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(1)
                .AlignMiddle()
                .Column(
                    column =>
                        {
                            column.Item().Text(NFeResource.DANFE).FontSize(10).Bold().AlignCenter().LineHeight(1.5f);
                            column.Item().Text(NFCeResource.DOC_AUX).FontSize(6.6f).AlignCenter().LineHeight(1);
                            column.Item().Height(3);
                            column.Item().Element(ComposeEntradaSaida);
                            column.Item().Height(3);

                            column.Item().AlignCenter().Text(
                                text =>
                                {
                                    text.Line($"{NFeResource.NUMERO} {_ide.nNF.ToNumeroNfe()}").FontSize(8).Bold();
                                    text.Span($"{NFeResource.SERIE} {_ide.serie.ToString()}").FontSize(8).Bold();
                                }
                            );

                            column.Item()
                                .AlignCenter()
                                .Text(
                                    text =>
                                    {
                                        text.DefaultTextStyle(d => d.FontSize(8).Bold());
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





