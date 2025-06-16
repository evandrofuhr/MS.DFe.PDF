using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using ZXing.Common;
using ZXing;

namespace MS.DFe.PDF.Helpers
{
    public static class CodigoDeBarrasHelper
    {
        public static byte[] GerarCodigoDeBarras(string codigo)
        {
            int larguraMinimaPorCaracter = 20;
            int largura = Math.Max(100, codigo.Length * larguraMinimaPorCaracter);

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

            var pixelData = writer.Write(codigo);

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
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
