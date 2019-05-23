using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Encryption
{
    public class Rijndael
    {
        public static void ResetDefault()
        {
            keyValue = "rgQSczbMMcdlTtSb";
            ivValue = "ELthwKSWWuNOCgDc";
        }
        private static string keyValue = "rgQSczbMMcdlTtSb";
        public static string Key
        {
            set { keyValue = value; }
        }

        private static string ivValue = "ELthwKSWWuNOCgDc";
        public static string iv
        {
            set { ivValue = value; }
        }
        public static string Encode(string TextString)
        {
            try
            {
                RijndaelManaged rjm = new RijndaelManaged();
                rjm.KeySize = 128;
                rjm.BlockSize = 128;
                rjm.Key = ASCIIEncoding.ASCII.GetBytes(keyValue);
                rjm.IV = ASCIIEncoding.ASCII.GetBytes(ivValue);

                //rjm.Padding = PaddingMode.ANSIX923

                byte[] input = Encoding.UTF8.GetBytes(TextString);
                byte[] output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
                return Convert.ToBase64String(output);
            }
            catch
            {
                return TextString;
            }
        }
        public static string Decode(string TextString)
        {
            try
            {
                RijndaelManaged rjm = new RijndaelManaged();

                rjm.KeySize = 128;
                rjm.BlockSize = 128;
                rjm.Key = ASCIIEncoding.ASCII.GetBytes(keyValue);
                rjm.IV = ASCIIEncoding.ASCII.GetBytes(ivValue);

                //rjm.Padding = PaddingMode.ANSIX923

                byte[] input = Convert.FromBase64String(TextString);
                byte[] output = rjm.CreateDecryptor().TransformFinalBlock(input, 0, input.Length);
                return Encoding.UTF8.GetString(output);
            }
            catch
            {
                return TextString;
            }
        }
    }
    public class RijndaelPassword : IPasswordEncryption
    {
        #region IPasswordEncryption Members

        public string Encode(string key)
        {
            return Rijndael.Encode(key);
        }

        public string Decode(string key)
        {
            return Rijndael.Decode(key);
        }

        #endregion
    }
}