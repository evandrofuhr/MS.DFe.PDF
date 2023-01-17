using MS.DFe.PDF.Extensoes;

namespace MS.DFe.PDF.Modelos
{
    /// <summary>
    /// Dados para impressão do grupo "NOTA FISCAL ELETRÔNICA"
    /// </summary>
    public class CCeDadosNota
    {
        /// <summary>
        /// Razão social da empresa
        /// </summary>
        public string Empresa { get; }
        /// <summary>
        /// CNPJ da empresa
        /// </summary>
        public string CNPJ { get; }
        /// <summary>
        /// Modelo de documento, somente numero ou descrição
        /// Ex.: 55 - NF-e
        /// </summary>
        public string ModeloDocumento { get; }
        /// <summary>
        /// Série da nota . 
        /// </summary>
        public int Serie { get; }

        /// <summary>
        /// Número da nota
        /// </summary>
        public long Numero { get; }

        /// <summary>
        /// Chave de acesso NFe
        /// </summary>
        public string ChaveNFe { get; set; }

        /// <summary>
        /// CNPJ da empresa formatado
        /// Ex.: 00.000.000/0000-00
        /// </summary>
        public string CNPJFormatado { get => CNPJ?.FormataCNPJCPF() ?? string.Empty; }
        /// <summary>
        /// Numero formatado
        /// Ex.: 000000001
        /// </summary>
        public string NumeroFormatado { get => Numero.FormataNumero();  }
        /// <summary>
        /// Serie formatada
        /// Ex.: 0001
        /// </summary>
        public string SerieFormatada { get => Serie.FormataSerie(); }

        /// <summary>
        /// Chave de acesso NFe formatada
        /// Ex.: 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000
        /// </summary>
        public string ChaveNFeFormatada { get => ChaveNFe.FormataChaveNFe(); }

        public CCeDadosNota(string empresa, string cnpj, string modeloDocumento, int serie, long numero, string chaveNFe)
        {
            Empresa = empresa;
            CNPJ = cnpj;
            ModeloDocumento = modeloDocumento;
            Serie = serie;
            Numero = numero;
            ChaveNFe = chaveNFe;
        }
    }
}
