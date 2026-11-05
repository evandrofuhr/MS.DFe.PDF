using DFe.Classes.Flags;
using MDFe.Classes.Flags;
using MDFe.Classes.Informacoes;
using MDFe.Classes.Retorno.MDFeRetRecepcao;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class Cabecalho : IComponent
    {
        private const string Fundo = "BCE0F6";
        private const string Borda = "90A4AE";

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
            var tot = inf.Tot;

            var chave = (_protocolo?.InfProt?.ChMDFe ?? inf.Id).NormalizarChaveMDFe() ?? string.Empty;
            if (string.IsNullOrEmpty(chave))
                chave = string.Empty.PadLeft(44, '0');

            var protocoloTexto = !string.IsNullOrEmpty(_protocolo?.InfProt?.NProt)
                ? $"{_protocolo.InfProt.NProt} - {_protocolo.InfProt.DhRecbto:dd/MM/yyyy} às {_protocolo.InfProt.DhRecbto:HH:mm:ss}"
                : string.Empty;

            var tpAmb = ide.TpAmb == TipoAmbiente.Homologacao ? "2" : "1";
            var qr = _mdfe.infMDFeSupl?.qrCodMDFe;
            if (string.IsNullOrWhiteSpace(qr))
                qr = $"https://dfe-portal.svrs.rs.gov.br/mdfe/qrCode?chMDFe={chave}&tpAmb={tpAmb}";

            var pesoRotulo = tot?.CUnid == MDFeCUnid.TON ? "Peso total (Ton)" : "Peso total (Kg)";
            var peso = (tot?.QCarga ?? 0m).ToString("#,##0.00", new CultureInfo("pt-BR"));

            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    if (_logo != null)
                        row.ConstantItem(72).Height(58).AlignMiddle().Image(_logo).FitArea();

                    row.RelativeItem().PaddingLeft(_logo != null ? 6 : 0).PaddingRight(6).Column(col =>
                    {
                        col.Item().Text(emit?.XNome ?? string.Empty).Bold().FontSize(11);
                        if (emit?.EnderEmit != null)
                        {
                            var end = emit.EnderEmit;
                            col.Item().Text(LinhaLogradouro(end)).FontSize(8);
                            col.Item().Text(LinhaMunicipio(end)).FontSize(8);
                        }
                        col.Item().Text($"CNPJ: {(emit?.CNPJ ?? emit?.CPF ?? string.Empty).FormataCNPJCPF()}   IE: {emit?.IE}   RNTRC: {rntrc}").FontSize(8);
                    });

                    row.ConstantItem(86).Height(86).AlignCenter().AlignMiddle().Image(BarCodeHelper.QrCode(qr)).FitArea();
                });

                column.Item().PaddingTop(6).Text("DAMDFE - Documento Auxiliar de Manifesto Eletrônico de Documentos Fiscais").Bold().FontSize(9);

                column.Item().PaddingTop(4).Row(row =>
                {
                    row.RelativeItem(0.8f).Element(e => CampoAzul(e, "Modelo", "58"));
                    row.RelativeItem(0.7f).Element(e => CampoAzul(e, "Série", ((int)ide.Serie).ToString(CultureInfo.InvariantCulture)));
                    row.RelativeItem(1.1f).Element(e => CampoAzul(e, "Número", ide.NMDF.ToString(CultureInfo.InvariantCulture)));
                    row.RelativeItem(0.7f).Element(CampoFolha);
                    row.RelativeItem(2.2f).Element(e => CampoAzul(e, "Data e hora de Emissão", ide.DhEmi.ToString("dd/MM/yyyy HH:mm:ss")));
                    row.RelativeItem(0.9f).Element(e => CampoAzul(e, "UF Carreg.", ide.ProxyUFIni ?? ide.UFIni.ToString()));
                    row.RelativeItem(1f).Element(e => CampoAzul(e, "UF Descarreg.", ide.ProxyUFFim ?? ide.UFFim.ToString()));
                });

                column.Item().PaddingTop(8).Row(row =>
                {
                    row.RelativeItem().AlignBottom().Text(NomeModal(ide.Modal)).Bold().FontSize(12);
                    row.RelativeItem().AlignRight().AlignBottom().Text(MDFeResource.CONTROLE_FISCO).FontSize(8);
                });

                column.Item().PaddingTop(2).Row(row =>
                {
                    row.RelativeItem(4.2f).AlignMiddle().Row(qtds =>
                    {
                        qtds.RelativeItem().Element(e => CampoAzul(e, "Qtd. CT-e", (tot?.QCTe ?? 0).ToString()));
                        qtds.RelativeItem().Element(e => CampoAzul(e, "Qtd. NF-e", (tot?.QNFe ?? 0).ToString()));
                        if ((tot?.QMDFe ?? 0) > 0)
                            qtds.RelativeItem().Element(e => CampoAzul(e, "Qtd. MDF-e", tot.QMDFe.ToString()));
                        qtds.RelativeItem(1.4f).Element(e => CampoAzul(e, pesoRotulo, peso));
                    });

                    row.RelativeItem(5.8f).Height(52).AlignCenter().AlignMiddle().Image(BarCodeHelper.Barcode128(chave)).FitArea();
                });

                column.Item().PaddingTop(4).Row(row =>
                {
                    row.RelativeItem(4.2f).PaddingRight(6).Column(col =>
                    {
                        col.Item().Text("Protocolo de autorização").Bold().FontSize(8);
                        col.Item().Text(protocoloTexto).FontSize(8);
                    });

                    row.RelativeItem(5.8f).Column(col =>
                    {
                        col.Item().Text("Chave de Acesso").Bold().FontSize(8);
                        col.Item().Text(chave.FormataChaveNFe()).FontSize(8);
                        col.Item().PaddingTop(1).Text(text =>
                        {
                            text.Span("Consulte em ").Bold().FontSize(7.5f);
                            text.Span(MDFeResource.URL_CONSULTA).Bold().FontSize(7.5f);
                        });
                    });
                });
            });
        }

        private static void CampoAzul(IContainer destino, string rotulo, string valor)
        {
            destino.PaddingHorizontal(1.5f).Column(col =>
            {
                col.Item().AlignCenter().Text(rotulo).FontSize(7);
                col.Item()
                    .Background(Fundo)
                    .Border(0.4f)
                    .BorderColor(Borda)
                    .MinHeight(18)
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(valor ?? string.Empty)
                    .Bold()
                    .FontSize(8);
            });
        }

        private static void CampoFolha(IContainer destino)
        {
            destino.PaddingHorizontal(1.5f).Column(col =>
            {
                col.Item().AlignCenter().Text("FL").FontSize(7);
                col.Item()
                    .Background(Fundo)
                    .Border(0.4f)
                    .BorderColor(Borda)
                    .MinHeight(18)
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(text =>
                    {
                        text.DefaultTextStyle(style => style.Bold().FontSize(8));
                        text.CurrentPageNumber();
                        text.Span("/");
                        text.TotalPages();
                    });
            });
        }

        private static string NomeModal(MDFeModal modal)
        {
            switch (modal)
            {
                case MDFeModal.Aereo:
                    return "Modelo Aéreo de Carga";
                case MDFeModal.Aquaviario:
                    return "Modelo Aquaviário de Carga";
                case MDFeModal.Ferroviari:
                    return "Modelo Ferroviário de Carga";
                default:
                    return "Modelo Rodoviário de Carga";
            }
        }

        private static string LinhaLogradouro(MDFeEnderEmit end)
        {
            var linha = end.XLgr ?? string.Empty;
            if (!string.IsNullOrEmpty(end.Nro))
                linha = string.IsNullOrEmpty(linha) ? $"nº {end.Nro}" : $"{linha}, nº {end.Nro}";
            if (!string.IsNullOrEmpty(end.XCpl))
                linha += $", {end.XCpl}";
            return linha;
        }

        private static string LinhaMunicipio(MDFeEnderEmit end)
        {
            var uf = end.ProxyUF ?? end.UF.ToString();
            var cep = FormataCep(end.ProxyCEP, end.CEP);
            var linha = $"{end.XMun} - {uf}";
            if (!string.IsNullOrEmpty(cep))
                linha += $" - CEP {cep}";
            return linha;
        }

        private static string FormataCep(string proxy, long cep)
        {
            var digits = string.IsNullOrWhiteSpace(proxy) ? cep.ToString(CultureInfo.InvariantCulture) : proxy;
            var numeros = string.Empty;
            foreach (var caractere in digits)
            {
                if (caractere >= '0' && caractere <= '9')
                    numeros += caractere;
            }
            if (numeros.Length == 0 || numeros == "0")
                return string.Empty;
            numeros = numeros.PadLeft(8, '0');
            if (numeros.Length > 8)
                numeros = numeros.Substring(numeros.Length - 8);
            return $"{numeros.Substring(0, 5)}-{numeros.Substring(5)}";
        }
    }
}
