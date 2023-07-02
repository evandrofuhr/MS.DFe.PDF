using MS.DFe.PDF.Extensoes;
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

        public string Numero { get => nNF.FormataNumero(); }
        public string Serie { get => serie.FormataSerie(); }
        public string Emissao { get => dhEmi.FormataDataHora(); }
        public string Recebimento { get => dhRecbto.FormataDataHora(); }

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
