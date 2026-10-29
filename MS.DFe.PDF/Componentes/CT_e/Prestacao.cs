using CTe.Classes.Informacoes.Identificacao;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Prestacao : IComponent
    {
        private readonly ide _ide;

        public Prestacao(ide ide)
        {
            _ide = ide;
        }

        public void Compose(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().PadraoInformacao(
                    CTeResource.ORIGEM_PRESTACAO,
                    $"{_ide.xMunIni} - {_ide.ProxyUFIni ?? _ide.UFIni.ToString()}");
                row.RelativeItem().PadraoInformacao(
                    CTeResource.DESTINO_PRESTACAO,
                    $"{_ide.xMunFim} - {_ide.ProxyUFFim ?? _ide.UFFim.ToString()}");
            });
        }
    }
}
