using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Rodape : IComponent
    {
        private readonly decimal _vTotTrib;

        public Rodape(decimal vTotTrib)
        {
            _vTotTrib = vTotTrib;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    c.Item().BorderBottom(0.8f);
                    c.Item().AlignCenter().Texto(NFCeResource.TRIBUTOS, NFCeResource.CIFRAO, _vTotTrib.Formata());
                }
            );
        }
    }
}
