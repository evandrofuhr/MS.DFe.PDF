using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Total;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;


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
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text("CALCULO DE IMPOSTO").SemiBold();

                    column.Item().Row(col =>
                    {
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("BASE DE CÁLC. DO ICMS", _icmsTot.vBC.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO ICMS", _icmsTot.vICMS.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("BASE DE CÁLC. ICMS S.T", _icmsTot.vBCST.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO ICMS SUBST.", _icmsTot.vST.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("V. IMP. IMPORTAÇÃO", _icmsTot.vII.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("V. ICMS UF REMET", _icmsTot.vICMSUFRemet.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO FCP", _icmsTot.vFCP.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO PIS", _icmsTot.vPIS.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("V. TOTAL PRODUTOS", _icmsTot.vProd.ToString()));
                    });

                    column.Item().Row(col =>
                    {
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO FRETE", _icmsTot.vFrete.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO SEGURO", _icmsTot.vSeg.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("DESCONTO", _icmsTot.vDesc.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("OUTRAS DESPESAS", _icmsTot.vOutro.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR IPI", _icmsTot.vIPI.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("V. ICMS UF DEST", _icmsTot.vICMSUFDest.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("V. TOT. TRIB.", _icmsTot.vTotTrib.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR DO COFINS", _icmsTot.vCOFINS.ToString()));
                        col.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("VALOR TOTAL DA NOTA", _icmsTot.vNF.ToString()));
                    });
                }
            );
        }
    }
}
