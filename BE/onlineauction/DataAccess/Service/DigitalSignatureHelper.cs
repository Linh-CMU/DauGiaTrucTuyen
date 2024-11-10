using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class DigitalSignatureHelper
    {
        private RSA _rsa;

        public DigitalSignatureHelper()
        {
            _rsa = RSA.Create(2048);
        }

        public (string publicKey, string privateKey) GenerateKeys()
        {
            string publicKey = Convert.ToBase64String(_rsa.ExportRSAPublicKey());
            string privateKey = Convert.ToBase64String(_rsa.ExportRSAPrivateKey());
            return (publicKey, privateKey);
        }

        public string SignData(string data, string privateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            _rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            byte[] signature = _rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signature);
        }

        public bool VerifyData(string data, string signature, string publicKey)
        {
            byte[] signatureBytes = Convert.FromBase64String(signature);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            _rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            return _rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
