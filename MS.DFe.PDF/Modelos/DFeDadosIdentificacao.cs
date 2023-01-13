using MS.DFe.PDF.Resources;
using System;

namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosIdentificacao
    {
        public long nNF { get; }
        public int serie { get; }
        public DateTimeOffset dhEmi { get; }
        public string nProt { get; }
        public DateTimeOffset dhRecbto { get; }

        public string Numero { get => nNF.ToString().PadLeft(9, '0'); }
        public string Serie { get => serie.ToString().PadLeft(3, '0'); }
        public string Emissao { get => dhEmi.ToString(TextoResource.FORMATO); }
        public string Recebimento { get => dhRecbto.ToString(TextoResource.FORMATO); }

        public DFeDadosIdentificacao(long nNF, int serie, DateTimeOffset dhEmi, string nProt, DateTimeOffset dhRecbto)
        {
            this.nNF = nNF;
            this.serie = serie;
            this.dhEmi = dhEmi;
            this.nProt = nProt;
            this.dhRecbto = dhRecbto;
        }
    }
}
