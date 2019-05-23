using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Data
{
    public class SubCollection<T>
        : ISubCollection<T>
        , ISubCollection
    {

        // PRIVATE MEMBERs
        private ICollection<T> _items = null;
        private ISubCollectionInfo _info = null;

        #region " CTORs "

        // CTOR
        public SubCollection(ICollection<T> items, ISubCollectionInfo info)
        {
            this._items = items;
            this._info = info;
        }

        // CTOR
        public SubCollection(IEnumerable<T> items, ISubCollectionInfo info)
        {
            this._items = new List<T>(items);
            this._info = info;
        }

        // CTOR
        public SubCollection(ICollection<T> items)
        {
            this._items = items;
            this._info = new SubCollectionInfo(items.Count);
        }

        #endregion

        #region ISubCollection<T> Members

        // ITEMS
        public ICollection<T> Items
        {
            get { return this._items; }
        }

        // INFO
        public ISubCollectionInfo Info
        {
            get { return this._info; }
        }

        #endregion

        #region ISubCollection Members

        // ITEMS
        ICollection ISubCollection.Items
        {
            get { return this._items as ICollection; }
        }

        // INFO
        ISubCollectionInfo ISubCollection.Info
        {
            get { return this._info; }
        }

        #endregion

        // CREATE EMPTY COLLECTION
        public static SubCollection<T> CreateEmptyCollection()
        {
            return new SubCollection<T>(new Collection<T>());
        }

    }
}
