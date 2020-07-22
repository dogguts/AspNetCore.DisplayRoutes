using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.DisplayRoutes {
    public static class Routes {
        private static string GetReport(Render.RenderBase renderer, ICollection<EndpointDataSource> dataSources) {
            var fullReport = new Dictionary<string, Report.ReportBase>();

            foreach (var dataSource in dataSources) {
                Report.ReportBase report = dataSource.GetType().FullName switch
                {
                    "Microsoft.AspNetCore.Routing.ModelEndpointDataSource" => new Report.ModelEndpointDataSourceReport(dataSource),
                    "Microsoft.AspNetCore.Mvc.Routing.ControllerActionEndpointDataSource" => new Report.ControllerActionEndpointDataSourceReport(dataSource),
                    _ => new Report.ControllerActionEndpointDataSourceReport(dataSource)
                };
                fullReport.Add(dataSource.ToString(), report);
            }

            return renderer.Render(fullReport).ToString();
        }

        public static IEndpointConventionBuilder PrintRoutes(this IEndpointRouteBuilder routeBuilder, Action<DisplayRouteOptions> setupAction = null) {
            var options = new DisplayRouteOptions();
            setupAction?.Invoke(options);

            var builder = routeBuilder.Map(RoutePatternFactory.Parse(options.Pattern), async context => {
                context.Response.ContentType = options.Renderer.ContentType;
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] = "no-cache";

                await context.Response.WriteAsync(GetReport(options.Renderer, routeBuilder.DataSources), Encoding.UTF8);
            });
            builder.WithDisplayName($"{options.Pattern} HTTP: {string.Join(", ", new[] { "GET" })}");
            builder.WithMetadata(options, (new HttpMethodMetadata(options.HttpMethods)));
            return builder;
        }

        public static IEndpointConventionBuilder PrintRoutes(this IEndpointRouteBuilder routeBuilder, PathString pathString) {
            return PrintRoutes(routeBuilder, c => {
                c.Pattern = pathString;
            });
        }
    }
}
