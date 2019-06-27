using GruppoCap.Core;
using PetaPoco;
using System;

namespace GruppoCap.Authentication.Core
{
    [TableName("REVO_AUTH_APPLICATIONS")]
    [PrimaryKey("APPLICATION_ID", autoIncrement = false)]
    public class Application : IApplication
    {
        public Application()
        {
            // EMPTY
        }

        [Column("APPLICATION_ID")]
        public String ApplicationId { get; set; }

        public String Description { get; set; }

        [Column("IS_ACTIVE")]
        public Boolean IsActive { get; set; }

        [Column("IS_IN_MAINTENANCE")]
        public Boolean IsInMaintenance { get; set; }

        [Column("MAINTENANCE_WARNING")]
        public String MaintenanceWarning { get; set; }


        //[Ignore]
        //public String InternalUrl { get; set; }

        //[Ignore]
        //public String ExternalUrl { get; set; }

        [Column("INTERNAL_URL")]
        public String InternalUrl { get; set; }

        [Column("EXTERNAL_URL")]
        public String ExternalUrl { get; set; }



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
            get { return this.ApplicationId; }
        }

        [Ignore]
        public String DisplayText
        {
            get { return this.Description; }
        }

        #endregion
    }
}
