using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ZXing;
using ZXing.Common;
using NFe.Classes.Protocolo;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System.Text.RegularExpressions;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Transporte;
using NFe.Classes.Informacoes.Cobranca;
using SkiaSharp;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using NFe.Classes;
using NFe.Classes.Informacoes;
using DocumentFormat.OpenXml.Math;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using MS.DFe.PDF.Extensoes;

namespace geradorPdfs
{
    public static class Auxilio
    {

        public const float BORDA = 0.5f;
        public const float PADDING = 1f;

    }

    public static class ImagemFunc
    {
        public enum TipoImg
        {
            SemImagem,
            Retangular,
            QuadradoOuVertical
        }

        public static TipoImg ObterTipoImg(byte[] imagemBytes)
        {
            if (imagemBytes == null || imagemBytes.Length.Equals(0)) return TipoImg.SemImagem;

            var _imagem = SKBitmap.Decode(imagemBytes);

            return _imagem.Width >= _imagem.Height * 1.4 ? TipoImg.Retangular : TipoImg.QuadradoOuVertical;
        }

    }

    public enum EAlinhamento
    {
        centro,
        esquerda,
        direita
    }

    public class CampoTabelaCodigo : CampoTabela
    {
        public CampoTabelaCodigo(string text) : base(text, EAlinhamento.centro)
        {
        }
    }

    public class CampoTabelaValor : CampoTabela
    {
        public CampoTabelaValor(string text) : base(text, EAlinhamento.direita)
        {
        }
    }

    public class CampoTabelaDescricao : CampoTabela
    {
        public CampoTabelaDescricao(string text) : base(text, EAlinhamento.esquerda)
        {
        }
    }
    public class CampoInformativoCodigo : CampoInformativo
    {
        public CampoInformativoCodigo(string label, string text) : base(label, text, EAlinhamento.centro)
        {

        }
    }

    public class CampoInformativoData : CampoInformativo
    {
        public CampoInformativoData(string label, DateTime? dateTime) : base(label, dateTime?.ToString("dd/MM/yyyy") ?? string.Empty, EAlinhamento.centro)
        {

        }
    }


    public class CampoInformativoHora : CampoInformativo
    {
        public CampoInformativoHora(string label, DateTimeOffset? dateTime) : base(label, dateTime?.ToString("HH:mm") ?? string.Empty, EAlinhamento.centro)
        {

        }
    }

    public class CampoInformativoValor : CampoInformativo
    {
        public CampoInformativoValor(string label, string text) : base(label, text, EAlinhamento.direita)
        {

        }
    }

    public class CampoInformativo : IComponent
    {
        private readonly string _label;
        private readonly string _text;
        private readonly EAlinhamento _alinhamento;

