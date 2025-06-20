using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Consulta : IComponent
    {
        private readonly infProt _infProt;
        private readonly infNFeSupl _infNFeSupl;

        public Consulta(infNFeSupl infNFeSupl, infProt infProt)
        {
            _infProt = infProt;
            _infNFeSupl = infNFeSupl;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(c => c.RelativeColumn());

                table.Cell().AlignCenter().Texto(NFCeResource.CONSULTE_CHAVE).Bold();
                table.Cell().AlignCenter().Texto(_infNFeSupl.urlChave);
                table.Cell().AlignCenter().Texto(_infProt.chNFe.FormataChaveNFe());

            });
        }
    }
}
