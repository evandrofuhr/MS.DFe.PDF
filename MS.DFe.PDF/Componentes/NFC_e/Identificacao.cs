using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Protocolo;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Identificacao : IComponent
    {
        private readonly ide _ide;
        private readonly infProt _infProt;
        public Identificacao(ide ide, infProt infProt)
        {
            _ide = ide;
            _infProt = infProt;
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
                            _ide.nNF.FormataNumero(),
                            NFCeResource.SERIE,
                            _ide.serie.FormataSerie(),
                            _ide.dhEmi.FormataDataHora(),
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
                                text.TextoSpan(string.Empty, _infProt.nProt);
                            }
                        );

                    t.Cell()
                        .AlignCenter()
                        .Text(
                            text =>
                            {
                                text.TextoSpan(NFCeResource.DATA).Bold();
                                text.TextoSpan(string.Empty, _infProt.dhRecbto.FormataDataHora());
                            }
                        );
                }
            );
        }
    }
}
