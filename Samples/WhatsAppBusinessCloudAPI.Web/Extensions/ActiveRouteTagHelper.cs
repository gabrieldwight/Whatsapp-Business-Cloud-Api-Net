using Microsoft.AspNetCore.Mvc.Rendering;

namespace WhatsAppBusinessCloudAPI.Web.Extensions
{
    public static class ActiveRouteTagHelper
    {
        public static string IsActive(this IHtmlHelper html, string controller, string action, string cssClass = "active")
        {
            var routeData = html.ViewContext.RouteData;

            var currentRouteAction = routeData.Values["action"] as string;
            var currentRouteController = routeData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (action ?? currentRouteAction).Split(',');
            IEnumerable<string> acceptedControllers = (controller ?? currentRouteController).Split(',');

            return acceptedActions.Contains(currentRouteAction) && acceptedControllers.Contains(currentRouteController) ?
                cssClass : string.Empty;
        }

        public static string IsActive(this IHtmlHelper html, string controller)
        {
            string cssClass = "active";
            var routeData = html.ViewContext.RouteData;

            var currentRouteController = routeData.Values["controller"] as string;

            IEnumerable<string> acceptedControllers = (controller ?? currentRouteController).Split(',');

            return acceptedControllers.Contains(currentRouteController) ?
                cssClass : string.Empty;
        }
    }
}
