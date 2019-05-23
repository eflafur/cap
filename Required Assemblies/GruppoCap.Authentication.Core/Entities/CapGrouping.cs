using GruppoCap.Core;
using PetaPoco;
using System;

namespace GruppoCap.Authentication.Core
{
    [TableName("REVO_AUTH_CAPGROUPINGS")]
    [PrimaryKey("CAPGROUPING_ID", autoIncrement = false)]
    public class CapGrouping : ICapGrouping
    {
        public CapGrouping()
        {
            CapGroupingId = Guid.NewGuid().ToString();
            IsActive = false;
        }

        [Column("CAPGROUPING_ID")]
        public String CapGroupingId { get; set; }

        [Column("CAPGROUPING_CODE")]
        public String CapGroupingCode { get; set; }

        [Column("APPLICATION_ID")]
        public String ApplicationId { get; set; }

        public String Description { get; set; }

        [Column("IS_ACTIVE")]
        public Boolean IsActive { get; set; }

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

        #region IEntity

        [Ignore]
        public Object EntityId
        {
            get { return this.CapGroupingId; }
        }

        [Ignore]
        public String DisplayText
        {
            get { return this.Description; }
        }

        #endregion
    }
}
