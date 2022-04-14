using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.Cryptos
{
    public class RSACrypto
    {
        /// <summary>
        /// RSA 키 생성
        /// </summary>
        /// <returns></returns>
        public static (string PrivateKey, string PublicKey) RSAEncrypt()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            RSAParameters privateKey = RSA.Create().ExportParameters(true);
            rsa.ImportParameters(privateKey);
            string privateKeyText = rsa.ToXmlString(true);

            RSAParameters publicKey = new RSAParameters();
            publicKey.Modulus = privateKey.Modulus;
            publicKey.Exponent = privateKey.Exponent;
            rsa.ImportParameters(publicKey);

            string publicKeyText = rsa.ToXmlString(false);

            return (privateKeyText, publicKeyText);
        }

        /// <summary>
        /// RSA 암호화
        /// </summary>
        /// <param name="val">암호화 값</param>
        /// <param name="publicKey">공개키</param>
        /// <returns></returns>
        public static string RSAEncrypt(string val, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            
            byte[] inbuf = (new UTF8Encoding()).GetBytes(val);
            byte[] encbuf = rsa.Encrypt(inbuf, false);

            return System.Convert.ToBase64String(encbuf);
        }

        /// <summary>
        /// RSA 복호화
        /// </summary>
        /// <param name="val">복호화 값</param>
        /// <param name="privateKey">개인키</param>
        /// <returns></returns>
        public static string RSADecrypt(string val, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            byte[] srcbuf = System.Convert.FromBase64String(val);
            byte[] decbuf = rsa.Decrypt(srcbuf, false);

            return (new UTF8Encoding()).GetString(decbuf, 0, decbuf.Length);
        }
    }
}
