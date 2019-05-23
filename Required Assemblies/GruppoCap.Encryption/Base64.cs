using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Encryption
{
    public class Base64
    {
        public static string Encode(string key)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(key);
                return Convert.ToBase64String(data);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Error during encryption in BASE64: {0}", ex.Message));
            }
        }
        public static string Decode(string key)
        {
            if (!IsBase64(key))
                throw new ApplicationException("Input string is not in BASE64 format.");
            try
            {
                byte[] data = Convert.FromBase64String(key);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
            }
            throw new ApplicationException("Input string is not in BASE64 format.");
        }
        public static bool IsBase64(string base64String)
        {
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
    public class Base64Password : IPasswordEncryption
    {

        #region IPasswordEncryption Members

        public string Encode(string key)
        {
            return Base64.Encode(key);
        }

        public string Decode(string key)
        {
            return Base64.Decode(key);
        }

        #endregion
    }
}