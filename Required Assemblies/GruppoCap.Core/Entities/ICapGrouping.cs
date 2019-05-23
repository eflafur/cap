using System;

namespace GruppoCap.Core
{
    public interface ICapGrouping: ITrackedEntity
    {
        String CapGroupingId { get; set; }
        String CapGroupingCode { get; set; }
        String ApplicationId { get; set; }
        String Description { get; set; }
        Boolean IsActive { get; set; }
    }
}