        public CampoInformativo(string label, string text, EAlinhamento alinhamento = EAlinhamento.esquerda)
        {
            _label = label;
            _text = text ?? string.Empty;
            _alinhamento = alinhamento;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().AlignLeft().Text(_label).FontSize(5).LineHeight(1).SemiBold();
                    var _container = column.Item();

                    if (_alinhamento == EAlinhamento.centro)
                        _container = _container.AlignCenter();
                    else if (_alinhamento == EAlinhamento.esquerda)
                        _container = _container.AlignLeft();
                    else if (_alinhamento == EAlinhamento.direita)
                        _container = _container.AlignRight();

                    _container.Text(_text).FontSize(7).Bold().LineHeight(1);
                }
            );
        }
    }

    public class CampoTabela : IComponent
    {
        private readonly string _text;
        private readonly EAlinhamento _alinhamento;


        public CampoTabela(string text, EAlinhamento alinhamento = EAlinhamento.esquerda)
        {
            _text = text;
            _alinhamento = alinhamento;
        }

        public void Compose(IContainer container)
        {
            container
               .Column(column =>
               {
                   var _container = column.Item();

                   if (_alinhamento == EAlinhamento.centro)
                       _container = _container.AlignCenter();
                   else if (_alinhamento == EAlinhamento.esquerda)
                       _container = _container.AlignLeft();
                   else if (_alinhamento == EAlinhamento.direita)
                       _container = _container.AlignRight();

                   _container.Padding(1).Text(_text).FontSize(6).LineHeight(1);
               });
        }
    }

    public class FaturaDuplicata : IComponent
    {
        private readonly cobr _cobr;

        public FaturaDuplicata(cobr cobr)
        {
            _cobr = cobr;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Padding(Auxilio.PADDING).Text("FATURA / DUPLICATA").SemiBold();

                const int maxPorLinha = 14;
                int total = _cobr.dup.Count;
                int blocos = (int)Math.Ceiling(total / (double)maxPorLinha);

                for (int b = 0; b < blocos; b++)
                {
                    var duplicatasBloco = _cobr.dup
                        .Skip(b * maxPorLinha)
                        .Take(maxPorLinha)
                        .ToList();

                    column.Item().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderLeft(Auxilio.BORDA).BorderTop(Auxilio.BORDA)
                                .Padding(Auxilio.PADDING).Text("Número:").Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).BorderTop(Auxilio.BORDA)
                                    .Padding(2).AlignCenter().Text(item.nDup).SemiBold();
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderLeft(Auxilio.BORDA)
                                .Padding(Auxilio.PADDING).Text("Vencimento:").Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA)
                                    .Padding(2).AlignCenter().Text(item.dVenc?.ToString("dd/MM/yyyy")).SemiBold();
                            }
                        });

                        col.Item().Row(row =>
                        {
                            row.ConstantItem(37).BorderBottom(Auxilio.BORDA).BorderLeft(Auxilio.BORDA)
                                .Padding(Auxilio.PADDING).Text("Valor:").Bold();

                            foreach (var item in duplicatasBloco)
                            {
                                row.ConstantItem(38.64f).BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).BorderBottom(Auxilio.BORDA)
                                    .Padding(2).AlignCenter().Text(item.vDup.ToString()).SemiBold();
                            }
                        });
                    });
                }
            });
        }



    }

    public class CalculoImposto : IComponent
    {

        private readonly ICMSTot _icmsTot;

        public CalculoImposto(ICMSTot icmsTot)
        {
            _icmsTot = icmsTot;
        }
        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().Padding(Auxilio.PADDING).Text("CALCULO DE IMPOSTO").SemiBold();

                    column.Item().Row(col =>
                    {
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("BASE DE CÁLC. DO ICMS", _icmsTot.vBC.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO ICMS", _icmsTot.vICMS.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("BASE DE CÁLC. ICMS S.T", _icmsTot.vBCST.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO ICMS SUBST.", _icmsTot.vST.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("V. IMP. IMPORTAÇÃO", _icmsTot.vII.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("V. ICMS UF REMET", _icmsTot.vICMSUFRemet.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO FCP", _icmsTot.vFCP.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO PIS", _icmsTot.vPIS.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("V. TOTAL PRODUTOS", _icmsTot.vProd.ToString()));
                    });

                    column.Item().Row(col =>
                    {
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO FRETE", _icmsTot.vFrete.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO SEGURO", _icmsTot.vSeg.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("DESCONTO", _icmsTot.vDesc.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("OUTRAS DESPESAS", _icmsTot.vOutro.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR IPI", _icmsTot.vIPI.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("V. ICMS UF DEST", _icmsTot.vICMSUFDest.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("V. TOT. TRIB.", _icmsTot.vTotTrib.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR DO COFINS", _icmsTot.vCOFINS.ToString()));
                        col.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("VALOR TOTAL DA NOTA", _icmsTot.vNF.ToString()));
                    });
                }
            );
        }
    }

    public class Transportador : IComponent
    {
        private readonly transp _transp;

        public Transportador(transp transp)
        {
            _transp = transp;
        }
        public void Compose(IContainer container)
        {

            var _frete = "";

            switch (Convert.ToInt32(_transp.modFrete))
            {
                case 0:
                    _frete = "0 - Contratação do Frete por conta do Remetente";
                    break;
                case 1:
                    _frete = "1 - Contratação do Frete por conta do Destinatário";
                    break;

                case 2:
                    _frete = "2 - Contratação do Frete por conta de Terceiros";
                    break;
                case 3:
                    _frete = "3 - Transporte Próprio por conta do Remetente";
                    break;
                case 4:
                    _frete = "4 - Transporte Próprio por conta do Destinatário";
                    break;

                case 9:
                    _frete = "9 - Sem Ocorrência de Transporte";
                    break;
                default:
                    _frete = "";
                    break;
            }
            container.Column(
                column =>
                {
                    column.Item().Padding(Auxilio.PADDING).Text("TRANSPORTADOR / VOLUMES TRANSPORTADOS").SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(7).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("RAZÃO SOCIAL", _transp.transporta?.xNome));
                        row.RelativeItem(6).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("FRETE", _frete));
                        row.RelativeItem(2).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("CÓDIGO ANTT", string.Empty));
                        row.RelativeItem(2).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("PLACA DO VEÍCULO", _transp.veicTransp?.placa));
                        row.ConstantItem(15).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("UF", _transp.veicTransp?.UF));
                        row.ConstantItem(69).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("CNPJ / CPF", _transp.transporta?.CNPJ.ToCnpjCpf() ?? _transp.transporta?.CPF.ToCnpjCpf()));
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(13).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("ENDEREÇO", _transp.transporta?.xEnder));
                        row.RelativeItem(9).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("MUNICÍPIO", _transp.transporta?.xMun));
                        row.ConstantItem(15).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("UF", _transp.transporta?.UF));
                        row.ConstantItem(69).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("INSCRIÇÃO ESTADUAL", _transp.transporta?.IE));
                    });

                    var _volume = _transp.vol.GroupBy(g => true).Select(s => new
                    {
                        Quantidade = s.Sum(t => t.qVol ?? 0),
                        PesoLiquido = s.Sum(t => t.pesoL ?? 0),
                        PesoBruto = s.Sum(t => t.pesoB ?? 0),
                        Especie = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.esp)).Select(t => t.esp).Distinct())
                    }).FirstOrDefault();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(5).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("QUANTIDADE", _volume?.Quantidade.ToString("F3")));
                        row.RelativeItem(6).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("ESPÉCIE", _volume?.Especie));
                        row.RelativeItem(5).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("MARCA", ""));
                        row.RelativeItem(4).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("NUMERAÇÃO", ""));
                        row.RelativeItem(8).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("PESO BRUTO", _volume?.PesoBruto.ToString()));
                        row.ConstantItem(80).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoValor("PESO LÍQUIDO", _volume?.PesoLiquido.ToString()));
                    });
                }
            );
        }
    }

    public class DestinatarioRemetente : IComponent
    {
        private readonly dest _dest;
        private readonly ide _ide;

        public DestinatarioRemetente(dest dest, ide ide)
        {
            _dest = dest;
            _ide = ide;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().Padding(Auxilio.PADDING).Text("DESTINATARIO / REMETENTE").SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(6).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("RAZÃO SOCIAL", _dest.xNome));
                        row.RelativeItem(3).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("CNPJ / CPF", (_dest.CNPJ ?? _dest.CPF).FormataCNPJCPF()));
                        row.RelativeItem(2).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoData("DATA DE EMISSÃO", _ide.dhEmi.DateTime));
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(4).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("ENDEREÇO", _dest.enderDest?.ToEndereco()));
                        row.RelativeItem(3).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("BAIRRO / DISTRITO", _dest.enderDest?.xBairro));
                        row.RelativeItem(2).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("CEP", _dest.enderDest?.CEP?.ToCep()));
                        row.RelativeItem(2).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoData("DATA ENTRADA / SAÍDA", _ide.dhSaiEnt?.DateTime));
                    });
                    column.Item().Row(col =>
                    {
                        col.RelativeItem(7).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("MUNICÍPIO", _dest.enderDest?.xMun));
                        col.RelativeItem(5).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("FONE / FAX", _dest.enderDest?.fone?.ToTelefone()));
                        col.RelativeItem(1).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("UF", _dest.enderDest?.UF));
                        col.RelativeItem(5).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("INSCRIÇÃO ESTADUAL", _dest.IE));
                        col.RelativeItem(4).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoHora("HORA ENTRADA / SAÍDA", _ide.dhSaiEnt));
                    });
                }
            );
        }
    }
    public class HeaderCanhoto : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly ICMSTot _icmstot;
        private readonly dest _dest;

        public HeaderCanhoto(ide ide, emit emit, ICMSTot icmstot, dest dest)
        {
            _ide = ide;
            _emit = emit;
            _icmstot = icmstot;
            _dest = dest;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(coll =>
                        {
                            coll.Item().Height(26).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Text($"Recebemos de {_emit.xNome} os produtos e/ou serviços constantes na Nota Fiscal Eletrônica indicada abaixo.\r\nEmissão: {_ide.dhEmi.ToString("dd/MM/yyyy")} Valor Total: {_icmstot.vNF.ToString()} Destinatário: {_dest.xNome}").FontSize(7);
                            coll.Item().Row(innerRow =>
                            {
                                innerRow.ConstantItem(244).Height(25).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Text("DATA DE RECEBIMENTO");
                                innerRow.ConstantItem(244).Height(25).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Text("IDENTIFICAÇÃO E ASSINATURA DO RECEBEDOR").AlignLeft().AlignStart().FontSize(7);
                            }); ;
                        });
                        row.ConstantItem(90)
                        .Border(Auxilio.BORDA)
                        .Padding(Auxilio.PADDING)
                        .AlignMiddle()
                        .AlignCenter()
                        .Element(innerContainer => innerContainer.Column(innerCol =>
                        {
                            innerCol.Item().AlignCenter().Text("NF-e").Bold().FontSize(14);
                            innerCol.Item().AlignCenter().Text($"Nº {_ide.nNF.ToNumeroNfe()}").FontSize(12);
                            innerCol.Item().AlignCenter().Text($"Série: {_ide.serie.ToString()}").FontSize(12);
                        }));

                    });
                    col.Item().PaddingVertical(7).LineHorizontal(1);
                }
            );
        }
    }


    public class HeaderIdentificacaoOperacao : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly infNFe _infnfe;
        private readonly protNFe _protocolo;




        public HeaderIdentificacaoOperacao(ide ide, emit emit, infNFe infnfe, protNFe protocolo)
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

                var _tipoImg = ImagemFunc.ObterTipoImg(_logoBytes);

                col.Item().Row(row =>
                {
                    if (_tipoImg.Equals(ImagemFunc.TipoImg.QuadradoOuVertical) && _logoBytes != null)
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(Auxilio.BORDA).Padding(5).AlignMiddle().Row(innerRow =>
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
                    else if (_tipoImg.Equals(ImagemFunc.TipoImg.Retangular) && _logoBytes != null)
                    {
                        row.RelativeItem(5).Element(c =>
                        {
                            c.Height(92).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignMiddle().Column(innerCol =>
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
                            c.Height(92).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignMiddle().Column(innerCol =>
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
                        c.Height(92).Border(Auxilio.BORDA).Padding(3).AlignMiddle().Column(innerCol =>
                        {
                            innerCol.Item().Text("DANFE").FontSize(10).Bold().AlignCenter();
                            innerCol.Item().Text("Documento Auxiliar da\r\nNota Fiscal Eletrônica").FontSize(7).AlignCenter();
                            innerCol.Item().Row(innerRow =>
                            {
                                innerRow.RelativeItem(3).Text("0 - ENTRADA\r\n1 - SAÍDA").FontSize(8).AlignCenter();

                                innerRow.ConstantItem(13)
                                .Height(17)
                                    .Border(Auxilio.BORDA)
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
                        innerRow.Item().Height(60).Border(Auxilio.BORDA).AlignCenter().Padding(3).Image(FuncoesEmplementares.GerarCodigoDeBarras(_chaveDeAcesso));
                        innerRow.Item().Height(15).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("CHAVE DE ACESSO", _chaveDeAcesso.ToChaveNfe()));
                        innerRow.Item().Height(17).Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Text("Consulta de autenticidade no portal nacional da NF-e\r\nwww.nfe.fazenda.gov.br/portal ou no site da Sefaz Autorizadora").AlignCenter().Bold();
                    });
                });
            });
        }
    }

    public class HeaderInfoComple : IComponent
    {

        private readonly ide _ide;
        private readonly emit _emit;
        private readonly protNFe _protocolo;

        public HeaderInfoComple(ide ide, emit emit, protNFe protocolo)
        {
            _ide = ide;
            _emit = emit;
            _protocolo = protocolo;
        }

        public void Compose(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Column(innerCol =>
                    {
                        innerCol.Item().Row(innerRow =>
                        {
                            innerRow.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativo("NATUREZA DA OPERAÇÃO", _ide.natOp));
                            innerRow.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("PROTOCOLO DE AUTORIZAÇÃO", $"{_protocolo?.infProt.nProt} - {_protocolo?.infProt.dhRecbto.DateTime}"));
                        });
                        innerCol.Item().Row(innerRow =>
                        {
                            innerRow.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("INSCRIÇÃO ESTADUAL", _emit.IE.ToString()));
                            innerRow.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("INSCRIÇÃO ESTADUAL DO SUBST. TRIBUTÁRIO", (_emit.IEST?.ToString()) ?? ""));
                            innerRow.RelativeItem().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoInformativoCodigo("CNPJ", _emit.CNPJ.FormataCNPJCPF()));
                        });
                    });
                });
            });
        }
    }
    public class Itens : IComponent
    {

        private class DadosIPI
        {
            public decimal? Aliq { get; set; }
            public decimal? Valor { get; set; }
        }

        private class DadosICMS
        {
            public int Origem { get; set; }
            public string CST { get; set; }
            public decimal? Base { get; set; }
            public decimal? Aliquota { get; set; }
            public decimal? Valor { get; set; }

            public string OrigemCST
            {
                get
                {
                    return $"{Origem}{CST?.ToString().PadLeft(2, '0')}";
                }
            }

        }

        private readonly CRT _crt;
        private readonly List<det> _det;
        private readonly int _casaValorUnitario;

        public Itens(List<det> det, CRT crt)
        {
            _det = det;
            _crt = crt;

            _casaValorUnitario = _det.Max(x => x.prod.vUnCom.QuantidadeDecimais());
        }


        private DadosICMS GetDadosICMS(det item)
        {
            var value = item.imposto?.ICMS;

            if (value == null) return null;

            var _type = value.TipoICMS.GetType();
            var _origem = _type.GetProperty("orig")?.GetValue(value.TipoICMS);
            var _cst = _type.GetProperty("CST")?.GetValue(value.TipoICMS);
            var _csosn = _type.GetProperty("CSOSN")?.GetValue(value.TipoICMS);
            var _base = _type.GetProperty("vBC")?.GetValue(value.TipoICMS);
            var _aliquota = _type.GetProperty("pICMS")?.GetValue(value.TipoICMS);
            var _icms = _type.GetProperty("vICMS")?.GetValue(value.TipoICMS);

            var _cstEnum = string.Empty;

            if (_cst != null)
            {
                _cstEnum = ((Csticms)_cst).ToString().SomenteNumeros();
            }
            else if (_csosn != null)
            {
                _cstEnum = ((Csosnicms)_csosn).ToString().SomenteNumeros();
            }

            return new DadosICMS
            {
                Origem = Convert.ToInt32(_origem),
                CST = _cstEnum,
                Base = _base == null || string.IsNullOrWhiteSpace(_base.ToString())
            ? 0m
            : Convert.ToDecimal(_base),
                Aliquota = Convert.ToDecimal(_aliquota),
                Valor = Convert.ToDecimal(_icms),
            };
        }

        private DadosIPI GetDadosIPI(det item)
        {
            var _value = item.imposto?.IPI;


            if (_value == null) return null;

            var _type = _value.TipoIPI.GetType();
            var _aliq = _type.GetProperty("pIPI")?.GetValue(_value.TipoIPI);
            var _valor = _type.GetProperty("vIPI")?.GetValue(_value.TipoIPI);

            return new DadosIPI
            {
                Aliq = Convert.ToDecimal(_aliq),
                Valor = Convert.ToDecimal(_valor)
            };



        }

        public void Compose(IContainer container)
        {
            var _cabecalho = _crt == CRT.RegimeNormal ? "CST" : "O/CSOSN";

            container
                .Column(column =>
                {
                    column.Item().Padding(Auxilio.PADDING).Text("DADOS DOS PRODUTOS / SERVICOS").SemiBold();

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(140);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).Text("CÓDIGO\r\nPRODUTO").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("DESCRIÇÃO DO PRODUTO / SERVIÇO").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("NCM/SH").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text(_cabecalho).Bold().FontSize(5);
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("CFOP").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("UN").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("QUANTI").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("VALOR\r\nUNIT.").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("VALOR\r\nTOTAL").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("B CÁLC\r\nICMS\r\n").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("VALOR\r\nICMS\r\n").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("VALOR\r\nIPI").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("ALIQ.\r\nICMS").Bold();
                            header.Cell().Border(Auxilio.BORDA).Padding(Auxilio.PADDING).AlignCenter().Text("ALIQ.\r\nIPI").Bold();
                        });

                        foreach (var _item in _det)
                        {
                            var _dadosICMS = GetDadosICMS(_item);
                            var _dadosICI = GetDadosIPI(_item);

                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaCodigo(_item.prod.cProd));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaDescricao($"{_item.prod.xProd} {_item.infAdProd}"));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaCodigo(_item.prod.NCM));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaCodigo(_dadosICMS?.OrigemCST));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaCodigo(_item.prod.CFOP.ToString()));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaCodigo(_item.prod.uCom));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_item.prod.qCom.ToString(_item.prod.qCom.QuantidadeDecimais())));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_item.prod.vUnCom.ToString(_casaValorUnitario)));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_item.prod.vProd.ToString(2)));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_dadosICMS?.Base?.ToString(2)));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_dadosICMS?.Valor?.ToString(2)));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_dadosICI?.Valor?.ToString(2)));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_dadosICMS?.Aliquota?.ToString(2)));
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).Padding(Auxilio.PADDING).Component(new CampoTabelaValor(_dadosICI?.Aliq?.ToString(2)));
                        }

                        for (int i = 0; i < 13; i++)
                        {
                            table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).BorderBottom(Auxilio.BORDA).Height(1);
                        }
                        table.Cell().BorderLeft(Auxilio.BORDA).BorderRight(Auxilio.BORDA).BorderBottom(Auxilio.BORDA).Extend().Padding(4).Text("");
                    });

                });
        }
    }


    public class HeaderNfe : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;
        private readonly protNFe _protocolo;
        public HeaderNfe(NFe.Classes.NFe nfe, protNFe protocolo)
        {
            _nfe = nfe;
            _protocolo = protocolo;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Component(new HeaderCanhoto(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe.total.ICMSTot, _nfe.infNFe.dest));
                column.Item().Component(new HeaderIdentificacaoOperacao(_nfe.infNFe.ide, _nfe.infNFe.emit, _nfe.infNFe, _protocolo));
                column.Item().Component(new HeaderInfoComple(_nfe.infNFe.ide, _nfe.infNFe.emit, _protocolo));
            });
        }
    }

    public class FooterNfe : IComponent
    {

        private readonly infNFe _infnfe;

        public FooterNfe(infNFe infenfe)
        {
            _infnfe = infenfe;
        }

        public void Compose(IContainer container)
        {
            var _informacaoAdicional = "Inf. fisco:";

            if (_infnfe.infAdic.infCpl != null)
                _informacaoAdicional = "Inf. Contribuinte:";


            container.Column(col =>
            {
                col.Item().Text("DADOS ADICIONAIS").SemiBold();
                col.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Border(Auxilio.BORDA).Height(80).Width(280).Padding(Auxilio.PADDING).Text(text =>
                    {
                        text.Line("INFORMAÇÕES COMPLEMENTARES");
                        text.Line($"{_informacaoAdicional} {_infnfe.infAdic.infAdFisco ?? _infnfe.infAdic.infCpl}\r\nValor Aproximado dos Tributos: {_infnfe.total.ICMSTot.vTotTrib.ToString()}").FontSize(7);
                    });
                    row.RelativeItem().Border(Auxilio.BORDA).Height(80).Padding(Auxilio.PADDING).Component(new CampoInformativo("RESERVADO AO FISCO", ""));
                });
                col.Item().Text("Documento impresso por MicroSales - www.microsales.com.br").FontSize(6).AlignRight().Italic();
            });
        }
    }

    public class FuncoesEmplementares
    {
        public static byte[] GerarCodigoDeBarras(string codigo)
        {
            int larguraMinimaPorCaracter = 20;
            int largura = Math.Max(100, codigo.Length * larguraMinimaPorCaracter);

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = largura,
                    Height = 202,
                    Margin = 2,
                    PureBarcode = true
                }
            };

            var pixelData = writer.Write(codigo);

            var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);

            var bmpData = bitmap.LockBits(
                new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            try
            {
                Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

    }
}