using System;

namespace GruppoCap.Core
{
    public interface ITrackedEntity : IEntity
    {
        // CREATION'S (MOMENT + USER ID)
        DateTime CreationMoment { get; set; }
        String CreationUserId { get; set; }

        // LAST UPDATE'S (MOMENT + USER ID)
        DateTime? LastUpdateMoment { get; set; }
        String LastUpdateUserId { get; set; }
    }
}
