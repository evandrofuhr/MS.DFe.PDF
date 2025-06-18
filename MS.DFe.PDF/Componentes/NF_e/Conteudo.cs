using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Emitente;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;

namespace MS.DFe.PDF.Componentes.Nfe
{
    public class ConteudoNFe : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;

        public ConteudoNFe(NFe.Classes.NFe nfe)
        {
            _nfe = nfe;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Component(new DestinatarioRemetente(_nfe.infNFe.dest, _nfe.infNFe.ide));
                if (_nfe.infNFe.cobr != null) column.Item().Component(new FaturaDuplicata(_nfe.infNFe.cobr));
                column.Item().Component(new CalculoImposto(_nfe.infNFe.total.ICMSTot));
                column.Item().Component(new Transportador(_nfe.infNFe.transp));
                column.Item().Component(new Item(_nfe.infNFe.det, _nfe.infNFe.emit.CRT));
            });
        }
    }
}
