using System.Web.Http.Filters;

namespace GruppoCap.Core.Api.Filters
{
    public interface IOrderedFilter : IFilter
    {
        int Order { get; set; }
    }
}
