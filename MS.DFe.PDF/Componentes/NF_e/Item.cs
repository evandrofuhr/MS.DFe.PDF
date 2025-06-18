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
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;



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

        public void Compose(IContainer container)
        {
            var _cabecalho = _crt == CRT.RegimeNormal ? NFeResource.CST : NFeResource.O_CSOSN;

            container
                .Column(column =>
                {
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.DADOS_PRODUTOS_SERVICOS).SemiBold();

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(145);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(25);
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
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.CODIGO_PRODUTO).Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(NFeResource.DESCRIÇÃO_PRODUTO_SERVIÇO).Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(NFeResource.NCM_SH).Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(_cabecalho).Bold().FontSize(5);
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(NFeResource.CFOP).Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(NFeResource.UN).Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text(NFeResource.QUANTI).Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.VALOR}\r\n{NFeResource.UNIT}.").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.VALOR}\r\n{NFeResource.TOTAL}").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.B_CALC}\r\n{NFeResource.ICMS}").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.VALOR}\r\n{NFeResource.ICMS}").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.VALOR}\r\n{NFeResource.IPI}").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.ALIQ}\r\n{NFeResource.ICMS}").Bold();
                            header.Cell().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).AlignCenter().Text($"{NFeResource.ALIQ}\r\n{NFeResource.IPI}").Bold();
                        });

                        foreach (var _item in _det)
                        {
                            var _dadosICMS = ImpostosHelper.GetDadosICMS(_item);
                            var _dadosICI = ImpostosHelper.GetDadosIPI(_item);

                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Codigo(_item.prod.cProd));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Descricao($"{_item.prod.xProd}\r\n{_item.infAdProd}"));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Codigo(_item.prod.NCM));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Codigo(_dadosICMS?.OrigemCST));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Codigo(_item.prod.CFOP.ToString("N0")));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Codigo(_item.prod.uCom));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_item.prod.qCom.ToString(_item.prod.qCom.QuantidadeDecimais())));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_item.prod.vUnCom.ToString(_casaValorUnitario)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_item.prod.vProd.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_dadosICMS?.Base?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_dadosICMS?.Valor?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_dadosICI?.Valor?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_dadosICMS?.Aliquota?.ToString(2)));
                            table.Cell().BorderLeft(DadoPadraoExtensoes.BORDA).BorderRight(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoTabela.Valor(_dadosICI?.Aliq?.ToString(2)));
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
