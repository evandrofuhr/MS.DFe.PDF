using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.QrCode;

namespace MS.DFe.PDF.Componentes
{
    internal class NFCeQrCode : IComponent
    {
        private readonly string _qrCode;

        public NFCeQrCode(string qrCode)
        {
            _qrCode = qrCode;
        }

        public void Compose(IContainer container)
        {
            var _writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 130,
                    Width = 130,
                    Margin = 0
                }
            };
            var _pixelData = _writer.Write(_qrCode);

            byte[] _byteArray = null;

            using (var _bm = new Bitmap(_pixelData.Width, _pixelData.Height, PixelFormat.Format32bppRgb))
            {
                using (var _ms = new MemoryStream())
                {
                    var _bmData = _bm.LockBits(
                        new Rectangle(
                            0,
                            0,
                            _pixelData.Width,
                            _pixelData.Height
                        ),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format32bppRgb
                    );
                    try
                    {
                        Marshal.Copy(_pixelData.Pixels, 0, _bmData.Scan0, _pixelData.Pixels.Length);
                    }
                    finally
                    {
                        _bm.UnlockBits(_bmData);
                    }
                    _bm.Save(_ms, ImageFormat.Png);
                    _byteArray = _ms.ToArray();
                }
            }

            container.AlignCenter().Width(130).Image(_byteArray, ImageScaling.FitWidth);
        }
    }
}
