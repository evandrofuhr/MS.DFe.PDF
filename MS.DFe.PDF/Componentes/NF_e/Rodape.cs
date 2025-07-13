using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

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
                    .AlignRight()
                    .Text
                    (text =>
                        {
                            text.Span(NFeResource.MICROSALES_INFO).FontSize(7.2f).Italic();
                            text.Hyperlink(NFeResource.URL_MS_TEXT, NFeResource.URL_MS).FontSize(7.2f).Italic().FontColor("3366CC");
                        }
                    );
            }
            else
            {
                container
                    .Row(
                        row =>
                        {
                            row.RelativeItem()
                                .AlignRight()
                                .Text(text =>
                                    {
                                        text.Span(NFeResource.MICROSALES_INFO).FontSize(7.2f).Italic();
                                        text.Hyperlink(NFeResource.URL_MS_TEXT, NFeResource.URL_MS).Italic().FontSize(7.2f).FontColor("3366CC");
                                    }
                                );

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
            var cpl = _infnfe.infAdic?.infCpl ?? string.Empty;
            var fisco = _infnfe.infAdic?.infAdFisco ?? string.Empty;

            var _informacaoAdicionalCpl = $"{NFeResource.INF_CONTRIBUENTE} {_infnfe.infAdic.infCpl}";
            var _informacaoAdicionalFisco = $"{NFeResource.INF_FISCO} {_infnfe.infAdic.infAdFisco}";
            var _tamanhoFonte = (cpl.Length + fisco.Length).ToTamanhoFonte();

            container.Column(
                column =>
                {
                    column.Item().PadraoLabelGrupo(NFeResource.DADOS_ADICIONAIS); ;

                    column.Item().Row(
                        row =>
                        {
                            row.ConstantItem(400)
                                .Border(ConstantsHelper.BORDA)
                                .AlignLeft()
                                .Height(110)
                                .Padding(ConstantsHelper.PADDING)
                                .Text(text =>
                                    {
                                        text.Line(NFeResource.INFORMAÇÕES_COMPLEMENTARES).FontSize(7);
                                        text.Line((!string.IsNullOrWhiteSpace(_infnfe.infAdic.infCpl) ? _informacaoAdicionalCpl + "\r\n" : "") +
                                        (!string.IsNullOrWhiteSpace(_infnfe.infAdic.infAdFisco) ? _informacaoAdicionalFisco + "\r\n" : "") + $"{NFeResource.VALOR_APROXIMADO_TRIBUTOS} {NFeResource.CIFRAO} {_infnfe.total.ICMSTot.vTotTrib.ToString()}").FontSize(_tamanhoFonte);
                                    }
                                );

                            row.RelativeItem()
                                .Border(ConstantsHelper.BORDA)
                                .Height(70)
                                .Padding(ConstantsHelper.PADDING)
                                .Text(NFeResource.RESERVADO_FISCO).FontSize(7);
                        }
                    );
                    column.Item().Element(ComposeSoftwareHouse);
                    column.Item().Height(7, Unit.Millimetre);
                }
            );
        
        }

    }
}
