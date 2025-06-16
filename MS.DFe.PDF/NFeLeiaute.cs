using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;
using System;
using geradorPdfs;


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

                page.Header().Component(new HeaderNfe(_nfe, _protocolo));

                page.Content().Component(new ConteudoNFe(_nfe));

                page.Footer().ShowOnce().Component(new FooterNfe(_nfe.infNFe));
            });
        }

        public class ConteudoNFe : IComponent
        {
            private readonly NFe.Classes.NFe _nfe;

            public ConteudoNFe(NFe.Classes.NFe nfe)
            {
                _nfe = nfe;
            }

            public void Compose(IContainer container)
            {
                container.Column(column =>
                {
                    column.Item().Component(new DestinatarioRemetente(_nfe.infNFe.dest, _nfe.infNFe.ide));
                    if (_nfe.infNFe.cobr != null) column.Item().Component(new FaturaDuplicata(_nfe.infNFe.cobr));
                    column.Item().Component(new CalculoImposto(_nfe.infNFe.total.ICMSTot));
                    column.Item().Component(new Transportador(_nfe.infNFe.transp));
                    column.Item().Component(new Itens(_nfe.infNFe.det, _nfe.infNFe.emit.CRT));
                });
            }
        }

        public DocumentMetadata GetMetadata()
        {
            throw new NotImplementedException();
        }
    }

}