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
using System.Collections.Generic;


namespace MS.DFe.PDF.Componentes.Nfe
{
    public class Transportador : IComponent
    {
        private readonly transp _transp;
        private readonly Dictionary<int, string> _tipoFrete = new Dictionary<int, string>
        {
            {0, "Por conta Remetente"},
            {1, "Por conta Destinatário"},
            {2, "Por conta Terceiros"},
            {3, "Próprio, por conta Rem."},
            {4, "Próprio, por conta Dest."},
            {9, "Sem Transporte"}
        };

        public Transportador(transp transp)
        {
            _transp = transp;
        }
        public void Compose(IContainer container)
        {
            var _modFrete = 9;
            if (_transp.modFrete != null)
                _modFrete = (int)_transp.modFrete;

            var _frete = $"{_modFrete}-{_tipoFrete[_modFrete]}";
           
            container.Column(
                column =>
                {
                    column.Item().PadraoLabelGrupo(NFeResource.TRANSPORTADOR);

                    column.Item().Row(
                        row =>
                        {
                            row.ConstantItem(237).PadraoInformacao(NFeResource.RAZAO_SOCIAL, _transp.transporta?.xNome);
                            row.ConstantItem(110).PadraoInformacao(NFeResource.FRETE, _frete, true);
                            row.ConstantItem(93).PadraoInformacao(NFeResource.CODIGO_ANTT, string.Empty);
                            row.RelativeItem().PadraoInformacao(NFeResource.PLACA_VEICULO, _transp.veicTransp?.placa, true);
                            row.ConstantItem(15).PadraoInformacao(NFeResource.UF, _transp.veicTransp?.UF, true);
                            row.ConstantItem(69).PadraoInformacao(NFeResource.CNPJ_CPF, _transp.transporta?.CNPJ.FormataCNPJCPF() ?? _transp.transporta?.CPF.FormataCNPJCPF(), true);
                        }
                    );

                    column.Item().Row(
                        row =>
                        {
                            row.ConstantItem(237).PadraoInformacao(NFeResource.ENDERECO, _transp.transporta?.xEnder);
                            row.RelativeItem().PadraoInformacao(NFeResource.MUNICIPIO, _transp.transporta?.xMun);
                            row.ConstantItem(15).PadraoInformacao(NFeResource.UF, _transp.transporta?.UF, true);
                            row.ConstantItem(69).PadraoInformacao(NFeResource.INSCRICAO_ESTADUAL, _transp.transporta?.IE, true);
                        }
                    );

                    var _volume = _transp.vol
                        .GroupBy(g => true)
                        .Select(
                            s => new
                            {
                                Quantidade = s.Sum(t => t.qVol ?? 0),
                                PesoLiquido = s.Sum(t => t.pesoL ?? 0),
                                PesoBruto = s.Sum(t => t.pesoB ?? 0),
                                Especie = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.esp)).Select(t => t.esp).Distinct()),
                                Numeracao = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.nVol)).Select(t => t.nVol).Distinct()),
                                Marca = string.Join(",", s.Where(w => !string.IsNullOrEmpty(w.marca)).Select(t => t.marca).Distinct())
                            }
                        )
                        .FirstOrDefault();

                    column.Item().Row(row =>
                    {
                        if (_volume.Quantidade == 0)
                            row.ConstantItem(110).PadraoInformacao(NFeResource.QUANTIDADE, string.Empty);
                        else
                            row.ConstantItem(110).PadraoInformacao(NFeResource.QUANTIDADE, _volume.Quantidade);

                        row.ConstantItem(127).PadraoInformacao(NFeResource.ESPECIE, _volume.Especie);
                        row.ConstantItem(110).PadraoInformacao(NFeResource.MARCA, _volume.Marca);
                        row.ConstantItem(93).PadraoInformacao(NFeResource.NUMERACAO, _volume.Numeracao);
                        
                        if (_volume.PesoBruto == 0)
                            row.RelativeItem().PadraoInformacao(NFeResource.PESO_BRUTO, string.Empty);
                        else
                            row.RelativeItem().PadraoInformacao(NFeResource.PESO_BRUTO, _volume.PesoBruto);
                        
                        
                        if (_volume.PesoLiquido == 0)
                            row.ConstantItem(69).PadraoInformacao(NFeResource.PESO_LIQUIDO, string.Empty);
                        else
                            row.ConstantItem(69).PadraoInformacao(NFeResource.PESO_LIQUIDO, _volume.PesoLiquido);
                    });
                }
            );
        }
    }
}
