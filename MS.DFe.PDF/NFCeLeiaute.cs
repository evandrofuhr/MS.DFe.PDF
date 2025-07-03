using DFe.Classes.Flags;
using DFe.Utils;
using MS.DFe.PDF.Componentes.NFCe;
using MS.DFe.PDF.Resources;
using NFe.Classes;
using NFe.Classes.Protocolo;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;



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

        private void ComposeWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(-30).TranslateX(55).Text(NFeResource.PRE_VISUALIZACAO).FontSize(20).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeHomWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(-30).TranslateX(45).Text(NFeResource.HOMOLOGACAO).FontSize(30).Bold().FontColor(Colors.Grey.Lighten1);
        }


        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                    {
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(TextStyle.Default.FontSize(7).FontFamily("Lucida Console"));

                        page.MarginHorizontal(3);

                        page.ContinuousSize(72.1f, Unit.Millimetre);

                        page.Background().Element(backgroundContainer =>
                        {
                            if (_nfe.infNFe.ide.tpAmb == null || _nfe.infNFe.ide.tpAmb ==0)
                                ComposeWaterMark(backgroundContainer);
                            else if (_nfe.infNFe.ide.tpAmb == TipoAmbiente.Homologacao)
                                ComposeHomWaterMark(backgroundContainer);
                            else
                                backgroundContainer.Background(Colors.White);
                        });

                        page.Header().Component(new Cabecalho(_nfe.infNFe.emit));

                        page.Content().Element(ComposeContent);

                        page.Footer().Component(new Rodape(_nfe.infNFe));
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
            container.Table(table =>
            {
                table.ColumnsDefinition(column => column.RelativeColumn());

                table.Cell().Component(new Detalhe(_nfe));
                table.Cell().Component(new Total(_nfe.infNFe.total));
                table.Cell().Component(new Pagamento(_nfe.infNFe.pag));

                if (_nfe.infNFeSupl != null || _protocolo?.infProt != null)
                {
                    table.Cell().Component(new Consulta(_nfe.infNFeSupl, _protocolo?.infProt));
                }

                table.Cell().Height(5);

                if (_nfe.infNFe.dest != null)
                {
                    table.Cell().Component(new Consumidor(_nfe.infNFe.dest));
                }

                table.Cell().Height(5);

                table.Cell().Component(new Identificacao(_nfe.infNFe.ide, _protocolo?.infProt));

                table.Cell().Height(5);

                if (!string.IsNullOrWhiteSpace(_nfe.infNFeSupl?.qrCode))
                {
                    table.Cell().Component(new QrCode(_nfe.infNFeSupl.qrCode));
                    table.Cell().Height(5);
                }
            });
        }


        private void Validar()
        {
            if (_nfe == null) throw new Exception("Não foi possível converter o arquivo XML.");

            if (_nfe.infNFe.ide.mod != ModeloDocumento.NFCe) throw new Exception("Tipo de documento XML inválido para NFC-e.");
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
