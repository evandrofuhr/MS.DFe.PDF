using MS.DFe.PDF.Componentes.Nfe;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    public class ConteudoRetrato : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;

        public ConteudoRetrato(NFe.Classes.NFe nfe)
        {
            _nfe = nfe;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Component(new DestinatarioRemetente(_nfe.infNFe.dest, _nfe.infNFe.ide));
                if (FaturaRetrato.TemConteudo(_nfe.infNFe.cobr))
                    column.Item().Component(new FaturaRetrato(_nfe.infNFe.cobr));
                column.Item().Component(new CalculoImpostoRetrato(_nfe.infNFe.total));
                if (CalculoIssqnRetrato.DeveExibir(_nfe.infNFe.total.ISSQNtot))
                    column.Item().Component(new CalculoIssqnRetrato(_nfe.infNFe.emit, _nfe.infNFe.total.ISSQNtot));
                column.Item().Component(new Transportador(_nfe.infNFe.transp, Transportador.TemDetalhe(_nfe.infNFe.transp)));
                column.Item().Component(new ItemRetrato(_nfe.infNFe.det, _nfe.infNFe.emit.CRT));
            });
        }
    }
}
