using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class RodapeExclusivo : IComponent
    {
        public void Compose(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Border(ConstantsHelper.BORDA).MinHeight(35).Padding(ConstantsHelper.PADDING)
                    .Text(CTeResource.USO_EXCLUSIVO).FontSize(6);
                row.RelativeItem().Border(ConstantsHelper.BORDA).MinHeight(35).Padding(ConstantsHelper.PADDING)
                    .Text(CTeResource.RESERVADO_FISCO).FontSize(6);
            });
        }
    }
}
