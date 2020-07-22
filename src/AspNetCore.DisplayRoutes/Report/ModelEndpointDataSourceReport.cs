using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AspNetCore.DisplayRoutes.Report {

    public class ModelEndpointDataSourceReport : ReportBase {

        public class RouteInfo {
            public string DisplayName { get; set; }
            public string RoutePattern { get; set; }
            public string HttpMethods { get; set; }
            public string Meta { get; set; }
        }

        private readonly IList<string[]> _rows = new List<string[]>();

        private static string ExtractMetadata(EndpointMetadataCollection epMetaCollection) {

            var metaFiltered = epMetaCollection.Where(m => !(new[] { typeof(System.Diagnostics.DebuggerStepThroughAttribute) }.Contains(m.GetType())))
                                               .Where(t => (!t.GetType().FullName.StartsWith("System.Runtime.CompilerServices.")) && t.GetType().GetProperties().Count() > 0)
                                               .GroupBy(m => m.GetType()).Select(group => group.First());

            string metaFallback;
            int maxDepth = 2;
            using (var strWriter = new StringWriter()) {
                using (var jsonWriter = new Json.DepthCountingJsonTextWriter(strWriter)) {
                    bool include() => jsonWriter.CurrentDepth <= maxDepth;
                    var resolver = new Json.DepthLimitedContractResolver(include);
                    var jsonSerializerSettings = new JsonSerializerSettings() {
                        ContractResolver = resolver,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented,
                        Converters = new[] { new Json.ShortTypeNameJsonConverter(typeof(System.Type), typeof(System.Reflection.Module), typeof(System.Reflection.MethodInfo)) }
                    };

                    var serializer = JsonSerializer.CreateDefault(jsonSerializerSettings);
                    serializer.Serialize(jsonWriter, metaFiltered);
                }
                metaFallback = strWriter.ToString();
            }

            return metaFallback;
        }

        public ModelEndpointDataSourceReport(EndpointDataSource endpointDataSource) : base(endpointDataSource) {
            foreach (var endpoint in endpointDataSource.Endpoints.OrderBy(ep => (ep as RouteEndpoint)?.Order ?? -1)) {
                var meta = ExtractMetadata(endpoint.Metadata);
                var routeInfo = new RouteInfo() {
                    DisplayName = endpoint.DisplayName,
                    Meta = meta,
                    RoutePattern = (endpoint as RouteEndpoint)?.RoutePattern.RawText.ToString(),
                    HttpMethods = string.Join(",", endpoint.Metadata.OfType<HttpMethodMetadata>().SelectMany(h => h.HttpMethods).Distinct())
                };

                var values = typeof(RouteInfo).GetProperties().Select(p => p.GetValue(routeInfo)?.ToString() ?? string.Empty).ToArray();
                _rows.Add(values);
            }
        }

        public override IList<string[]> Rows => _rows;

        public override string[] HeaderNames => base.GetHeaderNames<RouteInfo>().ToArray();
    }
}
