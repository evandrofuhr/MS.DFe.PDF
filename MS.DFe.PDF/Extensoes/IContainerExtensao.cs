using MS.DFe.PDF.Elementos;
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

        public static void PadraoLabelGrupo(this IContainer container, string label)
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

        public static void PadraoDadosTabela(this IContainer container, string valor, bool centro = false)
        {
            container
                .BorderRight(DadoPadraoExtensoes.BORDA)
                .BorderLeft(DadoPadraoExtensoes.BORDA)
                .Padding(2)
                .Text(
                text =>
                    {
                        text.Span(valor);
                        if (centro) text.AlignCenter();
                        else text.AlignLeft();
                    }
                );
        }

        public static void PadraoDadosTabela(this IContainer container, string valor)
        {
            container
                .BorderRight(DadoPadraoExtensoes.BORDA)
                .BorderLeft(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .AlignRight()
                .Text(valor);
        }

        public static void PadraoInformacaoTabela(this IContainer container, string label)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .AlignCenter()
                .Text(label)
                .SemiBold()
                .FontSize(4);
        }

        public static void PadraoInformacaoTabela(this IContainer container, string label, string _label)
        {
            container
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .AlignCenter()
                .Text(
                text =>
                    {
                        text.DefaultTextStyle(d => d.SemiBold().FontSize(4));
                        text.Line(label);
                        text.Span(_label);
                    }
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
