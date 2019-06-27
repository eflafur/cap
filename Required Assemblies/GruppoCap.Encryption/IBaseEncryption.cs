using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Encryption
{
    public interface IPasswordEncryption
    {
         string Encode(string key);
         string Decode(string key);
    }
}
