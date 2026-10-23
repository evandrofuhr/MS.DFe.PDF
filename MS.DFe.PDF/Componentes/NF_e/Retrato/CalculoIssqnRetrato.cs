using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Emitente;
using NFe.Classes.Informacoes.Total;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NF_e.Retrato
{
    public class CalculoIssqnRetrato : IComponent
    {
        private readonly emit _emit;
        private readonly ISSQNtot _issqn;

        public CalculoIssqnRetrato(emit emit, ISSQNtot issqn)
        {
            _emit = emit;
            _issqn = issqn;
        }

        /// <summary>
        /// NT 2026.010: o quadro só entra quando o grupo ISSQNtot se aplica.
        /// Inscrição municipal sozinha não autoriza o bloco.
        /// </summary>
        public static bool DeveExibir(ISSQNtot issqn)
        {
            return issqn != null
                && (issqn.vServ.HasValue || issqn.vBC.HasValue || issqn.vISS.HasValue);
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().PadraoLabelGrupo(NFeResource.CALCULO_ISSQN);

                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(NFeResource.INSCRICAO_MUNICIPAL, _emit?.IM ?? string.Empty, true);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_TOTAL_SERVICOS, _issqn?.vServ);
                    row.RelativeItem().PadraoInformacao(NFeResource.BASE_CALCULO_ISSQN, _issqn?.vBC);
                    row.RelativeItem().PadraoInformacao(NFeResource.VALOR_ISSQN, _issqn?.vISS);
                });
            });
        }
    }
}
