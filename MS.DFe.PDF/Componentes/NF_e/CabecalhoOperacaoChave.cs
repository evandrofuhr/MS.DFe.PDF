using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e
{
    public class CabecalhoOperacaoChave : IComponent
    {
        private readonly infNFe _infnfe;
        private readonly protNFe _protocolo;

        public CabecalhoOperacaoChave(infNFe infnfe, protNFe protocolo)
        {
            _infnfe = infnfe;
            _protocolo = protocolo;
        }


        public void Compose(IContainer container)
        {
            var _chaveDeAcesso = _protocolo?.infProt.chNFe.ToString() ?? _infnfe.Id?.SomenteNumeros() ?? string.Empty.PadLeft(44, '0');

            container.Column(
                column =>
                {
                    column.Item().Height(60).Border(ConstantsHelper.BORDA).AlignCenter().Padding(3).Image(BarCodeHelper.Barcode128(_chaveDeAcesso));
                    column.Item().PadraoInformacao(NFeResource.CHAVE_ACESSO, _chaveDeAcesso.FormataChaveNFe(), true);
                    column.Item()
                        .BorderRight(ConstantsHelper.BORDA)
                        .Padding(5)
                        .AlignCenter()
                        .Text(
                            text =>
                            {
                                text.DefaultTextStyle(d => d.Bold().FontSize(8f));
                                text.Line(NFeResource.CONSULTA1);
                                text.Hyperlink(NFeResource.URL_NFE_TEXT, NFeResource.URL_NFE).FontColor("3366CC");
                                text.Span(NFeResource.CONSULTA2);
                            }
                        );
                }
            );
        }
    }
}





