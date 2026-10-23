using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    public class RodapeRetrato : IComponent
    {
        private const float AlturaCaixa = 90f;

        private readonly infNFe _infnfe;
        private readonly byte[] _logo;
        private readonly string _qrCode;

        public RodapeRetrato(infNFe infnfe, byte[] logo, string qrCode = null)
        {
            _infnfe = infnfe;
            _logo = logo;
            _qrCode = qrCode;
        }

        private void ComposeSoftwareHouse(IContainer container)
        {
            if (_logo == null)
            {
                container
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span(NFeResource.MICROSALES_INFO).FontSize(7.2f).Italic();
                        text.Hyperlink(NFeResource.URL_MS_TEXT, NFeResource.URL_MS).FontSize(7.2f).Italic().FontColor("3366CC");
                    });
                return;
            }

            container.Row(row =>
            {
                row.RelativeItem()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span(NFeResource.MICROSALES_INFO).FontSize(7.2f).Italic();
                        text.Hyperlink(NFeResource.URL_MS_TEXT, NFeResource.URL_MS).Italic().FontSize(7.2f).FontColor("3366CC");
                    });

                row.ConstantItem(1).ShrinkHorizontal();

                row.ConstantItem(10)
                    .PaddingTop(1)
                    .Image(_logo);
            });
        }

        public void Compose(IContainer container)
        {
            var cpl = _infnfe.infAdic?.infCpl ?? string.Empty;
            var fisco = _infnfe.infAdic?.infAdFisco ?? string.Empty;

            var informacao = new List<string>();
            if (!string.IsNullOrEmpty(cpl))
                informacao.Add($"{NFeResource.INF_CONTRIBUENTE} {cpl}");
            if (!string.IsNullOrEmpty(fisco))
                informacao.Add($"{NFeResource.INF_FISCO} {fisco}");

            var final = string.Join("\r\n", informacao);
            var tamanhoFonte = final.Length.ToTamanhoFonte();
            var qr = string.IsNullOrWhiteSpace(_qrCode) ? null : BarCodeHelper.QrCode(_qrCode);

            container.Column(column =>
            {
                column.Item().PadraoLabelGrupo(NFeResource.DADOS_ADICIONAIS);

                column.Item().Row(row =>
                {
                    row.RelativeItem(3)
                        .Border(ConstantsHelper.BORDA)
                        .AlignLeft()
                        .Height(AlturaCaixa)
                        .Padding(ConstantsHelper.PADDING)
                        .Column(c =>
                        {
                            c.Item().Text(NFeResource.INFORMAÇÕES_COMPLEMENTARES).FontSize(7);
                            c.Item().Text(final).ClampLines(12).FontSize(tamanhoFonte);
                            c.Item().Text($"{NFeResource.VALOR_APROXIMADO_TRIBUTOS} {NFeResource.CIFRAO} {_infnfe.total.ICMSTot.vTotTrib}").FontSize(7);
                        });

                    row.RelativeItem()
                        .Border(ConstantsHelper.BORDA)
                        .Height(AlturaCaixa)
                        .Padding(ConstantsHelper.PADDING)
                        .Text(NFeResource.RESERVADO_FISCO).FontSize(7);

                    if (qr != null)
                    {
                        row.ConstantItem(78)
                            .Border(ConstantsHelper.BORDA)
                            .Height(AlturaCaixa)
                            .Padding(4)
                            .Image(qr);
                    }
                });

                column.Item().PaddingTop(2).Element(ComposeSoftwareHouse);
            });
        }
    }
}
