using System.Collections.Generic;

namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosComprovante
    {
        public IEnumerable<string> Textos { get; }
        public DFeDadosComprovante(IEnumerable<string> textos)
        {
            Textos = textos;
        }

    }
}
