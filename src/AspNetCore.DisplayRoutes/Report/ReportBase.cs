using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.DisplayRoutes.Report {
    public abstract class ReportBase {
        protected EndpointDataSource _endpointDataSource;


        public ReportBase(EndpointDataSource endpointDataSource) {
            _endpointDataSource = endpointDataSource;
        }
        protected virtual IEnumerable<string> GetHeaderNames<T>() {
            return typeof(T).GetProperties().Select(p => p.Name).ToArray();
        }
        public abstract IList<string[]> Rows { get; }
        public abstract string[] HeaderNames { get; }

    }
}
