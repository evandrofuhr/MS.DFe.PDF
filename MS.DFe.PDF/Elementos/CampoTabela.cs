using MS.DFe.PDF.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Elementos
{
    public class CampoTabela : IComponent
    {
        private readonly string _text;
        private readonly EAlinhamento _alinhamento;


        public CampoTabela(string text, EAlinhamento alinhamento = EAlinhamento.esquerda)
        {
            _text = text;
            _alinhamento = alinhamento;
        }

        public void Compose(IContainer container)
        {
            container.
                Column(column =>
               {
                   var _container = column.Item();

                   if (_alinhamento == EAlinhamento.centro)
                       _container = _container.AlignCenter();
                   else if (_alinhamento == EAlinhamento.esquerda)
                       _container = _container.AlignLeft();
                   else if (_alinhamento == EAlinhamento.direita)
                       _container = _container.AlignRight();

                   _container.Padding(1).Text(_text).FontSize(6).LineHeight(1);
               });
        }
    }
    public class CampoTabelaCodigo : CampoTabela
    {
        public CampoTabelaCodigo(string text) : base(text, EAlinhamento.centro)
        {
        }
    }
    public class CampoTabelaValor : CampoTabela
    {
        public CampoTabelaValor(string text) : base(text, EAlinhamento.direita)
        {
        }
    }

    public class CampoTabelaDescricao : CampoTabela
    {
        public CampoTabelaDescricao(string text) : base(text, EAlinhamento.esquerda)
        {
        }
    }
}
