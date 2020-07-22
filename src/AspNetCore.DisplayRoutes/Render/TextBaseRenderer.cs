using AspNetCore.DisplayRoutes.Report;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.DisplayRoutes.Render {
    public abstract class TextBaseRenderer : RenderBase {
        public override string ContentType => "text/plain; charset=UTF-8";

        protected static string[] GetLines(string text) {
            return text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected (IList<int> ColumnWidth, List<int> RowHeight) CalculateSizing(ReportBase report) {
            var columnWidth = new List<int>();
            var rowHeight = new List<int>();

            foreach (var headerCol in report.HeaderNames) {
                int headerLen = GetLines(headerCol).Max(s => s.Length);
                columnWidth.Add(headerLen);
            }

            for (var rowNr = 0; rowNr < report.Rows.Count(); rowNr++) {
                var row = report.Rows[rowNr];
                rowHeight.Add(0);
                for (var colNr = 0; colNr < row.Length; colNr++) {
                    var cellLines = GetLines(row[colNr]);
                    columnWidth[colNr] = Math.Max(columnWidth[colNr], cellLines.Any() ? cellLines.Max(s => s.Length) : 0);
                    rowHeight[rowNr] = Math.Max(rowHeight[rowNr], cellLines.Length);
                }
            }
            return (columnWidth, rowHeight);
        }
    }
}
