using System.Collections.Generic;
using System.Linq;

namespace MS.DFe.PDF.Modelos
{
    public class DFeDados
    {
        public DFeDadosEmitente emit { get; }
        public IEnumerable<DFeDadosItem> det { get; }
        public DFeDadosTotal total { get; }
        public IEnumerable<DFeDadosPagamento> pag { get; }
        public DFeDadosConsumidor dest { get; }
        public DFeDadosConsulta consulta { get; }
        public DFeDadosIdentificacao identificacao { get; }
        public string qrCode { get; }
        public decimal vTotTrib { get; }

        public int TotalItens { get => det.Count(); }

        public DFeDados(DFeDadosEmitente emit, IEnumerable<DFeDadosItem> det, DFeDadosTotal total, IEnumerable<DFeDadosPagamento> pag, DFeDadosConsumidor dest, DFeDadosConsulta consulta, DFeDadosIdentificacao identificacao, string qrCode, decimal vTotTrib)
        {
            this.emit = emit;
            this.det = det;
            this.total = total;
            this.pag = pag;
            this.dest = dest;
            this.consulta = consulta;
            this.identificacao = identificacao;
            this.qrCode = qrCode;
            this.vTotTrib = vTotTrib;
        }
    }
}
