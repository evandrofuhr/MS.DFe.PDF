using CTe.Classes.Informacoes;
using CTe.Classes.Informacoes.Tipos;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Identificacao : IComponent
    {
        private readonly infCte _inf;

        public Identificacao(infCte inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var ide = _inf.ide;
            var toma = ObterToma(ide);

            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(CTeResource.TIPO_CTE, CTeHelper.TipoCTe(ide.tpCTe));
                    row.RelativeItem().PadraoInformacao(CTeResource.TIPO_SERVICO, CTeHelper.TipoServico(ide.tpServ));
                    row.RelativeItem().PadraoInformacao(CTeResource.TOMADOR, CTeHelper.TomadorNome(toma));
                    row.RelativeItem().PadraoInformacao(CTeResource.FORMA_PAGAMENTO, CTeHelper.FormaPagamento(ide.forPagSpecified ? ide.forPag : null));
                });

                column.Item().PadraoInformacao(CTeResource.CFOP_NATUREZA, $"{ide.CFOP} - {ide.natOp}");

                if (ide.tpCTe == tpCTe.Complemento || ide.tpCTe == tpCTe.Anulacao)
                {
                    var chaveOriginal = ide.tpCTe == tpCTe.Complemento
                        ? (_inf.infCteComp?.chCTe ?? _inf.infCteComp?.chave)
                        : _inf.infCteAnu?.chCte;

                    var texto = (chaveOriginal ?? string.Empty).NormalizarChaveCTe()?.FormataChaveNFe() ?? string.Empty;
                    if (ide.tpCTe == tpCTe.Anulacao && _inf.infCteAnu != null)
                        texto += $"   emissão: {_inf.infCteAnu.dEmi:dd/MM/yyyy}";

                    column.Item().PadraoInformacao(CTeResource.CHAVE_ORIGINAL, texto);
                }
            });
        }

        internal static toma ObterToma(CTe.Classes.Informacoes.Identificacao.ide ide)
        {
            if (ide.tomaBase3 != null)
                return ide.tomaBase3.toma;
            if (ide.toma4 != null)
                return ide.toma4.toma;
            return toma.Outros;
        }
    }
}
