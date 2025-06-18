using MS.DFe.PDF.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;

namespace MS.DFe.PDF.Elementos
{
    public class CampoInformativo : IComponent
    {
        private readonly string _label;
        private readonly string _text;
        private readonly EAlinhamento _alinhamento;

        private CampoInformativo(string label, string text, EAlinhamento alinhamento)
        {
            _label = label;
            _text = text ?? string.Empty;
            _alinhamento = alinhamento;
        }

        public static CampoInformativo Codigo(string label, string text) =>
            new CampoInformativo(label, text, EAlinhamento.centro);

        public static CampoInformativo Data(string label, DateTime? dateTime) =>
            new CampoInformativo(label, dateTime?.ToString("dd/MM/yyyy") ?? string.Empty, EAlinhamento.centro);

        public static CampoInformativo Hora(string label, DateTimeOffset? dateTime) =>
            new CampoInformativo(label, dateTime?.ToString("HH:mm") ?? string.Empty, EAlinhamento.centro);

        public static CampoInformativo Valor(string label, string text) =>
            new CampoInformativo(label, text, EAlinhamento.direita);

        public static CampoInformativo Padrao(string label, string text) =>
            new CampoInformativo(label, text, EAlinhamento.esquerda);

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().AlignLeft().Text(_label).FontSize(4);

                var content = column.Item();

                switch (_alinhamento)
                {
                    case EAlinhamento.centro:
                        content = content.AlignCenter();
                        break;
                    case EAlinhamento.direita:
                        content = content.AlignRight();
                        break;
                    case EAlinhamento.esquerda:
                    default:
                        content = content.AlignLeft();
                        break;
                }

                content.Text(_text).FontSize(8).Bold().LineHeight(1);
            });
        }
    }
}
