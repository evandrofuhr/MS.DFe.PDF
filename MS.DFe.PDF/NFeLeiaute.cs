using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;
using System;
using MS.DFe.PDF.Componentes.Nfe;
using DFe.Utils;
using NFe.Classes;


namespace MS.DFe.PDF
{
    public class NFeLeiaute : IDocument
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;

        public NFeLeiaute(byte[] logo, string xml)
        {
            _logo = logo;

            try
            {
                var nfeProc = FuncoesXml.ArquivoXmlParaClasse<nfeProc>(xml);
                _nfe = nfeProc.NFe;
                _protocolo = nfeProc.protNFe;
            }
            catch
            {
                _nfe = FuncoesXml.ArquivoXmlParaClasse<NFe.Classes.NFe>(xml);
                _protocolo = null;
            }

            if (_nfe == null)
                throw new Exception("Não foi possível converter o arquivo XML.");
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

                page.Header().Component(new Cabecalho(_nfe, _protocolo, _logo));

                page.Content().Component(new ConteudoNFe(_nfe));

                page.Footer().ShowOnce().Component(new Rodape(_nfe.infNFe));
            });
        }

        public DocumentMetadata GetMetadata()
        {
            throw new NotImplementedException();
        }

        public DocumentSettings GetSettings()
        {
            return new DocumentSettings
            {
            };
        }
    }

}