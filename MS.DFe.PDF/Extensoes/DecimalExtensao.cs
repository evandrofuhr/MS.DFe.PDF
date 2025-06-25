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

        public static string ToString(this decimal item, int casas = 2)
        {
            var _formato = $"#,##0.{string.Empty.PadRight(casas, '0')}";
            return item.ToString(_formato);
        }

        public static int QuantidadeDecimais(this decimal value)
        {
            var _valueStr = value.ToString("0.###############");

            var _posicao = _valueStr.IndexOf(',');

            if (_posicao == -1)
                return 0;

            var _decimais = 0;
            for (var i = _valueStr.Length - 1; i > _posicao; i--)
            {
                if (_valueStr[i] != '0')
                {
                    _decimais = i - _posicao;
                    break;
                }
            }

            return _decimais;
        }

        public static string ToNumeroNfe(this long value)
        {
            return value.ToString(@"000\.000\.000");
        }

        public static string ToTelefone(this long value)
        {
            return value.ToString().ToTelefone();
        }

        public static string FormataNumero(this long valor) => valor.ToString().PadLeft(9, '0');

        public static string FormataSerie(this int valor) => valor.ToString().PadLeft(3, '0');
    }
}
