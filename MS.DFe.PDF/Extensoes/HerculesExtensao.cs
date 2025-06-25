using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Pagamento;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace MS.DFe.PDF.Extensoes
{
    internal static class HerculesExtensao
    {
        public static string ObterDescricao(this Enum valor)
        {
            var tipo = valor.GetType();
            var membro = tipo.GetMember(valor.ToString());
            if (membro.Length > 0)
            {
                var atributo = membro[0].GetCustomAttribute<DescriptionAttribute>();
                if (atributo != null)
                    return atributo.Description;
            }
            return valor.ToString(); 
        }

        public static string PagamentoDescricao(this detPag pagamento)
        {
            int codigo = (int)pagamento.tPag;
            return NFCeResource.ResourceManager.GetString($"TIPO_PAGAMENTO_{codigo}")
                   ?? NFCeResource.TIPO_PAGAMENTO_99;
        }

        public static string ToEndereco(this enderDest value)
        {
            var _value = $"{value.xLgr}, {value.nro}";
            if (!string.IsNullOrEmpty(value.xCpl))
                _value += $" - {value.xCpl}";
            return _value;
        }

        public static string ToEndereco1(this enderEmit value)
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

        public static string FormataAmbiente(this EAmbiente valor)
        {
            if (valor == EAmbiente.PRODUCAO) return CCeResource.AMBIENTE_PRODUCAO;
            return CCeResource.AMBIENTE_HOMOLOGACAO;
        }
    }
}
