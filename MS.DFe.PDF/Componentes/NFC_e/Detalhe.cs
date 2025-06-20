using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Detalhe : IComponent
    {
        private readonly NFe.Classes.NFe _nfe;

        public Detalhe(NFe.Classes.NFe nfe)
        {
            _nfe = nfe;
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

                    t.Cell().Texto(NFCeResource.CODIGO).Bold();
                    t.Cell().Texto(NFCeResource.DESCRICAO).Bold();
                });

                c.Item().Table(t =>
                {
                    t.ColumnsDefinition(d =>
                    {
                        d.RelativeColumn(4);
                        d.RelativeColumn(4);
                        d.RelativeColumn(4);
                    });

                    t.Cell().AlignRight().Texto(NFCeResource.QTDE).Bold();
                    t.Cell().AlignRight().Texto(NFCeResource.UNITARIO).Bold();
                    t.Cell().AlignRight().Texto(NFCeResource.VL_TOTAL).Bold();
                });
                c.Item().Height(3);
                c.Item().BorderBottom(0.8f);

            });
        }

        private void ComporItens(TableDescriptor table)
        {
            table.Cell().Column(c =>
            {
                foreach (var item in _nfe.infNFe.det)
                {
                    c.Item().Table(t =>
                    {
                        t.ColumnsDefinition(d =>
                        {
                            d.RelativeColumn(3);
                            d.RelativeColumn(7);
                        });

                        t.Cell().Texto(item.prod.cProd);
                        t.Cell().Texto(item.prod.xProd);
                    });

                    c.Item().Table(t =>
                    {
                        t.ColumnsDefinition(d =>
                        {
                            d.RelativeColumn(4);
                            d.RelativeColumn(4);
                            d.RelativeColumn(4);
                        });

                        t.Cell().AlignRight().Texto(item.prod.qCom.Formata(), item.prod.uCom);
                        t.Cell().AlignRight().Texto(item.prod.vUnCom);
                        t.Cell().AlignRight().Texto(item.prod.vProd);
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
                t.Cell().AlignLeft().Texto(NFCeResource.QTDE_TOTAL_ITENS);
                t.Cell().AlignRight().Texto(_nfe.infNFe.det.Count.ToString());
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
