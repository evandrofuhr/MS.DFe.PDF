using CTe.Classes.Informacoes.Impostos.ICMS;
using CTe.Classes.Informacoes.Impostos.ICMS.Tipos;
using CTe.Classes.Informacoes.Impostos.Tributacao;
using CTe.Classes.Informacoes.Tipos;
using System.Globalization;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal static class CTeHelper
    {
        private static readonly CultureInfo PtBr = new CultureInfo("pt-BR");

        public static string ModalNome(modal valor)
        {
            switch (valor)
            {
                case modal.aereo: return "AÉREO";
                case modal.aquaviario: return "AQUAVIÁRIO";
                case modal.ferroviario: return "FERROVIÁRIO";
                case modal.dutoviario: return "DUTOVIÁRIO";
                case modal.multimodal: return "MULTIMODAL";
                default: return "RODOVIÁRIO";
            }
        }

        public static string TipoCTe(tpCTe valor)
        {
            switch (valor)
            {
                case tpCTe.Complemento: return "COMPLEMENTO";
                case tpCTe.Anulacao: return "ANULAÇÃO";
                case tpCTe.Substituto: return "SUBSTITUTO";
                case tpCTe.Simplificado: return "SIMPLIFICADO";
                case tpCTe.SubstitutoSimplificado: return "SUBSTITUTO SIMPLIFICADO";
                default: return "NORMAL";
            }
        }

        public static string TipoServico(tpServ valor)
        {
            switch (valor)
            {
                case tpServ.subcontratacao: return "SUBCONTRATAÇÃO";
                case tpServ.redespacho: return "REDESPACHO";
                case tpServ.redespachoIntermediario: return "REDESPACHO INTERMEDIÁRIO";
                case tpServ.servicoVinculadoMultimodal: return "SERV. VINC. MULTIMODAL";
                case tpServ.transportePessoas: return "TRANSPORTE DE PESSOAS";
                case tpServ.transporteValores: return "TRANSPORTE DE VALORES";
                case tpServ.excessoBagagem: return "EXCESSO DE BAGAGEM";
                default: return "NORMAL";
            }
        }

        public static string FormaPagamento(forPag? valor)
        {
            if (valor == null) return string.Empty;
            switch (valor.Value)
            {
                case forPag.Pago: return "PAGO";
                case forPag.Apagar: return "A PAGAR";
                default: return "OUTROS";
            }
        }

        public static string TomadorNome(toma valor)
        {
            switch (valor)
            {
                case toma.Remetente: return "REMETENTE";
                case toma.Expedidor: return "EXPEDIDOR";
                case toma.Recebedor: return "RECEBEDOR";
                case toma.Destinatario: return "DESTINATÁRIO";
                default: return "OUTROS";
            }
        }

        public static string RespSeguro(respSeg valor)
        {
            switch (valor)
            {
                case respSeg.Remetente: return "REMETENTE";
                case respSeg.Expedidor: return "EXPEDIDOR";
                case respSeg.Recebedor: return "RECEBEDOR";
                case respSeg.Destinatario: return "DESTINATÁRIO";
                case respSeg.EmitenteDoCTe: return "EMITENTE";
                case respSeg.TomadorDoServico: return "TOMADOR";
                default: return string.Empty;
            }
        }

        public static string FormatQ(decimal valor) => valor.ToString("#,##0.000", PtBr);

        public static void ExtrairIcms(ICMS icms, out string situacao, out string vBc, out string pIcms, out string vIcms, out string pRed)
        {
            situacao = string.Empty;
            vBc = string.Empty;
            pIcms = string.Empty;
            vIcms = string.Empty;
            pRed = string.Empty;

            var tipo = icms?.TipoICMS;
            if (tipo == null) return;

            if (tipo is ICMSSN)
            {
                situacao = "SN - SIMPLES NACIONAL";
            }
            else if (tipo is ICMS00 icms00)
            {
                situacao = "00 - TRIBUTAÇÃO NORMAL DO ICMS";
                vBc = icms00.vBC.ToString("#,##0.00", PtBr);
                pIcms = icms00.pICMS.ToString("#,##0.00", PtBr);
                vIcms = icms00.vICMS.ToString("#,##0.00", PtBr);
            }
            else if (tipo is ICMS20 icms20)
            {
                situacao = "20 - TRIBUTAÇÃO COM REDUÇÃO DE BC DO ICMS";
                vBc = icms20.vBC.ToString("#,##0.00", PtBr);
                pIcms = icms20.pICMS.ToString("#,##0.00", PtBr);
                vIcms = icms20.vICMS.ToString("#,##0.00", PtBr);
                pRed = icms20.pRedBC.ToString("#,##0.00", PtBr);
            }
            else if (tipo is ICMS45 icms45)
            {
                switch (icms45.CST)
                {
                    case CST.ICMS40: situacao = "40 - ICMS ISENÇÃO"; break;
                    case CST.ICMS41: situacao = "41 - ICMS NÃO TRIBUTADO"; break;
                    case CST.ICMS51: situacao = "51 - ICMS DIFERIDO"; break;
                    default: situacao = "45 - ICMS ISENTO/NÃO TRIBUTADO"; break;
                }
            }
            else if (tipo is ICMS60)
            {
                situacao = "60 - ICMS COBRADO ANTERIORMENTE POR SUBSTITUIÇÃO TRIBUTÁRIA";
            }
            else if (tipo is ICMS90 icms90)
            {
                situacao = "90 - ICMS OUTROS";
                vBc = icms90.vBC.ToString("#,##0.00", PtBr);
                pIcms = icms90.pICMS.ToString("#,##0.00", PtBr);
                vIcms = icms90.vICMS.ToString("#,##0.00", PtBr);
                if (icms90.pRedBCSpecified && icms90.pRedBC.HasValue)
                    pRed = icms90.pRedBC.Value.ToString("#,##0.00", PtBr);
            }
            else if (tipo is ICMSOutraUF outra)
            {
                situacao = "90 - ICMS Outra UF";
                vBc = outra.vBCOutraUF.ToString("#,##0.00", PtBr);
                pIcms = outra.pICMSOutraUF.ToString("#,##0.00", PtBr);
                vIcms = outra.vICMSOutraUF.ToString("#,##0.00", PtBr);
                if (outra.pRedBCOutraUFSpecified && outra.pRedBCOutraUF.HasValue)
                    pRed = outra.pRedBCOutraUF.Value.ToString("#,##0.00", PtBr);
            }
            else if (tipo is ICMSBasico)
            {
                situacao = tipo.GetType().Name;
            }
        }
    }
}
