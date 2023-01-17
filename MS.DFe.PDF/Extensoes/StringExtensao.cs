using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
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

        public static string FormataAmbiente(this EAmbiente valor)
        {
            if (valor == EAmbiente.PRODUCAO) return CCeResource.AMBIENTE_PRODUCAO;
            return CCeResource.AMBIENTE_HOMOLOGACAO;
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
    }
}
