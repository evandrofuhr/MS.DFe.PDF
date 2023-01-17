using MS.DFe.PDF.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.DFe.PDF.Extensoes
{
    internal static class DateTimeExtensao
    {
        public static string FormataDataHora(this DateTimeOffset valor) =>  valor.ToString(NFCeResource.FORMATO);
    }
}
