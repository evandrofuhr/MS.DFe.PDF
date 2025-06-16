using MS.DFe.PDF.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;

namespace MS.DFe.PDF.Elementos
{
    public class CampoInformativoCodigo : CampoInformativo
    {
        public CampoInformativoCodigo(string label, string text) : base(label, text, EAlinhamento.centro)
        {

        }
    }

    public class CampoInformativoData : CampoInformativo
    {
        public CampoInformativoData(string label, DateTime? dateTime) : base(label, dateTime?.ToString("dd/MM/yyyy") ?? string.Empty, EAlinhamento.centro)
        {

        }
    }


    public class CampoInformativoHora : CampoInformativo
    {
        public CampoInformativoHora(string label, DateTimeOffset? dateTime) : base(label, dateTime?.ToString("HH:mm") ?? string.Empty, EAlinhamento.centro)
        {

        }
    }

    public class CampoInformativoValor : CampoInformativo
    {
        public CampoInformativoValor(string label, string text) : base(label, text, EAlinhamento.direita)
        {

        }
    }

    public class CampoInformativo : IComponent
    {
        private readonly string _label;
        private readonly string _text;
        private readonly EAlinhamento _alinhamento;

        public CampoInformativo(string label, string text, EAlinhamento alinhamento = EAlinhamento.esquerda)
        {
            _label = label;
            _text = text ?? string.Empty;
            _alinhamento = alinhamento;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().AlignLeft().Text(_label).FontSize(5).LineHeight(1).SemiBold();
                    var _container = column.Item();

                    if (_alinhamento == EAlinhamento.centro)
                        _container = _container.AlignCenter();
                    else if (_alinhamento == EAlinhamento.esquerda)
                        _container = _container.AlignLeft();
                    else if (_alinhamento == EAlinhamento.direita)
                        _container = _container.AlignRight();

                    _container.Text(_text).FontSize(7).Bold().LineHeight(1);
                }
            );
        }
    }
}
