using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Math;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
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

        public static string ToEndereco(this enderDest value)
        {
            var _value = $"{value.xLgr}, {value.nro}";
            if (!string.IsNullOrEmpty(value.xCpl))
                _value += $" - {value.xCpl}";
            return _value;
        }

        public static string ToEndereco1 (this enderEmit value)
        {

            var _value = $"{value.xLgr}, {value.nro}";
            if (!string.IsNullOrEmpty(value.xCpl))
                _value += $", {value.xCpl}";
            _value += $", {value.xBairro}";
            return _value;
        }

        public static string ToEndereco2(this enderEmit value)
        {
            return $"{NFeResource.CEP}: {value.CEP.ToCep()} - {value.xMun} - {value.UF}";
        }

        public static string ToEnderecoEmit(this enderEmit value)
        {
            return $"{value.xLgr} {value.nro}, {value.xBairro}, {value.xMun}, {value.UF}";
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


        public static string ToTelefone(this long value)
        {
            return value.ToString().ToTelefone();
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

        public static string ToNumeroNfe(this long value)
        {
            return value.ToString(@"000\.000\.000");
        }
    }
}
