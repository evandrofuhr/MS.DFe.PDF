using CTe.Classes.Informacoes.infCTeNormal;
using CTe.Classes.Informacoes.Tipos;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Carga : IComponent
    {
        private readonly infCTeNorm _norm;

        public Carga(infCTeNorm norm)
        {
            _norm = norm;
        }

        public void Compose(IContainer container)
        {
            if (_norm?.infCarga == null) return;

            var carga = _norm.infCarga;
            var m3 = new StringBuilder();
            var kg = new StringBuilder();
            var ton = new StringBuilder();
            var vol = new StringBuilder();
            var lit = new StringBuilder();
            var mmbtu = new StringBuilder();

            if (carga.infQ != null)
            {
                foreach (var q in carga.infQ)
                {
                    var texto = CTeHelper.FormatQ(q.qCarga) + "\n";
                    switch (q.cUnid)
                    {
                        case cUnid.M3: m3.Append(texto); break;
                        case cUnid.KG: kg.Append(texto); break;
                        case cUnid.TON: ton.Append(texto); break;
                        case cUnid.UNIDADE: vol.Append(texto); break;
                        case cUnid.LITROS: lit.Append(texto); break;
                        case cUnid.MMBTU: mmbtu.Append(texto); break;
                    }
                }
            }

            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem(4).PadraoInformacao(CTeResource.PRODUTO_PREDOMINANTE, carga.proPred);
                    row.RelativeItem(4).PadraoInformacao(CTeResource.OUTRAS_CARACTERISTICAS, carga.xOutCat);
                    row.RelativeItem(2).PadraoInformacao(CTeResource.VALOR_TOTAL_MERCADORIA, carga.vCarga);
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(CTeResource.CUBAGEM, m3.ToString().Trim());
                    row.RelativeItem().PadraoInformacao(CTeResource.PESO_KG, kg.ToString().Trim());
                    row.RelativeItem().PadraoInformacao(CTeResource.PESO_TON, ton.ToString().Trim());
                    row.RelativeItem().PadraoInformacao(CTeResource.VOLUMES, vol.ToString().Trim());
                    row.RelativeItem().PadraoInformacao(CTeResource.LITROS, lit.ToString().Trim());
                    row.RelativeItem().PadraoInformacao(CTeResource.MMBTU, mmbtu.ToString().Trim());
                });

                var seg = _norm.seg?.FirstOrDefault();
                if (seg != null)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().PadraoInformacao(CTeResource.RESPONSAVEL_SEGURO, CTeHelper.RespSeguro(seg.respSeg));
                        row.RelativeItem().PadraoInformacao(CTeResource.SEGURADORA, seg.xSeg);
                        row.RelativeItem().PadraoInformacao(CTeResource.APOLICE, seg.nApol);
                        row.RelativeItem().PadraoInformacao(CTeResource.AVERBACAO, seg.nAver);
                    });
                }
            });
        }
    }
}
