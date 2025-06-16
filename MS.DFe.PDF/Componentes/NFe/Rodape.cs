using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;

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
            var _informacaoAdicional = "Inf. fisco:";

            if (_infnfe.infAdic.infCpl != null)
                _informacaoAdicional = "Inf. Contribuinte:";


            container.Column(col =>
            {
                col.Item().Text("DADOS ADICIONAIS").SemiBold();
                col.Item().Row(row =>
                {
                    row.RelativeItem().AlignLeft().Border(DadoPadraoExtensoes.BORDA).Height(80).Width(280).Padding(DadoPadraoExtensoes.PADDING).Text(text =>
                    {
                        text.Line("INFORMAÇÕES COMPLEMENTARES");
                        text.Line($"{_informacaoAdicional} {_infnfe.infAdic.infAdFisco ?? _infnfe.infAdic.infCpl}\r\nValor Aproximado dos Tributos: {_infnfe.total.ICMSTot.vTotTrib.ToString()}").FontSize(7);
                    });
                    row.RelativeItem().Border(DadoPadraoExtensoes.BORDA).Height(80).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("RESERVADO AO FISCO", ""));
                });
                col.Item().Text("Documento impresso por MicroSales - www.microsales.com.br").FontSize(6).AlignRight().Italic();
            });
        }
    }
}
