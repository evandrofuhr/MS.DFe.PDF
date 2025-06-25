using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using System;

namespace MS.DFe.PDF.Helpers
{
    public static class ImpostosHelper
    {
        public static DadosICMS GetDadosICMS(det item)
        {
            var value = item.imposto?.ICMS;

            if (value == null) return null;

            var _type = value.TipoICMS.GetType();
            var _origem = _type.GetProperty("orig")?.GetValue(value.TipoICMS);
            var _cst = _type.GetProperty("CST")?.GetValue(value.TipoICMS);
            var _csosn = _type.GetProperty("CSOSN")?.GetValue(value.TipoICMS);
            var _base = _type.GetProperty("vBC")?.GetValue(value.TipoICMS);
            var _aliquota = _type.GetProperty("pICMS")?.GetValue(value.TipoICMS);
            var _icms = _type.GetProperty("vICMS")?.GetValue(value.TipoICMS);

            var _cstEnum = string.Empty;

            if (_cst != null)
            {
                _cstEnum = ((Csticms)_cst).ToString().SomenteNumeros();
            }
            else if (_csosn != null)
            {
                _cstEnum = ((Csosnicms)_csosn).ToString().SomenteNumeros();
            }

            return new DadosICMS
            {
                Origem = Convert.ToInt32(_origem),
                CST = _cstEnum,
                Base = _base == null || string.IsNullOrWhiteSpace(_base.ToString())
            ? 0m
            : Convert.ToDecimal(_base),
                Aliquota = Convert.ToDecimal(_aliquota),
                Valor = Convert.ToDecimal(_icms),
            };
        }

        public static DadosIPI GetDadosIPI(det item)
        {
            var _value = item.imposto?.IPI;


            if (_value == null) return null;

            var _type = _value.TipoIPI.GetType();
            var _aliq = _type.GetProperty("pIPI")?.GetValue(_value.TipoIPI);
            var _valor = _type.GetProperty("vIPI")?.GetValue(_value.TipoIPI);

            if (_aliq == null && _valor == null)
                return null;

            return new DadosIPI
            {
                Aliq = Convert.ToDecimal(_aliq),
                Valor = Convert.ToDecimal(_valor)
            };
        }
    }

}
