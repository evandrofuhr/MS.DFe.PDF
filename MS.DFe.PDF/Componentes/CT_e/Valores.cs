using CTe.Classes.Informacoes.Valores;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Valores : IComponent
    {
        private readonly vPrest _vPrest;

        public Valores(vPrest vPrest)
        {
            _vPrest = vPrest;
        }

        public void Compose(IContainer container)
        {
            if (_vPrest == null) return;

            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Padding(ConstantsHelper.PADDING).Text(CTeResource.COMPONENTES_VALOR).Bold().FontSize(6);

                var comps = _vPrest.Comp ?? Enumerable.Empty<Comp>();
                var lista = comps.ToList();
                var metade = (lista.Count + 1) / 2;

                column.Item().Padding(ConstantsHelper.PADDING).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        for (var i = 0; i < metade && i < lista.Count; i++)
                            col.Item().Text($"{lista[i].xNome}: {lista[i].vComp.Formata()}").FontSize(7);
                    });
                    row.RelativeItem().Column(col =>
                    {
                        for (var i = metade; i < lista.Count; i++)
                            col.Item().Text($"{lista[i].xNome}: {lista[i].vComp.Formata()}").FontSize(7);
                    });
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(CTeResource.VALOR_TOTAL_SERVICO, _vPrest.vTPrest);
                    row.RelativeItem().PadraoInformacao(CTeResource.VALOR_RECEBER, _vPrest.vRec);
                });
            });
        }
    }
}
