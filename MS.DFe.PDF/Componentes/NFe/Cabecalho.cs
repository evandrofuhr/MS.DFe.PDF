using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;


namespace MS.DFe.PDF.Componentes.Nfe
{
    public class Cabecalho : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        public Cabecalho(NFe.Classes.NFe nfe, protNFe protocolo)
        {
            _nfe = nfe;
            _protocolo = protocolo;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Component(new Canhoto(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe.total.ICMSTot, _nfe.infNFe.dest));
                column.Item().Component(new DadosOperacao(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe, _protocolo));
                column.Item().Component(new DadoFiscal(_nfe.infNFe.ide, _nfe.infNFe.emit, _protocolo));
            });
        }
    }
}
