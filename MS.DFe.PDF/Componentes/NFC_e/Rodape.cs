using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Rodape : IComponent
    {
        private readonly decimal _vTotTrib;
        private readonly DFeDadosComprovante _comprovante;
        private readonly emit _emit;

        public Rodape(decimal vTotTrib, DFeDadosComprovante comprovante, emit emit)
        {
            _vTotTrib = vTotTrib;
            _comprovante = comprovante;
            _emit = emit;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    c.Item().BorderBottom(0.8f);
                    c.Item().AlignCenter().Texto(NFCeResource.TRIBUTOS, NFCeResource.CIFRAO, _vTotTrib.Formata());

                    if (_comprovante?.Textos?.Any() ?? false)
                    {
                        c.Item().BorderBottom(0.8f);
                        c.Item().Height(10).MinimalBox();
                        c.Item().Component(new Comprovante(_comprovante, _emit));
                    }
                }
            );
        }
    }
}
