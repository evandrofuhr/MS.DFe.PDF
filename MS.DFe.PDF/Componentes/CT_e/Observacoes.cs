using CTe.Classes.Informacoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Observacoes : IComponent
    {
        private readonly infCte _inf;

        public Observacoes(infCte inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(_inf.compl?.xObs))
                sb.AppendLine(_inf.compl.xObs);

            if (_inf.compl?.ObsCont != null)
            {
                foreach (var obs in _inf.compl.ObsCont.Where(o => o != null))
                    sb.AppendLine($"{obs.xCampo}: {obs.xTexto}");
            }

            if (_inf.compl?.ObsFisco != null)
            {
                foreach (var obs in _inf.compl.ObsFisco.Where(o => o != null))
                    sb.AppendLine($"{obs.xCampo}: {obs.xTexto}");
            }

            if (!string.IsNullOrEmpty(_inf.imp?.infAdFisco))
                sb.AppendLine(_inf.imp.infAdFisco);

            container.Border(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(column =>
            {
                column.Item().Text(CTeResource.OBSERVACOES).Bold().FontSize(6);
                column.Item().MinHeight(30).Text(sb.ToString().Trim()).FontSize(7);
            });
        }
    }
}
