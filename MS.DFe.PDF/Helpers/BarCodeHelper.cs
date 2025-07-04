using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.SkiaSharp.Rendering;

namespace MS.DFe.PDF.Helpers
{
    public static class BarCodeHelper
    {
        public static byte[] QrCode(string value)
        {
            var writer = new BarcodeWriter<SKBitmap>
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 130,
                    Height = 130,
                    Margin = 0
                },
                Renderer = new SKBitmapRenderer()
            };

            using (var bitmap = writer.Write(value))
            using (var imagem = SKImage.FromBitmap(bitmap))
            using (var dados = imagem.Encode(SKEncodedImageFormat.Png, 100))
            {
                return dados.ToArray();
            }
        }

        public static byte[] Barcode128(string value)
        {

            int larguraMinimaPorCaracter = 20;
            int largura = Math.Max(100, value.Length * larguraMinimaPorCaracter);

            var writer = new ZXing.SkiaSharp.BarcodeWriter
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

            using (var image = writer.Write(value))
            {
                using (var stream = new MemoryStream())
                {
                    image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(stream);
                    return stream.ToArray();
                }
            }




            //int larguraMinimaPorCaracter = 20;
            //int largura = Math.Max(100, value.Length * larguraMinimaPorCaracter);

            //var writer = new BarcodeWriterPixelData
            //{
            //    Format = BarcodeFormat.CODE_128,
            //    Options = new EncodingOptions
            //    {
            //        Width = largura,
            //        Height = 202,
            //        Margin = 2,
            //        PureBarcode = true
            //    }
            //};

            //var pixelData = writer.Write(value);

            //var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);

            //var bmpData = bitmap.LockBits(
            //    new Rectangle(0, 0, pixelData.Width, pixelData.Height),
            //    ImageLockMode.WriteOnly,
            //    PixelFormat.Format32bppRgb);

            //try
            //{
            //    Marshal.Copy(pixelData.Pixels, 0, bmpData.Scan0, pixelData.Pixels.Length);
            //}
            //finally
            //{
            //    bitmap.UnlockBits(bmpData);
            //}

            //var ms = new MemoryStream();
            //bitmap.Save(ms, ImageFormat.Png);
            //return ms.ToArray();
        }
    }
}
