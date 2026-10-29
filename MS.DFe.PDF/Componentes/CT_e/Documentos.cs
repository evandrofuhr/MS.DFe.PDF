using CTe.Classes.Informacoes.infCTeNormal;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Documentos : IComponent
    {
        private readonly infCTeNorm _norm;

        public Documentos(infCTeNorm norm)
        {
            _norm = norm;
        }

        public void Compose(IContainer container)
        {
            var docs = new List<string>();
            var infDoc = _norm?.infDoc;
            if (infDoc?.infNFe != null)
            {
                foreach (var nfe in infDoc.infNFe)
                    docs.Add($"NF-e  {(nfe.chave ?? string.Empty).NormalizarChaveNFe().FormataChaveNFe()}");
            }
            if (infDoc?.infNF != null)
            {
                foreach (var nf in infDoc.infNF)
                    docs.Add($"NF  Série {nf.serie} Nº {nf.nDoc}");
            }
            if (infDoc?.infOutros != null)
            {
                foreach (var o in infDoc.infOutros)
                {
                    var tp = (int)o.tpDoc;
                    if (tp == 0)
                        docs.Add($"DECLARAÇÃO  Declaração Doc.: {o.nDoc}");
                    else if (tp == 10)
                        docs.Add($"DUTOVIÁRIO  Dutoviário Doc.: {o.nDoc}");
                    else
                        docs.Add($"OUTROS  Doc.: {o.nDoc}    Descr.: {o.descOutros}");
                }
            }

            if (docs.Count == 0) return;

            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Padding(ConstantsHelper.PADDING).Text(CTeResource.DOCUMENTOS_ORIGINARIOS).Bold().FontSize(6);
                var metade = (docs.Count + 1) / 2;
                column.Item().Padding(ConstantsHelper.PADDING).Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        for (var i = 0; i < metade && i < docs.Count; i++)
                            col.Item().Text(docs[i]).FontSize(6);
                    });
                    row.RelativeItem().Column(col =>
                    {
                        for (var i = metade; i < docs.Count; i++)
                            col.Item().Text(docs[i]).FontSize(6);
                    });
                });
            });
        }
    }
}
