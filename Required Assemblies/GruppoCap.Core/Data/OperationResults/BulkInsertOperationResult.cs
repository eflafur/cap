using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruppoCap.Core
{
    public class BulkInsertOperationResult
        : OperationResult
        , IBulkInsertOperationResult
    {

        // PRIVATE MEMBERs
        private Int64 _RowsEffected;

        #region " CTORs "

        // CTOR
        public BulkInsertOperationResult(Int64 rowsEffected, Boolean genericMeaning)
            : base(genericMeaning)
        {
            this._RowsEffected = rowsEffected;
        }

        // CTOR
        public BulkInsertOperationResult(Int64 rowsEffected, Boolean genericMeaning, String description)
            : this(rowsEffected, genericMeaning)
        {
            this._Description = description;
        }

        // CTOR
        public BulkInsertOperationResult(Int64 rowsEffected, Boolean genericMeaning, String description, String[] warnings)
            : this(rowsEffected, genericMeaning, description)
        {
            this._Warnings = warnings;
        }

        #endregion

        #region IInsertOperationResult Members

        // CREATED ENTITY ID
        public Int64 RowsEffected
        {
            get { return this._RowsEffected; }
        }

        #endregion

    }
}
