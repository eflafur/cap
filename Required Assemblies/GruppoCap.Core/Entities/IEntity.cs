using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IEntity
    {
        Object EntityId { get; }
        String DisplayText { get;  }
    }
}
