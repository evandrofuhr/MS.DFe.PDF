using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Emitente;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class CabecalhoFiscal : IComponent
    {

        private readonly ide _ide;
        private readonly emit _emit;
        private readonly protNFe _protocolo;

        public CabecalhoFiscal(ide ide, emit emit, protNFe protocolo)
        {
            _ide = ide;
            _emit = emit;
            _protocolo = protocolo;
        }

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(innerCol =>
                    {
                        innerCol.Item().Row(innerRow =>
                        {
                            innerRow.RelativeItem().PadraoInformacao(NFeResource.NATUREZA_OPERACAO, _ide.natOp);
                            innerRow.RelativeItem().PadraoInformacao(NFeResource.PROTOCOLO_AUTORIZACAO, $"{_protocolo?.infProt.nProt} - {_protocolo?.infProt.dhRecbto.DateTime}", true);
                        });
                        innerCol.Item().Row(innerRow =>
                        {
                            innerRow.RelativeItem().PadraoInformacao(NFeResource.INSCRICAO_ESTADUAL, _emit.IE.ToString(), true);
                            innerRow.RelativeItem().PadraoInformacao(NFeResource.INSCRICAO_ESTADUAL_SUBST_TRIBUTÁRIO, (_emit.IEST?.ToString()) ?? "", true);
                            innerRow.RelativeItem().PadraoInformacao(NFeResource.CNPJ, _emit.CNPJ.FormataCNPJCPF(), true);
                        });
                    });
                });
            });
        }
    }
}
