using MS.DFe.PDF.Resources;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CCe
{
    internal class Cabecalho : IDynamicComponent<int>
    {
        private readonly byte[] _logo;

        public Cabecalho(byte[] logo)
        {
            if (logo == null || logo.Length == 0)
                _logo = CCeResource.logo_placeholder;
            else
                _logo = logo;
        }

        public int State { get; set; }

        public DynamicComponentComposeResult Compose(DynamicContext context)
        {
            return new DynamicComponentComposeResult
            {
                Content = context.CreateElement(
                    container =>
                    {
                        container.Border(0.8f).Row(
                            row =>
                            {
                                row.ConstantItem(110).Padding(3).MaxWidth(110).Image(_logo, ImageScaling.FitWidth);
                                row.RelativeItem().AlignMiddle().Column(
                                    column =>
                                    {
                                        column.Item().AlignCenter().Text(CCeResource.TITULO).FontSize(14).Bold();
                                        column.Item().AlignCenter().Text(CCeResource.SUBTITULO);
                                        column.Item().AlignCenter().Text(CCeResource.TITULO_CONSULTE);
                                    }
                                );
                                row.AutoItem().AlignBottom().Padding(3).Text($"{context.PageNumber}/{context.TotalPages}");
                            }
                        );
                    }
                ),
                HasMoreContent = false
            };
        }
    }
}
