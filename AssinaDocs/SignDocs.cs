using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace AssinaDocs
{
    public class SignDocs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathCert" value="caminho\\para\\certificado.pfx"></param>
        /// <param name="passCert" value="senha certificado"></param>
        /// <param name="pathDoc" value="caminho\\para\\seu\\arquivo.pdf"></param>
        /// 
        public static void AssinaDocs(string pathCert, string passCert, string pathDoc)
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
    }
}
