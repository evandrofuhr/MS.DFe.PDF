using DFe.Classes.Flags;
using DFe.Utils;
using MS.DFe.PDF.Componentes.NF_e;
using MS.DFe.PDF.Componentes.NF_e.Retrato;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace MS.DFe.PDF
{
    /// <summary>
    /// DANFE retrato da NF-e, modelo 55, conforme a NT 2026.010 v1.00
    /// (DANFE da Reforma Tributária do Consumo). O <see cref="NFeLeiaute"/> permanece o layout atual.
    /// </summary>
    /// <remarks>
    /// A nota técnica vale para o leiaute inteiro, não só para um quadro:
    /// a ordem é canhoto, identificação, destinatário, fatura, totais dos produtos,
    /// ICMS/IPI, IBS/CBS/IS, ISSQN, transportador e itens.
    /// Campo ou quadro facultativo (azul e tracejado no modelo) só é impresso quando
    /// a tag existe no XML. Nada é inferido.
    /// Cabeçalho de identificação e rodapé saem em todas as páginas; o canhoto, só na primeira.
    /// O tipo de regime de apuração do IBS e da CBS não tem tag publicada (itens 4.2 e 4.4)
    /// e permanece omitido até a NT que definir a origem. A previsão está em <see cref="RegimeRetrato"/>.
    /// </remarks>
    public class NFeLeiauteRetrato : IDocument
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;
        private byte[] _logoSoftwareHouse;

        public NFeLeiauteRetrato(NFe.Classes.NFe nfe, protNFe protocolo, byte[] logo)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            _nfe = nfe;
            _protocolo = protocolo;
            _logo = logo;

            Validar();
        }

        public NFeLeiauteRetrato(string xml)
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

        public NFeLeiauteRetrato(string xml, byte[] logo)
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
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120).Text(NFeResource.PRE_VISUALIZACAO).FontSize(55).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeHomWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120).Text(NFeResource.HOMOLOGACAO).FontSize(70).Bold().FontColor(Colors.Grey.Lighten1);
        }

        public void Compose(IDocumentContainer container)
        {
            FontesDFe.GarantirTimesNewRoman();

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(3, Unit.Millimetre);
                page.DefaultTextStyle(TextStyle.Default.FontSize(9f).FontFamily(FontesDFe.TimesNewRoman));
                page.PageColor(Colors.White);

                page.Background().Element(backgroundContainer =>
                {
                    if (_protocolo == null)
                        ComposeWaterMark(backgroundContainer);
                    else if (_nfe.infNFe.ide.tpAmb == TipoAmbiente.Homologacao)
                        ComposeHomWaterMark(backgroundContainer);
                    else
                        backgroundContainer.Background(Colors.White);
                });

                page.Header().Column(column =>
                {
                    column.Item().ShowOnce().Component(new CanhotoRetrato(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe.total.ICMSTot, _nfe.infNFe.dest));
                    column.Item().Component(new CabecalhoOperacao(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe, _protocolo, _logo));
                    column.Item().Component(new CabecalhoFiscal(_nfe.infNFe.ide, _nfe.infNFe.emit, _protocolo));
                    column.Item().Component(new RegimeRetrato(_nfe.infNFe.emit.CRT));
                });

                page.Content().Component(new ConteudoRetrato(_nfe));

                page.Footer().Component(new RodapeRetrato(_nfe.infNFe, _logoSoftwareHouse, _nfe.infNFeSupl?.qrCode));
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
