using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Total;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;

namespace MS.DFe.PDF.Componentes.Nfe
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
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.CALCULO_IMPOSTO).SemiBold();

                    column.Item().Row(col =>
                    {
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.BASE_CALCULO_ICMS, _icmsTot.vBC.ToString("N2")));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_ICMS, _icmsTot.vICMS.ToString("N2")));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.BASE_CALCULO_ICMS_ST, _icmsTot.vBCST.ToString("N2")));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_ICMS_SUBST, _icmsTot.vST.ToString("N2")));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.V_IMP_IMPORTAÇÃO, _icmsTot.vII.ToString("N2")));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.V_ICMS_UF_REMET, _icmsTot.vICMSUFRemet.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_FCP, _icmsTot.vFCP.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_PIS, _icmsTot.vPIS.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.V_TOTAL_PRODUTOS, _icmsTot.vProd.ToString("N2")));
                    });

                    column.Item().Row(col =>
                    {
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_FRETE, _icmsTot.vFrete.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_SEGURO, _icmsTot.vSeg.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.DESCONTO, _icmsTot.vDesc.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.OUTRAS_DESPESAS, _icmsTot.vOutro.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_IPI, _icmsTot.vIPI.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.V_ICMS_UF_DEST, _icmsTot.vICMSUFDest.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.V_TOT_TRIB, _icmsTot.vTotTrib.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_COFINS, _icmsTot.vCOFINS.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.VALOR_TOTAL_NOTA, _icmsTot.vNF.ToString("N2")));
                    });
                }
            );
        }
    }
}
