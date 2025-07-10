// See https://aka.ms/new-console-template for more information

using MS.DFe.PDF;

using QuestPDF.Companion;

var _escolha = 1;
while (true)
{
    Console.WriteLine("Qual tipo de impressão NF-e(1), NFC-e(2), CC-e(3), Sair(0): ");
    var _tela = Console.ReadLine();
    if (int.TryParse(_tela, out int _int))
    {
        if (_int <= 0) Environment.Exit(0);
        if (_int <= 3)
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
    var _xml = File.ReadAllText(_xmlPath);

    var _nfce = new NFCeLeiaute(_xml);
    _nfce.ShowInCompanion();

}

if (_escolha == 3)
{

}







