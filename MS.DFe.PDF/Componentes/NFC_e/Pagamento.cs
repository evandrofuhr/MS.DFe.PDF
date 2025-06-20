using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
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
                    table.Cell().AlignLeft().Texto(item.PagamentoDescricao());
                    table.Cell().AlignRight().Texto(item.vPag.ToString());
                }

                var _troco = _pag.Sum(s => s.vTroco);

                table.Cell().AlignLeft().Texto(NFCeResource.TROCO, NFCeResource.CIFRAO);
                table.Cell().AlignRight().Texto(_troco.ToString());

                table.Cell().ColumnSpan(2).BorderBottom(0.8f);
            });
        }



    }
}

public static class PagamentoExtensoes
{
    public static string PagamentoDescricao(this pag pagamento)
    {
        var chave = $"TIPO_PAGAMENTO_{pagamento.tPag}";
        var descricao = NFCeResource.ResourceManager.GetString(chave);
        return descricao ?? NFCeResource.TIPO_PAGAMENTO_99;
    }
}