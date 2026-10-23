using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    /// <summary>
    /// Itens no leiaute do DANFE retrato de referência: descrição com NCM,
    /// CST/CFOP, quantidade e tributos em linhas, não a grade clássica de 14 colunas.
    /// </summary>
    public class ItemRetrato : IComponent
    {
        private const int Colunas = 8;
        private const float Fonte = 6f;
        private const float FonteMiuda = 5.2f;

        private readonly List<det> _det;
        private readonly int _casaValorUnitario;

        public ItemRetrato(List<det> det, CRT crt)
        {
            _det = det ?? new List<det>();
            _casaValorUnitario = _det.Count == 0
                ? 2
                : _det.Max(x => x.prod.vUnCom.QuantidadeDecimais());
            if (_casaValorUnitario < 2)
                _casaValorUnitario = 2;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(col =>
                {
                    col.RelativeColumn(3.1f);
                    col.ConstantColumn(46);
                    col.ConstantColumn(58);
                    col.ConstantColumn(52);
                    col.ConstantColumn(52);
                    col.RelativeColumn(2.1f);
                    col.RelativeColumn(2.2f);
                    col.RelativeColumn(2.5f);
                });

                table.Header(header =>
                {
                    header.Cell().ColumnSpan(Colunas)
                        .PaddingTop(1)
                        .PaddingBottom(1)
                        .Text("DADOS DOS PRODUTOS / SERVIÇOS")
                        .FontSize(6.5f)
                        .Bold();

                    Cabecalho(header.Cell(), true, "DESCRIÇÃO DO PRODUTO / SERVIÇO");
                    Cabecalho(header.Cell(), false, "CST /\nCFOP");
                    Cabecalho(header.Cell(), false, "QTD / UN");
                    Cabecalho(header.Cell(), false, "VLR UNIT");
                    Cabecalho(header.Cell(), false, "VLR TOTAL");
                    Cabecalho(header.Cell(), false, "BASES DE CÁLCULO");
                    Cabecalho(header.Cell(), false, "ALÍQUOTAS");
                    Cabecalho(header.Cell(), false, "VALOR DOS TRIBUTOS");
                });

                foreach (var item in _det)
                    LinhaItem(table, item);
            });
        }

        private void LinhaItem(TableDescriptor table, det item)
        {
            var icms = ImpostosHelper.GetDadosICMS(item);
            var ipi = ImpostosHelper.GetDadosIPI(item);
            var ibs = item.imposto?.IBSCBS;
            var grupo = ibs?.gIBSCBS;
            var impostoSeletivo = item.imposto?.IS;
            var ipiBase = ValorPropriedade(item.imposto?.IPI?.TipoIPI, "vBC");

            table.Cell().Element(c => Caixa(c, true, true)).Column(col =>
            {
                col.Item().Text(t =>
                {
                    t.DefaultTextStyle(s => s.FontSize(Fonte));
                    if (!string.IsNullOrWhiteSpace(item.prod.cProd))
                    {
                        t.Span(item.prod.cProd).SemiBold();
                        t.Span("  ");
                    }
                    t.Span(item.prod.xProd ?? string.Empty);
                });

                if (!string.IsNullOrWhiteSpace(item.infAdProd))
                    col.Item().Text("(" + item.infAdProd.Trim().Trim('(', ')') + ")").FontSize(FonteMiuda);

                col.Item().PaddingTop(2).Text(t =>
                {
                    t.DefaultTextStyle(s => s.FontSize(FonteMiuda));
                    t.Span("[NCM " + (item.prod.NCM ?? string.Empty) + "]");
                    if (!string.IsNullOrWhiteSpace(ibs?.cClassTrib))
                        t.Span("  [cClassTrib " + ibs.cClassTrib + "]");
                });
            });

            table.Cell().Element(c => Caixa(c, false, true)).Column(col =>
            {
                RotuloValor(col, "CST", icms?.OrigemCST ?? "000");
                RotuloValor(col, "CFOP", item.prod.CFOP.ToString("0000"));
            });

            table.Cell().Element(c => Caixa(c, false, true)).AlignCenter().Column(col =>
            {
                col.Item().AlignCenter().Text(item.prod.qCom.ToString(item.prod.qCom.QuantidadeDecimais())).FontSize(Fonte);
                col.Item().AlignCenter().Text("(" + (item.prod.uCom ?? string.Empty) + ")").FontSize(FonteMiuda);
            });

            table.Cell().Element(c => Caixa(c, false, true)).AlignRight().AlignTop()
                .Text(item.prod.vUnCom.ToString(_casaValorUnitario)).FontSize(Fonte);
            table.Cell().Element(c => Caixa(c, false, true)).AlignRight().AlignTop()
                .Text(item.prod.vProd.ToString(2)).FontSize(Fonte);

            table.Cell().Element(c => Caixa(c, false, true)).Column(col =>
            {
                RotuloValor(col, "ICMS", Dinheiro(icms?.Base));
                RotuloValor(col, "IBS / CBS", Dinheiro(grupo?.vBC));
                RotuloValor(col, "IS", Dinheiro(impostoSeletivo?.vBCIS));
                RotuloValor(col, "IPI", Dinheiro(ipiBase));
            });

            table.Cell().Element(c => Caixa(c, false, true)).Column(col =>
            {
                Par(col, "ICMS", Percentual(icms?.Aliquota), "CBS", Percentual(Aliquota(grupo?.gCBS?.pCBS, grupo?.gCBS?.gRed)));
                Par(col, "IBS UF", Percentual(Aliquota(grupo?.gIBSUF?.pIBSUF, grupo?.gIBSUF?.gRed)), "IPI", Percentual(ipi?.Aliq));
                Par(col, "IBS MUN", Percentual(Aliquota(grupo?.gIBSMun?.pIBSMun, grupo?.gIBSMun?.gRed)), "IS", Percentual(impostoSeletivo?.pIS));
            });

            table.Cell().Element(c => Caixa(c, false, true)).Column(col =>
            {
                Par(col, "ICMS", Dinheiro(icms?.Valor), "CBS", Dinheiro(grupo?.gCBS?.vCBS));
                Par(col, "IBS UF", Dinheiro(grupo?.gIBSUF?.vIBSUF), "IPI", Dinheiro(ipi?.Valor));
                Par(col, "IBS MUN", Dinheiro(grupo?.gIBSMun?.vIBSMun), "IS", Dinheiro(impostoSeletivo?.vIS));
            });
        }

        private static void Cabecalho(IContainer cell, bool primeira, string texto)
        {
            Caixa(cell, primeira, true)
                .MinHeight(18)
                .AlignCenter()
                .AlignMiddle()
                .Padding(1)
                .Text(texto)
                .FontSize(FonteMiuda)
                .Bold()
                .AlignCenter();
        }

        private static IContainer Caixa(IContainer cell, bool primeira, bool fundo)
        {
            if (primeira)
                cell = cell.BorderLeft(ConstantsHelper.BORDA);
            cell = cell.BorderRight(ConstantsHelper.BORDA).BorderTop(ConstantsHelper.BORDA);
            if (fundo)
                cell = cell.BorderBottom(ConstantsHelper.BORDA);
            return cell.Padding(1.2f);
        }

        private static void RotuloValor(ColumnDescriptor col, string rotulo, string valor)
        {
            col.Item().Row(row =>
            {
                row.AutoItem().Text(rotulo).FontSize(FonteMiuda);
                row.RelativeItem().AlignRight().Text(valor).FontSize(FonteMiuda);
            });
        }

        private static void Par(ColumnDescriptor col, string a, string va, string b, string vb)
        {
            col.Item().Text(a + " " + va + "  /  " + b + " " + vb).FontSize(FonteMiuda);
        }

        private static string Dinheiro(decimal? valor)
        {
            return (valor ?? 0m).ToString(2);
        }

        private static string Percentual(decimal? valor)
        {
            return (valor ?? 0m).ToString(2) + "%";
        }

        /// <summary>NT 2026.010: com gRed, imprime pAliqEfet; senão a alíquota vigente.</summary>
        private static decimal? Aliquota(decimal? vigente, gRed reducao)
        {
            return reducao != null ? reducao.pAliqEfet : vigente;
        }

        private static decimal? ValorPropriedade(object origem, string nome)
        {
            if (origem == null || string.IsNullOrEmpty(nome))
                return null;
            var propriedade = origem.GetType().GetProperty(nome);
            var valor = propriedade?.GetValue(origem);
            if (valor == null)
                return null;
            return System.Convert.ToDecimal(valor);
        }
    }
}
