using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Resources;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class Rodape : IComponent
    {
        private readonly infNFe _infnfe;
        private readonly byte[] _logo;

        public Rodape(infNFe infenfe, byte[] logo)
        {
            _infnfe = infenfe;
            _logo = logo;
        }

        private void ComposeSoftwareHouse(IContainer container)
        {
            if (_logo == null)
            {
                container
                    .Text(NFeResource.MICROSALES_INFO)
                    .AlignRight()
                    .FontSize(6)
                    .Italic();
            }
            else
            {
                container
                    .Row(
                        row =>
                        {
                            row.RelativeItem()
                                .Text(NFeResource.MICROSALES_INFO)
                                .AlignRight()
                                .FontSize(6)
                                .Italic();

                            row.ConstantItem(1).ShrinkHorizontal();

                            row.ConstantItem(10)
                                .PaddingTop(1)
                                .Image(_logo);
                        }
                    );
            }
        }

        public void Compose(IContainer container)
        {
            var _informacaoAdicionalCpl = $"{NFeResource.INF_CONTRIBUENTE} {_infnfe.infAdic.infCpl}";
            var _informacaoAdicionalFisco = $"{NFeResource.INF_FISCO} {_infnfe.infAdic.infAdFisco}";

            container.Column(
                column =>
                {
                    column.Item().Text(NFeResource.DADOS_ADICIONAIS).SemiBold();
                    column.Item().Row(
                        row =>
                        {
                            row.ConstantItem(370)
                                .Border(DadoPadraoExtensoes.BORDA)
                                .AlignLeft()
                                .Height(80)
                                .Padding(DadoPadraoExtensoes.PADDING)
                                .Text(
                                    text =>
                                    {
                                        text.Line(NFeResource.INFORMAÇÕES_COMPLEMENTARES);
                                        text.Line((!string.IsNullOrWhiteSpace(_informacaoAdicionalCpl) ? _informacaoAdicionalCpl + "\r\n" : "") +
                                        (!string.IsNullOrWhiteSpace(_informacaoAdicionalFisco) ? _informacaoAdicionalFisco + "\r\n" : "") + $"{NFeResource.VALOR_APROXIMADO_TRIBUTOS} {NFeResource.CIFRAO} {_infnfe.total.ICMSTot.vTotTrib.ToString()}").FontSize(7);
                                    }
                                );
                            row.RelativeItem()
                                .Border(DadoPadraoExtensoes.BORDA)
                                .Height(80)
                                .Padding(DadoPadraoExtensoes.PADDING)
                                .Component(CampoInformativo.Padrao(NFeResource.RESERVADO_FISCO, NFeResource.NULL));
                        }
                    );
                    column.Item().Element(ComposeSoftwareHouse);
                }
            );
        }
    }
}
