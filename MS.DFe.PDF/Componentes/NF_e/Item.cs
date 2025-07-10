using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;



namespace MS.DFe.PDF.Componentes.NF_e
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

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(
                            innerCol =>
                            {
                                innerCol.RelativeColumn(2);
                                innerCol.ConstantColumn(145);
                                innerCol.RelativeColumn(2);
                                innerCol.ConstantColumn(25);
                                innerCol.RelativeColumn(1);
                                innerCol.RelativeColumn(1);
                                innerCol.RelativeColumn(2);
                                innerCol.RelativeColumn(2);
                                innerCol.RelativeColumn(2);
                                innerCol.RelativeColumn(2);
                                innerCol.RelativeColumn(2);
                                innerCol.RelativeColumn(2);
                                innerCol.RelativeColumn(1);
                                innerCol.RelativeColumn(1);
                            }
                        );

                        table.Header(
                            header =>
                            {
                                header.Cell().ColumnSpan(14).PadraoLabelGrupo(NFeResource.DADOS_PRODUTOS_SERVICOS);

                                header.Cell().PadraoInformacaoTabela(NFeResource.CODIGO_PRODUTO);
                                header.Cell().PadraoInformacaoTabela(NFeResource.DESCRIÇÃO_PRODUTO_SERVIÇO);
                                header.Cell().PadraoInformacaoTabela(NFeResource.NCM_SH);
                                header.Cell().PadraoInformacaoTabela(_cabecalho);
                                header.Cell().PadraoInformacaoTabela(NFeResource.CFOP);
                                header.Cell().PadraoInformacaoTabela(NFeResource.UN);
                                header.Cell().PadraoInformacaoTabela(NFeResource.QUANTI);
                                header.Cell().PadraoInformacaoTabela(NFeResource.VALOR, NFeResource.UNIT);
                                header.Cell().PadraoInformacaoTabela(NFeResource.VALOR, NFeResource.TOTAL);
                                header.Cell().PadraoInformacaoTabela(NFeResource.B_CALC, NFeResource.ICMS);
                                header.Cell().PadraoInformacaoTabela(NFeResource.VALOR, NFeResource.ICMS);
                                header.Cell().PadraoInformacaoTabela(NFeResource.VALOR, NFeResource.IPI);
                                header.Cell().PadraoInformacaoTabela(NFeResource.ALIQ, NFeResource.ICMS);
                                header.Cell().PadraoInformacaoTabela(NFeResource.ALIQ, NFeResource.IPI);
                            }
                        );

                        foreach (var _item in _det)
                        {
                            var _dadosICMS = ImpostosHelper.GetDadosICMS(_item);
                            var _dadosICI = ImpostosHelper.GetDadosIPI(_item);

                            table.Cell().PadraoDadosTabela(_item.prod.cProd, true);
                            table.Cell().PadraoDadosTabela($"{_item.prod.xProd}\r\n{_item.infAdProd}", false);
                            table.Cell().PadraoDadosTabela(_item.prod.NCM, true);
                            table.Cell().PadraoDadosTabela(_dadosICMS?.OrigemCST, true);
                            table.Cell().PadraoDadosTabela(_item.prod.CFOP.ToString("N0"), true);
                            table.Cell().PadraoDadosTabela(_item.prod.uCom, true);
                            table.Cell().PadraoDadosTabela(_item.prod.qCom.ToString(_item.prod.qCom.QuantidadeDecimais()));
                            table.Cell().PadraoDadosTabela(_item.prod.vUnCom.ToString(_casaValorUnitario));
                            table.Cell().PadraoDadosTabela(_item.prod.vProd.ToString(2));
                            table.Cell().PadraoDadosTabela(_dadosICMS?.Base?.ToString(2));
                            table.Cell().PadraoDadosTabela(_dadosICMS?.Valor?.ToString(2));
                            table.Cell().PadraoDadosTabela(_dadosICI?.Valor?.ToString(2));
                            table.Cell().PadraoDadosTabela(_dadosICMS?.Aliquota?.ToString(2));
                            table.Cell().PadraoDadosTabela(_dadosICI?.Aliq?.ToString(2));
                        }

                        for (int i = 0; i < 13; i++)
                        {
                            table.Cell().BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA).BorderBottom(ConstantsHelper.BORDA);
                        }
                        table.Cell().BorderLeft(ConstantsHelper.BORDA).BorderRight(ConstantsHelper.BORDA).BorderBottom(ConstantsHelper.BORDA).Extend();

                        table.Footer(footer =>
                        {
                            footer.Cell()
                                .ColumnSpan(14)
                                .BorderTop(ConstantsHelper.BORDA);
                        });

                    });

                });
        }
    }
}
