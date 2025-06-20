using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Pagamento;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Pagamento : IComponent
    {
        private readonly IEnumerable<pag> _pag;

        public Pagamento(IEnumerable<pag> pag)
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

                table.Cell().AlignLeft().Texto(NFCeResource.FORMA_PAGAMENTO);
                table.Cell().AlignRight().Texto(NFCeResource.PAGO, NFCeResource.CIFRAO);

                foreach (var item in _pag)
                {
                    foreach (var det in item.detPag)
                    {
                        table.Cell().AlignLeft().Texto(det.PagamentoDescricao());
                        table.Cell().AlignRight().Texto(det.vPag.Formata() ?? "0,00");
                    }
                }

                var _troco = _pag.Sum(s => s.vTroco);

                table.Cell().AlignLeft().Texto(NFCeResource.TROCO, NFCeResource.CIFRAO);
                table.Cell().AlignRight().Texto(_troco.ToString());

                table.Cell().ColumnSpan(2).BorderBottom(0.8f);
            });
        }



    }
}


