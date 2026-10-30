using DFe.Classes.Flags;
using DFe.Utils;
using MS.DFe.PDF.Componentes.NF_e;
using MS.DFe.PDF.Componentes.NF_e.Paisagem;
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
    /// DANFE paisagem da NF-e, modelo 55, na mesma NT 2026.010 v1.00 do <see cref="NFeLeiauteRetrato"/>.
    /// </summary>
    /// <remarks>
    /// Os quadros, as condições e a regra de cabeçalho e rodapé em todas as páginas são os do retrato.
    /// A diferença é o canhoto: na primeira página ele ocupa a faixa direita, no lugar do topo.
    /// Nas demais páginas essa faixa some e o miolo usa a largura inteira.
    /// </remarks>
    public class NFeLeiautePaisagem : IDocument
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;
        private byte[] _logoSoftwareHouse;

        public NFeLeiautePaisagem(NFe.Classes.NFe nfe, protNFe protocolo, byte[] logo)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            _nfe = nfe;
            _protocolo = protocolo;
            _logo = logo;

            Validar();
        }

        public NFeLeiautePaisagem(string xml)
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

        public NFeLeiautePaisagem(string xml, byte[] logo)
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
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(80).TranslateX(-80).Text(NFeResource.PRE_VISUALIZACAO).FontSize(55).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeHomWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(80).TranslateX(-80).Text(NFeResource.HOMOLOGACAO).FontSize(70).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private static void ReservarCanhoto(RowDescriptor row)
        {
            row.ConstantItem(CanhotoPaisagem.Largura).ShowIf(ctx => ctx.PageNumber == 1);
        }

        public void Compose(IDocumentContainer container)
        {
            FontesDFe.GarantirTimesNewRoman();

            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
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

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Component(new CabecalhoOperacao(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe, _protocolo, _logo));
                        column.Item().Component(new CabecalhoFiscal(_nfe.infNFe.ide, _nfe.infNFe.emit, _protocolo));
                        column.Item().Component(new RegimeRetrato(_nfe.infNFe.emit.CRT));
                    });
                    ReservarCanhoto(row);
                });

                page.Content().Row(row =>
                {
                    row.RelativeItem().Component(new ConteudoRetrato(_nfe));
                    ReservarCanhoto(row);
                });

                page.Footer().Row(row =>
                {
                    row.RelativeItem().Component(new RodapeRetrato(_nfe.infNFe, _logoSoftwareHouse, _nfe.infNFeSupl?.qrCode));
                    ReservarCanhoto(row);
                });

                page.Foreground()
                    .ShowIf(ctx => ctx.PageNumber == 1)
                    .PaddingTop(3, Unit.Millimetre)
                    .PaddingRight(3, Unit.Millimetre)
                    .PaddingBottom(3, Unit.Millimetre)
                    .AlignRight()
                    .Width(CanhotoPaisagem.Largura)
                    .Component(new CanhotoPaisagem(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe.total.ICMSTot, _nfe.infNFe.dest));
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
