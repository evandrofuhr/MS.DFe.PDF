using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{

    public class CalculoImposto : IComponent
    {

        private readonly ICMSTot _icmsTot;

        public CalculoImposto(ICMSTot icmsTot)
        {
            _icmsTot = icmsTot;
        }
        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().PadraoLabelGrupo(NFeResource.CALCULO_IMPOSTO);

                    column.Item().Row(
                        row =>
                        {
                            row.RelativeItem().PadraoInformacao(NFeResource.BASE_CALCULO_ICMS, _icmsTot.vBC);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_ICMS, _icmsTot.vICMS);
                            row.RelativeItem().PadraoInformacao(NFeResource.BASE_CALCULO_ICMS_ST, _icmsTot.vBCST);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_ICMS_SUBST, _icmsTot.vST);
                            row.RelativeItem().PadraoInformacao(NFeResource.V_IMP_IMPORTACAO, _icmsTot.vII);
                            row.RelativeItem().PadraoInformacao(NFeResource.V_ICMS_UF_REMET, _icmsTot.vICMSUFRemet);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_FCP, _icmsTot.vFCP);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_PIS, _icmsTot.vPIS);
                            row.RelativeItem().PadraoInformacao(NFeResource.V_TOTAL_PRODUTOS, _icmsTot.vProd);
                        }
                    );

                    column.Item().Row(
                        row =>
                        {
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_FRETE, _icmsTot.vFrete);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_SEGURO, _icmsTot.vSeg);
                            row.RelativeItem().PadraoInformacao(NFeResource.DESCONTO, _icmsTot.vDesc);
                            row.RelativeItem().PadraoInformacao(NFeResource.OUTRAS_DESPESAS, _icmsTot.vOutro);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_IPI, _icmsTot.vIPI);
                            row.RelativeItem().PadraoInformacao(NFeResource.V_ICMS_UF_DEST, _icmsTot.vICMSUFDest);
                            row.RelativeItem().PadraoInformacao(NFeResource.V_TOT_TRIB, _icmsTot.vTotTrib);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_COFINS, _icmsTot.vCOFINS);
                            row.RelativeItem().PadraoInformacao(NFeResource.VALOR_TOTAL_NOTA, _icmsTot.vNF);
                        }
                    );
                }
            );
        }
    }
}
