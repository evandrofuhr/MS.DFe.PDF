using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class DadosOperacaoEmitente : IComponent
    {
        private readonly emit _emit;
        private readonly byte[] _logo;

        public DadosOperacaoEmitente(emit emit, byte[] logo)
        {
            _emit = emit;
            _logo = logo;
        }

        private void ComposeSemLogo(IContainer container)
        {
            container
                .Height(92)
                .Border(DadoPadraoExtensoes.BORDA)
                .Padding(DadoPadraoExtensoes.PADDING)
                .Column(
                    _column =>
                    {
                        _column.Item().Height(20).AlignCenter().Text(NFeResource.IDENTIFICACAO_EMITENTE);
                        _column.Item().AlignCenter().AlignMiddle().Element(e => ComposeText(e, 12f));
                    }
                );
        }

        private void ComposeText(IContainer container, float fontSizeBase = 12f)
        {
            container.AlignCenter().Text(
                text =>
                {
                    text.Line(_emit.xFant).FontSize(fontSizeBase).Bold();
                    text.Line(_emit.enderEmit.ToEndereco1()).FontSize(fontSizeBase - 2f);
                    text.Line(_emit.enderEmit.ToEndereco2()).FontSize(fontSizeBase - 2f);
                    if (_emit.enderEmit.fone != null) text.Line($"{NFeResource.FONE}: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(fontSizeBase - 1f).Bold();
                }
            );
        }

        private void ComposeComLogo(IContainer container)
        {
            var _tipo = ImagemHelper.TipoImagem(_logo);

            if (_tipo == ETipoImg.QuadradoOuVertical)
            {
                container.Height(92)
                    .Border(DadoPadraoExtensoes.BORDA)
                    .Padding(5)
                    .AlignMiddle()
                    .Row(
                        row =>
                        {
                            row.ConstantItem(80).AlignCenter().AlignMiddle().Height(80).Image(_logo);
                            row.ConstantItem(5);
                            row.RelativeItem().AlignMiddle().AlignCenter().Element(e => ComposeText(e, 12f));
                        }
                    );
            }
            else
            {

                container.Height(92)
                    .Border(DadoPadraoExtensoes.BORDA)
                    .Padding(DadoPadraoExtensoes.PADDING)
                    .AlignMiddle()
                    .Column(
                        column =>
                        {
                            column.Item().AlignCenter().Height(34).Image(_logo);
                            column.Item().Height(1f).ShrinkHorizontal();
                            column.Item().AlignCenter().Element(e => ComposeText(e, 10f));
                        }
                    );

            }
        }

        public void Compose(IContainer container)
        {
            if (_logo == null)
                container.Element(ComposeSemLogo);
            else
                container.Element(ComposeComLogo);
        }
    }

    public class DadosOperacao : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly infNFe _infnfe;
        private readonly protNFe _protocolo;
        private readonly byte[] _logo;

        public DadosOperacao(ide ide, emit emit, infNFe infnfe, protNFe protocolo, byte[] logo)
        {
            _ide = ide;
            _emit = emit;
            _infnfe = infnfe;
            _protocolo = protocolo;
            _logo = logo;
        }

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(
                    row =>
                    {
                        row.RelativeItem(5).Component(new DadosOperacaoEmitente(_emit, _logo));


                        row.RelativeItem(2).Element(c =>
                        {
                            c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(1).AlignMiddle().Column(innerCol =>
                            {
                                innerCol.Item().Text(NFeResource.DANFE).FontSize(10).Bold().AlignCenter().LineHeight(1.5f);
                                innerCol.Item().Text(NFCeResource.DOC_AUX).FontSize(6.6f).AlignCenter().LineHeight(1);
                                innerCol.Item().Height(3);
                                innerCol.Item().Row(innerRow =>
                                {
                                    innerRow.RelativeItem(3).Text($"{NFeResource.ENTRADA}\r\n{NFeResource.SAIDA}").FontSize(8).AlignCenter();

                                    innerRow.ConstantItem(13)
                                    .Height(17)
                                        .Border(DadoPadraoExtensoes.BORDA)
                                        .AlignCenter()
                                        .Text(_ide.tpNF == 0 ? "0" : "1")
                                        .FontSize(12)
                                        .Bold();

                                });

                                innerCol.Item().Height(3);

                                innerCol.Item().Text($"{NFeResource.NUMERO}.: {_ide.nNF.ToNumeroNfe()}\r\n{NFeResource.SERIE} {_ide.serie.ToString()}").FontSize(8).AlignCenter().Bold();

                                innerCol.Item().Element(innerContainer =>
                                {
                                    innerContainer.AlignCenter().Text(text =>
                                    {
                                        text.Span("Folha ").FontSize(8).Bold();
                                        text.CurrentPageNumber().FontSize(8).Bold();
                                        text.Span("/").FontSize(8).Bold();
                                        text.TotalPages().FontSize(8).Bold();
                                    });
                                });

                            });
                        });

                        var _chaveDeAcesso = _protocolo?.infProt.chNFe.ToString() ?? _infnfe.Id?.SomenteNumeros() ?? string.Empty.PadLeft(44, '0');

                        row.RelativeItem(5).Column(innerRow =>
                        {
                            innerRow.Item().Height(60).Border(DadoPadraoExtensoes.BORDA).AlignCenter().Padding(3).Image(BarCodeHelper.Barcode128(_chaveDeAcesso));
                            innerRow.Item().Height(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.CHAVE_ACESSO, _chaveDeAcesso.FormataChaveNFe()));
                            innerRow.Item().Height(17).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text($"{NFeResource.CONSULTA_AUTENTICIDADE}\r\n{NFeResource.URL_NFE}").AlignCenter().Bold();
                        });
                    });
            });
        }
    }
}





