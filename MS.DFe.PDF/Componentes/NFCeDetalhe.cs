using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes
{
    internal class NFCeDetalhe : IComponent
    {
        private readonly DFeDados _dados;

        public NFCeDetalhe(DFeDados dados)
        {
            _dados = dados;
        }

        private void ComporCabecalho(TableDescriptor table)
        {
            table.Cell().Column(c =>
            {
                c.Item().Table(t =>
                {
                    t.ColumnsDefinition(d =>
                    {
                        d.RelativeColumn(3);
                        d.RelativeColumn(7);
                    });

                    t.Cell().Texto(TextoResource.CODIGO).Bold();
                    t.Cell().Texto(TextoResource.DESCRICAO).Bold();
                });

                c.Item().Table(t =>
                {
                    t.ColumnsDefinition(d =>
                    {
                        d.RelativeColumn(4);
                        d.RelativeColumn(4);
                        d.RelativeColumn(4);
                    });

                    t.Cell().AlignRight().Texto(TextoResource.QTDE).Bold();
                    t.Cell().AlignRight().Texto(TextoResource.UNITARIO).Bold();
                    t.Cell().AlignRight().Texto(TextoResource.VL_TOTAL).Bold();
                });
                c.Item().Height(3);
                c.Item().BorderBottom(0.8f);

            });
        }

        private void ComporItens(TableDescriptor table)
        {
            table.Cell().Column(c =>
            {
                foreach (var item in _dados.det)
                {
                    c.Item().Table(t =>
                    {
                        t.ColumnsDefinition(d =>
                        {
                            d.RelativeColumn(3);
                            d.RelativeColumn(7);
                        });

                        t.Cell().Texto(item.cProd);
                        t.Cell().Texto(item.xProd);
                    });

                    c.Item().Table(t =>
                    {
                        t.ColumnsDefinition(d =>
                        {
                            d.RelativeColumn(4);
                            d.RelativeColumn(4);
                            d.RelativeColumn(4);
                        });

                        t.Cell().AlignRight().Texto(item.qCom.Formata(), item.uCom);
                        t.Cell().AlignRight().Texto(item.vUnCom);
                        t.Cell().AlignRight().Texto(item.vProd);
                    });
                }
                c.Item().Height(3);
            });
        }

        private void ComporTotalItens(TableDescriptor table)
        {
            table.Cell().Table(t =>
            {
                t.ColumnsDefinition(c =>
                {
                    c.RelativeColumn();
                    c.RelativeColumn();
                });

                t.Cell().ColumnSpan(2).BorderBottom(0.8f);
                t.Cell().AlignLeft().Texto(TextoResource.QTDE_TOTAL_ITENS);
                t.Cell().AlignRight().Texto(_dados.TotalItens.ToString());
            });
        }

        public void Compose(IContainer container)
        {
            container.Table(
                table =>
                {
                    table.ColumnsDefinition(column => column.RelativeColumn());

                    ComporCabecalho(table);
                    ComporItens(table);
                    ComporTotalItens(table);
                }
            );
        }
    }
}
