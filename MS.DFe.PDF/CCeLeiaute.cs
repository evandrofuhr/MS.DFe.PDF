using MS.DFe.PDF.Componentes.CCe;
using MS.DFe.PDF.Modelos;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF
{
    public class CCeLeiaute : IDocument
    {
        private readonly byte[] _logo;
        private readonly CCeDados _dados;

        public CCeLeiaute(CCeDados dados, byte[] logo = null)
        {
            _logo = logo;
            _dados = dados;
        }   

        public void Compose(IDocumentContainer container)
        {
            container.Page(
                page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(16);
                    page.DefaultTextStyle(TextStyle.Default.FontSize(9).FontFamily("Consolas"));

                    page.Header().Dynamic(new Cabecalho(_logo)); 
                    page.Content().Component(new Conteudo(_dados));
                }
            );
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;


        public byte[] Gerar()
        {
            return this.GeneratePdf();
        }
    }
}
