using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual.Tipos;
using NFe.Classes.Informacoes.Transporte;
using System.Linq;
using System;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;


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
            var _frete = ModoFreteHelper.Frete(_transp);
           
            container.Column(
                column =>
                {
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.TRANSPORTADOR).SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(7).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.RAZAO_SOCIAL, _transp.transporta?.xNome));
                        row.RelativeItem(6).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.FRETE, _frete));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.CODIGO_ANTT, string.Empty));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.PLACA_VEICULO, _transp.veicTransp?.placa));
                        row.ConstantItem(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.UF, _transp.veicTransp?.UF));
                        row.ConstantItem(69).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.CNPJ_CPF, _transp.transporta?.CNPJ.FormataCNPJCPF() ?? _transp.transporta?.CPF.FormataCNPJCPF()));
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(13).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.ENDERECO, _transp.transporta?.xEnder));
                        row.RelativeItem(9).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.MUNICIPIO, _transp.transporta?.xMun));
                        row.ConstantItem(15).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.UF, _transp.transporta?.UF));
                        row.ConstantItem(69).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.INSCRICAO_ESTADUAL, _transp.transporta?.IE));
                    });

                    var _volume = _transp.vol.GroupBy(g => true).Select(s => new
                    {
                        Quantidade = s.Sum(t => t.qVol ?? null),
                        PesoLiquido = s.Sum(t => t.pesoL ?? null),
                        PesoBruto = s.Sum(t => t.pesoB ?? null),
                        Especie = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.esp)).Select(t => t.esp).Distinct()),
                        Numeracao = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.nVol)).Select(t => t.nVol).Distinct()),
                        Marca = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.marca)).Select(t => t.marca).Distinct())
                    }).FirstOrDefault();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.QUANTIDADE, _volume?.Quantidade.ToString()));
                        row.RelativeItem(6).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.ESPECIE, _volume?.Especie));
                        row.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.MARCA, _volume?.Marca));
                        row.RelativeItem(4).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.NUMERACAO, _volume?.Numeracao));
                        row.RelativeItem(8).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.PESO_BRUTO, (_volume?.PesoBruto) == 0 ? "" : _volume?.PesoBruto.ToString()));
                        row.ConstantItem(80).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Valor(NFeResource.PESO_LIQUIDO, (_volume?.PesoLiquido) == 0 ? "" : _volume?.PesoLiquido.ToString()));
                    });
                }
            );
        }
    }
}
