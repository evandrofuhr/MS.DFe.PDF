using MS.DFe.PDF.Componentes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO;

namespace MS.DFe.PDF
{
    public class NFCeLeiaute : IDocument
    {
        private readonly DFeDados _dados;

        public NFCeLeiaute(DFeDados dados)
        {
            _dados = dados;

            FontManager.RegisterFont(new MemoryStream(FontResource.LUCON));
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                    {
                        page.DefaultTextStyle(TextStyle.Default.FontSize(7).FontFamily("Lucida Console"));

                        page.MarginHorizontal(3);

                        page.ContinuousSize(72.1f, Unit.Millimetre);

                        page.Header().Component(new NFCeCabecalho(_dados.emit));

                        page.Content().Element(ComposeContent);

                        page.Footer().Component(new NFCeRodape(_dados.vTotTrib));
                    }
                );
        }

        public DocumentMetadata GetMetadata()
        {
            var _metadata = DocumentMetadata.Default;
            _metadata.RasterDpi = 72;
            return _metadata;
        }

        private void ComposeContent(IContainer container)
        {
            container.Table(
                table =>
                {
                    table.ColumnsDefinition(column => column.RelativeColumn());

                    table.Cell().Component(new NFCeDetalhe(_dados));
                    table.Cell().Component(new NFCeTotal(_dados.total));
                    table.Cell().Component(new NFCePagamento(_dados.pag));

                    table.Cell().Component(new NFCeConsulta(_dados.consulta));

                    table.Cell().Height(5);

                    table.Cell().Component(new NFCeConsumidor(_dados.dest));

                    table.Cell().Height(5);

                    table.Cell().Component(new NFCeIdentificacao(_dados.identificacao));

                    table.Cell().Height(5);

                    table.Cell().Component(new NFCeQrCode(_dados.qrCode));

                    table.Cell().Height(5);
                }
            );
        }
    }
}
