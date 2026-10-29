using CTe.Classes.Informacoes;
using CTe.Classes.Informacoes.Tipos;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace MS.DFe.PDF.Componentes.CT_e
{
    internal class Participantes : IComponent
    {
        private readonly infCte _inf;

        public Participantes(infCte inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(c => BlocoParticipante(c, CTeResource.REMETENTE, DadosRem()));
                    row.RelativeItem().Element(c => BlocoParticipante(c, CTeResource.DESTINATARIO, DadosDest()));
                });
                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(c => BlocoParticipante(c, CTeResource.EXPEDIDOR, DadosExped()));
                    row.RelativeItem().Element(c => BlocoParticipante(c, CTeResource.RECEBEDOR, DadosReceb()));
                });
                column.Item().Element(c => BlocoParticipante(c, CTeResource.TOMADOR, DadosToma()));
            });
        }

        private void BlocoParticipante(IContainer container, string titulo, ParticipanteDados dados)
        {
            container.Border(ConstantsHelper.BORDA).Padding(ConstantsHelper.PADDING).Column(col =>
            {
                col.Item().Text(titulo).Bold().FontSize(6);
                col.Item().Text(dados.Nome ?? string.Empty).FontSize(7).Bold();
                col.Item().Text($"{CTeResource.ENDERECO}: {dados.Endereco}").FontSize(6);
                col.Item().Text($"{CTeResource.MUNICIPIO}: {dados.Municipio}  {CTeResource.CEP}: {dados.Cep}  {CTeResource.PAIS}: {dados.Pais}").FontSize(6);
                col.Item().Text($"{CTeResource.CNPJ_CPF}: {dados.Documento}  {CTeResource.IE}: {dados.Ie}  {CTeResource.FONE}: {dados.Fone}").FontSize(6);
            });
        }

        private ParticipanteDados DadosRem()
        {
            var rem = _inf.rem;
            if (rem == null) return ParticipanteDados.Vazio;
            var end = rem.enderReme;
            return new ParticipanteDados
            {
                Nome = rem.xNome,
                Endereco = FormatarEndereco(end?.xLgr, end?.nro, end?.xCpl, end?.xBairro),
                Municipio = end == null ? string.Empty : $"{end.xMun} - {end.ProxyUF ?? end.UF.ToString()}",
                Cep = end?.ProxyCEP ?? (end != null ? end.CEP.ToString() : string.Empty),
                Pais = end?.xPais ?? "BRASIL",
                Documento = (rem.CNPJ ?? rem.CPF ?? string.Empty).FormataCNPJCPF(),
                Ie = rem.IE,
                Fone = rem.fone?.ToTelefone()
            };
        }

        private ParticipanteDados DadosDest()
        {
            var dest = _inf.dest;
            if (dest == null) return ParticipanteDados.Vazio;
            var end = dest.enderDest;
            return new ParticipanteDados
            {
                Nome = dest.xNome,
                Endereco = FormatarEndereco(end?.xLgr, end?.nro, end?.xCpl, end?.xBairro),
                Municipio = end == null ? string.Empty : $"{end.xMun} - {end.ProxyUF ?? end.UF.ToString()}",
                Cep = end?.ProxyCEP ?? (end != null ? end.CEP.ToString() : string.Empty),
                Pais = end?.xPais ?? "BRASIL",
                Documento = (dest.CNPJ ?? dest.CPF ?? string.Empty).FormataCNPJCPF(),
                Ie = dest.IE,
                Fone = dest.fone?.ToTelefone()
            };
        }

        private ParticipanteDados DadosExped()
        {
            var exped = _inf.exped;
            if (exped == null) return ParticipanteDados.Vazio;
            var end = exped.enderExped;
            return new ParticipanteDados
            {
                Nome = exped.xNome,
                Endereco = FormatarEndereco(end?.xLgr, end?.nro, end?.xCpl, end?.xBairro),
                Municipio = end == null ? string.Empty : $"{end.xMun} - {end.ProxyUF ?? end.UF.ToString()}",
                Cep = end?.ProxyCEP ?? (end != null ? end.CEP.ToString() : string.Empty),
                Pais = end?.xPais ?? "BRASIL",
                Documento = (exped.CNPJ ?? exped.CPF ?? string.Empty).FormataCNPJCPF(),
                Ie = exped.IE,
                Fone = exped.fone?.ToTelefone()
            };
        }

        private ParticipanteDados DadosReceb()
        {
            var receb = _inf.receb;
            if (receb == null) return ParticipanteDados.Vazio;
            var end = receb.enderReceb;
            return new ParticipanteDados
            {
                Nome = receb.xNome,
                Endereco = FormatarEndereco(end?.xLgr, end?.nro, end?.xCpl, end?.xBairro),
                Municipio = end == null ? string.Empty : $"{end.xMun} - {end.ProxyUF ?? end.UF.ToString()}",
                Cep = end?.ProxyCEP ?? (end != null ? end.CEP.ToString() : string.Empty),
                Pais = end?.xPais ?? "BRASIL",
                Documento = (receb.CNPJ ?? receb.CPF ?? string.Empty).FormataCNPJCPF(),
                Ie = receb.IE,
                Fone = receb.fone?.ToTelefone()
            };
        }

        private ParticipanteDados DadosToma()
        {
            var toma = Identificacao.ObterToma(_inf.ide);
            switch (toma)
            {
                case toma.Remetente: return DadosRem();
                case toma.Expedidor: return DadosExped();
                case toma.Recebedor: return DadosReceb();
                case toma.Destinatario: return DadosDest();
                default:
                    var t4 = _inf.ide.toma4;
                    if (t4 == null) return ParticipanteDados.Vazio;
                    var end = t4.enderToma;
                    return new ParticipanteDados
                    {
                        Nome = t4.xNome,
                        Endereco = FormatarEndereco(end?.xLgr, end?.nro, end?.xCpl, end?.xBairro),
                        Municipio = end == null ? string.Empty : $"{end.xMun} - {end.ProxyUF ?? end.UF.ToString()}",
                        Cep = end?.ProxyCEP ?? (end?.CEP?.ToString() ?? string.Empty),
                        Pais = end?.xPais ?? "BRASIL",
                        Documento = (t4.CNPJ ?? t4.CPF ?? string.Empty).FormataCNPJCPF(),
                        Ie = t4.IE,
                        Fone = t4.fone?.ToTelefone()
                    };
            }
        }

        private static string FormatarEndereco(string xLgr, string nro, string xCpl, string xBairro)
        {
            var valor = $"{xLgr}, {nro}";
            if (!string.IsNullOrEmpty(xCpl)) valor += $" - {xCpl}";
            if (!string.IsNullOrEmpty(xBairro)) valor += $" - {xBairro}";
            return valor.Trim(' ', ',', '-');
        }

        private class ParticipanteDados
        {
            public static readonly ParticipanteDados Vazio = new ParticipanteDados();
            public string Nome { get; set; }
            public string Endereco { get; set; }
            public string Municipio { get; set; }
            public string Cep { get; set; }
            public string Pais { get; set; }
            public string Documento { get; set; }
            public string Ie { get; set; }
            public string Fone { get; set; }
        }
    }
}
