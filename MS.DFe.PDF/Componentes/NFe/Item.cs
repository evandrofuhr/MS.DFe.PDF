using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Emitente;
using System.Collections.Generic;
using System.Linq;
using System;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Elementos;



namespace MS.DFe.PDF.Componentes.Nfe
{
    public class Item : IComponent
    {

        private readonly CRT _crt;
        private readonly List<det> _det;
        private readonly int _casaValorUnitario;

        public Item(List<det> det, CRT crt)
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
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text("DADOS DOS PRODUTOS / SERVICOS").SemiBold();

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
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text("CÓDIGO\r\nPRODUTO").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("DESCRIÇÃO DO PRODUTO / SERVIÇO").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("NCM/SH").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(_cabecalho).Bold().FontSize(5);
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("CFOP").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("UN").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("QUANTI").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("VALOR\r\nUNIT.").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("VALOR\r\nTOTAL").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("B CÁLC\r\nICMS\r\n").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("VALOR\r\nICMS\r\n").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("VALOR\r\nIPI").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("ALIQ.\r\nICMS").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text("ALIQ.\r\nIPI").Bold();
                        });

                        foreach (var _item in _det)
                        {
                            var _dadosICMS = GetDadosICMS(_item);
                            var _dadosICI = GetDadosIPI(_item);

                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaCodigo(_item.prod.cProd));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaDescricao($"{_item.prod.xProd} {_item.infAdProd}"));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaCodigo(_item.prod.NCM));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaCodigo(_dadosICMS?.OrigemCST));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaCodigo(_item.prod.CFOP.ToString()));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaCodigo(_item.prod.uCom));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_item.prod.qCom.ToString(_item.prod.qCom.QuantidadeDecimais())));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_item.prod.vUnCom.ToString(_casaValorUnitario)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_item.prod.vProd.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_dadosICMS?.Base?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_dadosICMS?.Valor?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_dadosICI?.Valor?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_dadosICMS?.Aliquota?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoTabelaValor(_dadosICI?.Aliq?.ToString(2)));
                        }

                        for (int i = 0; i < 13; i++)
                        {
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).BorderBottom(DadoPadraoExtensoes.BORDA).Height(1);
                        }
                        table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).BorderBottom(DadoPadraoExtensoes.BORDA).Extend().Padding(4).Text("");
                    });

                });
        }
    }
}
