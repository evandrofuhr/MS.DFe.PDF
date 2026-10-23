using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    public class CanhotoRetrato : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly ICMSTot _icmstot;
        private readonly dest _dest;

        public CanhotoRetrato(ide ide, emit emit, ICMSTot icmstot, dest dest)
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
                                .Padding(ConstantsHelper.PADDING)
                                .Text(
                                    text =>
                                    {
                                        text.Line(
                                            string.Format(
                                                NFeResource.CANHOTO1,
                                                _emit.xNome
                                            )
                                        ).FontSize(7);

                                        text.Span(
                                            string.Format(
                                                NFeResource.CANHOTO2,
                                                _ide.dhEmi.ToString("dd/MM/yyyy"),
                                                _icmstot.vNF.ToString("N2"),
                                                _dest.xNome
                                            )
                                        ).FontSize(7);
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

        private static void ComposePicote(IContainer container)
        {
            container.Height(5).Svg(size =>
            {
                var w = size.Width.ToString(CultureInfo.InvariantCulture);
                var h = size.Height.ToString(CultureInfo.InvariantCulture);
                var y = (size.Height / 2f).ToString(CultureInfo.InvariantCulture);
                return $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{w}\" height=\"{h}\"><line x1=\"0\" y1=\"{y}\" x2=\"{w}\" y2=\"{y}\" stroke=\"#000\" stroke-width=\"0.7\" stroke-dasharray=\"4 2\" /></svg>";
            });
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().Height(48).Element(ComposeRow);
                    column.Item().PaddingTop(2).Element(ComposePicote);
                }
            );
        }
    }
}
