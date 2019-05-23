using GruppoCap.Core;
using GruppoCap.Core.Data;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface ICredentialRepo : IRepository<Credential> 
    {
        Credential GetCredentialByEmail(String email);

        Credential GetCredentialByEmailAndPassword(String email, String password);

        Credential GetCredentialByForgotPasswordToken(String token);

        ISubCollection<Credential> GetCredentialsByAccount(String userId);
    }
}
