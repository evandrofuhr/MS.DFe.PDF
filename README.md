# MS.DFe.PDF

Biblioteca .NET para geração de PDF de documentos fiscais eletrônicos (DANFE / DACTE / DAMDFE e correlatos), com layout baseado em [QuestPDF](https://www.questpdf.com/).

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE.txt)
[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-512BD4.svg)](https://dotnet.microsoft.com/)
[![Release](https://img.shields.io/github/v/release/evandrofuhr/MS.DFe.PDF)](https://github.com/evandrofuhr/MS.DFe.PDF/releases)

## Documentos suportados

| Documento | Classe | Observações |
|-----------|--------|-------------|
| **NF-e** | `NFeLeiaute`, `NFeLeiauteRetrato`, `NFeLeiautePaisagem` | A4; logo do emitente e da software house |
| **NFC-e** | `NFCeLeiaute` | Bobina 72 mm, com QR Code |
| **CC-e** | `CCeLeiaute` | Carta de Correção (via modelo `CCeDados`) |
| **CT-e** | `CTeLeiaute` | Modal rodoviário; flags `Cancelado` / `Encerrado` |
| **MDF-e** | `MDFeLeiaute` | Flags `Cancelado` / `Encerrado` |

Marcas d'água automáticas para **pré-visualização** (sem protocolo) e **homologação**.

## Requisitos

- .NET Standard 2.0 (compatível com .NET Framework 4.6.1+, .NET Core / .NET 5+)
- XML autorizado (`nfeProc`, `cteProc`, `mdfeProc`) ou documento sem protocolo
- Dependências principais: QuestPDF, Hercules.NET (NFe/NFCe, CTe, MDFe), ZXing.Net, SkiaSharp

A licença Community do QuestPDF é configurada pela própria biblioteca.

## Instalação

Clone o repositório e referencie o projeto, ou gere o pacote NuGet localmente:

```bash
git clone https://github.com/evandrofuhr/MS.DFe.PDF.git
cd MS.DFe.PDF
dotnet build MS.DFe.PDF/MS.DFe.PDF.csproj -c Release
```

O pacote `MS.DFe.PDF.*.nupkg` é gerado em `MS.DFe.PDF/bin/Release/`.

```xml
<ProjectReference Include="caminho\MS.DFe.PDF\MS.DFe.PDF.csproj" />
```

## Uso rápido

### NF-e / NFC-e (atalho)

`LeiauteFactory` detecta o modelo pelo XML:

```csharp
using MS.DFe.PDF;

var xml = File.ReadAllText("nfe.xml");
var logo = File.ReadAllBytes("logo.png"); // opcional

byte[] pdf = LeiauteFactory.Gerar(xml, logo);
File.WriteAllBytes("danfe.pdf", pdf);
```

### NF-e (explícito)

```csharp
var pdf = new NFeLeiaute(xml, logo);
pdf.AdicionarLogoSoftwareHouse(logoSoftwareHouse); // opcional
File.WriteAllBytes("nfe.pdf", pdf.Gerar());

// Orientação alternativa
var retrato = new NFeLeiauteRetrato(xml, logo).Gerar();
var paisagem = new NFeLeiautePaisagem(xml, logo).Gerar();
```

### NFC-e

```csharp
var pdf = new NFCeLeiaute(xml).Gerar();
File.WriteAllBytes("nfce.pdf", pdf);
```

### CT-e / MDF-e

```csharp
var cte = new CTeLeiaute(xml, logo) { Cancelado = false };
File.WriteAllBytes("cte.pdf", cte.Gerar());

var mdfe = new MDFeLeiaute(xml, logo) { Encerrado = false };
File.WriteAllBytes("mdfe.pdf", mdfe.Gerar());
```

### CC-e

A CC-e usa um modelo tipado (não lê o XML do evento diretamente):

```csharp
using MS.DFe.PDF;
using MS.DFe.PDF.Modelos;

var nota = new CCeDadosNota(
    empresa: "Empresa Exemplo LTDA",
    cnpj: "00000000000191",
    modeloDocumento: "55 - NF-e",
    serie: 1,
    numero: 123,
    chaveNFe: "35200100000000000191550010000001231000001234");

var dados = new CCeDados(
    nota,
    protocolo: "135200000000001",
    ambiente: EAmbiente.Producao,
    dataHoraEvento: DateTimeOffset.Now,
    dataHoraRegistro: DateTimeOffset.Now,
    correcoes: new[] { "Correção do endereço do destinatário." });

var pdf = new CCeLeiaute(dados, logo).Gerar();
File.WriteAllBytes("cce.pdf", pdf);
```

## App de teste

O projeto `MS.DFe.PDF.AppTeste` gera PDF a partir de um XML e abre no [QuestPDF Companion](https://www.questpdf.com/companion/):

```bash
dotnet run --project MS.DFe.PDF.AppTeste
```

Escolha o tipo de documento, informe o caminho do XML e, quando aplicável, o logo.

## Estrutura

```
MS.DFe.PDF/                 # biblioteca
  Componentes/              # blocos de layout por documento
  Resources/                # textos e fontes embutidos
MS.DFe.PDF.AppTeste/        # console de demonstração
```

## Contribuição

Issues e pull requests são bem-vindos.

1. Faça um fork e crie uma branch a partir de `develop` (ou da branch padrão do momento)
2. Mantenha o estilo do código existente
3. Inclua XML de exemplo ou passos para validar o layout alterado
4. Abra o PR descrevendo o que mudou e por quê

## Licença

Distribuído sob [Apache License 2.0](LICENSE.txt).

---

Mantido por [MicroSales](https://github.com/evandrofuhr) · Issues em [github.com/evandrofuhr/MS.DFe.PDF](https://github.com/evandrofuhr/MS.DFe.PDF/issues)
