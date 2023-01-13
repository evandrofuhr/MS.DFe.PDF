using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes
{
    internal class NFCeIdentificacao : IComponent
    {
        private readonly DFeDadosIdentificacao _dados;

        public NFCeIdentificacao(DFeDadosIdentificacao dados)
        {
            _dados = dados;
        }

        public void Compose(IContainer container)
        {
            container.Table(
                t =>
                {
                    t.ColumnsDefinition(d => d.RelativeColumn());
                    t.Cell()
                        .AlignCenter()
                        .Texto(
                            TextoResource.NFCE_NR,
                            _dados.Numero,
                            TextoResource.SERIE,
                            _dados.Serie,
                            _dados.Emissao,
                            "-",
                            TextoResource.VIA_CONSUMIDOR
                        )
                        .Bold();

                    t.Cell()
                        .AlignCenter()
                        .Text(
                            text =>
                            {
                                text.TextoSpan(TextoResource.PROTOCOLO).Bold();
                                text.TextoSpan(string.Empty, _dados.nProt);
                            }
                        );

                    t.Cell()
                        .AlignCenter()
                        .Text(
                            text =>
                            {
                                text.TextoSpan(TextoResource.DATA).Bold();
                                text.TextoSpan(string.Empty, _dados.Recebimento);
                            }
                        );
                }
            );
        }
    }
}
