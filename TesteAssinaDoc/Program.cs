// See https://aka.ms/new-console-template for more information
using AssinaDocs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

Console.WriteLine("Informe o Caminho para o Arquivo:");

var PatchFile = Console.ReadLine();

if (PatchFile != null)
{
    try
    {
        var DadosCert = GetDadosCert();

        string arqCert = DadosCert[0];
        string passCert = DadosCert[1];

        Console.WriteLine("Certificado: " + arqCert);
        Console.WriteLine("Senha: " + passCert);

        var arqfile = new FileInfo(PatchFile);

        Console.WriteLine("Nome: " + arqfile.Name);
        Console.WriteLine("Nome Completo: " + arqfile.FullName);
        Console.WriteLine("Diretório: " + arqfile.Directory);
        Console.WriteLine("Diretório Comp: " + arqfile.DirectoryName);
        Console.WriteLine("Extensão: " + arqfile.Extension);

        Console.WriteLine("\nAssinando Documento...\n");

        //SignDocs.AssinaDocs(arqCert, passCert, arqfile.FullName);
        AssinaPDF(arqCert, passCert, arqfile.FullName);

        Console.WriteLine("Documento Assinado!!!");

    }
    catch (Exception ex)
    {

        Console.WriteLine("Erro ao Assinar Documento!!!\n\n" + ex.Message);
    }
}


string[] GetDadosCert()
{
    string[] DadosCert = { };

    try
    {
        FileStream fs = new FileStream("DadosCert.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        var certificado = sr.ReadLine();
        sr.Close(); //grava e fecha

        if (certificado != null)
        {
            DadosCert = certificado.Split("|");
        }                
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao buscar o arquivo de Conexão!!!\n\n" + ex.Message);
    }

    return DadosCert;
}

void AssinaPDF(string pathCert, string passCert, string pathDoc)
{
    // Carregue o certificado digital
    X509Certificate2 certificado = new X509Certificate2(pathCert, passCert);

    var ArqDoc = new FileInfo(pathDoc);

    // Carregue o conteúdo do PDF (substitua pelo seu próprio PDF)
    byte[] conteudoPdf = File.ReadAllBytes(pathDoc);

    // Crie um objeto de assinatura
    using (var rsa = certificado.GetRSAPrivateKey())
    {
        byte[] assinatura = rsa.SignData(conteudoPdf, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        // Anexe a assinatura ao PDF (substitua pelo seu próprio método de manipulação de PDF)
        byte[] pdfComAssinatura = new byte[conteudoPdf.Length + assinatura.Length];
        conteudoPdf.CopyTo(pdfComAssinatura, 0);
        assinatura.CopyTo(pdfComAssinatura, conteudoPdf.Length);

        // Salve o PDF com a assinatura
        var docAssinado = ArqDoc.DirectoryName + "/Assinado_" + ArqDoc.Name;
        File.WriteAllBytes(docAssinado, pdfComAssinatura);
    }

}