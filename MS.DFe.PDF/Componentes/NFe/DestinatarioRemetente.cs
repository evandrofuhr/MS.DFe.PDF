using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;


namespace MS.DFe.PDF.Componentes.Nfe
{
    public class DestinatarioRemetente : IComponent
    {
        private readonly dest _dest;
        private readonly ide _ide;

        public DestinatarioRemetente(dest dest, ide ide)
        {
            _dest = dest;
            _ide = ide;
        }

        public void Compose(IContainer container)
        {
            container.Column(
                column =>
                {
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text("DESTINATARIO / REMETENTE").SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(6).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("RAZÃO SOCIAL", _dest.xNome));
                        row.RelativeItem(3).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("CNPJ / CPF", (_dest.CNPJ ?? _dest.CPF).FormataCNPJCPF()));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoData("DATA DE EMISSÃO", _ide.dhEmi.DateTime));
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(4).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("ENDEREÇO", _dest.enderDest?.ToEndereco()));
                        row.RelativeItem(3).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("BAIRRO / DISTRITO", _dest.enderDest?.xBairro));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("CEP", _dest.enderDest?.CEP?.ToCep()));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoData("DATA ENTRADA / SAÍDA", _ide.dhSaiEnt?.DateTime));
                    });
                    column.Item().Row(col =>
                    {
                        col.RelativeItem(7).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativo("MUNICÍPIO", _dest.enderDest?.xMun));
                        col.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("FONE / FAX", _dest.enderDest?.fone?.ToTelefone()));
                        col.RelativeItem(1).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("UF", _dest.enderDest?.UF));
                        col.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoCodigo("INSCRIÇÃO ESTADUAL", _dest.IE));
                        col.RelativeItem(4).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(new CampoInformativoHora("HORA ENTRADA / SAÍDA", _ide.dhSaiEnt));
                    });
                }
            );
        }
    }
}
