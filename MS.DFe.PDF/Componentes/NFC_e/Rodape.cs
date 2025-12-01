using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Rodape : IComponent
    {
        private decimal _vTotTrib { get => _infNFe.total.ICMSTot.vTotTrib; }
        private emit _emit { get => _infNFe.emit; }
        private readonly infNFe _infNFe;

        private IEnumerable<string> _comprovantes
        {
            get => ObtemComprovante();
        }

        private IEnumerable<string> ObtemComprovante()
        {
            foreach (var _pag in _infNFe.pag)
            {

                if (_pag.detPag == null) continue;

                foreach (var _detPag in _pag.detPag)
                {

                    var _formaPagamento = _detPag.tPag.ObterDescricao();
                    var _cartao = _detPag.card?.tBand?.ObterDescricao();
                    if (!string.IsNullOrEmpty(_cartao))
                        yield return $"{_formaPagamento} : {_cartao}";
                    else
                        yield return _formaPagamento;

                    var _valor = _detPag.vPag.Formata();
                    yield return $"Valor: R$ {_valor}";

                    var _nsu = _detPag.card?.cAut;
                    if (!string.IsNullOrEmpty(_nsu))
                        yield return $"NSU: {_nsu}";
                }
            }
        }

        public Rodape(infNFe infNFe)
        {
            _infNFe = infNFe;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    c.Item().BorderBottom(0.8f);
                    c.Item().AlignCenter().Texto(NFCeResource.TRIBUTOS, NFCeResource.CIFRAO, _vTotTrib.Formata());

                    if (_comprovantes.Any())
                    {
                        c.Item().BorderBottom(0.8f);
                        c.Item().Height(10).ShrinkHorizontal();
                        c.Item().Component(new Comprovante(_comprovantes, _emit));
                    }
                }
            );
        }
    }
}
