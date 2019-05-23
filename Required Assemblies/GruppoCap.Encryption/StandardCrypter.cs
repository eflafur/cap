using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Encryption
{
    public class StandardCrypter
    {
        public static string Encode(string TextString)
        {
            if (TextString.Length == 0)
                return "";
            Rijndael.ResetDefault();
            return Rijndael.Encode(TextString);
        }
        public static string Decode(string TextString)
        {
            Rijndael.ResetDefault();
            return Rijndael.Decode(TextString);
        }
    }
    public class StandardCrypterPassword : IPasswordEncryption
    {
        #region IPasswordEncryption Members

        public string Encode(string key)
        {
            return StandardCrypter.Encode(key);
        }

        public string Decode(string key)
        {
            return StandardCrypter.Decode(key);
        }

        #endregion
    }
}