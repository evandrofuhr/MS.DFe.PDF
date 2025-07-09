using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class CabecalhoOperacao : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly infNFe _infnfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;

        public CabecalhoOperacao(ide ide, emit emit, infNFe infnfe, protNFe protocolo, byte[] logo)
        {
            _ide = ide;
            _emit = emit;
            _infnfe = infnfe;
            _protocolo = protocolo;
            _logo = logo;
        }

        public void Compose(IContainer container)
        {
            container.Row(
                row =>
                {
                    row.RelativeItem(5).Component(new CabecalhoOperacaoEmitente(_emit, _logo));
                    row.RelativeItem(2).Component(new CabecalhoOperacaoNumero(_ide));
                    row.RelativeItem(5).Component(new CabecalhoOperacaoChave(_infnfe, _protocolo));
                }
            );
        }
    }
}





