using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Consumidor : IComponent
    {
        private readonly DFeDadosConsumidor _dest;

        public Consumidor(DFeDadosConsumidor dest)
        {
            _dest = dest;
        }
        public void Compose(IContainer container)
        {
            container.Table(
                t =>
                {
                    t.ColumnsDefinition(c => c.RelativeColumn());
                    t.Cell().AlignCenter().Texto(_dest.Descricao).Bold();
                }
            );
        }
    }
}
