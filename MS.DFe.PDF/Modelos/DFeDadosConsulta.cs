using MS.DFe.PDF.Extensoes;

namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosConsulta
    {
        public string urlChave { get; set; }
        public string chNFe { get; }
        public string ChaveNFe { get => chNFe.FormataChaveNFe(); }

        public DFeDadosConsulta(string urlChave, string chNFe)
        {
            this.urlChave = urlChave;
            this.chNFe = chNFe;
        }
    }
}
