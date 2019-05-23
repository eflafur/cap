using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core
{
    public interface IRevoService
    {
        // EMPTY
    }

    public interface IRevoService<T>
        where T : class, IEntity
    {
        
    }
}
