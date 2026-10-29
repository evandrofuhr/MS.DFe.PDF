using MDFe.Classes.Informacoes;
using MDFe.Classes.Retorno.MDFeRetRecepcao;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class Cabecalho : IComponent
    {
        private readonly MDFe.Classes.Informacoes.MDFe _mdfe;
        private readonly MDFeProtMDFe _protocolo;
        private readonly byte[] _logo;

        public Cabecalho(MDFe.Classes.Informacoes.MDFe mdfe, MDFeProtMDFe protocolo, byte[] logo)
        {
            _mdfe = mdfe;
            _protocolo = protocolo;
            _logo = logo;
        }

        public void Compose(IContainer container)
        {
            var inf = _mdfe.InfMDFe;
            var emit = inf.Emit;
            var ide = inf.Ide;
            var rodo = inf.InfModal?.Modal as MDFeRodo;
            var rntrc = rodo?.infANTT?.RNTRC ?? rodo?.RNTRC ?? string.Empty;

            var chave = (_protocolo?.InfProt?.ChMDFe ?? inf.Id).NormalizarChaveMDFe() ?? string.Empty;
            if (string.IsNullOrEmpty(chave))
                chave = string.Empty.PadLeft(44, '0');

            var protocoloTexto = !string.IsNullOrEmpty(_protocolo?.InfProt?.NProt)
                ? $"{_protocolo.InfProt.NProt} - {_protocolo.InfProt.DhRecbto:dd/MM/yyyy HH:mm:ss}"
                : string.Empty;

            var qr = _mdfe.infMDFeSupl?.qrCodMDFe;

            container.Border(ConstantsHelper.BORDA).Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem(4).BorderRight(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(col =>
                    {
                        if (_logo != null)
                            col.Item().Height(40).AlignCenter().Image(_logo).FitArea();

                        col.Item().AlignCenter().Text(emit?.XNome ?? string.Empty).Bold().FontSize(9);
                        if (emit?.EnderEmit != null)
                        {
                            var end = emit.EnderEmit;
                            var linha = $"{end.XLgr}, {end.Nro}";
                            if (!string.IsNullOrEmpty(end.XCpl)) linha += $" - {end.XCpl}";
                            linha += $" - {end.XBairro}";
                            col.Item().AlignCenter().Text(linha).FontSize(7);
                            col.Item().AlignCenter().Text($"{end.XMun} - {end.ProxyUF ?? end.UF.ToString()}  CEP: {end.ProxyCEP ?? end.CEP.ToString()}").FontSize(7);
                        }
                        col.Item().AlignCenter().Text($"CNPJ: {(emit?.CNPJ ?? emit?.CPF ?? string.Empty).FormataCNPJCPF()}  IE: {emit?.IE}").FontSize(7);
                        col.Item().AlignCenter().Text($"RNTRC: {rntrc}").FontSize(7);
                    });

                    row.RelativeItem(3).BorderRight(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(col =>
                    {
                        col.Item().AlignCenter().Text($"{MDFeResource.TITULO} -").Bold().FontSize(14);
                        col.Item().AlignCenter().Text(MDFeResource.SUBTITULO).FontSize(6);
                        col.Item().PaddingTop(4).AlignCenter().Text(MDFeResource.MODAL_RODOVIARIO).Bold().FontSize(8);
                        col.Item().AlignCenter().Text($"{MDFeResource.MODELO} 58").FontSize(8);
                        col.Item().AlignCenter().Text($"{MDFeResource.SERIE} {((int)ide.Serie).FormataSerie()}").FontSize(8);
                        col.Item().AlignCenter().Text($"{MDFeResource.NUMERO} {ide.NMDF.ToNumeroNfe()}").Bold().FontSize(10);
                        col.Item().AlignCenter().Text(text =>
                        {
                            text.Span($"{MDFeResource.FOLHA} ").FontSize(7);
                            text.CurrentPageNumber().FontSize(7);
                            text.Span("/").FontSize(7);
                            text.TotalPages().FontSize(7);
                        });
                        col.Item().AlignCenter().Text($"{MDFeResource.DATA_HORA_EMISSAO}\n{ide.DhEmi:dd/MM/yyyy HH:mm:ss}").FontSize(7);
                        col.Item().AlignCenter().Text($"{MDFeResource.UF_CARGA}: {ide.ProxyUFIni ?? ide.UFIni.ToString()}   {MDFeResource.UF_DESCARGA}: {ide.ProxyUFFim ?? ide.UFFim.ToString()}").FontSize(7);
                    });

                    row.RelativeItem(5).Padding(ConstantsHelper.PADDING).Column(col =>
                    {
                        col.Item().AlignCenter().Text(MDFeResource.CONTROLE_FISCO).Bold().FontSize(7);
                        col.Item().Height(45).AlignCenter().AlignMiddle().Image(BarCodeHelper.Barcode128(chave)).FitArea();
                        col.Item().PadraoInformacao(MDFeResource.CHAVE_ACESSO, chave.FormataChaveNFe(), true);
                        col.Item().PadraoInformacao(MDFeResource.PROTOCOLO, protocoloTexto);
                        col.Item().AlignCenter().Text($"{MDFeResource.CONSULTE_EM} {MDFeResource.URL_CONSULTA}").FontSize(6).FontColor("3366CC");

                        if (!string.IsNullOrEmpty(qr))
                        {
                            col.Item().PaddingTop(2).Height(55).AlignCenter()
                                .Image(BarCodeHelper.QrCode(qr)).FitArea();
                        }
                    });
                });
            });
        }
    }
}
