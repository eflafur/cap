using GruppoCap.Core;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace GruppoCap.Authentication.Core
{
    [TableName("REVO_AUTH_CREDENTIALS")]
    [PrimaryKey("EMAIL", autoIncrement = false)]
    public class Credential : ICredential
    {
        // CTORs
        public Credential()
        {
            // EMPTY
        }

        // CTORs
        public Credential(String userId)
        {
            UserId = userId;
        }

        [Column("USER_ID")] 
        public String UserId { get; set; }

        [Column("EMAIL")] 
        public String EMail { get; set; }

        [Column("PASSWORD_HASH")] 
        public String PasswordHash { get; set; }

        [Column("FORGOT_PASSWORD_TOKEN")]
        public String ForgotPasswordToken { get; set; }

        [Column("FORGOT_PASSWORD_TOKEN_MOMENT")]
        public DateTime? ForgotPasswordTokenMoment { get; set; }

        [Column("IS_ACTIVE")] 
        public Boolean IsActive { get; set; }

        #region IEntity

        [Ignore]
        public Object EntityId
        {
            get { return EMail; }
        }

        [Ignore]
        public String DisplayText
        {
            get { return EMail; }
        }

        #endregion

        #region ITrackedEntity

        [Column("CREATION_MOMENT")]
        public DateTime CreationMoment { get; set; }

        [Column("CREATION_USER_ID")]
        public String CreationUserId { get; set; }

        [Column("LAST_UPDATE_MOMENT")]
        public DateTime? LastUpdateMoment { get; set; }

        [Column("LAST_UPDATE_USER_ID")]
        public String LastUpdateUserId { get; set; }

        #endregion

        // GENERATE PASSWORD HASH
        public static String GeneratePasswordHash(String password)
        {
            return StringUtils.GenerateSHA256Hash(password);
        }

        // SET PASSWORD
        public String SetPassword(String password)
        {
            PasswordHash = GeneratePasswordHash(password);
            return PasswordHash;
        }
        
    }
}
