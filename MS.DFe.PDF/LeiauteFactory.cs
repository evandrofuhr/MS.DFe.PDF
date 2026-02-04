using DFe.Classes.Flags;
using DFe.Utils;
using NFe.Classes;
using NFe.Classes.Protocolo;
using QuestPDF.Infrastructure;
using System;
using System.Linq;

namespace MS.DFe.PDF
{
    public static class LeiauteFactory
    {
        private static byte[] Definir(NFe.Classes.NFe nfe, protNFe protocolo, byte[] logo = null, byte[] logoSoftwareHouse = null)
        {
            if (nfe.infNFe.ide.mod != ModeloDocumento.NFe)
            {
                var _leiaute = new NFeLeiaute(nfe, protocolo, logo);
                if (logoSoftwareHouse?.Any() ?? false)
                    _leiaute.AdicionarLogoSoftwareHouse(logoSoftwareHouse);

                return _leiaute.Gerar();
            }
            else if (nfe.infNFe.ide.mod == ModeloDocumento.NFCe)
            {
                var _leiaute = new NFCeLeiaute(nfe, protocolo);
                return _leiaute.Gerar();
            }
            throw new Exception("Arquivo XML inválido. NF-e e NFC-e são permitidos.");
        }

        public static byte[] Gerar(string xml, byte[] logo = null, byte[] logoSoftwareHouse = null)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            try
            {
                var nfeProc = FuncoesXml.XmlStringParaClasse<nfeProc>(xml);
                var _nfe = nfeProc.NFe;
                var _protocolo = nfeProc.protNFe;
                if (_nfe == null)
                    throw new Exception("Não foi possível converter o arquivo XML.");

                return Definir(_nfe, _protocolo, logo, logoSoftwareHouse);
            }
            catch
            {
                var _nfe = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xml);
                return Definir(_nfe, null);
            }
        }

    }
}
