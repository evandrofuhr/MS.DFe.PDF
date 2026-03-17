using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class CabecalhoOperacaoEmitente : IComponent
    {
        private readonly emit _emit;
        private readonly byte[] _logo;

        public CabecalhoOperacaoEmitente(emit emit, byte[] logo)
        {
            _emit = emit;
            _logo = logo;
        }

        private void ComposeSemLogo(IContainer container)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .Column(
                    _column =>
                    {
                        _column.Item().Height(20).AlignCenter().Text(NFeResource.IDENTIFICACAO_EMITENTE);
                        _column.Item().AlignCenter().AlignMiddle().Element(e => ComposeText(e, 11f));
                    }
                );
        }

        private void ComposeText(IContainer container, float fontSizeBase = 11f)
        {
            container.AlignCenter().AlignMiddle().Text(
                text =>
                {
                    text.Line(_emit.xFant).FontSize(fontSizeBase).Bold();
                    text.Line(_emit.enderEmit.ToEndereco1()).FontSize(fontSizeBase - 2f);
                    if (_emit.enderEmit.fone != null)
                        text.Line(_emit.enderEmit.ToEndereco2()).FontSize(fontSizeBase - 2f);
                    else
                        text.Span(_emit.enderEmit.ToEndereco2()).FontSize(fontSizeBase - 2f);
                    if (_emit.enderEmit.fone != null) text.Span($"{NFeResource.FONE}: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(fontSizeBase - 1f).Bold();
                }
            );
        }

        private void ComposeComLogo(IContainer container)
        {
            var _tipo = ImagemHelper.TipoImagem(_logo);

            if (_tipo == ETipoImg.QuadradoOuVertical)
            {
                container
                    .Border(ConstantsHelper.BORDA)
                    .Padding(ConstantsHelper.PADDING)
                    .AlignMiddle()
                    .Row(
                        row =>
                        {
                            row.ConstantItem(80).AlignCenter().AlignMiddle().Height(80).Image(_logo).FitArea();
                            row.ConstantItem(5);
                            row.RelativeItem().AlignMiddle().AlignCenter().Element(e => ComposeText(e, 11f));
                        }
                    );
            }
            else
            {

                container
                    .Border(ConstantsHelper.BORDA)
                    .Padding(ConstantsHelper.PADDING)
                    .AlignMiddle()
                    .Column(
                        column =>
                        {
                            column.Item().AlignCenter().Height(34).Image(_logo).FitArea();
                            column.Item().Height(1f).ShrinkHorizontal();
                            column.Item().AlignCenter().Element(e => ComposeText(e, 11f));
                        }
                    );

            }
        }

        public void Compose(IContainer container)
        {
            if (_logo == null)
                container.Element(ComposeSemLogo);
            else
                container.Element(ComposeComLogo);
        }
    }
}





