
namespace MS.DFe.PDF.Modelos
{
    public class DadosICMS
    {
        public int Origem { get; set; }
        public string CST { get; set; }
        public decimal? Base { get; set; }
        public decimal? Aliquota { get; set; }
        public decimal? Valor { get; set; }

        public string OrigemCST
        {
            get
            {
                return $"{Origem}{CST?.ToString().PadLeft(2, '0')}";
            }
        }

    }

}
