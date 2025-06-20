using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Emitente : IComponent
    {
        private readonly emit _emit;

        public Emitente(emit emit)
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
            column.Item().AlignCenter().Texto(_emit.enderEmit.ToEnderecoEmit());
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
