using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CryptLib
{

    [ComVisible(true)]
    [Guid("61F0194E-6F98-4F9C-9D16-AB32E8F3B878")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ICryptLib
    {
        [DispId(1)]
        void SignFile(string fileToSign, string certFile, bool detachedSignature = false, string sigFile = "", string password = "");
        
        [DispId(2)]
        void CreateCertificate(string certDir, string issuer, string sigAlg, string hashAlg, string passCert = "", int years = 1);
        
        [DispId(3)]
        void VerifySignature(string fileToDecode, out string info);

        [DispId(4)]
        void RSAEncrypt(string fileToEncrypt, string certFile, string hashAlg);

        [DispId(5)]
        void RSADecrypt(string fileToDecrypt, string certFile, string hashAlg, string password = "");

        [DispId(6)]
        void AESEncrypt(string Base64key, string fileToEncrypt, out string Base64IV);

        [DispId(7)]
        void AESDecrypt(string Base64key, string Base64IV, string fileToDecrypt);

        [DispId(8)]
        string GenerateECDiffieHellmanKey(string pathToMyCert, string pathToOtherCert, string hasAlg, string password = "");
        
        [DispId(9)]
        void ConvertCertFormat(string certFile, string toWhat, out string info, string privateKey = "", string certPass = "");

        [DispId(10)]
        string ComputeFileHash(string fileToHash, string hashAlg, out long size);

        [DispId(11)]
        void SignApplication(string fileToSign, string certFile, string password = "", bool isAppX = false);
    }

    [ComVisible(true)]
    [Guid("C85D24AD-8DE0-4EA8-82F5-99525350B2EA")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("CryptLib.Functions")]
    public class CryptLib : ICryptLib
    {
        [DllImport("AppSign.dll")]
        private static extern long SignApp(IntPtr bSigningCertContext, ulong cbCertEncoded, [MarshalAs(UnmanagedType.LPWStr)] string packageFilePath, [MarshalAs(UnmanagedType.LPWStr)] string timestampUrl, [MarshalAs(UnmanagedType.Bool)] bool isSigningAppx);

        [DllImport("AppSign.dll")]
        private static extern void GetHRMessage(long hr, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder message);

        public void SignApplication(string fileToSign, string certFile, string password = "", bool isAppX = false)
        {
            if (!File.Exists(fileToSign))
                throw new Exception("File to sign is not specified or does not exist!");
            if (!File.Exists(certFile))
                throw new Exception("Certificate file is not specified or does not exist!");

            using X509Certificate2? cert = new(certFile, string.IsNullOrWhiteSpace(password) ? null : password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            if (!cert.HasPrivateKey)
                throw new Exception("Specified certificate has no private key!");

            byte[] data = cert.Export(X509ContentType.Pfx);
            IntPtr bd = Marshal.AllocCoTaskMem(data.Length);
            Marshal.Copy(data, 0, bd, data.Length);
            long res = SignApp(bd, (ulong)data.Length, fileToSign, "http://timestamp.sectigo.com", isAppX);
            Marshal.FreeCoTaskMem(bd);

            if (res != 0)
            {
                StringBuilder message = new(2048);
                GetHRMessage(res, message);
                throw new Exception("HRESULT is 0x" + res.ToString("x") + ":\r\n" + message);
            }
        }

        public void SignFile(string fileToSign, string certFile, bool detachedSignature = false, string sigFile = "", string password = "")
        {
            if (!File.Exists(fileToSign))
                throw new Exception("File to sign is not specified or does not exist!");
            if (!File.Exists(certFile))
                throw new Exception("Certificate file is not specified or does not exist!");
            if (detachedSignature && string.IsNullOrWhiteSpace(sigFile))
                throw new Exception("Signature file not specified!");

            using X509Certificate2? cert = new(certFile, string.IsNullOrWhiteSpace(password) ? null : password);
            if (!cert.HasPrivateKey)
                throw new Exception("Specified certificate has no private key!");

            SignedCms cms = new(new ContentInfo(File.ReadAllBytes(fileToSign)), detachedSignature);
            cms.ComputeSignature(new CmsSigner(cert), false);
            File.WriteAllBytes(detachedSignature ? sigFile : fileToSign, cms.Encode());
        }

        public void CreateCertificate(string certDir, string issuer, string sigAlg, string hashAlg, string passCert = "", int years = 1)
        {
            if (!Directory.Exists(certDir))
                throw new Exception("Directory for certificate files is not specified or does not exist!");
            if (string.IsNullOrEmpty(issuer))
                throw new Exception("Issuer not specified");

            CertificateRequest req;

            if (sigAlg.StartsWith("RSA"))
            {
                int keySize = int.Parse(sigAlg[3..]);
                RSA rsa = RSA.Create(keySize);
                req = new("CN=" + issuer, rsa, new HashAlgorithmName(hashAlg), RSASignaturePadding.Pkcs1);
            }
            else if (sigAlg == "ECDSA")
            {
                ECDsa ecdsa = ECDsa.Create();
                req = new("CN=" + issuer, ecdsa, new HashAlgorithmName(hashAlg));
            }
            else
                throw new Exception("Unknown signature algorithm!");

            using X509Certificate2? cert = req?.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(years));
            if (cert == null)
                throw new Exception("Failed to crate certificate!");


            File.WriteAllBytes(certDir + "\\" + issuer + ".pfx", cert.Export(X509ContentType.Pfx, string.IsNullOrWhiteSpace(passCert) ? null : passCert));
            SaveCertificateAsCer(cert, certDir + "\\" + issuer + ".cer");
        }

        public void SaveCertificateAsCer(X509Certificate2 cert, string path)
        {
            if (cert == null)
                throw new ArgumentNullException(nameof(cert));

            File.WriteAllText(path, new string(PemEncoding.Write("CERTIFICATE", cert.RawData)));
        }

        public void ConvertCertFormat(string certFile, string toWhat, out string info, string privateKey = "", string certPass = "")
        {
            if (!File.Exists(certFile))
                throw new Exception("Certificate file is not specified or does not exist!");
            if (string.IsNullOrWhiteSpace(toWhat))
                throw new Exception("Unknown output format!");

            info = "";
            string? dirName = Path.GetDirectoryName(certFile);
            using X509Certificate2? cert = new(certFile, string.IsNullOrWhiteSpace(certPass) ? null : certPass, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

            switch (toWhat)
            {
                case "To CRT":
                    info = dirName + "\\Certificate.crt";
                    File.WriteAllBytes(info, cert.Export(X509ContentType.Cert));
                    break;
                case "To CER":
                    info = dirName + "\\Certificate.cer";
                    SaveCertificateAsCer(cert, info);
                    break;
                case "To PFX":
                    info = dirName + "\\Certificate.pfx";
                    File.WriteAllBytes(info, cert.Export(X509ContentType.Pfx, string.IsNullOrWhiteSpace(certPass) ? null : certPass));
                    break;
                case "PFX to PEM":

                    info = dirName + "\\Certificate.pem";
                    File.WriteAllText(info, new string(PemEncoding.Write("CERTIFICATE", cert.RawData)));
                    AsymmetricAlgorithm? key = (AsymmetricAlgorithm?)cert?.GetRSAPrivateKey() ?? cert?.GetECDsaPrivateKey();
                    RSA rsa1 = RSA.Create();
                    //rsa1.ImportEncryptedPkcs8PrivateKey(certPass, key.ExportEncryptedPkcs8PrivateKey(certPass, new PbeParameters(
                    //        PbeEncryptionAlgorithm.Aes128Cbc,
                    //        HashAlgorithmName.SHA256,
                    //        1)), out int read);
                    if (key != null)
                    {
                        info += "\r\n" + dirName + "\\Public.pem\r\n" + dirName + "\\Private.pem";
                        File.WriteAllText(dirName + "\\Public.pem", new string(PemEncoding.Write("PUBLIC KEY", key.ExportSubjectPublicKeyInfo())));
                        //File.WriteAllBytes(dirName + "\\key", rsa1.ExportRSAPrivateKey());

                        File.WriteAllText(dirName + "\\Private.pem", new string(PemEncoding.Write("PRIVATE KEY", key.ExportEncryptedPkcs8PrivateKey(certPass, new PbeParameters(
                            PbeEncryptionAlgorithm.Aes128Cbc,
                            HashAlgorithmName.SHA256,
                            1)))));
                    }
                    break;

                case "PEM to PFX":
                    
                    if (!File.Exists(privateKey))
                        throw new Exception("Private key file is not specified or does not exist!");

                    string[] privBlocks = File.ReadAllText(privateKey).Split("-", StringSplitOptions.RemoveEmptyEntries);
                    byte[] privateKeyBytes = Convert.FromBase64String(privBlocks[1]);
                    info = dirName + "\\Certificate.pfx";

                    if (cert.GetRSAPublicKey() != null)
                    {
                        using RSA rsa = RSA.Create();
                        if (privBlocks[0] == "BEGIN PRIVATE KEY")
                        {
                            rsa.ImportEncryptedPkcs8PrivateKey(string.IsNullOrWhiteSpace(certPass) ? null : certPass, privateKeyBytes, out _);
                        }
                        else if (privBlocks[0] == "BEGIN RSA PRIVATE KEY")
                        {
                            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
                        };
                        File.WriteAllBytes(info, cert.CopyWithPrivateKey(rsa).Export(X509ContentType.Pfx, string.IsNullOrWhiteSpace(certPass) ? null : certPass));
                    }
                    else if (cert.GetDSAPublicKey != null)
                    {
                        using DSA dsa = DSA.Create();
                        if (privBlocks[0] == "BEGIN PRIVATE KEY")
                        {
                            dsa.ImportEncryptedPkcs8PrivateKey(string.IsNullOrWhiteSpace(certPass) ? null : certPass, privateKeyBytes, out _);
                        };
                        File.WriteAllBytes(info, cert.CopyWithPrivateKey(dsa).Export(X509ContentType.Pfx, string.IsNullOrWhiteSpace(certPass) ? null : certPass));
                    }
                    else if (cert.GetECDsaPublicKey() != null)
                    {
                        using ECDsa dsa = ECDsa.Create();
                        if (privBlocks[0] == "BEGIN PRIVATE KEY")
                        {
                            dsa.ImportEncryptedPkcs8PrivateKey(string.IsNullOrWhiteSpace(certPass) ? null : certPass, privateKeyBytes, out _);
                        };
                        File.WriteAllBytes(info, cert.CopyWithPrivateKey(dsa).Export(X509ContentType.Pfx, string.IsNullOrWhiteSpace(certPass) ? null : certPass));
                    }
                    else if (cert.GetECDiffieHellmanPublicKey() != null)
                    {
                        using ECDiffieHellmanCng dsa = new();
                        if (privBlocks[0] == "BEGIN PRIVATE KEY")
                        {
                            dsa.ImportEncryptedPkcs8PrivateKey(string.IsNullOrWhiteSpace(certPass) ? null : certPass, privateKeyBytes, out _);
                        };
                        File.WriteAllBytes(info, cert.CopyWithPrivateKey(dsa).Export(X509ContentType.Pfx, string.IsNullOrWhiteSpace(certPass) ? null : certPass));
                    }
                    else
                        throw new Exception("Unknown key algorithm!");

                    break;

                default:
                    throw new Exception("Unknown output format!");
            }
        }

        public void VerifySignature(string fileToDecode, out string info)
        {
            if (!File.Exists(fileToDecode))
                throw new Exception("File to decode is not specified or does not exist!");

            SignedCms cms = new();
            cms.Decode(File.ReadAllBytes(fileToDecode));
            cms.CheckSignature(true);
            
            info = "";
            for (int i = 0; i < cms.SignerInfos.Count; i++)
            {
                using X509Certificate2? cert = cms.SignerInfos[i].Certificate;
                
                string certPath = Path.GetDirectoryName(fileToDecode) + "\\Certificate" + (i + 1) + ".cer";
                info += "Signature " + (i + 1) + ":\r\n\r\nIssuer: " + cert?.Issuer + "\r\nSubject: " + cert?.Subject + "\r\nSerial #: " + cert?.SerialNumber + "\r\nNot before: " + cert?.NotBefore + "\r\nNot after: " + cert?.NotAfter + "\r\nSignature algorithm: " + cert?.SignatureAlgorithm.FriendlyName + "\r\n\r\nCertificate extracted and saved as \"" + certPath + "\"" + (i == cms.SignerInfos.Count - 1 ? "" : "\r\n\r\n");

                if (cert != null)
                    SaveCertificateAsCer(cert, certPath);
                cms.RemoveSignature(i);
            }

            File.WriteAllBytes(fileToDecode, cms.ContentInfo.Content);

        }

        public string GenerateECDiffieHellmanKey(string pathToMyCert, string pathToOtherCert, string hasAlg, string password = "")
        {
            if (!File.Exists(pathToMyCert) || !File.Exists(pathToOtherCert))
                throw new Exception("Certificate file is not specified or does not exist!");

            using X509Certificate2 myCert = new(pathToMyCert, string.IsNullOrWhiteSpace(password) ? null : password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            if (!myCert.HasPrivateKey)
                throw new Exception("Specified certificate has no private key!");

            ECDsa? prk = myCert.GetECDsaPrivateKey();
            using dynamic? privateKey = prk != null ? prk : myCert.GetECDiffieHellmanPrivateKey();
            if (privateKey == null)
                throw new Exception("No ECDH or ECDSA private key found!");

            using X509Certificate2 otherCert = new(pathToOtherCert);
            
            ECDsa? puk = myCert.GetECDsaPublicKey();
            using dynamic? publicKey = puk != null ? puk : myCert.GetECDiffieHellmanPublicKey();
            if (publicKey == null)
                throw new Exception("No ECDH or ECDSA public key found!");

            using ECDsa myEcdsa = ECDsa.Create();
            myEcdsa.ImportEncryptedPkcs8PrivateKey(password, privateKey.ExportEncryptedPkcs8PrivateKey(password, new PbeParameters(
                PbeEncryptionAlgorithm.Aes128Cbc,
                HashAlgorithmName.SHA256,
                1)), out int _);

            using ECDiffieHellmanCng myEcdh = new();
            myEcdh.ImportParameters(myEcdsa.ExportParameters(true));

            using ECDiffieHellmanCng otherECDH = new();
            otherECDH.ImportParameters(publicKey.ExportParameters(false));

            return Convert.ToBase64String(myEcdh.DeriveKeyFromHash(otherECDH.PublicKey, new HashAlgorithmName(hasAlg)));
        }

        private void InitBytes(ref byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = 0;
        }

        public void RSAEncrypt(string fileToEncrypt, string certFile, string hashAlg)
        {
            if (!File.Exists(fileToEncrypt))
                throw new Exception("File to encrypt is not specified or does not exist!");
            if (!File.Exists(certFile))
                throw new Exception("Certificate file is not specified or does not exist!");
            if (!int.TryParse(hashAlg[3..], out int hashSize))
                throw new Exception("Unknown hash algorithm!");

            using X509Certificate2 cert = new(certFile);
            using RSA? rsa = cert.GetRSAPublicKey();
            if (rsa == null)
                throw new Exception("No RSA public key found!");
            if (rsa.KeySize < 2048 && hashAlg == "SHA512")
                throw new Exception("OAEP SHA512 padding is not applicable when key size is less than 2048!");

            byte[] data = new byte[rsa.KeySize / 8 - 2 * hashSize / 8 - 2];
            int keySizeInBytes = rsa.KeySize / 8;

            using FileStream fs = new(fileToEncrypt, FileMode.Open, FileAccess.Read);
            using MemoryStream ms = new();

            while (fs.Read(data, 0, data.Length) > 0)
            {
                ms.Write(rsa.Encrypt(data, RSAEncryptionPadding.CreateOaep(new HashAlgorithmName(hashAlg))), 0, keySizeInBytes);
                InitBytes(ref data);
            }
            
            fs.Close();

            File.WriteAllBytes(fileToEncrypt, ms.ToArray());
            ms.Close();

        }

        public void RSADecrypt(string fileToDecrypt, string certFile, string hashAlg, string password = "")
        {
            if (!File.Exists(fileToDecrypt))
                throw new Exception("File to decrypt is not specified or does not exist!");
            if (!File.Exists(certFile))
                throw new Exception("Certificate file is not specified or does not exist!");
            if (!int.TryParse(hashAlg[3..], out int hashSize))
                throw new Exception("Unknown hash algorithm!");

            using X509Certificate2 cert = new(certFile, string.IsNullOrWhiteSpace(password) ? null : password);
            if (!cert.HasPrivateKey)
                throw new Exception("Specified certificate has no private key!");

            using RSA? rsa = cert.GetRSAPrivateKey();
            if (rsa == null)
                throw new Exception("No RSA private key found!");
            if (rsa.KeySize < 2048 && hashAlg == "SHA512")
                throw new Exception("OAEP SHA512 padding is not applicable when key size is less than 2048!");

            int blockSize = rsa.KeySize / 8 - 2 * hashSize / 8 - 2;
            byte[] data = new byte[rsa.KeySize / 8];

            using FileStream fs = new(fileToDecrypt, FileMode.Open, FileAccess.Read);
            using MemoryStream ms = new();

            while (fs.Read(data, 0, data.Length) > 0)
            {
                ms.Write(rsa.Decrypt(data, RSAEncryptionPadding.CreateOaep(new HashAlgorithmName(hashAlg))), 0, blockSize);
                InitBytes(ref data);
            }

            fs.Close();

            File.WriteAllBytes(fileToDecrypt, ms.ToArray());
            ms.Close();

        }

        public void AESEncrypt(string Base64key, string fileToEncrypt, out string Base64IV)
        {
            byte[] key = Convert.FromBase64String(Base64key);
            
            if (!File.Exists(fileToEncrypt))
                throw new Exception("File to encrypt is not specified or does not exist!");
            if (key.Length == 0)
                throw new Exception("Encryption key is not specified!");

            using AesCng aes = new();

            aes.Key = aes.KeySize / 8 != key.Length ? HashAlgorithm.Create("SHA256").ComputeHash(key) : key;
            Base64IV = Convert.ToBase64String(aes.IV);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] plainTextBytes = File.ReadAllBytes(fileToEncrypt);
            cs.Write(plainTextBytes, 0, plainTextBytes.Length);
            cs.Close();

            File.WriteAllBytes(fileToEncrypt, ms.ToArray());
            ms.Close();
        }

        public void AESDecrypt(string Base64key, string Base64IV, string fileToDecrypt)
        {
            byte[] key = Convert.FromBase64String(Base64key);
            byte[] IV = Convert.FromBase64String(Base64IV);

            if (!File.Exists(fileToDecrypt))
                throw new Exception("File to decrypt is not specified or does not exist!");
            if (key.Length == 0)
                throw new Exception("Encryption key is not specified!");
            if (IV.Length == 0)
                throw new Exception("Encryption initialization vector is not specified!");

            using AesCng aes = new();
            aes.Key = aes.KeySize / 8 != key.Length ? HashAlgorithm.Create("SHA256").ComputeHash(key) : key;
            aes.IV = IV;

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] cipherTextBytes = File.ReadAllBytes(fileToDecrypt);
            cs.Write(cipherTextBytes, 0, cipherTextBytes.Length);
            cs.Close();

            File.WriteAllBytes(fileToDecrypt, ms.ToArray());
            ms.Close();
        }

        public string ComputeFileHash(string fileToHash, string hashAlg, out long size)
        {
            size = 0;
            
            if (!File.Exists(fileToHash))
                throw new Exception("File to hash is not specified or does not exist!");


            HashAlgorithm? hash = HashAlgorithm.Create(hashAlg);
            if (hash == null)
                throw new Exception("Unknown hash algorithm!");

            byte[] fileBytes = File.ReadAllBytes(fileToHash);
            size = fileBytes.LongLength;
            
            return Convert.ToBase64String(hash.ComputeHash(fileBytes));
        }
    }

    public static class Conversion
    {
        public static string ConvertString(string data, string type, string encoding = "UTF-8")
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(type))
                return string.Empty;

            Encoding enc = Encoding.GetEncoding(encoding);
            switch (type)
            {
                case "To Base64":
                    return Convert.ToBase64String(enc.GetBytes(data));
                case "From Base64":
                    return enc.GetString(Convert.FromBase64String(data));
                case "To Hex":
                    return BitConverter.ToString(enc.GetBytes(data));
                case "From Hex":
                    return enc.GetString(Convert.FromHexString(data.Replace("-", "").Replace(" ", "")));
                case "URL encode":
                    return HttpUtility.UrlEncode(data, enc);
                case "URL decode":
                    return HttpUtility.UrlDecode(data, enc);
                case "HTML encode":
                    return HttpUtility.HtmlEncode(data);
                case "HTML decode":
                    return HttpUtility.HtmlDecode(data);
                case "To JS string":
                    return HttpUtility.JavaScriptStringEncode(data);
                case "From JS string":
                    return Uri.UnescapeDataString(Regex.Unescape(data));
                case "To UTF-7":
                    return enc.GetString(Encoding.Convert(enc, Encoding.UTF7, enc.GetBytes(data)));
                case "From UTF-7":
                    return enc.GetString(Encoding.Convert(Encoding.UTF7, enc, enc.GetBytes(data)));
                case "To deflated SAML":
                    return EncodeSAMLRequestString(data, enc);
                case "From deflated SAML":
                    return DecodeSAMLRequestString(data, enc);
                case "To MD5":
                    using (MD5 p = MD5.Create())
                    {
                        byte[] hash = p.ComputeHash(enc.GetBytes(data));
                        return BitConverter.ToString(hash) + "\r\n\r\n" + Convert.ToBase64String(hash);
                    };
                case "To SHA1":
                    using (SHA1 p = SHA1.Create())
                    {
                        byte[] hash = p.ComputeHash(enc.GetBytes(data));
                        return BitConverter.ToString(hash) + "\r\n\r\n" + Convert.ToBase64String(hash);
                    };
                case "To SHA256":
                    using (SHA256 p = SHA256.Create())
                    {
                        byte[] hash = p.ComputeHash(enc.GetBytes(data));
                        return BitConverter.ToString(hash) + "\r\n\r\n" + Convert.ToBase64String(hash);
                    };
                case "To SHA384":
                    using (SHA384 p = SHA384.Create())
                    {
                        byte[] hash = p.ComputeHash(enc.GetBytes(data));
                        return BitConverter.ToString(hash) + "\r\n\r\n" + Convert.ToBase64String(hash);
                    };
                case "To SHA512":
                    using (SHA512 p = SHA512.Create())
                    {
                        byte[] hash = p.ComputeHash(enc.GetBytes(data));
                        return BitConverter.ToString(hash) + "\r\n\r\n" + Convert.ToBase64String(hash);
                    };
                default:
                    return data;
            }
        }

        private static string DecodeSAMLRequestString(string compressedData, Encoding enc)
        {
            using (DeflateStream deflate = new DeflateStream(new MemoryStream(Convert.FromBase64String(HttpUtility.UrlDecode(compressedData, enc))), CompressionMode.Decompress))
            {
                using (StreamReader reader = new StreamReader(deflate, enc))
                    return HttpUtility.UrlDecode(reader.ReadToEnd());
            }
        }

        private static string EncodeSAMLRequestString(string data, Encoding enc)
        {
            using (MemoryStream source = new MemoryStream(enc.GetBytes(HttpUtility.UrlEncode(data, enc))))
            {
                using (MemoryStream dest = new MemoryStream())
                {
                    using (DeflateStream deflate = new DeflateStream(dest, CompressionMode.Compress))
                    {
                        source.CopyTo(deflate);
                        deflate.Close();
                        return HttpUtility.UrlEncode(Convert.ToBase64String(dest.ToArray()), enc);
                    }
                }
            }
        }

    }

}