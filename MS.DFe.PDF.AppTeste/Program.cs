// See https://aka.ms/new-console-template for more information

using DFe.Classes.Flags;
using MS.DFe.PDF;
using MS.DFe.PDF.Modelos;
using NFe.Classes;

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

string? _xml = null;
while (string.IsNullOrEmpty(_xml))
{
    Console.WriteLine("Informe o caminho para o xml processado da NFC-e");
    _xml = Console.ReadLine();
    if (!File.Exists(_xml))
    {
        Console.Clear();
        Console.WriteLine("Arquivo não encontrado.");
        _xml = null;
    }
}

var _conteudo = File.ReadAllText(_xml);
var _nfeProc = new nfeProc().CarregarDeXmlString(_conteudo);

if (_nfeProc.NFe.infNFe.ide.mod != ModeloDocumento.NFCe)
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

var _fileInfo = new FileInfo(_xml);
var _novoNome = $"{_fileInfo.Name}_{DateTime.Now.ToString("yyyyMMdd")}.pdf";
var _output = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

if (!Directory.Exists(_output))
    Directory.CreateDirectory(_output);
var _file = Path.Combine(_output, _novoNome);

File.WriteAllBytes(_file, _pdf);

Console.WriteLine($"Arquivo salvo em {_file}");
Console.ReadKey();
