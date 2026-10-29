using CTe.Classes.Informacoes;
using CTe.Classes.Informacoes.infCTeNormal.infModals;
using CTe.Classes.Informacoes.Tipos;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class ModalRodoviario : IComponent
    {
        private readonly infCte _inf;

        public ModalRodoviario(infCte inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            if (_inf.ide.tpCTe != tpCTe.Normal || _inf.ide.modal != modal.rodoviario)
                return;

            var rodo = _inf.infCTeNorm?.infModal?.ContainerModal as rodo;
            if (rodo == null) return;

            var lotacao = rodo.lotaSpecified && rodo.lota.HasValue && rodo.lota.Value == lota.Sim
                ? CTeResource.SIM
                : CTeResource.NAO;

            var dPrev = rodo.dPrev.HasValue
                ? rodo.dPrev.Value.ToString("dd/MM/yyyy HH:mm")
                : string.Empty;

            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Padding(ConstantsHelper.PADDING).Text(CTeResource.MODAL_RODOVIARIO).Bold().FontSize(6);
                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(CTeResource.RNTRC, rodo.RNTRC);
                    row.RelativeItem().PadraoInformacao(CTeResource.CIOT, rodo.CIOT);
                    row.RelativeItem().PadraoInformacao(CTeResource.LOTACAO, lotacao);
                    row.RelativeItem().PadraoInformacao(CTeResource.DATA_PREVISTA, dPrev);
                });
                column.Item().Padding(ConstantsHelper.PADDING).AlignCenter()
                    .Text(CTeResource.LEGENDA_RODO).FontSize(6).Italic();
            });
        }
    }
}
