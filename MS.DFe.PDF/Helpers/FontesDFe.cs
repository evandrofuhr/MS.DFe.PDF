using QuestPDF.Drawing;
using System;
using System.IO;
using System.Reflection;

namespace MS.DFe.PDF.Helpers
{
    /// <summary>
    /// Registra Liberation Serif sob o nome "Times New Roman" para o QuestPDF.
    /// No Windows a fonte do SO costuma existir; em Linux/Docker (Hostinger) não —
    /// sem registro o layout do DANFE/DACTE/DAMDFE estoura (DocumentLayoutException).
    /// Liberation Serif é métrica-compatível e licenciada OFL.
    /// </summary>
    internal static class FontesDFe
    {
        public const string TimesNewRoman = "Times New Roman";

        private static readonly object Gate = new object();
        private static bool _registrado;

        private static readonly string[] Recursos =
        {
            "MS.DFe.PDF.Resources.Fonts.LiberationSerif-Regular.ttf",
            "MS.DFe.PDF.Resources.Fonts.LiberationSerif-Bold.ttf",
            "MS.DFe.PDF.Resources.Fonts.LiberationSerif-Italic.ttf",
            "MS.DFe.PDF.Resources.Fonts.LiberationSerif-BoldItalic.ttf"
        };

        public static void GarantirTimesNewRoman()
        {
            if (_registrado)
                return;

            lock (Gate)
            {
                if (_registrado)
                    return;

                var assembly = typeof(FontesDFe).GetTypeInfo().Assembly;
                foreach (var recurso in Recursos)
                {
                    using (var stream = assembly.GetManifestResourceStream(recurso))
                    {
                        if (stream == null)
                            throw new InvalidOperationException(
                                "Fonte embutida não encontrada: " + recurso +
                                ". Verifique EmbeddedResource no MS.DFe.PDF.csproj.");

                        using (var copia = new MemoryStream())
                        {
                            stream.CopyTo(copia);
                            copia.Position = 0;
                            FontManager.RegisterFontWithCustomName(TimesNewRoman, copia);
                        }
                    }
                }

                _registrado = true;
            }
        }
    }
}
