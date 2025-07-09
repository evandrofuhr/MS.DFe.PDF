using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Resources;
using NFe.Classes.Informacoes.Destinatario;
using NFe.Classes.Informacoes.Identificacao;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


namespace MS.DFe.PDF.Componentes.NF_e
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
                    column.Item().PadraoLabelGrupo(NFeResource.DESTINATARIO_REMETENTE);

                    column.Item().Row(
                        row =>
                        {
                            row.RelativeItem(6).PadraoInformacao(NFeResource.RAZAO_SOCIAL, _dest.xNome);
                            row.RelativeItem(3).PadraoInformacao(NFeResource.CNPJ_CPF, (_dest.CNPJ ?? _dest.CPF).FormataCNPJCPF(), true);
                            row.ConstantItem(75).PadraoInformacao(NFeResource.DATA_EMISSAO, _ide.dhEmi.DateTime);
                        }
                    );

                    column.Item().Row(
                        row =>
                        {
                            row.RelativeItem(4).PadraoInformacao(NFeResource.ENDERECO, _dest.enderDest?.ToEndereco());
                            row.RelativeItem(3).PadraoInformacao(NFeResource.BAIRRO, _dest.enderDest?.xBairro);
                            row.RelativeItem(1).PadraoInformacao(NFeResource.CEP, _dest.enderDest?.CEP?.ToCep(), true);
                            row.ConstantItem(75).PadraoInformacao(NFeResource.DATA_ENTRADA_SAIDA, _ide.dhSaiEnt?.DateTime);
                        }
                    );
                    column.Item().Row(
                        row =>
                        {
                            row.RelativeItem(7).PadraoInformacao(NFeResource.MUNICIPIO, _dest.enderDest?.xMun);
                            row.RelativeItem(5).PadraoInformacao(NFeResource.FONE_FAX, _dest.enderDest?.fone?.ToTelefone(), true);
                            row.RelativeItem(1).PadraoInformacao(NFeResource.UF, _dest.enderDest?.UF, true);
                            row.RelativeItem(5).PadraoInformacao(NFeResource.INSCRICAO_ESTADUAL, _dest.IE, true);
                            row.ConstantItem(75).PadraoInformacao(NFeResource.HORA_ENTRADA_SAÍDA, _ide.dhSaiEnt);
                        }
                    );
                }
            );
        }
    }
}
