using MS.DFe.PDF.Componentes.NF_e;
using MS.DFe.PDF.Helpers;
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
            container.PaddingVertical(ConstantsHelper.PADDING).Text(label).FontSize(7);
        }

        public static void PadraoInformacao(this IContainer container, string label, decimal? valor)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .Component(
                    CampoInformativo.Valor(label, valor?.Formata())
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, string valor, bool center = false)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .Component(
                    center ? CampoInformativo.Codigo(label, valor) : CampoInformativo.Padrao(label, valor)
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, string valor, string _)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .Component(CampoInformativo.Valor(label, valor)
                );
        }

        public static void PadraoDadosTabela(this IContainer container, string valor, bool centro = false)
        {
            container
                .BorderRight(ConstantsHelper.BORDA)
                .BorderLeft(ConstantsHelper.BORDA)
                .BorderTop(0.09f)
                .Padding(0.8f)
                .Text(
                text =>
                    {
                        text.DefaultTextStyle(d => d.FontSize(6));
                        text.Span(valor);
                        if (centro) text.AlignCenter();
                        else text.AlignLeft();
                    }
                );
        }

        public static void PadraoDadosTabela(this IContainer container, string valor)
        {
            container
                .BorderRight(ConstantsHelper.BORDA)
                .BorderLeft(ConstantsHelper.BORDA)
                .BorderTop(0.09f)
                .Padding(0.8f)
                .AlignRight()
                .Text(valor)
                .FontSize(7);
        }



        public static void PadraoInformacaoTabela(this IContainer container, string label)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .AlignCenter()
                .AlignMiddle()
                .Text(label)
                .SemiBold()
                .FontSize(5.5f);
        }

        public static void PadraoInformacaoTabela(this IContainer container, string label, string _label)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .AlignCenter()
                .Text(
                text =>
                    {
                        text.DefaultTextStyle(d => d.SemiBold().FontSize(5.5f));
                        text.Line(label);
                        text.Span(_label);
                    }
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, DateTimeOffset? valor)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
                .Component(
                   CampoInformativo.Hora(label, valor)
                );
        }

        public static void PadraoInformacao(this IContainer container, string label, DateTime? valor)
        {
            container
                .Border(ConstantsHelper.BORDA)
                .Padding(ConstantsHelper.PADDING)
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
