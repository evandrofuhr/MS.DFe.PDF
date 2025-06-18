using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Emitente : IComponent
    {
        private readonly DFeDadosEmitente _emit;

        public Emitente(DFeDadosEmitente emit)
        {
            _emit = emit;
        }

        private void ComporDadosEmitente(ColumnDescriptor column)
        {
            column.Item().AlignCenter().Texto(_emit.xNome).Bold();
            column.Item().AlignCenter().Texto(NFCeResource.CNPJ, _emit.CNPJ);
        }

        private void ComporEnderecoEmitente(ColumnDescriptor column)
        {
            column.Item().AlignCenter().Texto(_emit.Endereco);
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    ComporDadosEmitente(c);
                    ComporEnderecoEmitente(c);
                }
            );
        }
    }
}
