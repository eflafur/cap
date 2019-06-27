using GruppoCap.Core;
using GruppoCap.Core.Data;
using GruppoCap.DAL;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace GruppoCap.Authentication.Core
{
    public class SqlCredentialRepo : SqlRepositoryBase<Credential>, ICredentialRepo
    {
        // GET CREDENTIAL BY EMAIL
        public Credential GetCredentialByEmail(String email)
        {
            try
            {
                return this.GetById(email);
            }
            catch (Exception ex)
            {
                Object[] parameters = new Object[] { email };
                RevoContextHelpers.GetCurrentRevoContext().ContextLogger.ErrorEx(ex, "I had an error trying to recover the credential by email with this message: {0}".FormatWith(ex.Message), parameters);
                return null;
            }
        }

        // GET CREDENTIAL BY EMAIL AND PASSWORD
        public Credential GetCredentialByEmailAndPassword(String email, String password)
        {
            try
            {
                return db.SingleOrDefault<Credential>(" WHERE EMAIL = @0 AND PASSWORD_HASH = @1 ", email, password);
            }
            catch (Exception ex)
            {
                Object[] parameters = new Object[] { email, password };
                RevoContextHelpers.GetCurrentRevoContext().ContextLogger.ErrorEx(ex, "I had an error trying to recover the credential by email and password with this message: {0}".FormatWith(ex.Message), parameters);
                return null;
            }
        }

        // GET CREDENTIAL BY FORGOT PASSWORD TOKEN
        public Credential GetCredentialByForgotPasswordToken(String token)
        {
            try
            {
                return db.SingleOrDefault<Credential>(" WHERE FORGOT_PASSWORD_TOKEN = @0 ", token);
            }
            catch (Exception ex)
            {
                RevoContextHelpers.GetCurrentRevoContext().ContextLogger.ErrorEx(ex, "I had an error trying to recover the credential by the forgotten password token");
                return null;
            }
        }

        // GET CREDENTIAL BY ACCOUNT
        public ISubCollection<Credential> GetCredentialsByAccount(String userId)
        {
            try
            {
                return db.Query<Credential>(" WHERE USER_ID = @0 ", userId).ToSubCollection<Credential>();
            }
            catch (Exception ex)
            {
                Object[] parameters = new Object[] { userId };
                RevoContextHelpers.GetCurrentRevoContext().ContextLogger.ErrorEx(ex, "I had an error trying to recover the credentials by user id with this message: {0}".FormatWith(ex.Message), parameters);
                return null;
            }
        }
    }
}
