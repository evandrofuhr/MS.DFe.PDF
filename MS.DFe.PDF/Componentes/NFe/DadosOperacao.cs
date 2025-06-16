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

namespace MS.DFe.PDF.Componentes.Nfe
{
    public class DadosOperacao : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly infNFe _infnfe;
        private readonly protNFe _protocolo;

        public DadosOperacao(ide ide, emit emit, infNFe infnfe, protNFe protocolo)
        {
            _ide = ide;
            _emit = emit;
            _infnfe = infnfe;
            _protocolo = protocolo;
        }

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                byte[] _logoBytes = null;

                var caminho = @"D:\projetos\Testes\geradorPdfs\imgr.jpg";
                if (File.Exists(caminho))
                    _logoBytes = File.ReadAllBytes(caminho);

                var _tipoImg = ImagemHelper.ObterTipoImagem(_logoBytes);

                col.Item().Row(row =>
                {
                    if (_tipoImg.Equals(ETipoImg.QuadradoOuVertical) && _logoBytes != null)
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(5).AlignMiddle().Row(innerRow =>
                            {
                                innerRow.ConstantItem(80).Height(80).Image(_logoBytes);

                                innerRow.ConstantItem(5);

                                innerRow.RelativeItem().Column(innerCol =>
                                {
                                    innerCol.Item().AlignMiddle().AlignCenter().Text(text =>
                                    {
                                        text.Span($"{_emit.xFant}\r\n").FontSize(12).Bold();
                                        text.Span($"{_emit.enderEmit.xLgr}, {_emit.enderEmit.nro.ToString()}, {_emit.enderEmit.xBairro} - CEP: {_emit.enderEmit.CEP.ToCep()}\r\n{_emit.enderEmit.xMun} - {_emit.enderEmit.UF}\r\n").FontSize(10);
                                        text.Span($"Fone: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(11).Bold();
                                    });
                                });
                            });
                        });
                    }
                    else if (_tipoImg.Equals(ETipoImg.Retangular) && _logoBytes != null)
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignMiddle().Column(innerCol =>
                            {
                                innerCol.Item().Padding(0f).AlignCenter().Width(78).Image(_logoBytes);

                                innerCol.Item().Column(_innerCol =>
                                {
                                    _innerCol.Item().AlignCenter().Text(text =>
                                    {
                                        text.Span($"{_emit.xFant}\r\n").FontSize(10).Bold();
                                        text.Span($"{_emit.enderEmit.xLgr}, {_emit.enderEmit.nro}, {_emit.enderEmit.xBairro} - CEP: {_emit.enderEmit.CEP.ToCep()}\r\n{_emit.enderEmit.xMun} - {_emit.enderEmit.UF.ToString()}\r\n").FontSize(8);
                                        text.Span($"Fone: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(9).Bold();
                                    });
                                });
                            });
                        });
                    }
                    else
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignMiddle().Column(innerCol =>
                            {
                                innerCol.Item().Column(_innerCol =>
                                {
                                    _innerCol.Item().AlignCenter().Text(text =>
                                    {
                                        text.Span($"{_emit.xFant}\r\n").FontSize(12).Bold();
                                        text.Span($"{_emit.enderEmit.xLgr}, {_emit.enderEmit.nro.ToString()}, {_emit.enderEmit.xBairro} - CEP: {_emit.enderEmit.CEP.ToCep()}\r\n{_emit.enderEmit.xMun} - {_emit.enderEmit.UF}\r\n").FontSize(10);
                                        text.Span($"Fone: {_emit.enderEmit.fone?.ToTelefone()}").FontSize(11).Bold();
                                    });
                                });
                            });
                        });
                    }

                    row.RelativeItem(2).Element(c =>
                    {
                        c.Height(92).Border(DadoPadraoExtensoes.BORDA).Padding(3).AlignMiddle().Column(innerCol =>
                        {
                            innerCol.Item().Text("DANFE").FontSize(10).Bold().AlignCenter();
                            innerCol.Item().Text("Documento Auxiliar da\r\nNota Fiscal Eletrônica").FontSize(7).AlignCenter();
                            innerCol.Item().Row(innerRow =>
                            {
                                innerRow.RelativeItem(3).Text("0 - ENTRADA\r\n1 - SAÍDA").FontSize(8).AlignCenter();

                                innerRow.ConstantItem(13)
                                .Height(17)
                                    .Border(DadoPadraoExtensoes.BORDA)
                                    .AlignCenter()
                                    .Text(_ide.tpNF == 0 ? "0" : "1")
                                    .FontSize(12)
                                    .Bold();

                            });

                            innerCol.Item().Text($"Nº.: {_ide.nNF.ToNumeroNfe()}\r\nSérie: {_ide.serie.ToString()}").FontSize(8).AlignCenter().Bold();

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
                        innerRow.Item().Height(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("CHAVE DE ACESSO", _chaveDeAcesso.ToChaveNfe()));
                        innerRow.Item().Height(17).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text("Consulta de autenticidade no portal nacional da NF-e\r\nwww.nfe.fazenda.gov.br/portal ou no site da Sefaz Autorizadora").AlignCenter().Bold();
                    });
                });
            });
        }
    }
}





