using System;

namespace GruppoCap.Core
{
    public interface IActivity : IEntity
    {
        String ActivityId { get; set; }
        String ActorEntityId { get; set; }
        String ActorEntityDisplayText { get; set; }
        String ImpersonatedEntityId { get; set; }
        String ImpersonatedEntityDisplayText { get; set; }
        ActivityVerb Verb { get; set; }
        String ObjectEntityType { get; set; }
        String ObjectEntityId { get; set; }
        String ObjectEntityDisplayText { get; set; }
        String RelatedEntityType { get; set; }
        String RelatedEntityId { get; set; }
        String RelatedEntityDisplayText { get; set; }
        DateTime Moment { get; set; }
        Company Company { get; set; }
        Boolean IsPrivileged { get; set; }
        String IPAddress { get; set; }

        void SetupActor(IUser user);
        void SetupActor(IRevoWebRequest req);
    }
}
