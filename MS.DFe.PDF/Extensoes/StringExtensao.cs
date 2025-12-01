using System;
using System.Text.RegularExpressions;

namespace MS.DFe.PDF.Extensoes
{
    internal static class StringExtensao
    {
        public static string SomenteNumeros(this string valor) => Regex.Replace(valor, @"[^+\d]", "");

        public static string FormataCNPJCPF(this string valor)
        {
            var _valor = valor?.SomenteNumeros() ?? string.Empty;
            if (_valor.Length == 11)
                return Convert.ToUInt64(_valor).ToString(@"000\.000\.000\-00");
            else if (_valor.Length == 14) //CNPJ
                return Convert.ToUInt64(_valor).ToString(@"00\.000\.000/0000\-00");
            return valor;
        }

        public static string FormataChaveNFe(this string valor)
        {
            var _nova = string.Empty;
            var _conta = 0;

            foreach (char c in valor)
            {
                _conta++;
                _nova += c;

                if (_conta == 4)
                {
                    _nova += " ";
                    _conta = 0;
                }
            }
            return _nova;
        }

        public static string ToTelefone(this string value)
        {
            var _base = value?.Trim() ?? string.Empty;
            var _value = string.Empty;
            if (string.IsNullOrEmpty(_base))
                return string.Empty;
            if (_base.StartsWith("+"))
                return _base;
            if (_base.StartsWith("0800"))
                _value = Convert.ToUInt64(_base).ToString(@"0000 000\-0000");
            else if (_base.Length <= 10)
                _value = Convert.ToUInt64(_base).ToString(@"\(00\) 0000\-0000");
            else
                _value = Convert.ToUInt64(_base).ToString(@"\(00\) 0 0000\-0000");
            return _value;
        }

        public static string ToCep(this string value)
        {
            return Convert.ToUInt64(value).ToString(@"00000\-000");
        }

    }
}
