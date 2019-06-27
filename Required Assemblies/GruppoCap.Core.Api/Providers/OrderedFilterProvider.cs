using GruppoCap.Core.Api.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GruppoCap.Core.Api.Providers
{
    public class OrderedFilterProvider : IFilterProvider
    {
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            // controller-specific
            IEnumerable<FilterInfo> controllerSpecificFilters = OrderFilters(actionDescriptor.ControllerDescriptor.GetFilters(), FilterScope.Controller);

            // action-specific
            IEnumerable<FilterInfo> actionSpecificFilters = OrderFilters(actionDescriptor.GetFilters(), FilterScope.Action);

            return controllerSpecificFilters.Concat(actionSpecificFilters);
        }

        private IEnumerable<FilterInfo> OrderFilters(IEnumerable<IFilter> filters, FilterScope scope)
        {
            return filters.OfType<IOrderedFilter>()
                            .OrderBy(filter => filter.Order)
                            .Select(instance => new FilterInfo(instance, scope));
        }
    }
}
