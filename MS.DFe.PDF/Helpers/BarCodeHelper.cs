using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace MS.DFe.PDF.Helpers
{
    public static class BarCodeHelper
    {
        public static byte[] QrCode(string value)
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
            var _pixelData = _writer.Write(value);

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
            return _byteArray;
        }

        public static byte[] Barcode128(string value)
        {
            int larguraMinimaPorCaracter = 20;
            int largura = Math.Max(100, value.Length * larguraMinimaPorCaracter);

            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = largura,
                    Height = 202,
                    Margin = 2,
                    PureBarcode = true
                }
            };

            var pixelData = writer.Write(value);

            var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);

            var bmpData = bitmap.LockBits(
                new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb);

            try
            {
                Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
