using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Encryption
{
    public static class extensions
    {
        public static string ToBase64(this string key)
        {
            return Base64.Encode(key);
        }
        public static string FromBase64(this string key)
        {
            return Base64.Decode(key);
        }
        public static bool isBase64(this string key)
        {
            return Base64.IsBase64(key);
        }
        public static string StandardEncrypt(this string key)
        {
            return StandardCrypter.Encode(key);
        }
        public static string StandardDecrypt(this string key)
        {
            return StandardCrypter.Decode(key);
        }
        public static string RijndaelEncrypt(this string key)
        {
            return Rijndael.Encode(key);
        }
        public static string RijndaelDecrypt(this string key)
        {
            return Rijndael.Decode(key);
        }
        public static string RijndaelEncrypt(this string key, string keyValue, string ivValue)
        {
            try
            {
                Rijndael.Key = keyValue;
                Rijndael.iv = ivValue;
                return Rijndael.Encode(key);
            }
            finally
            {
                Rijndael.ResetDefault();
            }
        }
        public static string RijndaelDecrypt(this string key, string keyValue, string ivValue)
        {
            try
            {
                Rijndael.Key = keyValue;
                Rijndael.iv = ivValue;
                return Rijndael.Decode(key);
            }
            finally
            {
                Rijndael.ResetDefault();
            }
        }
    }
}
