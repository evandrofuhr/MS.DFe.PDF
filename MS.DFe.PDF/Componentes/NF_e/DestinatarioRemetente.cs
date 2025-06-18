using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Elementos;
using MS.DFe.PDF.Resources;


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
                    column.Item().Padding(DadoPadraoExtensoes.PADDING).Text(NFeResource.DESTINATARIO_REMETENTE).SemiBold();

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(6).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.RAZAO_SOCIAL, _dest.xNome));
                        row.RelativeItem(3).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.CNPJ_CPF, (_dest.CNPJ ?? _dest.CPF).FormataCNPJCPF()));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Data(NFeResource.DATA_EMISSAO, _ide.dhEmi.DateTime));
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem(4).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.ENDERECO, _dest.enderDest?.ToEndereco()));
                        row.RelativeItem(3).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.BAIRRO, _dest.enderDest?.xBairro));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.CEP, _dest.enderDest?.CEP?.ToCep()));
                        row.RelativeItem(2).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Data(NFeResource.DATA_ENTRADA_SAIDA, _ide.dhSaiEnt?.DateTime));
                    });
                    column.Item().Row(col =>
                    {
                        col.RelativeItem(7).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Padrao(NFeResource.MUNICIPIO, _dest.enderDest?.xMun));
                        col.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.FONE_FAX, _dest.enderDest?.fone?.ToTelefone()));
                        col.RelativeItem(1).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.UF, _dest.enderDest?.UF));
                        col.RelativeItem(5).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Codigo(NFeResource.INSCRICAO_ESTADUAL, _dest.IE));
                        col.RelativeItem(4).Border(DadoPadraoExtensoes.BORDA).Padding(DadoPadraoExtensoes.PADDING).Component(CampoInformativo.Hora(NFeResource.HORA_ENTRADA_SAÍDA, _ide.dhSaiEnt));
                    });
                }
            );
        }
    }
}
