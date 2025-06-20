using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Destinatario;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.NFCe
{
    internal class Consumidor : IComponent
    {
        private readonly dest _dest;

        public Consumidor(dest dest)
        {
            _dest = dest;
        }
        public void Compose(IContainer container)
        {
            container.Table(
                t =>
                {
                    var _texto = "";

                    if (string.IsNullOrEmpty(_dest?.CNPJ) && string.IsNullOrEmpty(_dest?.CPF))
                    {
                        _texto = NFCeResource.CONSUMIDOR_NAO_IDENTIFICADO;
                    }
                    else
                    {
                        var tipo = !string.IsNullOrEmpty(_dest.CNPJ) ? NFCeResource.CNPJ : NFCeResource.CPF;
                        var valor = !string.IsNullOrEmpty(_dest.CNPJ) ? _dest.CNPJ : _dest.CPF;
                        _texto = $"{NFCeResource.CONSUMIDOR} - {tipo} {valor}";
                    }

                    t.ColumnsDefinition(c => c.RelativeColumn());
                    t.Cell().AlignCenter().Texto(_texto).Bold();
                }
            );
        }
    }
}
