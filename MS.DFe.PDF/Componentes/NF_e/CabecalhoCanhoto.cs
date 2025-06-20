using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class CabecalhoCanhoto : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly ICMSTot _icmstot;
        private readonly dest _dest;

        public CabecalhoCanhoto(ide ide, emit emit, ICMSTot icmstot, dest dest)
        {
            _ide = ide;
            _emit = emit;
            _icmstot = icmstot;
            _dest = dest;
        }

        private void ComposeQuadroNumero(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().AlignCenter().Text(NFeResource.NFE).Bold().FontSize(14);
                    column.Item().AlignCenter().Text($"{NFeResource.NUMERO} {_ide.nNF.ToNumeroNfe()}").FontSize(12);
                    column.Item().AlignCenter().Text($"{NFeResource.SERIE} {_ide.serie.ToString()}").FontSize(12);
                }
            );
        }

        private void ComposeRecebimento(IContainer container)
        {
            container.Row(
                row =>
                {
                    row.RelativeItem().PadraoInformacao(NFeResource.DATA_RECEBIMENTO, string.Empty);
                    row.RelativeItem().PadraoInformacao(NFeResource.IDENTIFICACAO_ASSINATURA_RECEBEDOR, string.Empty);
                }
           );
        }

        private void ComposeRow(IContainer container)
        {
            container.Row(
                row =>
                {
                    row.RelativeItem().Column(
                         column =>
                         {
                             column.Item()
                                .Border(ConstantsHelper.BORDA)
                                .Padding(5f)
                                .Text(
                                    text =>
                                    {
                                        text.DefaultTextStyle(s => s.FontSize(7));
                                        text.Line(
                                            string.Format(
                                                NFeResource.CANHOTO1,
                                                _emit.xNome
                                            )
                                        );

                                        text.Span(
                                            string.Format(
                                                NFeResource.CANHOTO2,
                                                _ide.dhEmi.ToString("dd/MM/yyyy"),
                                                _icmstot.vNF.ToString("N2"),
                                                _dest.xNome
                                            )
                                        );
                                    }
                                );

                             column.Item().ExtendVertical().Element(ComposeRecebimento);
                         }
                    );
                    row.ConstantItem(90)
                        .Border(ConstantsHelper.BORDA)
                        .Padding(ConstantsHelper.PADDING)
                        .AlignMiddle()
                        .AlignCenter()
                        .Element(ComposeQuadroNumero);

                }
            );
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().Height(48).Element(ComposeRow);
                    column.Item().PaddingVertical(4).LineHorizontal(ConstantsHelper.BORDA);
                }
            );
        }
    }
}
