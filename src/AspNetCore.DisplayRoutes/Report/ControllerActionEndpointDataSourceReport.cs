
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.DisplayRoutes.Report {
    public class ControllerActionEndpointDataSourceReport : ReportBase {

        public class RouteInfo {
            public string RouteName { get; set; }
            public string RoutePattern { get; set; }
            public string HttpMethods { get; set; }
            public string Area { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
        }
        private readonly IList<string[]> _rows = new List<string[]>();

        public ControllerActionEndpointDataSourceReport(EndpointDataSource endpointDataSource) : base(endpointDataSource) {

            foreach (var endpoint in endpointDataSource.Endpoints.OrderBy(ep => (ep as RouteEndpoint)?.Order ?? -1)) {
                var routeInfo = new RouteInfo() {
                    RoutePattern = (endpoint as RouteEndpoint)?.RoutePattern.RawText.ToString(),
                    HttpMethods = string.Join(",", endpoint.Metadata.OfType<HttpMethodMetadata>().SelectMany(h => h.HttpMethods).Distinct())
                };
                //.SelectMany(h => h.HttpMethods).Distinct()

                var controllerActionDescriptor = endpoint.Metadata.OfType<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>().SingleOrDefault();
                if (controllerActionDescriptor != null) {
                    routeInfo.Controller = controllerActionDescriptor.ControllerTypeInfo.FullName;
                    routeInfo.Area = controllerActionDescriptor.RouteValues.SingleOrDefault(rv => rv.Key == "area").Value ?? "-";
                    routeInfo.Action = controllerActionDescriptor.ActionName;
                }

                var routeNameMetadata = endpoint.Metadata.OfType<Microsoft.AspNetCore.Routing.RouteNameMetadata>().SingleOrDefault();
                routeInfo.RouteName = routeNameMetadata?.RouteName ?? "-";

                _rows.Add(typeof(RouteInfo).GetProperties().Select(p => p.GetValue(routeInfo)?.ToString() ?? string.Empty).ToArray());

            }

        }

        public override IList<string[]> Rows => _rows;

        public override string[] HeaderNames => base.GetHeaderNames<RouteInfo>().ToArray();
    }
}
