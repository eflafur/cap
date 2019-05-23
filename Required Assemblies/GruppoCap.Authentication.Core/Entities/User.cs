using GruppoCap.Core;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace GruppoCap.Authentication.Core
{
    [TableName("REVO_AUTH_USERS")]
    [PrimaryKey("USER_ID", autoIncrement = false)]
    public class User : IUser
    {
        public User()
        {
            // EMPTY
        }

        #region IEntity

        [Ignore]
        public Object EntityId
        {
            get { return UserId; }
        }

        [Ignore]
        public String DisplayText
        {
            get { return "{0} {1}".FormatWith(FirstName, LastName); }
        }

        #endregion

        [Column("USER_ID")] 
        public String UserId { get; set; }

        [Column("FIRST_NAME")] 
        public String FirstName { get; set; }

        [Column("LAST_NAME")] 
        public String LastName { get; set; }

        [Column("DOMAIN")] 
        public String Domain { get; set; }

        [Column("COMPANY_ID")]
        public Company Company { get; set; }

        [Column("IS_ACTIVE")] 
        public Boolean IsActive { get; set; }

        [Column("IS_PRIVILEGED")]
        public Boolean IsPrivileged { get; set; }

        [Ignore]
        public IList<ICredential> Credentials { get; set; }

        [Ignore]
        public IList<String> GroupingIds { get; set; }

        [Ignore]
        public IList<String> GroupingCodes { get; set; }

        [Ignore]
        public String MainGroupingCode { get; set; }

        [Ignore]
        public String MainGroupingId { get; set; }

        [Ignore]
        public IList<Company> TrustedCompanies { get; set; }

        [Ignore]
        public IList<Int32> TrustedCompanyIds { get; set; }


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

    }
}
