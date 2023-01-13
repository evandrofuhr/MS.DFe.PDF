namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosPagamento
    {
        public decimal vTroco { get; }
        public int tPag { get; }
        public decimal vPag { get; }

        public DFeDadosPagamento(decimal vTroco, int tPag, decimal vPag)
        {
            this.vTroco = vTroco;
            this.tPag = tPag;
            this.vPag = vPag;
        }
    }
}
