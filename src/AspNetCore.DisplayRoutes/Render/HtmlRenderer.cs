using AspNetCore.DisplayRoutes.Report;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspNetCore.DisplayRoutes.Render {

    public class HtmlRenderer : RenderBase {
        public override string ContentType => "text/html; charset=UTF-8";

        protected override StringBuilder Render(string title) {
            return new StringBuilder($"<h3>{WebUtility.HtmlEncode(title)}</h3>");
        }

        private static string PageHeader() => "<html>" + System.Environment.NewLine +
                                              " <head>" + System.Environment.NewLine +
                                              "  <style>" + System.Environment.NewLine +
                                              "   TD { " + System.Environment.NewLine +
                                              "    vertical-align: top; " + System.Environment.NewLine +
                                              "   }" + System.Environment.NewLine +
                                              "  </style>" + System.Environment.NewLine +
                                              " </head>" + System.Environment.NewLine +
                                              " <body>" + System.Environment.NewLine;

        private static string PageFooter() => " </body>" + System.Environment.NewLine +
                                              "</html>" + System.Environment.NewLine;

        protected override StringBuilder Render(ReportBase report) {
            //header
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("  <table border=\"1\">");
            sb.AppendLine("   <thead>");
            sb.AppendLine("    <tr>");
            foreach (string headerName in report.HeaderNames) {
                sb.AppendLine($"     <th>{WebUtility.HtmlEncode(headerName)}</th>");
            }
            sb.AppendLine("    </tr>");
            sb.AppendLine("   </thead>");
            sb.AppendLine("   <tbody>");


            foreach (string[] row in report.Rows) {
                sb.AppendLine("    <tr>");
                foreach (string cell in row) {
                    var htmlCell = WebUtility.HtmlEncode(cell).Replace("\r\n", "<br/>");
                    sb.AppendLine($"     <td><pre>{htmlCell}</pre></td>");
                }
                sb.AppendLine("    </tr>");
            }
            sb.AppendLine("   </tbody>");
            sb.AppendLine("  </table>");

            return sb;
        }

        public override StringBuilder Render(IDictionary<string, ReportBase> reports) {
            var ret = new System.Text.StringBuilder();
            ret.Append(PageHeader());
            foreach (var kvp in reports) {
                ret.Append(Render(kvp.Key));
                ret.Append(Render(kvp.Value));
            }
            ret.Append(PageFooter());
            return ret;
        }

    }
}
