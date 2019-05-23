using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Data
{
    public interface ISubCollectionInfo
    {
        Int32 CurrentPage { get; set; }
        Int32 ItemsPerPage { get; set; }
        Int32 TotalPages { get; set; }
        Int32 TotalItems { get; set; }
    }
}
