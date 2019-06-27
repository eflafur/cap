using System.Web.Http;

namespace GruppoCap.Core.Api.Filters
{
    public class AuthorizationFilterWithOrderAttribute : AuthorizeAttribute, IOrderedFilter
    {
        public int Order { get; set; }
    }
}
