// See https://aka.ms/new-console-template for more information

using DFe.Classes.Flags;
using DFe.Utils;
using MS.DFe.PDF;
using MS.DFe.PDF.Modelos;
using NFe.Classes;
using QuestPDF.Infrastructure;

using QuestPDF.Companion;




// abaixo exemplo de chamada para impressão da CC-e
/*
var _output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
var _cce = new CCeLeiaute(
    new CCeDados(
        new CCeDadosNota(
            "NOME DA EMPRESA TESTE LTDA",
            "00000000000000",
            "55 - NF-e",
            1,
            1,
            "12341234123412341234123412341234123412341234"
        ),
        "12323021392399",
        EAmbiente.HOMOLOGACAO,
        DateTimeOffset.Now,
        DateTimeOffset.Now,
        new string[]
        {
            "Produto 1 Alterado de X para Y. Produto 1 Alterado de X para Y. Produto 1 Alterado de X para Y. Produto 1 Alterado de X para Y. Produto 1 Alterado de X para Y.",
            "Produto 1 Alterado de X para Z",
            "Produto 1 Alterado de X para Z",
            "Produto 1 Alterado de X para Z",
            "Produto 1 Alterado de X para Z",
            "Produto 1 Alterado de X para Z",
            "Produto 1 Alterado de X para Z"
        }
    ),
    null // logo
);

var _pdf = _cce.Gerar();

var _file = Path.Combine(_output, $"cce-{DateTime.Now.ToString("yyyyMMdd")}.pdf");

File.WriteAllBytes(_file, _pdf);
*/
var _escolha = 1;
while (true)
{
    Console.WriteLine("Qual tipo de impressão NF-e(1), NFC-e(2), Sair(0): ");
    var _tela = Console.ReadLine();
    if (int.TryParse(_tela, out int _int))
    {
        if (_int <= 0) Environment.Exit(0);
        if (_int <= 2)
        {
            _escolha = _int;
            break;
        }
    }

    Console.Clear();
    Console.WriteLine("Valor invalido");

}

string? _xmlPath = null;
while (string.IsNullOrEmpty(_xmlPath))
{
    Console.WriteLine($"Informe o caminho para o xml processado da {(_escolha == 1 ? "NF-e" : "NFC-e")}:");
    _xmlPath = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(_xmlPath))
        _xmlPath = "c:/temp/nfc.xml";

    if (!File.Exists(_xmlPath))
    {
        Console.Clear();
        Console.WriteLine("Arquivo não encontrado.");
        _xmlPath = null;
    }
}

byte[]? _logoBytes = null;

if (_escolha == 1)
{
    string? _logoPath = null;
    while (_logoPath == null)
    {
        Console.WriteLine("Informe o caminho para o logo (0 para sair):");
        _logoPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(_logoPath))
            _logoPath = "c:/temp/bruh.png";

        if (string.IsNullOrEmpty(_logoPath))
        {
            Console.Clear();
            Console.WriteLine("Caminho inválido.");
            continue;
        }
        if (_logoPath.Trim().Equals("0"))
        {
            _logoPath = null;
            break;
        }
        if (!File.Exists(_logoPath))
        {
            Console.Clear();
            Console.WriteLine("Arquivo não encontrado.");
            _logoPath = null;
        }
    }

    if (!string.IsNullOrEmpty(_logoPath))
        _logoBytes = File.ReadAllBytes(_logoPath);
}


if (_escolha == 1)
{
    var _xml = File.ReadAllText(_xmlPath);
    var pdf = new NFeLeiaute(_xml, _logoBytes);

    //pdf.AdicionarLogoSoftwareHouse(_logoBytes);


    pdf.ShowInCompanion();
}

if (_escolha == 2)
{
    NFe.Classes.NFe? _nfe = null;
    nfeProc? _nfeProc = null;

    try
    {
        _nfeProc = FuncoesXml.ArquivoXmlParaClasse<nfeProc>(_xmlPath);
        _nfe = _nfeProc.NFe;
    }
    catch
    {
        _nfe = FuncoesXml.ArquivoXmlParaClasse<NFe.Classes.NFe>(_xmlPath);
    }

    if (_nfe == null)
    {
        Console.WriteLine("Não foi possível converter o arquivo XML.");
        return;
    }

    if (_nfe.infNFe.ide.mod != ModeloDocumento.NFCe && _escolha == 2)
    {
        Console.WriteLine("O modelo de documento deve ser NFC-e");
        Console.ReadLine();
        return;
    }

    var _pags = new List<DFeDadosPagamento>();
    var _troco = _nfeProc.NFe.infNFe.pag.Sum(s => s.vTroco ?? 0m);
    foreach (var _pag in _nfeProc.NFe.infNFe.pag.SelectMany(s => s.detPag))
    {
        _pags.Add(new DFeDadosPagamento(_troco, (int)_pag.tPag, _pag.vPag));
    }
    var _xml = File.ReadAllText(_xmlPath);
    
    
    var _nfce = new NFCeLeiaute(_xml);

    var _pdf = _nfce.Gerar();

    var _fileInfo = new FileInfo(_xmlPath);
    var _novoNome = $"{_fileInfo.Name}_{DateTime.Now.ToString("yyyyMMdd")}.pdf";
    var _output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

    if (!Directory.Exists(_output))
        Directory.CreateDirectory(_output);
    var _file = Path.Combine(_output, _novoNome);

    File.WriteAllBytes(_file, _pdf);

    Console.WriteLine($"Arquivo salvo em {_file}");
    Console.ReadKey();

}







