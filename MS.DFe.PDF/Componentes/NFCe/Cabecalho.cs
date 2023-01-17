using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Cabecalho : IComponent
    {
        private readonly DFeDadosEmitente _emit;
        public Cabecalho(DFeDadosEmitente emit)
        {
            _emit = emit;
        }
        private void ComporDadosEmitente(TableDescriptor table)
        {
            table.Cell().AlignCenter().Texto(_emit.xNome).Bold();
            table.Cell().AlignCenter().Texto(NFCeResource.CNPJ, _emit.CNPJ);
        }

        private void ComporEnderecoEmitente(TableDescriptor table)
        {
            table.Cell().AlignCenter().Texto(_emit.Endereco);
        }

        private void ComporEmitente(TableDescriptor table)
        {
            ComporDadosEmitente(table);
            ComporEnderecoEmitente(table);
        }

        private void ComporTitulo(TableDescriptor table)
        {
            table.Cell().AlignCenter().DefaultTextStyle(TextStyle.Default.FontSize(6)).Texto(NFCeResource.DANFE).Bold();
            table.Cell().AlignCenter().DefaultTextStyle(TextStyle.Default.FontSize(6)).Texto(NFCeResource.DOC_AUX).Bold();
            table.Cell().Height(3);
            table.Cell().Element(e => e.BorderBottom(0.8f));
        }

        public void Compose(IContainer container)
        {
            container.Table(
                table =>
                {
                    table.ColumnsDefinition(column => column.RelativeColumn(1));
                    ComporEmitente(table);
                    table.Cell().Height(3);
                    ComporTitulo(table);
                }
            );
        }
    }
}
