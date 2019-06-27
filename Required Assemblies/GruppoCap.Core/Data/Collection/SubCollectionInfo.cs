using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Data
{
    public class SubCollectionInfo : ISubCollectionInfo
    {
        #region " CTORs "

        // CTOR
        public SubCollectionInfo(Int32 totalItems)
        {
            TotalItems = totalItems;
        }

        // CTOR
        public SubCollectionInfo(Int32 currentPage, Int32 itemsPerPage, Int32 totalPages, Int32 totalItems)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        #endregion

        public Int32 CurrentPage { get; set; }

        public Int32 ItemsPerPage { get; set; }

        public Int32 TotalPages { get; set; }

        public Int32 TotalItems { get; set; }


    }
}
