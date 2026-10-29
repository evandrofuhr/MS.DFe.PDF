using DFe.Classes.Flags;
using DFe.Utils;
using MDFe.Classes.Informacoes;
using MDFe.Classes.Retorno;
using MDFe.Classes.Retorno.MDFeRetRecepcao;
using MS.DFe.PDF.Componentes.MDF_e;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace MS.DFe.PDF
{
    public class MDFeLeiaute : IDocument
    {
        private readonly MDFe.Classes.Informacoes.MDFe _mdfe;
        private readonly MDFeProtMDFe _protocolo;
        private readonly byte[] _logo;

        public bool Cancelado { get; set; }
        public bool Encerrado { get; set; }

        public MDFeLeiaute(MDFeProcMDFe proc, byte[] logo = null)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _logo = logo;
            _mdfe = proc?.MDFe;
            _protocolo = proc?.ProtMDFe;
            Validar();
        }

        public MDFeLeiaute(MDFe.Classes.Informacoes.MDFe mdfe, MDFeProtMDFe protocolo, byte[] logo = null)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _logo = logo;
            _mdfe = mdfe;
            _protocolo = protocolo;
            Validar();
        }

        public MDFeLeiaute(string xml, byte[] logo = null)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _logo = logo;

            try
            {
                var proc = FuncoesXml.XmlStringParaClasse<MDFeProcMDFe>(xml);
                _mdfe = proc.MDFe;
                _protocolo = proc.ProtMDFe;
            }
            catch
            {
                _mdfe = FuncoesXml.XmlStringParaClasse<MDFe.Classes.Informacoes.MDFe>(xml);
                _protocolo = null;
            }

            Validar();
        }

        private void Validar()
        {
            if (_mdfe?.InfMDFe == null)
                throw new Exception("Não foi possível converter o arquivo XML.");

            if (_mdfe.InfMDFe.Ide.Mod != ModeloDocumento.MDFe)
                throw new Exception("Tipo de documento XML inválido para MDF-e.");
        }

        private void ComposeWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120)
                .Text(MDFeResource.PRE_VISUALIZACAO).FontSize(55).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeHomWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120)
                .Text(MDFeResource.HOMOLOGACAO).FontSize(70).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeBanner(IContainer container, string texto)
        {
            container.AlignCenter().AlignMiddle()
                .Text(texto).FontSize(40).Bold().FontColor(Colors.Grey.Medium);
        }

        public void Compose(IDocumentContainer container)
        {
            var inf = _mdfe.InfMDFe;
            var temProtocolo = !string.IsNullOrEmpty(_protocolo?.InfProt?.NProt);

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(3, Unit.Millimetre);
                page.DefaultTextStyle(TextStyle.Default.FontSize(9f).FontFamily("Times New Roman"));
                page.PageColor(Colors.White);

                page.Background().Element(backgroundContainer =>
                {
                    if (!temProtocolo)
                        ComposeWaterMark(backgroundContainer);
                    else if (inf.Ide.TpAmb == TipoAmbiente.Homologacao)
                        ComposeHomWaterMark(backgroundContainer);
                    else if (Encerrado)
                        ComposeBanner(backgroundContainer, MDFeResource.DOCUMENTO_ENCERRADO);
                    else if (Cancelado)
                        ComposeBanner(backgroundContainer, MDFeResource.DOCUMENTO_CANCELADO);
                    else
                        backgroundContainer.Background(Colors.White);
                });

                page.Content().Column(column =>
                {
                    column.Item().Component(new Cabecalho(_mdfe, _protocolo, _logo));
                    column.Item().Component(new IdentificacaoResumo(inf));
                    column.Item().Component(new VeiculoCondutor(inf));
                    column.Item().Component(new ValePedagioSeguro(inf));
                    column.Item().Component(new ComposicaoCarga(inf));
                });
            });
        }

        public byte[] Gerar()
        {
            return this.GeneratePdf();
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }

        public DocumentSettings GetSettings()
        {
            return DocumentSettings.Default;
        }
    }
}
