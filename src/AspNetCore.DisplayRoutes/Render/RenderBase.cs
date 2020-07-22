using AspNetCore.DisplayRoutes.Report;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.DisplayRoutes.Render {
    public abstract class RenderBase {
        public abstract string ContentType { get; }
        public virtual string Type => this.GetType().Name;

        protected abstract StringBuilder Render(string title);
        protected abstract StringBuilder Render(ReportBase report);

        public virtual StringBuilder Render(IDictionary<string, Report.ReportBase> reports) {
            var ret = new StringBuilder();

            foreach (var kvp in reports) {
                ret.Append(Render(kvp.Key));
                ret.Append(Render(kvp.Value));
            }
            return ret;
        }
    }
}
