using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Emitente;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class Canhoto : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly ICMSTot _icmstot;
        private readonly dest _dest;

        public Canhoto(ide ide, emit emit, ICMSTot icmstot, dest dest)
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
                    row.ConstantItem(244).Height(25).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.DATA_RECEBIMENTO);
                    row.ConstantItem(244).Height(25).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.IDENTIFICACAO_ASSINATURA_RECEBEDOR);
                }
           );
        }

        private void ComposeRow(IContainer container)
        {
            container.Row(
                row =>
                {
                    row.RelativeItem().Column(
                         coll =>
                         {
                             coll.Item()
                                .Height(26)
                                .Border(DadoPadraoExtensoes.BORDA)
                                .Padding(DadoPadraoExtensoes.PADDING)
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

                             coll.Item().Element(ComposeRecebimento);
                         }
                    );
                    row.ConstantItem(90)
                        .Border(DadoPadraoExtensoes.BORDA)
                        .Padding(DadoPadraoExtensoes.PADDING)
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
                    column.Item().Element(ComposeRow);
                    column.Item().PaddingVertical(7).LineHorizontal(DadoPadraoExtensoes.BORDA);
                }
            );
        }
    }
}
