using CTe.Classes.Protocolo;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using ZeusCTe = CTe.Classes.CTe;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Cabecalho : IComponent
    {
        private readonly ZeusCTe _cte;
        private readonly protCTe _protocolo;
        private readonly byte[] _logo;

        public Cabecalho(ZeusCTe cte, protCTe protocolo, byte[] logo)
        {
            _cte = cte;
            _protocolo = protocolo;
            _logo = logo;
        }

        public void Compose(IContainer container)
        {
            var emit = _cte.infCte.emit;
            var ide = _cte.infCte.ide;
            var chave = (_protocolo?.infProt?.chCTe ?? _cte.infCte.Id).NormalizarChaveCTe() ?? string.Empty;
            if (string.IsNullOrEmpty(chave))
                chave = string.Empty.PadLeft(44, '0');

            var qr = _cte.infCTeSupl?.qrCodCTe;
            var protocoloTexto = !string.IsNullOrEmpty(_protocolo?.infProt?.nProt)
                ? $"{_protocolo.infProt.nProt} - {_protocolo.infProt.dhRecbto:dd/MM/yyyy HH:mm:ss}"
                : CTeResource.SEM_PROTOCOLO;

            container.Border(ConstantsHelper.BORDA).Row(row =>
            {
                row.RelativeItem(4).BorderRight(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(col =>
                {
                    if (_logo != null)
                        col.Item().Height(40).AlignCenter().Image(_logo).FitArea();

                    col.Item().AlignCenter().Text(emit?.xNome ?? string.Empty).Bold().FontSize(9);
                    if (emit?.enderEmit != null)
                    {
                        var end = emit.enderEmit;
                        var linha = $"{end.xLgr}, {end.nro}";
                        if (!string.IsNullOrEmpty(end.xCpl)) linha += $" - {end.xCpl}";
                        linha += $" - {end.xBairro}";
                        col.Item().AlignCenter().Text(linha).FontSize(7);
                        col.Item().AlignCenter().Text($"{end.xMun} - {end.ProxyUF ?? end.UF.ToString()}  CEP: {end.ProxyCEP ?? end.CEP.ToString()}").FontSize(7);
                        col.Item().AlignCenter().Text($"CNPJ: {(emit.CNPJ ?? string.Empty).FormataCNPJCPF()}  IE: {emit.IE}").FontSize(7);
                        if (!string.IsNullOrEmpty(end.fone))
                            col.Item().AlignCenter().Text($"Fone: {end.fone.ToTelefone()}").FontSize(7);
                    }
                });

                row.RelativeItem(3).BorderRight(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(col =>
                {
                    col.Item().AlignCenter().Text(CTeResource.TITULO).Bold().FontSize(16);
                    col.Item().AlignCenter().Text(CTeResource.SUBTITULO).FontSize(6);
                    col.Item().PaddingTop(2).AlignCenter().Text($"{CTeResource.MODAL}: {CTeHelper.ModalNome(ide.modal)}").Bold().FontSize(8);
                    col.Item().AlignCenter().Text($"{CTeResource.MODELO} 57").FontSize(8);
                    col.Item().AlignCenter().Text($"{CTeResource.SERIE} {((int)ide.serie).FormataSerie()}").FontSize(8);
                    col.Item().AlignCenter().Text($"{CTeResource.NUMERO} {ide.nCT.ToNumeroNfe()}").Bold().FontSize(10);
                    col.Item().AlignCenter().Text(text =>
                    {
                        text.Span($"{CTeResource.FOLHA} ").FontSize(7);
                        text.CurrentPageNumber().FontSize(7);
                        text.Span("/").FontSize(7);
                        text.TotalPages().FontSize(7);
                    });
                    col.Item().AlignCenter().Text($"{CTeResource.DATA_HORA_EMISSAO}\n{ide.dhEmi:dd/MM/yyyy HH:mm:ss}").FontSize(7);
                });

                row.RelativeItem(5).Padding(ConstantsHelper.PADDING).Column(col =>
                {
                    col.Item().Height(45).AlignCenter().AlignMiddle().Image(BarCodeHelper.Barcode128(chave)).FitArea();
                    col.Item().PadraoInformacao(CTeResource.CHAVE_ACESSO, chave.FormataChaveNFe(), true);
                    col.Item().PaddingTop(2).AlignCenter().Text(CTeResource.CONSULTA).FontSize(6);
                    col.Item().AlignCenter().Text(CTeResource.URL_CTE).FontSize(6).FontColor("3366CC");
                    col.Item().PadraoInformacao(CTeResource.PROTOCOLO, protocoloTexto);

                    if (!string.IsNullOrEmpty(qr))
                    {
                        col.Item().PaddingTop(2).Height(55).AlignCenter()
                            .Image(BarCodeHelper.QrCode(qr)).FitArea();
                    }
                });
            });
        }
    }
}
