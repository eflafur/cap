using System.Web.Http.Filters;

namespace GruppoCap.Core.Api.Filters
{
    public class ExceptionFilterWithOrderAttribute : ExceptionFilterAttribute, IOrderedFilter
    {
        public int Order { get; set; }
    }
}
