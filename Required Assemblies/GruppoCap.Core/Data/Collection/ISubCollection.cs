using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Data
{
    public interface ISubCollection
    {
        // ITEMS
        ICollection Items { get; }

        // INFO
        ISubCollectionInfo Info { get; }
    }

    public interface ISubCollection<T>
    {
        // ITEMS
        ICollection<T> Items { get; }

        // INFO
        ISubCollectionInfo Info { get; }
    }
}
