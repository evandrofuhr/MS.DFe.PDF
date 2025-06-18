using System.Globalization;

namespace MS.DFe.PDF.Extensoes
{
    internal static class NumberExtensao
    {
        public static string Formata(this decimal valor)
        {
            CultureInfo bz = new CultureInfo("pt-BR");
            return valor.ToString("#,##0.00", bz);
        }

        public static string Formata(this int valor)
        {
            return valor.ToString();
        }

        public static string FormataNumero(this long valor) => valor.ToString().PadLeft(9, '0');

        public static string FormataSerie(this int valor) => valor.ToString().PadLeft(3, '0');
    }
}
