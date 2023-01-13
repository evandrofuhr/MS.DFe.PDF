namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosConsulta
    {
        public string urlChave { get; set; }
        public string chNFe { get; }

        public string ChaveNFe
        {
            get
            {
                var _nova = string.Empty;
                var _conta = 0;

                foreach (char c in chNFe)
                {
                    _conta++;
                    _nova += c;

                    if (_conta == 4)
                    {
                        _nova += " ";
                        _conta = 0;
                    }
                }
                return _nova;
            }
        }

        public DFeDadosConsulta(string urlChave, string chNFe)
        {
            this.urlChave = urlChave;
            this.chNFe = chNFe;
        }
    }
}
