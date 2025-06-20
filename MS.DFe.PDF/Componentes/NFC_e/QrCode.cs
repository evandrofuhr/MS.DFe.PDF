using MS.DFe.PDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class QrCode : IComponent
    {
        private readonly string _qrCode;

        public QrCode(string qrCode)
        {
            _qrCode = qrCode;
        }

        public void Compose(IContainer container)
        {
            var _byteArray = BarCodeHelper.QrCode(_qrCode);
            container.AlignCenter().Width(130).Image(_byteArray);
        }
    }
}
