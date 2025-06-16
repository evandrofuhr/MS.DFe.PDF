using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Transporte;
using System.Linq;
using System;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;


namespace MS.DFe.PDF.Componentes.Nfe
{
    public class Transportador : IComponent
    {
        private readonly transp _transp;

        public Transportador(transp transp)
        {
            _transp = transp;
        }
        public void Compose(IContainer container)
        {

            var _frete = "";

            switch (Convert.ToInt32(_transp.modFrete))
            {
                case 0:
                    _frete = "0 - Contratação do Frete por conta do Remetente";
                    break;
                case 1:
                    _frete = "1 - Contratação do Frete por conta do Destinatário";
                    break;

                case 2:
                    _frete = "2 - Contratação do Frete por conta de Terceiros";
                    break;
                case 3:
                    _frete = "3 - Transporte Próprio por conta do Remetente";
                    break;
                case 4:
                    _frete = "4 - Transporte Próprio por conta do Destinatário";
                    break;

                case 9:
                    _frete = "9 - Sem Ocorrência de Transporte";
                    break;
                default:
                    _frete = "";
                    break;
            }
            container.Column(
                column =>
                {
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text("TRANSPORTADOR / VOLUMES TRANSPORTADOS").SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(7).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("RAZÃO SOCIAL", _transp.transporta?.xNome));
                        row.RelativeItem(6).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("FRETE", _frete));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("CÓDIGO ANTT", string.Empty));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("PLACA DO VEÍCULO", _transp.veicTransp?.placa));
                        row.ConstantItem(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("UF", _transp.veicTransp?.UF));
                        row.ConstantItem(69).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("CNPJ / CPF", _transp.transporta?.CNPJ.FormataCNPJCPF() ?? _transp.transporta?.CPF.FormataCNPJCPF()));
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(13).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("ENDEREÇO", _transp.transporta?.xEnder));
                        row.RelativeItem(9).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("MUNICÍPIO", _transp.transporta?.xMun));
                        row.ConstantItem(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("UF", _transp.transporta?.UF));
                        row.ConstantItem(69).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("INSCRIÇÃO ESTADUAL", _transp.transporta?.IE));
                    });

                    var _volume = _transp.vol.GroupBy(g => true).Select(s => new
                    {
                        Quantidade = s.Sum(t => t.qVol ?? 0),
                        PesoLiquido = s.Sum(t => t.pesoL ?? 0),
                        PesoBruto = s.Sum(t => t.pesoB ?? 0),
                        Especie = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.esp)).Select(t => t.esp).Distinct())
                    }).FirstOrDefault();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("QUANTIDADE", _volume?.Quantidade.ToString("F3")));
                        row.RelativeItem(6).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("ESPÉCIE", _volume?.Especie));
                        row.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("MARCA", ""));
                        row.RelativeItem(4).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("NUMERAÇÃO", ""));
                        row.RelativeItem(8).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("PESO BRUTO", _volume?.PesoBruto.ToString()));
                        row.ConstantItem(80).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoValor("PESO LÍQUIDO", _volume?.PesoLiquido.ToString()));
                    });
                }
            );
        }
    }
}
