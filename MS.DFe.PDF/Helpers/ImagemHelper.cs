using MS.DFe.PDF.Modelos;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.DFe.PDF.Helpers
{
    public static class ImagemHelper
    {
        public static ETipoImg ObterTipoImagem(byte[] imagemBytes)
        {
            if (imagemBytes == null || imagemBytes.Length.Equals(0)) return ETipoImg.SemImagem;

            var _imagem = SKBitmap.Decode(imagemBytes);

            return _imagem.Width >= _imagem.Height * 1.4 ? ETipoImg.Retangular : ETipoImg.QuadradoOuVertical;
        }

    }
}
