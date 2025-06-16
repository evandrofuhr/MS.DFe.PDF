using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao;
using NFe.Classes.Informacoes.Total;
using NFe.Classes.Informacoes.Emitente;
using MS.DFe.PDF.Extensoes;

namespace MS.DFe.PDF.Componentes.Nfe
{
    public class Canhoto : IComponent
    {
        private readonly ide _ide;
        private readonly emit _emit;
        private readonly ICMSTot _icmstot;
        private readonly dest _dest;

        public Canhoto(ide ide, emit emit, ICMSTot icmstot, dest dest)
        {
            _ide = ide;
            _emit = emit;
            _icmstot = icmstot;
            _dest = dest;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(coll =>
                        {
                            coll.Item().Height(26).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text($"Recebemos de {_emit.xNome} os produtos e/ou serviços constantes na Nota Fiscal Eletrônica indicada abaixo.\r\nEmissão: {_ide.dhEmi.ToString("dd/MM/yyyy")} Valor Total: {_icmstot.vNF.ToString()} Destinatário: {_dest.xNome}").FontSize(7);
                            coll.Item().Row(innerRow =>
                            {
                                innerRow.ConstantItem(244).Height(25).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text("DATA DE RECEBIMENTO");
                                innerRow.ConstantItem(244).Height(25).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Text("IDENTIFICAÇÃO E ASSINATURA DO RECEBEDOR").AlignLeft().AlignStart().FontSize(7);
                            }); ;
                        });
                        row.ConstantItem(90)
                        .Border(DadoPadraoExtensoes.BORDA)
                        .Padding(DadoPadraoExtensoes.PADDING)
                        .AlignMiddle()
                        .AlignCenter()
                        .Element(innerContainer => innerContainer.Column(innerCol =>
                        {
                            innerCol.Item().AlignCenter().Text("NF-e").Bold().FontSize(14);
                            innerCol.Item().AlignCenter().Text($"Nº {_ide.nNF.ToNumeroNfe()}").FontSize(12);
                            innerCol.Item().AlignCenter().Text($"Série: {_ide.serie.ToString()}").FontSize(12);
                        }));

                    });
                    col.Item().PaddingVertical(7).LineHorizontal(1);
                }
            );
        }
    }
}
