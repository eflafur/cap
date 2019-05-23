using System;

namespace GruppoCap.Core
{
    public interface IOrderableEntity : IEntity
    {
        Int32 Position { get; set; }
    }
}
