using MS.DFe.PDF.Resources;
using System;

namespace MS.DFe.PDF.Extensoes
{
    internal static class DateTimeExtensao
    {
        public static string FormataDataHora(this DateTimeOffset valor) => valor.ToString(NFCeResource.FORMATO);
    }
}
