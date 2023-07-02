using MS.DFe.PDF.Componentes.NFCe;
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

            FontManager.RegisterFont(new MemoryStream(NFCeResource.LUCON));
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                    {
                        page.DefaultTextStyle(TextStyle.Default.FontSize(7).FontFamily("Lucida Console"));

                        page.MarginHorizontal(3);

                        page.ContinuousSize(72.1f, Unit.Millimetre);

                        page.Header().Component(new Cabecalho(_dados.emit));

                        page.Content().Element(ComposeContent);

                        page.Footer().Component(new Rodape(_dados.vTotTrib, _dados.comprovante, _dados.emit));
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

                    table.Cell().Component(new Detalhe(_dados));
                    table.Cell().Component(new Total(_dados.total));
                    table.Cell().Component(new Pagamento(_dados.pag));

                    table.Cell().Component(new Consulta(_dados.consulta));

                    table.Cell().Height(5);

                    table.Cell().Component(new Consumidor(_dados.dest));

                    table.Cell().Height(5);

                    table.Cell().Component(new Identificacao(_dados.identificacao));

                    table.Cell().Height(5);

                    table.Cell().Component(new QrCode(_dados.qrCode));

                    table.Cell().Height(5);
                }
            );
        }

        public byte[] Gerar()
        {
            return this.GeneratePdf();
        }
    }
}
