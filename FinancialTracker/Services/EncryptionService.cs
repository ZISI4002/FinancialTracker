using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Services
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    namespace FinancialTracker.Services
    {
        internal static class EncryptionService
        {
            // 32 bytes for AES-256 encryption key
            private static readonly byte[] Key = Encoding.UTF8.GetBytes("Your32CharSecretKey12345678901234");
            // 16 bytes for AES Initialization Vector (IV)
            private static readonly byte[] IV = Encoding.UTF8.GetBytes("Your16CharInitVec");

            /// <summary>
            /// Encrypts plain text using AES and returns byte array
            /// </summary>
            public static byte[] Encrypt(string plainText)
            {
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException(nameof(plainText));

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                // Write plain text to the encryption stream
                                swEncrypt.Write(plainText);
                            }
                        }
                        // Return the encrypted byte array AFTER the stream is closed
                        return msEncrypt.ToArray();
                    }
                }
            }


            /// <summary>
            /// Decrypts AES encrypted byte array back to plain text
            /// </summary>
            public static string Decrypt(byte[] cipherBytes)
            {
                if (cipherBytes == null || cipherBytes.Length == 0)
                    throw new ArgumentNullException(nameof(cipherBytes));

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes)) {

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read and return the decrypted text
                                return srDecrypt.ReadToEnd();

                            }
                        }

                    }

                }
            }
        }
    }

}
