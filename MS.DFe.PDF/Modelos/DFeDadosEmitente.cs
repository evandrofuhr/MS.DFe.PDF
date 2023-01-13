namespace MS.DFe.PDF.Modelos
{
    public class DFeDadosEmitente
    {
        public string xNome { get; }
        public string CNPJ { get; }
        public string xLgr { get; }
        public string nro { get; }
        public string xBairro { get; }
        public string xMun { get; }
        public string UF { get; }

        public string Endereco
        {
            get
            {
                return $"{xLgr} {nro}, {xBairro}, {xMun}, {UF}";
            }
        }
        public DFeDadosEmitente(string xNome, string cnpj, string xLgr, string nro, string xBairro, string xMun, string uf)
        {
            this.xNome = xNome;
            CNPJ = cnpj;
            this.xLgr = xLgr;
            this.nro = nro;
            this.xBairro = xBairro;
            this.xMun = xMun;
            UF = uf;
        }
    }
}
