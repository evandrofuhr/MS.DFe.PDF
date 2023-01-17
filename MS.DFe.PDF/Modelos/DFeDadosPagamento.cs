using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosPagamento
    {
        public decimal vTroco { get; }
        public int tPag { get; }
        public decimal vPag { get; }

        public string Pagamento { get => NFCeResource.ResourceManager.GetString($"TIPO_PAGAMENTO_{tPag}"); }

        public DFeDadosPagamento(decimal vTroco, int tPag, decimal vPag)
        {
            this.vTroco = vTroco;
            this.tPag = tPag;
            this.vPag = vPag;
        }
    }
}
