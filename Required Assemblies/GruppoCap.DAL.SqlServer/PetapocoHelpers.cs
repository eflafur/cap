using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using GruppoCap;
using System.Data;

namespace GruppoCap.DAL.SqlServer
{
    public static class PetapocoHelpers
    {
        // TO SUB COLLECTION
        public static ISubCollection<T> ToSubCollection<T>(this Page<T> pageResult)
        {
            if (pageResult.Items.HasValues() == false)
            {
                return SubCollection<T>.CreateEmptyCollection();
            }

            return new SubCollection<T>(
                pageResult.Items,
                new SubCollectionInfo(
                    currentPage: pageResult.CurrentPage.CoerceTo<Int32>(),
                    itemsPerPage: pageResult.ItemsPerPage.CoerceTo<Int32>(),
                    totalPages: pageResult.TotalPages.CoerceTo<Int32>(),
                    totalItems: pageResult.TotalItems.CoerceTo<Int32>()
                )
            );
        }

        // TO SUB COLLECTION
        public static ISubCollection<T> ToSubCollection<T>(this IEnumerable<T> result)
        {
            if (result.HasValues() == false)
            {
                return SubCollection<T>.CreateEmptyCollection();
            }

            return new SubCollection<T>(
                result,
                new SubCollectionInfo(
                    currentPage: 1,
                    itemsPerPage: result.Count(),
                    totalPages: 1,
                    totalItems: result.Count()
                )
            );
        }
    }
}
