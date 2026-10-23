using MS.DFe.PDF.Componentes.NF_e.Retrato;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace MS.DFe.PDF.Componentes.NF_e.Paisagem
{
    /// <summary>
    /// Canhoto da NT 2026.010 no retrato, girado para a faixa direita da página paisagem.
    /// O texto passa a dizer que a nota está indicada ao lado.
    /// </summary>
    public class CanhotoPaisagem : IComponent
    {
        public const float Margem = 3f * 72f / 25.4f;
        public const float LarguraFaixa = 58f;
        public const float Largura = LarguraFaixa + Margem;

        private readonly ide _ide;
        private readonly emit _emit;
        private readonly ICMSTot _icmstot;
        private readonly dest _dest;

        public CanhotoPaisagem(ide ide, emit emit, ICMSTot icmstot, dest dest)
        {
            _ide = ide;
            _emit = emit;
            _icmstot = icmstot;
            _dest = dest;
        }

        public void Compose(IContainer container)
        {
            container.PaddingLeft(Margem).ExtendVertical().Row(row =>
            {
                row.ConstantItem(8).Element(ComposePicote);
                row.RelativeItem()
                    .RotateRight()
                    .Component(new CanhotoRetrato(_ide, _emit, _icmstot, _dest, indicadaAoLado: true, picote: false));
            });
        }

        private static void ComposePicote(IContainer container)
        {
            container.Svg(size =>
            {
                var w = size.Width.ToString(CultureInfo.InvariantCulture);
                var h = size.Height.ToString(CultureInfo.InvariantCulture);
                var x = (size.Width / 2f).ToString(CultureInfo.InvariantCulture);
                return $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{w}\" height=\"{h}\"><line x1=\"{x}\" y1=\"0\" x2=\"{x}\" y2=\"{h}\" stroke=\"#000\" stroke-width=\"0.7\" stroke-dasharray=\"4 2\" /></svg>";
            });
        }
    }
}
