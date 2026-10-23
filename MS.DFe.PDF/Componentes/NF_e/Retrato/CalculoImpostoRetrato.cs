using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    /// <summary>
    /// Totais na ordem da NT 2026.010. Campos em azul do modelo só entram
    /// quando a tag correspondente existe no XML.
    /// </summary>
    public class CalculoImpostoRetrato : IComponent
    {
        private readonly total _total;

        public CalculoImpostoRetrato(total total)
        {
            _total = total;
        }

        public void Compose(IContainer container)
        {
            var icms = _total.ICMSTot;

            container.Column(column =>
            {
                column.Item().PadraoLabelGrupo("TOTAL DOS PRODUTOS E TOTAL DA NOTA");
                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(NFeResource.V_TOTAL_PRODUTOS, icms.vProd);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_FRETE, icms.vFrete);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_SEGURO, icms.vSeg);
                    row.RelativeItem().PadraoInformacao(NFeResource.DESCONTO, icms.vDesc);
                    row.RelativeItem().PadraoInformacao(NFeResource.OUTRAS_DESPESAS, icms.vOutro);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_TOTAL_NOTA, icms.vNF);
                });

                column.Item().PadraoLabelGrupo("TOTAL DO ICMS / IPI");
                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(NFeResource.BASE_CALCULO_ICMS, icms.vBC);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_ICMS, icms.vICMS);
                    row.RelativeItem().PadraoInformacao(NFeResource.BASE_CALCULO_ICMS_ST, icms.vBCST);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_ICMS_SUBST, icms.vST);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_IPI, icms.vIPI);
                });

                LinhaSeHouver(
                    column,
                    ("VALOR DO FCP", Informado(icms.vFCP, icms.vFCPSpecified)),
                    ("VALOR DO FCP RETIDO POR ST", Informado(icms.vFCPST, icms.vFCPSTSpecified)),
                    ("VALOR DO DIFAL NA UF DE DESTINO", icms.vICMSUFDest),
                    ("VALOR DO FCP NA UF DE DESTINO", icms.vFCPUFDest));

                LinhaSeHouver(
                    column,
                    ("BC DO ICMS MONOFÁSICO", icms.qBCMono),
                    ("VALOR DO ICMS MONOFÁSICO", icms.vICMSMono),
                    ("BC DO ICMS MONOFÁSICO POR RETENÇÃO", icms.qBCMonoReten),
                    ("VALOR DO ICMS MONOFÁSICO POR RETENÇÃO", icms.vICMSMonoReten));

                ComporIbs(column);
            });
        }

        private void ComporIbs(ColumnDescriptor column)
        {
            var ibs = _total.IBSCBSTot;
            var impostoSeletivo = _total.ISTot;
            if (ibs == null && impostoSeletivo == null)
                return;

            column.Item().PadraoLabelGrupo("TOTAL DO IBS / CBS / IS");
            column.Item().Row(row =>
            {
                row.RelativeItem().PadraoInformacao("VALOR DA CBS", ibs?.gCBS?.vCBS);
                row.RelativeItem().PadraoInformacao("VALOR DO IBS UF", ibs?.gIBS?.gIBSUF?.vIBSUF);
                row.RelativeItem().PadraoInformacao("VALOR DO IBS MUNICÍPIO", ibs?.gIBS?.gIBSMun?.vIBSMun);
                row.RelativeItem().PadraoInformacao("VALOR DO IMPOSTO SELETIVO", impostoSeletivo?.vIS);
            });

            var mono = ibs?.gMono;
            if (mono == null)
                return;

            column.Item().Row(row =>
            {
                row.RelativeItem().PadraoInformacao("VALOR DO IBS MONOFÁSICO", mono.vIBSMono);
                row.RelativeItem().PadraoInformacao("VALOR DA CBS MONOFÁSICA", mono.vCBSMono);
                if (mono.vIBSMonoReten != 0m || mono.vCBSMonoReten != 0m)
                {
                    row.RelativeItem().PadraoInformacao("VALOR DO IBS MONOFÁSICO POR RETENÇÃO", mono.vIBSMonoReten);
                    row.RelativeItem().PadraoInformacao("VALOR DA CBS MONOFÁSICA POR RETENÇÃO", mono.vCBSMonoReten);
                }
            });
        }

        private static void LinhaSeHouver(ColumnDescriptor column, params (string Label, decimal? Valor)[] campos)
        {
            var algum = false;
            for (var i = 0; i < campos.Length; i++)
            {
                if (campos[i].Valor.HasValue)
                {
                    algum = true;
                    break;
                }
            }

            if (!algum)
                return;

            column.Item().Row(row =>
            {
                for (var i = 0; i < campos.Length; i++)
                {
                    var campo = campos[i];
                    if (campo.Valor.HasValue)
                        row.RelativeItem().PadraoInformacao(campo.Label, campo.Valor);
                    else
                        row.RelativeItem().PadraoInformacao(campo.Label, string.Empty, "valor");
                }
            });
        }

        private static decimal? Informado(decimal? valor, bool specified)
        {
            if (valor.HasValue)
                return valor;
            if (specified)
                return 0m;
            return null;
        }
    }
}
