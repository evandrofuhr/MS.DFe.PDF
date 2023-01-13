namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosItem
    {
        public string cProd { get; }
        public string xProd { get; }
        public decimal qCom { get; }
        public string uCom { get; }
        public decimal vUnCom { get; }
        public decimal vProd { get; }

        public DFeDadosItem(string cProd, string xProd, decimal qCom, string uCom, decimal vUnCom, decimal vProd)
        {
            this.cProd = cProd;
            this.xProd = xProd;
            this.qCom = qCom;
            this.uCom = uCom;
            this.vUnCom = vUnCom;
            this.vProd = vProd;
        }
    }
}
