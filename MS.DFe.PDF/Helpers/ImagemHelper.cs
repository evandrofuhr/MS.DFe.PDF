﻿using MS.DFe.PDF.Modelos;
using SkiaSharp;

namespace MS.DFe.PDF.Helpers
{
    public static class ImagemHelper
    {
        public static ETipoImg TipoImagem(byte[] imagemBytes)
        {
            if (imagemBytes == null || imagemBytes.Length.Equals(0)) return ETipoImg.SemImagem;

            var _imagem = SKBitmap.Decode(imagemBytes);

            return _imagem.Width >= _imagem.Height * 1.4 ? ETipoImg.Retangular : ETipoImg.QuadradoOuVertical;
        }

    }
}
