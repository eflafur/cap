using System.Web.Http.Filters;

namespace GruppoCap.Core.Api.Filters
{
    public class ActionFilterWithOrderAttribute : ActionFilterAttribute, IOrderedFilter
    {
        public int Order { get; set; }
    }
}
