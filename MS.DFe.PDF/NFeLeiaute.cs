using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;
using System;
using MS.DFe.PDF.Componentes.Nfe;


namespace MS.DFe.PDF
{
    public class NFeLeiaute : IDocument
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;

        public NFeLeiaute(NFe.Classes.NFe nfe, protNFe protocolo)
        {
            _nfe = nfe;
            _protocolo = protocolo;
        }

        public DocumentSettings GetSettings()
        {
            return new DocumentSettings
            {
            };
        }
        private void ComposeWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Text("PRÉ-VISUALIZAÇÃO").FontSize(70).Bold().FontColor(Colors.Grey.Lighten2);
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(3, Unit.Millimetre);
                page.DefaultTextStyle(TextStyle.Default.FontSize(6));
                page.PageColor(Colors.White);

                if (_protocolo == null)
                    page.Background().Element(ComposeWaterMark);

                page.Header().Component(new Cabecalho(_nfe, _protocolo));

                page.Content().Component(new ConteudoNFe(_nfe));

                page.Footer().ShowOnce().Component(new Rodape(_nfe.infNFe));
            });
        }


        public DocumentMetadata GetMetadata()
        {
            throw new NotImplementedException();
        }
    }

}