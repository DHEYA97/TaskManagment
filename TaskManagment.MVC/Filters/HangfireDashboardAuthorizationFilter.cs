using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagment.Mvc.Filters
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _policyName;
        public HangfireDashboardAuthorizationFilter(string policyName)
        {
            _policyName = policyName;
        }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var isAuth = authService.AuthorizeAsync(httpContext.User, _policyName)
                                    .ConfigureAwait(false)
                                    .GetAwaiter()
                                    .GetResult()
                                    .Succeeded;
            return isAuth;
        }
    }
}
