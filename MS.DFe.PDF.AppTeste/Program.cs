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
        _xmlPath = "c:/temp/43250142404799000121550000000002121903625971-proc-nfe.xml";

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
            _logoPath = "c:/temp/logo80.png";

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

    var _nfce = new NFCeLeiaute(
        new DFeDados(
            new DFeDadosEmitente(
                _nfeProc.NFe.infNFe.emit.xNome,
                _nfeProc.NFe.infNFe.emit.CNPJ,
                _nfeProc.NFe.infNFe.emit.enderEmit.xLgr,
                _nfeProc.NFe.infNFe.emit.enderEmit.nro,
                _nfeProc.NFe.infNFe.emit.enderEmit.xBairro,
                _nfeProc.NFe.infNFe.emit.enderEmit.xMun,
                _nfeProc.NFe.infNFe.emit.enderEmit.UF.ToString()
            ),
            _nfeProc.NFe.infNFe.det.Select(
                s => new DFeDadosItem(
                    s.prod.cProd,
                    s.prod.xProd,
                    s.prod.qCom,
                    s.prod.uCom,
                    s.prod.vUnCom,
                    s.prod.vProd
                )
            ),
            new DFeDadosTotal(
                _nfeProc.NFe.infNFe.total.ICMSTot.vProd,
                _nfeProc.NFe.infNFe.total.ICMSTot.vDesc,
                _nfeProc.NFe.infNFe.total.ICMSTot.vFrete,
                _nfeProc.NFe.infNFe.total.ICMSTot.vOutro,
                _nfeProc.NFe.infNFe.total.ICMSTot.vSeg,
                _nfeProc.NFe.infNFe.total.ICMSTot.vNF
            ),
            _pags,
            new DFeDadosConsumidor(
                _nfeProc.NFe.infNFe.dest?.CNPJ,
                _nfeProc.NFe.infNFe.dest?.CPF
            ),
            new DFeDadosConsulta(
                _nfeProc.NFe.infNFeSupl.urlChave,
                _nfeProc.protNFe.infProt.chNFe
            ),
            new DFeDadosIdentificacao(
                _nfeProc.NFe.infNFe.ide.nNF,
                _nfeProc.NFe.infNFe.ide.serie,
                _nfeProc.NFe.infNFe.ide.dhEmi,
                _nfeProc.protNFe.infProt.nProt,
                _nfeProc.protNFe.infProt.dhRecbto
            ),
            new DFeDadosComprovante(
                new string[]
                {
                "Cartão de Débito: Visa",
                "Data/Hora: 02/07/2023 20:02",
                "Valor: R$ 120,00",
                "NSU: 1023882"
                }
            ),
            _nfeProc.NFe.infNFeSupl.qrCode,
            _nfeProc.NFe.infNFe.total.ICMSTot.vTotTrib
        )
    );

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







