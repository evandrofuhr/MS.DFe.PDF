using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class IdentificacaoResumo : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public IdentificacaoResumo(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var tot = _inf.Tot;
            container.Row(row =>
            {
                row.RelativeItem().PadraoInformacao(MDFeResource.QTD_CTE, (tot?.QCTe ?? 0).ToString());
                row.RelativeItem().PadraoInformacao(MDFeResource.QTD_NFE, (tot?.QNFe ?? 0).ToString());
                row.RelativeItem().PadraoInformacao(MDFeResource.QTD_MDFE, (tot?.QMDFe ?? 0).ToString());
                row.RelativeItem().PadraoInformacao(MDFeResource.PESO_TOTAL, tot?.QCarga);
                row.RelativeItem().PadraoInformacao(MDFeResource.VALOR_TOTAL_CARGA, tot?.vCarga);
            });
        }
    }
}
