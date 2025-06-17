using MS.DFe.PDF.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Elementos
{
    public class CampoTabela : IComponent
    {
        private readonly string _text;
        private readonly EAlinhamento _alinhamento;

        private CampoTabela(string text, EAlinhamento alinhamento)
        {
            _text = text ?? string.Empty;
            _alinhamento = alinhamento;
        }

        public static CampoTabela Codigo(string text) =>
            new CampoTabela(text, EAlinhamento.centro);

        public static CampoTabela Valor(string text) =>
            new CampoTabela(text, EAlinhamento.direita);

        public static CampoTabela Descricao(string text) =>
            new CampoTabela(text, EAlinhamento.esquerda);

        public static CampoTabela Padrao(string text, EAlinhamento alinhamento = EAlinhamento.esquerda) =>
            new CampoTabela(text, alinhamento);

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
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

                content.Padding(1).Text(_text).FontSize(6).LineHeight(1);
            });
        }
    }
}
