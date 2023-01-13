using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Extensoes
{
    internal static class IContainerExtensao
    {
        public static TextSpanDescriptor Texto(this IContainer container, params string[] textos)
        {
            return container.Text(string.Join(" ", textos) ?? string.Empty);
        }

        public static TextSpanDescriptor TextoSpan(this TextDescriptor text, params string[] textos)
        {
            return text.Span(string.Join(" ", textos) ?? string.Empty);
        }

        public static TextSpanDescriptor Texto(this IContainer container, decimal valor)
        {
            return container.Text(valor.Formata());
        }
    }
}
