using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Modelos;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CCe
{
    internal class Conteudo : IComponent
    {
        private readonly CCeDados _dados;

        public Conteudo(CCeDados dados)
        {
            _dados = dados;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Spacing(3);
                    column.Item().Text(CCeResource.TITULO_GRUPO_NOTA).Bold();
                    column.Item().Table(
                        table =>
                        {
                            table.ColumnsDefinition(
                                cs =>
                                {
                                    cs.RelativeColumn();
                                    cs.RelativeColumn();
                                    cs.RelativeColumn();
                                }
                            );

                            table.Cell().ColumnSpan(2).Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.EMPRESA).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Nota.Empresa);
                                }
                            );
                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.CNPJ).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Nota.CNPJFormatado);
                                }
                            );


                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.MODELO).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Nota.ModeloDocumento);
                                }
                            );
                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.SERIE).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Nota.SerieFormatada);
                                }
                            );
                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.NUMERO).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Nota.NumeroFormatado);
                                }
                            );

                            table.Cell().ColumnSpan(3).Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.CHAVE).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Nota.ChaveNFeFormatada);
                                }
                            );
                        }
                    );

                    column.Item().Text(CCeResource.TITULO_GRUPO_CARTA).Bold();
                    column.Item().Table(
                        table =>
                        {
                            table.ColumnsDefinition(
                                cs =>
                                {
                                    cs.RelativeColumn(1);
                                    cs.RelativeColumn(1);
                                }
                            );

                            // LINHA 1
                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.PROTOCOLO).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.Protocolo);
                                }
                            );
                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.AMBIENTE).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.AmbienteLiteral);
                                }
                            );

                            // LINHA 2
                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.DATA_HORA_EVENTO).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.DataHoraEventoFormatado);
                                }
                            );

                            table.Cell().Border(0.8f).Padding(3).Column(
                                tc =>
                                {
                                    tc.Item().Texto(CCeResource.DATA_HORA_REGISTRO).FontSize(4).Bold();
                                    tc.Item().Texto(_dados.DataHoraRegistroFormatado);
                                }
                            );
                        }
                    );

                    column.Item().Text(CCeResource.TITULO_GRUPO_CONDICOES).Bold();
                    column.Item().Border(0.8f).Padding(3).Text(CCeResource.CONDICOES_USO);

                    column.Item().Text(CCeResource.TITULO_GRUPO_CORRECAO).Bold();
                    column.Item().Extend().Border(0.8f).Padding(3).Table(
                        table =>
                        {
                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(0.6f).Text(CCeResource.CABECALHO_TABELA_CORRECAO);
                            });

                            table.ColumnsDefinition(
                                cd =>
                                {
                                    cd.RelativeColumn();
                                }
                            );
                            foreach (var _item in _dados.Correcoes)
                                table.Cell().Text(
                                    text =>
                                    {
                                        text.Span(" - ").Bold();
                                        text.Span(_item);
                                    }
                                );
                        }
                    );
                }
            );
        }
    }
}
