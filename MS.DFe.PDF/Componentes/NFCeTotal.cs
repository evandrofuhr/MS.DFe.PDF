using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes
{
    internal class NFCeTotal : IComponent
    {
        private readonly DFeDadosTotal _total;

        public NFCeTotal(DFeDadosTotal total)
        {
            _total = total;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn();
                    c.RelativeColumn();
                });

                table.Cell().AlignLeft().Texto(TextoResource.TOTAL, TextoResource.CIFRAO);
                table.Cell().AlignRight().Texto(_total.vProd);

                if (_total.TemDescontos || _total.TemFrete || _total.TemSeguro || _total.TemOutro)
                {
                    if (_total.TemDescontos)
                    {
                        table.Cell().AlignLeft().Texto(TextoResource.DESCONTO, TextoResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.vDesc);
                    }

                    if (_total.TemFrete)
                    {
                        table.Cell().AlignLeft().Texto(TextoResource.FRETE, TextoResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.vFrete);
                    }

                    if (_total.TemSeguro)
                    {
                        table.Cell().AlignLeft().Texto(TextoResource.SEGURO, TextoResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.vSeg);
                    }

                    if (_total.TemOutro)
                    {
                        table.Cell().AlignLeft().Texto(TextoResource.OUTROS, TextoResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.vOutro);
                    }

                    table.Cell().AlignLeft().Texto(TextoResource.PAGAR, TextoResource.CIFRAO).Bold();
                    table.Cell().AlignRight().Texto(_total.vNF).Bold();
                }
            });
        }
    }
}
