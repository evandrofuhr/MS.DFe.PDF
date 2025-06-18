using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Consulta : IComponent
    {
        private readonly DFeDadosConsulta _consulta;

        public Consulta(DFeDadosConsulta consulta)
        {
            _consulta = consulta;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(c => c.RelativeColumn());

                table.Cell().AlignCenter().Texto(NFCeResource.CONSULTE_CHAVE).Bold();
                table.Cell().AlignCenter().Texto(_consulta.urlChave);
                table.Cell().AlignCenter().Texto(_consulta.ChaveNFe);

            });
        }
    }
}
