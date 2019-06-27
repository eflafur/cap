using GruppoCap.Core;
using PetaPoco;
using System;
using GruppoCap;

namespace GruppoCap.Activity.Core
{
    [TableName("REVO_ACTIVITY")]
    [PrimaryKey("ACTIVITY_ID", autoIncrement = false)]
    public class Activity : IActivity
    {
        public Activity()
        {
            ActivityId = Guid.NewGuid().ToString();
            Moment = DateTime.Now;
        }

        #region IEntity

        [Ignore]
        public Object EntityId
        {
            get { return ActivityId; }
        }

        [Ignore]
        public String DisplayText
        {
            get {
                return ActivityId.ToString();
            }
        }

        #endregion

        [Column("ACTIVITY_ID")] 
        public String ActivityId { get; set; }

        [Column("ACTOR_ENTITY_ID")] 
        public String ActorEntityId { get; set; }

        [Column("ACTOR_ENTITY_TEXT")] 
        public String ActorEntityDisplayText { get; set; }

        [Column("IMPERSONATED_ENTITY_ID")]
        public String ImpersonatedEntityId { get; set; }

        [Column("IMPERSONATED_ENTITY_TEXT")]
        public String ImpersonatedEntityDisplayText { get; set; }

        public ActivityVerb Verb { get; set; }

        [Column("OBJECT_ENTITY_TYPE")]
        public String ObjectEntityType { get; set; }

        [Column("OBJECT_ENTITY_ID")]
        public String ObjectEntityId { get; set; }

        [Column("OBJECT_ENTITY_TEXT")]
        public String ObjectEntityDisplayText { get; set; }

        [Column("RELATED_ENTITY_TYPE")]
        public String RelatedEntityType { get; set; }

        [Column("RELATED_ENTITY_ID")]
        public String RelatedEntityId { get; set; }

        [Column("RELATED_ENTITY_TEXT")]
        public String RelatedEntityDisplayText { get; set; }

        public DateTime Moment { get; set; }

        [Column("COMPANY_ID")]
        public Company Company { get; set; }

        [Column("IS_PRIVILEGED")]
        public Boolean IsPrivileged { get; set; }

        [Column("IP_ADDRESS")]
        public String IPAddress { get; set; }


        // SETUP ACTOR
        public void SetupActor(IUser user)
        {
            ActorEntityId = user.UserId;
            ActorEntityDisplayText = user.DisplayText;
            Company = user.Company;
            IsPrivileged = user.IsPrivileged;
        }

        public void SetupActor(IRevoWebRequest req)
        {
            ActorEntityId = req.CurrentUser != null ? req.CurrentUser.UserId : req.CurrentUsername;
            ActorEntityDisplayText = req.CurrentUser != null ? req.CurrentUser.DisplayText : req.CurrentUsername;
            Company = req.CurrentUser != null ? req.CurrentUser.Company : Company.CapHolding;
            IsPrivileged = req.CurrentUser != null ? req.CurrentUser.IsPrivileged : false;
            IPAddress = req.CurrentIPAddress;
        }
    }
}
