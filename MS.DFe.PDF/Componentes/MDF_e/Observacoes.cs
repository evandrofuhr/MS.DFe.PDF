using MDFe.Classes.Informacoes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class Observacoes : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public Observacoes(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var adic = _inf.InfAdic;
            container.PaddingTop(10).Column(column =>
            {
                column.Item().Text("Observações").Bold().FontSize(11);
                column.Item().PaddingTop(2).Text(adic?.InfCpl ?? string.Empty).FontSize(8);

                if (!string.IsNullOrWhiteSpace(adic?.InfAdFisco))
                {
                    column.Item().PaddingTop(6).Text("Informações de interesse do fisco").Bold().FontSize(9);
                    column.Item().PaddingTop(1).Text(adic.InfAdFisco).FontSize(8);
                }
            });
        }
    }
}
