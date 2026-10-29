using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class VeiculoCondutor : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public VeiculoCondutor(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var rodo = _inf.InfModal?.Modal as MDFeRodo;
            if (rodo == null) return;

            var placas = new StringBuilder();
            var rntrcs = new StringBuilder();
            var cpfs = new StringBuilder();
            var nomes = new StringBuilder();

            var tracao = rodo.VeicTracao;
            if (tracao != null)
            {
                placas.AppendLine(tracao.Placa ?? string.Empty);
                var rntrc = tracao.Prop?.RNTRC;
                if (string.IsNullOrEmpty(rntrc))
                    rntrc = rodo.infANTT?.RNTRC ?? rodo.RNTRC;
                rntrcs.AppendLine(rntrc ?? string.Empty);

                if (tracao.Condutor != null)
                {
                    foreach (var c in tracao.Condutor)
                    {
                        cpfs.AppendLine((c.CPF ?? string.Empty).FormataCNPJCPF());
                        nomes.AppendLine(c.XNome ?? string.Empty);
                    }
                }
            }

            if (rodo.VeicReboque != null)
            {
                foreach (var reb in rodo.VeicReboque)
                {
                    placas.AppendLine(reb.Placa ?? string.Empty);
                    var rntrc = reb.Prop?.RNTRC;
                    if (string.IsNullOrEmpty(rntrc))
                        rntrc = rodo.RNTRC;
                    rntrcs.AppendLine(rntrc ?? string.Empty);
                }
            }

            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().BorderRight(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(col =>
                    {
                        col.Item().Text(MDFeResource.VEICULO).Bold().FontSize(6);
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().PadraoInformacao(MDFeResource.PLACA, placas.ToString().Trim());
                            r.RelativeItem().PadraoInformacao(MDFeResource.RNTRC, rntrcs.ToString().Trim());
                        });
                    });
                    row.RelativeItem().Padding(ConstantsHelper.PADDING).Column(col =>
                    {
                        col.Item().Text(MDFeResource.CONDUTOR).Bold().FontSize(6);
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().PadraoInformacao(MDFeResource.CPF, cpfs.ToString().Trim());
                            r.RelativeItem().PadraoInformacao(MDFeResource.NOME, nomes.ToString().Trim());
                        });
                    });
                });
            });
        }
    }
}
