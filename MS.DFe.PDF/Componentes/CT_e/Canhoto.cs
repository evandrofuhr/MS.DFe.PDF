using CTe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Canhoto : IComponent
    {
        private readonly infCte _inf;

        public Canhoto(infCte inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            container.Border(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(column =>
            {
                column.Item().Text(CTeResource.CANHOTO_TEXTO).FontSize(6);
                column.Item().PaddingTop(2).Row(row =>
                {
                    row.RelativeItem(4).PadraoInformacao(CTeResource.NOME, string.Empty);
                    row.RelativeItem(2).PadraoInformacao(CTeResource.RG, string.Empty);
                    row.RelativeItem(3).PadraoInformacao(CTeResource.ASSINATURA, string.Empty);
                    row.RelativeItem(2).PadraoInformacao(CTeResource.CHEGADA, string.Empty);
                    row.RelativeItem(2).PadraoInformacao(CTeResource.SAIDA, string.Empty);
                });
                column.Item().PaddingTop(2).AlignRight()
                    .Text($"CT-e Nº {_inf.ide.nCT.ToNumeroNfe()}  Série {((int)_inf.ide.serie).FormataSerie()}")
                    .FontSize(7).Bold();
            });
        }
    }
}
