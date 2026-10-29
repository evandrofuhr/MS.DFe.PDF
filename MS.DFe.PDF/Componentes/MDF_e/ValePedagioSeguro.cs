using MDFe.Classes.Flags;
using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using MS.DFe.PDF.Helpers;
using MS.DFe.PDF.Resources;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class ValePedagioSeguro : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public ValePedagioSeguro(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var rodo = _inf.InfModal?.Modal as MDFeRodo;
            var resp = new StringBuilder();
            var forn = new StringBuilder();
            var comp = new StringBuilder();

            void AppendDisp(System.Collections.Generic.IEnumerable<MDFeDisp> disps)
            {
                if (disps == null) return;
                foreach (var d in disps)
                {
                    var responsavel = !string.IsNullOrEmpty(d.CPFPg) ? d.CPFPg : d.CNPJPg;
                    resp.AppendLine((responsavel ?? string.Empty).FormataCNPJCPF());
                    forn.AppendLine((d.CNPJForn ?? string.Empty).FormataCNPJCPF());
                    comp.AppendLine(d.NCompra ?? string.Empty);
                }
            }

            AppendDisp(rodo?.ValePed?.Disp);
            if (rodo?.infANTT?.valePed?.Disp != null)
            {
                resp.Clear();
                forn.Clear();
                comp.Clear();
                AppendDisp(rodo.infANTT.valePed.Disp);
            }

            var contratantes = string.Empty;
            if (rodo?.infANTT?.infContratante != null)
            {
                var ultimo = rodo.infANTT.infContratante.LastOrDefault();
                if (ultimo != null)
                    contratantes = (!string.IsNullOrEmpty(ultimo.CNPJ) ? ultimo.CNPJ : ultimo.CPF ?? string.Empty).FormataCNPJCPF();
            }

            var segurosResp = new StringBuilder();
            var segurosSeg = new StringBuilder();
            var segurosApol = new StringBuilder();
            if (_inf.Seg != null)
            {
                foreach (var seg in _inf.Seg)
                {
                    var r = seg.InfResp?.RespSeg == MDFeRespSeg.EmitenteDoMDFe
                        ? MDFeResource.EMITENTE
                        : MDFeResource.CONTRATANTE;
                    segurosResp.AppendLine(r);
                    var xSeg = seg.InfSeg?.XSeg ?? string.Empty;
                    if (xSeg.Length > 30) xSeg = xSeg.Substring(0, 30);
                    segurosSeg.AppendLine(xSeg);
                    var apol = seg.NApol ?? string.Empty;
                    if (apol.Length > 20) apol = apol.Substring(0, 20);
                    segurosApol.AppendLine(apol);
                }
            }

            container.Column(column =>
            {
                column.Item().Border(ConstantsHelper.BORDA).Column(c =>
                {
                    c.Item().Padding(ConstantsHelper.PADDING).Text(MDFeResource.VALE_PEDAGIO).Bold().FontSize(6);
                    c.Item().Row(row =>
                    {
                        row.RelativeItem().PadraoInformacao(MDFeResource.RESP_CPF_CNPJ, resp.ToString().Trim());
                        row.RelativeItem().PadraoInformacao(MDFeResource.FORNECEDOR_CNPJ, forn.ToString().Trim());
                        row.RelativeItem().PadraoInformacao(MDFeResource.N_COMPROVANTE, comp.ToString().Trim());
                    });
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().PadraoInformacao(MDFeResource.CONTRATANTES, contratantes);
                });

                column.Item().Border(ConstantsHelper.BORDA).Column(c =>
                {
                    c.Item().Padding(ConstantsHelper.PADDING).Text(MDFeResource.SEGURO).Bold().FontSize(6);
                    c.Item().Row(row =>
                    {
                        row.RelativeItem().PadraoInformacao(MDFeResource.RESPONSAVEL, segurosResp.ToString().Trim());
                        row.RelativeItem().PadraoInformacao(MDFeResource.SEGURADORA, segurosSeg.ToString().Trim());
                        row.RelativeItem().PadraoInformacao(MDFeResource.APOLICE, segurosApol.ToString().Trim());
                    });
                });
            });
        }
    }
}
