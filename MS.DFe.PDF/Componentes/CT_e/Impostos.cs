using CTe.Classes.Informacoes.Impostos;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Impostos : IComponent
    {
        private readonly imp _imp;

        public Impostos(imp imp)
        {
            _imp = imp;
        }

        public void Compose(IContainer container)
        {
            CTeHelper.ExtrairIcms(_imp?.ICMS, out var situacao, out var vBc, out var pIcms, out var vIcms, out var pRed);

            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Padding(ConstantsHelper.PADDING).Text(CTeResource.INFORMACOES_IMPOSTO).Bold().FontSize(6);
                column.Item().Row(row =>
                {
                    row.RelativeItem(4).PadraoInformacao(CTeResource.SITUACAO_TRIBUTARIA, situacao);
                    row.RelativeItem().PadraoInformacao(CTeResource.BASE_CALCULO, vBc, "r");
                    row.RelativeItem().PadraoInformacao(CTeResource.ALIQ_ICMS, pIcms, "r");
                    row.RelativeItem().PadraoInformacao(CTeResource.VALOR_ICMS, vIcms, "r");
                    row.RelativeItem().PadraoInformacao(CTeResource.RED_BC, pRed, "r");
                });
            });
        }
    }
}
