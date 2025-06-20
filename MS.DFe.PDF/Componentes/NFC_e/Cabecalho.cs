using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Cabecalho : IComponent
    {
        private readonly emit _emit;
        public Cabecalho(emit emit)
        {
            _emit = emit;
        }

        private void ComporTitulo(ColumnDescriptor column)
        {
            column.Item().AlignCenter().DefaultTextStyle(TextStyle.Default.FontSize(6)).Texto(NFCeResource.DANFE).Bold();
            column.Item().AlignCenter().DefaultTextStyle(TextStyle.Default.FontSize(6)).Texto(NFCeResource.DOC_AUX).Bold();
            column.Item().Height(3).MinimalBox();
            column.Item().Element(e => e.BorderBottom(0.8f));
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    c.Item().Component(new Emitente(_emit));
                    c.Item().Height(3).MinimalBox();
                    ComporTitulo(c);
                }
            );
        }
    }
}
