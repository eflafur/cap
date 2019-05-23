using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruppoCap.Core
{
    public class InsertOperationResult
        : OperationResult
        , IInsertOperationResult
    {

        // PRIVATE MEMBERs
        private Object _CreatedEntityId;

        #region " CTORs "

        // CTOR
        public InsertOperationResult(Object createdEntityId, Boolean genericMeaning)
            : base(genericMeaning)
        {
            this._CreatedEntityId = createdEntityId;
        }

        // CTOR
        public InsertOperationResult(Object createdEntityId, Boolean genericMeaning, String description)
            : this(createdEntityId, genericMeaning)
        {
            this._Description = description;
        }

        // CTOR
        public InsertOperationResult(Object createdEntityId, Boolean genericMeaning, String description, String[] warnings)
            : this(createdEntityId, genericMeaning, description)
        {
            this._Warnings = warnings;
        }

        #endregion

        #region IInsertOperationResult Members

        // CREATED ENTITY ID
        public Object CreatedEntityId
        {
            get { return this._CreatedEntityId; }
        }

        #endregion

    }
}
