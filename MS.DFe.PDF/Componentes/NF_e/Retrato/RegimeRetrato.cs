using MS.DFe.PDF.Extensoes;
using NFe.Classes.Informacoes.Emitente;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    /// <summary>
    /// Quadro do emitente da NT 2026.010, item 4.2: imprime o CRT (C21).
    /// </summary>
    /// <remarks>
    /// O tipo de regime de apuração do IBS e da CBS não tem tag publicada.
    /// A NT manda omitir o campo até essa definição. A célula já está prevista
    /// em <see cref="ComporTipoRegimeIbsCbs"/> e só entra no papel quando
    /// <see cref="ImprimirTipoRegimeIbsCbs"/> for ligado, com o valor vindo do XML.
    /// </remarks>
    public class RegimeRetrato : IComponent
    {
        /// <summary>
        /// NT 2026.010, itens 4.2 e 4.4: sem origem publicada, o campo não é impresso.
        /// </summary>
        private static readonly bool ImprimirTipoRegimeIbsCbs = false;

        private readonly CRT _crt;

        public RegimeRetrato(CRT crt)
        {
            _crt = crt;
        }

        public void Compose(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().PadraoInformacao("CÓDIGO DO REGIME TRIBUTÁRIO", Texto(_crt), true);
                if (ImprimirTipoRegimeIbsCbs)
                    row.RelativeItem(2).Element(ComporTipoRegimeIbsCbs);
            });
        }

        private static void ComporTipoRegimeIbsCbs(IContainer container)
        {
            container.PadraoInformacao("TIPO DE REGIME DE APURAÇÃO DO IBS E DA CBS", string.Empty, true);
        }

        private static string Texto(CRT crt)
        {
            switch (crt)
            {
                case CRT.SimplesNacional:
                    return "1 - Simples Nacional";
                case CRT.SimplesNacionalExcessoSublimite:
                    return "2 - Simples Nacional, excesso de sublimite";
                case CRT.RegimeNormal:
                    return "3 - Regime Normal";
                case CRT.SimplesNacionalMei:
                    return "4 - Simples Nacional - MEI";
                default:
                    return ((int)crt).ToString();
            }
        }
    }
}
