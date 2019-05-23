using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Authentication.Core
{
    public interface ICredentialService : IRevoService
    {
        Credential InstanceNew();
        Credential InstanceNew(String userId);

        Credential GetCredentialByEmail(String email);
        Credential GetCredentialByEmailAndPassword(String email, String password);
        Credential GetCredentialByForgotPasswordToken(String token);

        ISubCollection<Credential> ListByUserId(String userId);

        IInsertOperationResult Create(Credential credential);
        IUpdateOperationResult Update(Credential credential);
        IDeleteOperationResult DeleteByEmail(String email);

        IUpdateOperationResult Activate(IRevoWebRequest rreq, String email);
        IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String email);

        IUpdateOperationResult SetForgotPasswordToken(String email);

        IUpdateOperationResult CleanUpOldForgotPasswordToken();
        IUpdateOperationResult CleanUpOldForgotPasswordToken(Credential credential);
    }
}
