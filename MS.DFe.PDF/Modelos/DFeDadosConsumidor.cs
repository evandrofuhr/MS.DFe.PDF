
using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosConsumidor
    {
        public string CNPJ { get; }
        public string CPF { get; }

        public string Descricao
        {
            get
            {
                if (string.IsNullOrEmpty(CNPJ) && string.IsNullOrEmpty(CPF)) return NFCeResource.CONSUMIDOR_NAO_IDENTIFICADO;
                return $"{NFCeResource.CONSUMIDOR} - {(!string.IsNullOrEmpty(CNPJ) ? NFCeResource.CNPJ : NFCeResource.CPF)} {(!string.IsNullOrEmpty(CNPJ) ? CNPJ : CPF)}";
            }
        }

        public DFeDadosConsumidor(string cnpj, string cpf)
        {
            CNPJ = cnpj;
            CPF = cpf;
        }
    }
}
