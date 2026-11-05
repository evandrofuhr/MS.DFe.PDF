using MDFe.Classes.Informacoes;
using MS.DFe.PDF.Extensoes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace MS.DFe.PDF.Componentes.MDF_e
{
    internal class VeiculoCondutor : IComponent
    {
        private readonly MDFeInfMDFe _inf;

        public VeiculoCondutor(MDFeInfMDFe inf)
        {
            _inf = inf;
        }

        public void Compose(IContainer container)
        {
            var rodo = _inf.InfModal?.Modal as MDFeRodo;
            if (rodo == null)
                return;

            var rntrcAntt = rodo.infANTT?.RNTRC ?? rodo.RNTRC ?? string.Empty;
            var placas = new List<string>();
            var rntrcs = new List<string>();
            var cpfs = new List<string>();
            var nomes = new List<string>();

            var tracao = rodo.VeicTracao;
            if (tracao != null)
            {
                placas.Add(tracao.Placa ?? string.Empty);
                rntrcs.Add(string.IsNullOrEmpty(tracao.Prop?.RNTRC) ? rntrcAntt : tracao.Prop.RNTRC);

                if (tracao.Condutor != null)
                {
                    foreach (var condutor in tracao.Condutor)
                    {
                        cpfs.Add((condutor.CPF ?? string.Empty).FormataCNPJCPF());
                        nomes.Add(condutor.XNome ?? string.Empty);
                    }
                }
            }

            if (rodo.VeicReboque != null)
            {
                foreach (var reboque in rodo.VeicReboque)
                {
                    placas.Add(reboque.Placa ?? string.Empty);
                    rntrcs.Add(string.IsNullOrEmpty(reboque.Prop?.RNTRC) ? rntrcAntt : reboque.Prop.RNTRC);
                }
            }

            if (placas.Count == 0)
            {
                placas.Add(string.Empty);
                rntrcs.Add(string.Empty);
            }
            if (cpfs.Count == 0)
            {
                cpfs.Add(string.Empty);
                nomes.Add(string.Empty);
            }

            container.PaddingTop(10).Row(row =>
            {
                row.RelativeItem().PaddingRight(8).Column(col =>
                {
                    col.Item().Text("Veículo").Bold().FontSize(11);
                    col.Item().PaddingTop(3).Row(r =>
                    {
                        r.RelativeItem().Text("Placa").FontSize(8);
                        r.RelativeItem().Text("RNTRC").FontSize(8);
                    });
                    for (var i = 0; i < placas.Count; i++)
                    {
                        var indice = i;
                        col.Item().PaddingTop(1).Row(r =>
                        {
                            r.RelativeItem().Text(placas[indice]).Bold().FontSize(8);
                            r.RelativeItem().Text(rntrcs[indice]).Bold().FontSize(8);
                        });
                    }
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Condutor").Bold().FontSize(11);
                    col.Item().PaddingTop(3).Row(r =>
                    {
                        r.RelativeItem(1.1f).Text("CPF").FontSize(8);
                        r.RelativeItem(2.2f).Text("Nome").FontSize(8);
                    });
                    for (var i = 0; i < cpfs.Count; i++)
                    {
                        var indice = i;
                        col.Item().PaddingTop(1).Row(r =>
                        {
                            r.RelativeItem(1.1f).Text(cpfs[indice]).FontSize(8);
                            r.RelativeItem(2.2f).Text(nomes[indice]).FontSize(8);
                        });
                    }
                });
            });
        }
    }
}
