using MS.DFe.PDF.Componentes.Nfe;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class Conteudo : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;

        public Conteudo(NFe.Classes.NFe nfe)
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
