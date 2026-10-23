using System;
using System.Text.RegularExpressions;

namespace MS.DFe.PDF.Extensoes
{
    internal static class StringExtensao
    {
        public static string SomenteNumeros(this string valor) => Regex.Replace(valor, @"[^+\d]", "");

        public static string NormalizarCnpjCpf(this string valor)
        {
            if (valor == null)
                return null;

            var normalizado = new char[valor.Length];
            var tamanho = 0;
            foreach (var caractere in valor)
            {
                var maiusculo = char.ToUpperInvariant(caractere);
                if ((maiusculo >= '0' && maiusculo <= '9') || (maiusculo >= 'A' && maiusculo <= 'Z'))
                    normalizado[tamanho++] = maiusculo;
            }
            return new string(normalizado, 0, tamanho);
        }

        public static string NormalizarChaveNFe(this string valor)
        {
            var chave = valor?.NormalizarCnpjCpf();
            if (string.IsNullOrEmpty(chave))
                return chave;
            if (chave.StartsWith("NFE") && chave.Length > 3)
                chave = chave.Substring(3);
            return chave;
        }

        public static string FormataCNPJCPF(this string valor)
        {
            var _valor = valor?.NormalizarCnpjCpf() ?? string.Empty;
            if (_valor.Length == 11 && SomenteDigitos(_valor))
                return $"{_valor.Substring(0, 3)}.{_valor.Substring(3, 3)}.{_valor.Substring(6, 3)}-{_valor.Substring(9, 2)}";
            if (_valor.Length == 14 && CnpjNoFormato(_valor))
                return $"{_valor.Substring(0, 2)}.{_valor.Substring(2, 3)}.{_valor.Substring(5, 3)}/{_valor.Substring(8, 4)}-{_valor.Substring(12, 2)}";
            return valor;
        }

        private static bool SomenteDigitos(string valor)
        {
            foreach (var caractere in valor)
            {
                if (caractere < '0' || caractere > '9')
                    return false;
            }
            return true;
        }

        private static bool CnpjNoFormato(string item)
        {
            for (var i = 0; i < 12; i++)
            {
                var caractere = item[i];
                if (!((caractere >= '0' && caractere <= '9') || (caractere >= 'A' && caractere <= 'Z')))
                    return false;
            }
            return item[12] >= '0' && item[12] <= '9' && item[13] >= '0' && item[13] <= '9';
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
