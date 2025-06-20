using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Total : IComponent
    {
        private readonly total _total;

        public Total(total total)
        {
            _total = total;
        }

        private bool TemDescontos => _total.ICMSTot.vDesc > 0;
        private bool TemFrete => _total.ICMSTot.vFrete > 0;
        private bool TemSeguro => _total.ICMSTot.vSeg > 0;
        private bool TemOutro => _total.ICMSTot.vOutro > 0;

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn();
                    c.RelativeColumn();
                });

                table.Cell().AlignLeft().Texto(NFCeResource.TOTAL, NFCeResource.CIFRAO);
                table.Cell().AlignRight().Texto(_total.ICMSTot.vProd);

                if (TemDescontos || TemFrete || TemSeguro || TemOutro)
                {
                    if (TemDescontos)
                    {
                        table.Cell().AlignLeft().Texto(NFCeResource.DESCONTO, NFCeResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.ICMSTot.vDesc);
                    }

                    if (TemFrete)
                    {
                        table.Cell().AlignLeft().Texto(NFCeResource.FRETE, NFCeResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.ICMSTot.vFrete);
                    }

                    if (TemSeguro)
                    {
                        table.Cell().AlignLeft().Texto(NFCeResource.SEGURO, NFCeResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.ICMSTot.vSeg);
                    }

                    if (TemOutro)
                    {
                        table.Cell().AlignLeft().Texto(NFCeResource.OUTROS, NFCeResource.CIFRAO);
                        table.Cell().AlignRight().Texto(_total.ICMSTot.vOutro);
                    }

                    table.Cell().AlignLeft().Texto(NFCeResource.PAGAR, NFCeResource.CIFRAO).Bold();
                    table.Cell().AlignRight().Texto(_total.ICMSTot.vNF).Bold();
                }
            });
        }
    }
}
