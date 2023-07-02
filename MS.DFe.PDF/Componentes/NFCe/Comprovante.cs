using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Comprovante : IComponent
    {
        private readonly DFeDadosComprovante _comprovante;
        private readonly DFeDadosEmitente _emit;
        public Comprovante(DFeDadosComprovante comprovante, DFeDadosEmitente emit)
        {
            _comprovante = comprovante;
            _emit = emit;
        }

        private void ComporTitulo(ColumnDescriptor column)
        {
            column.Item().AlignCenter().Texto(NFCeResource.COMPROVANTE_POS).Bold();
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    ComporTitulo(c);
                    c.Item().Component(new Emitente(_emit));
                    c.Item().Height(3).MinimalBox();

                    foreach (var _texto in _comprovante.Textos)
                        c.Item().AlignCenter().Text(_texto);
                }
            );
        }
    }
}
