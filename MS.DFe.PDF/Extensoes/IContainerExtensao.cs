using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;

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

        public static void PadraoLabelGrupo (this IContainer container, string label)
        {
            container.PaddingVertical(DadoPadraoExtensoes.PADDING).Text(label);
        }

        public static void PadraoInformacao(this IContainer container, string label, decimal? valor)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .Component(
                    CampoInformativo.Valor(label, valor?.Formata())
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, int? valor)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .Component(
                    CampoInformativo.Valor(label, valor.ToString())
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, string valor, bool center = false)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .Component(
                    center ? CampoInformativo.Codigo(label, valor) : CampoInformativo.Padrao(label, valor)
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, DateTimeOffset? valor)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .Component(
                   CampoInformativo.Hora(label, valor)
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, DateTime? valor)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .Component(
                    CampoInformativo.Data(label, valor)
                );
        }



        public static TextSpanDescriptor Texto(this IContainer container, decimal valor)
        {
            return container.Text(valor.Formata());
        }
    }
}
