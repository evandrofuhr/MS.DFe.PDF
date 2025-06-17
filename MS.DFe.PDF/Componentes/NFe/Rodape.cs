using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Componentes.Nfe
{
    public class Rodape : IComponent
    {

        private readonly infNFe _infnfe;

        public Rodape(infNFe infenfe)
        {
            _infnfe = infenfe;
        }

        public void Compose(IContainer container)
        {
            var _informacaoAdicionalCpl = $"{NFeResource.INF_CONTRIBUENTE} {_infnfe.infAdic.infCpl}";
            var _informacaoAdicionalFisco = $"{NFeResource.INF_FISCO} {_infnfe.infAdic.infAdFisco}";

            container.Column(col =>
            {
                col.Item().Text(NFeResource.DADOS_ADICIONAIS).SemiBold();
                col.Item().Row(row =>
                {
                    row.ConstantItem(370).Border(DadoPadraoExtensoes.BORDA).AlignLeft().Height(80).Padding(DadoPadraoExtensoes.PADDING).Text(text =>
                    {
                        text.Line(NFeResource.INFORMAÇÕES_COMPLEMENTARES);
                        text.Line((!string.IsNullOrWhiteSpace(_informacaoAdicionalCpl) ? _informacaoAdicionalCpl + "\r\n" : "") +
                        (!string.IsNullOrWhiteSpace(_informacaoAdicionalFisco) ? _informacaoAdicionalFisco + "\r\n" : "") + $"{NFeResource.VALOR_APROXIMADO_TRIBUTOS} {NFeResource.CIFRAO} {_infnfe.total.ICMSTot.vTotTrib.ToString()}").FontSize(7);
                    });
                    row.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Height(80).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.RESERVADO_FISCO, NFeResource.NULL));
                });
                col.Item().Text(NFeResource.MICROSALES_INFO).FontSize(6).AlignRight().Italic();
            });
        }
    }
}
