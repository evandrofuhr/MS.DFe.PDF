using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MS.DFe.PDF.Componentes
{
    internal class NFCePagamento : IComponent
    {
        private readonly IEnumerable<DFeDadosPagamento> _pag;

        public NFCePagamento(IEnumerable<DFeDadosPagamento> pag)
        {
            _pag = pag;
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

                table.Cell().AlignLeft().Texto(TextoResource.FORMA_PAGAMENTO);
                table.Cell().AlignRight().Texto(TextoResource.PAGO, TextoResource.CIFRAO);

                foreach (var item in _pag)
                {
                    table.Cell().AlignLeft().Texto(item.tPag.ToString());
                    table.Cell().AlignRight().Texto(item.vPag);
                }

                var _troco = _pag.Sum(s => s.vTroco);

                table.Cell().AlignLeft().Texto(TextoResource.TROCO, TextoResource.CIFRAO);
                table.Cell().AlignRight().Texto(_troco);

                table.Cell().ColumnSpan(2).BorderBottom(0.8f);
            });
        }
    }
}
