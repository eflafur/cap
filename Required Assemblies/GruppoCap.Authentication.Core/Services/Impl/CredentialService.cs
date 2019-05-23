using GruppoCap.Core;
using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppoCap;
using GruppoCap.DAL;

namespace GruppoCap.Authentication.Core
{
    public class CredentialService : ICredentialService
    {
        ICredentialRepo _credentialRepo = null;

        // CTOR
        public CredentialService(ICredentialRepo credentialRepo)
        {
            _credentialRepo = credentialRepo;
        }


        // INSTANCE NEW
        public Credential InstanceNew()
        {
            return _credentialRepo.CreateEntityInstance();
        }

        // INSTANCE NEW
        public Credential InstanceNew(String userId)
        {
            Credential _c = _credentialRepo.CreateEntityInstance();
            _c.UserId = userId;

            return _c;
        }


        // GET CREDENTIAL BY EMAIL
        public Credential GetCredentialByEmail(String email)
        {
            if (email.IsNullOrWhiteSpace())
                return null;

            return _credentialRepo.GetCredentialByEmail(email.Trim());
        }

        // GET CREDENTIAL BY EMAIL AND PASSWORD
        public Credential GetCredentialByEmailAndPassword(String email, String password)
        {
            if (email.IsNullOrWhiteSpace())
                return null;

            if (password.IsNullOrWhiteSpace())
                return null;

            return _credentialRepo.GetCredentialByEmailAndPassword(
                email.Trim(), 
                StringUtils.GenerateSHA256Hash(password.Trim())
            );
        }

        // GET CREDENTIAL BY FORGOT PASSWORD TOKEN
        public Credential GetCredentialByForgotPasswordToken(String token)
        {
            return _credentialRepo.GetCredentialByForgotPasswordToken(token);
        }



        // GET BY USER ID
        public ISubCollection<Credential> ListByUserId(String userId)
        {
            return _credentialRepo.GetCredentialsByAccount(userId);
        }

        // CREATE
        public IInsertOperationResult Create(Credential credential)
        {
            return _credentialRepo.Insert(credential);
        }

        // UPDATE
        public IUpdateOperationResult Update(Credential credential)
        {
            return _credentialRepo.Update(credential);
        }

        // DELETE BY EMAIL
        public IDeleteOperationResult DeleteByEmail(String email)
        {
            throw new NotImplementedException("La cancellazione delle credenziali non è attualmente consentita.");
        }

        // SET ACTIVE
        private IUpdateOperationResult SetActive(String email, Boolean isActive, String author)
        {
            if (email.IsNullOrWhiteSpace())
                return new UpdateOperationResult(false, "email cannot be null");

            ICredential _c = this.GetCredentialByEmail(email);

            if (_c == null)
                return new UpdateOperationResult(false, "Cannot find the requested credential");

            _c.IsActive = isActive;
            _c.LastUpdateMoment = DateTime.Now;
            _c.LastUpdateUserId = author;

            return this.Update((Credential)_c);
        }

        // ACTIVATE
        public IUpdateOperationResult Activate(IRevoWebRequest rreq, String email)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUsername.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(email, true, rreq.CurrentUser.UserId);
        }

        // DEACTIVATE
        public IUpdateOperationResult Deactivate(IRevoWebRequest rreq, String email)
        {
            if (rreq == null)
                throw new NullReferenceException("Revolution Web Request cannot be null");

            if (rreq.CurrentUser.UserId.IsNullOrWhiteSpace())
                throw new NullReferenceException("Revolution Web Request - Logon username cannot be empty");

            return SetActive(email, false, rreq.CurrentUser.UserId);
        }

        
        // SET FORGOT PASSWORD TOKEN
        public IUpdateOperationResult SetForgotPasswordToken(String email)
        {
            if(email.IsNullOrWhiteSpace())
                return new UpdateOperationResult(false, "Email cannot be null");

            Credential _c = _credentialRepo.GetCredentialByEmail(email.Trim());

            if(_c == null)
                return new UpdateOperationResult(false, "Credential non trovata");

            _c.ForgotPasswordToken = Guid.NewGuid().ToString();
            _c.ForgotPasswordTokenMoment = DateTime.Now;

            return _credentialRepo.Update(_c);
        }

        // CLEAN UP OLD FORGOT PASSWORD TOKEN
        public IUpdateOperationResult CleanUpOldForgotPasswordToken()
        {
            throw new NotImplementedException();
        }

        // CLEAN UP OLD FORGOT PASSWORD TOKEN
        public IUpdateOperationResult CleanUpOldForgotPasswordToken(Credential credential)
        {
            throw new NotImplementedException();
        }
    }
}
