using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Identificacao : IComponent
    {
        private readonly DFeDadosIdentificacao _dados;

        public Identificacao(DFeDadosIdentificacao dados)
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
                            NFCeResource.NFCE_NR,
                            _dados.Numero,
                            NFCeResource.SERIE,
                            _dados.Serie,
                            _dados.Emissao,
                            "-",
                            NFCeResource.VIA_CONSUMIDOR
                        )
                        .Bold();

                    t.Cell()
                        .AlignCenter()
                        .Text(
                            text =>
                            {
                                text.TextoSpan(NFCeResource.PROTOCOLO).Bold();
                                text.TextoSpan(string.Empty, _dados.nProt);
                            }
                        );

                    t.Cell()
                        .AlignCenter()
                        .Text(
                            text =>
                            {
                                text.TextoSpan(NFCeResource.DATA).Bold();
                                text.TextoSpan(string.Empty, _dados.Recebimento);
                            }
                        );
                }
            );
        }
    }
}
