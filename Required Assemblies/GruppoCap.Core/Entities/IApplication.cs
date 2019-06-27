using System;

namespace GruppoCap.Core
{
    public interface IApplication : ITrackedEntity
    {
        String ApplicationId { get; set; }
        String Description { get; set; }
        Boolean IsActive { get; set; }
        Boolean IsInMaintenance { get; set; }
        String MaintenanceWarning { get; set; }

        String InternalUrl { get; set; }
        String ExternalUrl { get; set; }
    }
}
