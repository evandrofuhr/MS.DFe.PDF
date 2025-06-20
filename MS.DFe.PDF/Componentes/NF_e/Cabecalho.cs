using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class Cabecalho : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;
        public Cabecalho(NFe.Classes.NFe nfe, protNFe protocolo, byte[] logo)
        {
            _nfe = nfe;
            _protocolo = protocolo;
            _logo = logo;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().Component(new CabecalhoCanhoto(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe.total.ICMSTot, _nfe.infNFe.dest));
                    column.Item().Component(new CabecalhoOperacao(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe, _protocolo, _logo));
                    column.Item().Component(new CabecalhoFiscal(_nfe.infNFe.ide, _nfe.infNFe.emit, _protocolo));
                }
            );
        }
    }
}
