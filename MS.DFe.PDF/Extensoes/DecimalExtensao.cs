using System.Globalization;

namespace MS.DFe.PDF.Extensoes
{
    internal static class DecimalExtensao
    {
        public static string Formata(this decimal valor)
        {
            CultureInfo bz = new CultureInfo("pt-BR");
            return valor.ToString("#,##0.00", bz);
        }
    }
}
