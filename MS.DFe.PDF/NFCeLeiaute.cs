using DFe.Utils;
using MS.DFe.PDF.Componentes.NFCe;
using MS.DFe.PDF.Resources;
using NFe.Classes.Protocolo;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO;
using NFe.Classes;
using System;
using DFe.Classes.Flags;
using MS.DFe.PDF.Modelos;
using QuestPDF.Helpers;



namespace MS.DFe.PDF
{
    public class NFCeLeiaute : IDocument
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        public NFCeLeiaute(string xml)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            FontManager.RegisterFont(new MemoryStream(NFCeResource.LUCON));

            try
            {
                var nfeProc = FuncoesXml.XmlStringParaClasse<nfeProc>(xml);
                _nfe = nfeProc.NFe;
                _protocolo = nfeProc.protNFe;
            }
            catch
            {
                _nfe = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xml);
                _protocolo = null;
            }

            Validar();

        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                    {
                        page.DefaultTextStyle(TextStyle.Default.FontSize(7).FontFamily("Lucida Console"));

                        page.MarginHorizontal(3);

                        page.ContinuousSize(72.1f, Unit.Millimetre);

                        page.Header().Component(new Cabecalho(_nfe.infNFe.emit));

                        page.Content().Element(ComposeContent);

                        page.Footer().Component(
                        new Rodape(
                            _nfe.infNFe.total.ICMSTot.vTotTrib,
                            new DFeDadosComprovante(new[]
                            {
                                "Cartão de Débito: Visa",
                                "Data/Hora: 02/07/2023 20:02",
                                "Valor: R$ 120,00",
                                "NSU: 1023882"
                            }),
                            _nfe.infNFe.emit
                        )
                    );
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

                    table.Cell().Component(new Detalhe(_nfe));
                    table.Cell().Component(new Total(_nfe.infNFe.total));
                    table.Cell().Component(new Pagamento(_nfe.infNFe.pag));

                    table.Cell().Component(new Consulta(_nfe.infNFeSupl, _protocolo.infProt));

                    table.Cell().Height(5);

                    table.Cell().Component(new Consumidor(_nfe.infNFe.dest));

                    table.Cell().Height(5);

                    table.Cell().Component(new Identificacao(_nfe.infNFe.ide, _protocolo?.infProt));

                    table.Cell().Height(5);

                    table.Cell().Component(new QrCode(_nfe.infNFeSupl.qrCode ?? string.Empty));


                    table.Cell().Height(5);
                }
            );
        }

        private void Validar()
        {
            if (_nfe == null) throw new Exception("Não foi possível converter o arquivo XML.");

            if (_nfe.infNFe.ide.mod != ModeloDocumento.NFCe) throw new Exception("Tipo de documento XML inválido para NF-e.");
        }

        public byte[] Gerar()
        {
            return this.GeneratePdf();
        }

        public DocumentSettings GetSettings()
        {
            return new DocumentSettings { };
        }

    }
}
