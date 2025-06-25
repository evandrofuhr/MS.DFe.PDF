using DFe.Classes.Flags;
using DFe.Utils;
using MS.DFe.PDF.Componentes.NF_e;
using MS.DFe.PDF.Resources;
using NFe.Classes;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;



namespace MS.DFe.PDF
{
    public class NFeLeiaute : IDocument
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;
        private byte[] _logoSoftwareHouse;

        public NFeLeiaute(string xml)
        {
            QuestPDF.Settings.License = LicenseType.Community;
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

        public NFeLeiaute(string xml, byte[] logo)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _logo = logo;

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

        public void AdicionarLogoSoftwareHouse(byte[] logo)
        {
            _logoSoftwareHouse = logo;
        }

        private void Validar()
        {
            if (_nfe == null)
                throw new Exception("Não foi possível converter o arquivo XML.");

            if (_nfe.infNFe.ide.mod != ModeloDocumento.NFe)
                throw new Exception("Tipo de documento XML inválido para NF-e.");
        }


        private void ComposeWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120).Text(NFeResource.PRE_VISUALIZACAO).FontSize(60).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeHomWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120).Text(NFeResource.HOMOLOGACAO).FontSize(70).Bold().FontColor(Colors.Grey.Lighten1);
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(3, Unit.Millimetre);
                page.DefaultTextStyle(TextStyle.Default.FontSize(6).FontFamily("Times New Roman"));
                page.PageColor(Colors.White);

                if (_protocolo == null)
                    page.Background().Element(ComposeWaterMark);
                if (_nfe.infNFe.ide.tpAmb == TipoAmbiente.Homologacao)
                    page.Background().Element(ComposeHomWaterMark);

                page.Header().Component(new Cabecalho(_nfe, _protocolo, _logo));
                page.Content().Component(new Conteudo(_nfe));
                page.Footer().ShowOnce().Component(new Rodape(_nfe.infNFe, _logoSoftwareHouse));
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