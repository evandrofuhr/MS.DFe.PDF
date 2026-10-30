using CTe.Classes;
using CTe.Classes.Protocolo;
using DFe.Classes.Flags;
using DFe.Utils;
using MS.DFe.PDF.Componentes.CT_e;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using ZeusCTe = CTe.Classes.CTe;

namespace MS.DFe.PDF
{
    public class CTeLeiaute : IDocument
    {
        private readonly ZeusCTe _cte;
        private readonly protCTe _protocolo;
        private readonly byte[] _logo;

        public bool Cancelado { get; set; }
        public bool Encerrado { get; set; }

        public CTeLeiaute(string xml, byte[] logo = null)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _logo = logo;

            try
            {
                var cteProc = FuncoesXml.XmlStringParaClasse<cteProc>(xml);
                _cte = cteProc.CTe;
                _protocolo = cteProc.protCTe;
            }
            catch
            {
                _cte = FuncoesXml.XmlStringParaClasse<ZeusCTe>(xml);
                _protocolo = null;
            }

            Validar();
        }

        private void Validar()
        {
            if (_cte?.infCte == null)
                throw new Exception("Não foi possível converter o arquivo XML.");

            if (_cte.infCte.ide.mod != ModeloDocumento.CTe)
                throw new Exception("Tipo de documento XML inválido para CT-e.");
        }

        private void ComposeWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120)
                .Text(CTeResource.PRE_VISUALIZACAO).FontSize(55).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeHomWaterMark(IContainer container)
        {
            container.AlignCenter().AlignMiddle().Rotate(-45).TranslateY(200).TranslateX(-120)
                .Text(CTeResource.HOMOLOGACAO).FontSize(70).Bold().FontColor(Colors.Grey.Lighten1);
        }

        private void ComposeBanner(IContainer container, string texto)
        {
            container.AlignCenter().AlignMiddle()
                .Text(texto).FontSize(40).Bold().FontColor(Colors.Grey.Medium);
        }

        public void Compose(IDocumentContainer container)
        {
            FontesDFe.GarantirTimesNewRoman();

            var inf = _cte.infCte;
            var temProtocolo = !string.IsNullOrEmpty(_protocolo?.infProt?.nProt);

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(3, Unit.Millimetre);
                page.DefaultTextStyle(TextStyle.Default.FontSize(9f).FontFamily(FontesDFe.TimesNewRoman));
                page.PageColor(Colors.White);

                page.Background().Element(backgroundContainer =>
                {
                    if (!temProtocolo)
                        ComposeWaterMark(backgroundContainer);
                    else if (inf.ide.tpAmb == TipoAmbiente.Homologacao)
                        ComposeHomWaterMark(backgroundContainer);
                    else if (Cancelado)
                        ComposeBanner(backgroundContainer, CTeResource.DOCUMENTO_CANCELADO);
                    else
                        backgroundContainer.Background(Colors.White);
                });

                page.Content().Column(column =>
                {
                    column.Item().Component(new Canhoto(inf));
                    column.Item().Component(new Cabecalho(_cte, _protocolo, _logo));
                    column.Item().Component(new Identificacao(inf));
                    column.Item().Component(new Prestacao(inf.ide));
                    column.Item().Component(new Participantes(inf));

                    if (inf.ide.tpCTe == CTe.Classes.Informacoes.Tipos.tpCTe.Normal)
                    {
                        column.Item().Component(new Carga(inf.infCTeNorm));
                        column.Item().Component(new Documentos(inf.infCTeNorm));
                        column.Item().Component(new ModalRodoviario(inf));
                    }

                    column.Item().Component(new Valores(inf.vPrest));
                    column.Item().Component(new Impostos(inf.imp));
                    column.Item().Component(new Observacoes(inf));
                    column.Item().Component(new RodapeExclusivo());
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
