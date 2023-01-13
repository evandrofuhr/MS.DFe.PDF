namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosTotal
    {
        public decimal vProd { get; }
        public decimal vDesc { get; }
        public decimal vFrete { get; }
        public decimal vOutro { get; }
        public decimal vSeg { get; }
        public decimal vNF { get; }


        public bool TemDescontos { get => vDesc > 0; }
        public bool TemFrete { get => vFrete > 0; }
        public bool TemOutro { get => vOutro > 0; }
        public bool TemSeguro { get => vSeg > 0; }

        public DFeDadosTotal(decimal vProd, decimal vDesc, decimal vFrete, decimal vOutro, decimal vSeg, decimal vNF)
        {
            this.vProd = vProd;
            this.vDesc = vDesc;
            this.vFrete = vFrete;
            this.vOutro = vOutro;
            this.vSeg = vSeg;
            this.vNF = vNF;
        }
    }
}
