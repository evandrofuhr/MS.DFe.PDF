using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes
{
    internal class NFCeRodape : IComponent
    {
        private readonly decimal _vTotTrib;

        public NFCeRodape(decimal vTotTrib)
        {
            _vTotTrib = vTotTrib;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                c =>
                {
                    c.Item().BorderBottom(0.8f);
                    c.Item().AlignCenter().Texto(TextoResource.TRIBUTOS, TextoResource.CIFRAO, _vTotTrib.Formata());
                }
            );
        }
    }
}
