using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;


namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Comprovante : IComponent
    {
        private readonly IEnumerable<string> _comprovante;
        private readonly emit _emit;
        public Comprovante(IEnumerable<string> comprovante, emit emit)
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
                    c.Item().Height(3).ShrinkHorizontal();

                    foreach (var _texto in _comprovante)
                        c.Item().AlignCenter().Text(_texto);
                }
            );
        }
    }
}
