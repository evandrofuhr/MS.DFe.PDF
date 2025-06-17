using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Protocolo;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes;
using System.IO;
using System;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Elementos;
using SkiaSharp;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Componentes.Nfe
{
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
                

                var _complemento = "";
                if (_emit.enderEmit.xBairro != null) _complemento = $" - {_emit.enderEmit.xCpl}";

                var _tipoImg = ImagemHelper.ObterTipoImagem(_logo);

                col.Item().Row(row =>
                {
                    if (_tipoImg.Equals(ETipoImg.QuadradoOuVertical) && _logo != null)
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(5).AlignMiddle().Row(innerRow =>
                            {
                                innerRow.ConstantItem(80).AlignCenter().AlignMiddle().Height(80).Image(_logo);

                                innerRow.ConstantItem(5);

                                innerRow.RelativeItem().Column(innerCol =>
                                {
                                    innerCol.Item().AlignMiddle().AlignCenter().Text(text =>
                                    {
                                        text.Span($"{_emit.xFant}\r\n").FontSize(12).Bold();
                                        text.Span($"{_emit.enderEmit.xLgr}, {_emit.enderEmit.nro.ToString()}{_complemento}, {_emit.enderEmit.xBairro} - {NFeResource.CEP}: {_emit.enderEmit.CEP.ToCep()}\r\n{_emit.enderEmit.xMun} - {_emit.enderEmit.UF}\r\n").FontSize(10);
                                        if(_emit.enderEmit.fone != null) text.Span($"{NFeResource.FONE}: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(11).Bold();
                                    });
                                });
                            });
                        });
                    }
                    else if (_tipoImg.Equals(ETipoImg.Retangular) && _logo != null)
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignMiddle().Column(innerCol =>
                            {
                                innerCol.Item().Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Width(78).Image(_logo);

                                innerCol.Item().Column(_innerCol =>
                                {
                                    _innerCol.Item().AlignCenter().Text(text =>
                                    {
                                        text.Span($"{_emit.xFant}\r\n").FontSize(10).Bold();
                                        text.Span($"{_emit.enderEmit.xLgr}, {_emit.enderEmit.nro}{_complemento}, {_emit.enderEmit.xBairro}\r\n{NFeResource.CEP}: {_emit.enderEmit.CEP.ToCep()} - {_emit.enderEmit.xMun} - {_emit.enderEmit.UF.ToString()}\r\n").FontSize(8);
                                        if (_emit.enderEmit.fone != null) text.Span($"{NFeResource.FONE}: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(9).Bold();
                                    });
                                });
                            });
                        });
                    }
                    else
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92)
                             .Border(DadoPadraoExtensoes.BORDA)
                             .Padding(DadoPadraoExtensoes.PADDING)
                             .Column(innerCol =>
                             {
                                 innerCol.Item().Height(20).AlignCenter().Text(NFeResource.IDENTIFICACAO_EMITENTE);

                                 innerCol.Item().Column(_innerCol =>
                                 {
                                     _innerCol.Item().AlignCenter().AlignMiddle().Text(text =>
                                     {
                                         text.Span($"{_emit.xFant}\r\n").FontSize(12).Bold();
                                         text.Span($"{_emit.enderEmit.xLgr}, {_emit.enderEmit.nro}{_complemento} , {_emit.enderEmit.xBairro} - {NFeResource.CEP}: {_emit.enderEmit.CEP.ToCep()}\r\n{_emit.enderEmit.xMun} - {_emit.enderEmit.UF}\r\n").FontSize(10);
                                         if (_emit.enderEmit.fone != null)
                                             text.Span($"{NFeResource.FONE}: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(11).Bold();
                                     });
                                 });
                             });
                        });

                    }

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
                        innerRow.Item().Height(60).Border(DadoPadraoExtensoes.BORDA).AlignCenter().Padding(3).Image(CodigoDeBarrasHelper.GerarCodigoDeBarras(_chaveDeAcesso));
                        innerRow.Item().Height(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.CHAVE_ACESSO, _chaveDeAcesso.ToChaveNfe()));
                        innerRow.Item().Height(17).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text($"{NFeResource.CONSULTA_AUTENTICIDADE}\r\n{NFeResource.URL_NFE}").AlignCenter().Bold();
                    });
                });
            });
        }
    }
}





