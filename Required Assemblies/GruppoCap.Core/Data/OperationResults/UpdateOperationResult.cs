using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GruppoCap.Core
{
    public class UpdateOperationResult
        : OperationResult
        , IUpdateOperationResult
    {

        #region " CTORs "

        // CTOR
        public UpdateOperationResult(Boolean genericMeaning)
            : base(genericMeaning)
        {
        }

        // CTOR
        public UpdateOperationResult(Boolean genericMeaning, String description)
            : this(genericMeaning)
        {
            this._Description = description;
        }

        // CTOR
        public UpdateOperationResult(Boolean genericMeaning, String description, String[] warnings)
            : this(genericMeaning, description)
        {
            this._Warnings = warnings;
        }

        #endregion

    }
}
