using NFe.Classes.Informacoes.Transporte;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.DFe.PDF.Helpers
{
    public static class ModoFreteHelper
    {
        public static string Frete(transp _transp)
        {
            var _frete = "";

            switch (Convert.ToInt32(_transp.modFrete))
            {
                case 0:
                    _frete = "0 - Contratação do Frete por conta do Remetente";
                    break;
                case 1:
                    _frete = "1 - Contratação do Frete por conta do Destinatário";
                    break;

                case 2:
                    _frete = "2 - Contratação do Frete por conta de Terceiros";
                    break;
                case 3:
                    _frete = "3 - Transporte Próprio por conta do Remetente";
                    break;
                case 4:
                    _frete = "4 - Transporte Próprio por conta do Destinatário";
                    break;

                case 9:
                    _frete = "9 - Sem Ocorrência de Transporte";
                    break;
                default:
                    _frete = "";
                    break;
            }
            return _frete;
        }
    }
}
