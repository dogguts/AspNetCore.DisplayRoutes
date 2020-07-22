using AspNetCore.DisplayRoutes.Report;
using System.Text;

namespace AspNetCore.DisplayRoutes.Render  {
    public class UnicodeBoxRenderer : TextBaseRenderer {
        protected override StringBuilder Render(string title) {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine("".PadLeft(title.Length, '┅'));
            return sb;
        }

        protected override StringBuilder Render(ReportBase report) {
            var (columnWidth, rowHeight) = CalculateSizing(report);
            var sb = new System.Text.StringBuilder();
            /* *** HEADER ***  */
            // first headerrow
            sb.Append("┏");
            for (int i = 0; i < columnWidth.Count; i++) {
                sb.Append("".PadLeft(columnWidth[i], '━'));
                if (i < (columnWidth.Count - 1)) {
                    sb.Append("┳");
                } else { sb.Append("┓"); }
            }
            sb.AppendLine();

            // header names row
            sb.Append("┃");
            for (int i = 0; i < columnWidth.Count; i++) {
                sb.Append(report.HeaderNames[i].PadRight(columnWidth[i]));
                sb.Append("┃");
            }
            sb.AppendLine();

            // row below header names
            // first headerrow
            sb.Append("┡");
            for (int i = 0; i < columnWidth.Count; i++) {
                sb.Append("".PadLeft(columnWidth[i], '━'));
                if (i < (columnWidth.Count - 1)) {
                    sb.Append("╇");
                } else { sb.Append("┩"); }
            }
            sb.AppendLine();
            /* *** BODY ***  */

            // datarows
            for (int rowNr = 0; rowNr < report.Rows.Count; rowNr++) {
                var row = report.Rows[rowNr];
                var isLastRow = (rowNr + 1 >= report.Rows.Count);

                for (int lineNr = 0; lineNr < rowHeight[rowNr]; lineNr++) {
                    for (int colNr = 0; colNr < row.Length; colNr++) {
                        sb.Append("│");
                        var col = row[colNr];
                        var lines = GetLines(col);
                        if (lineNr < lines.Length) {
                            sb.Append(lines[lineNr].PadRight(columnWidth[colNr]));
                        } else { sb.Append("".PadRight(columnWidth[colNr])); }
                    }
                    sb.AppendLine("│");
                }
                // (multi-)row separator
                sb.Append(isLastRow ? "└" : "├");

                for (int i = 0; i < columnWidth.Count; i++) {
                    sb.Append("".PadLeft(columnWidth[i], '─'));
                    if (i < (columnWidth.Count - 1)) {
                        sb.Append(isLastRow ? "┴" : "┼");
                    } else { sb.Append(isLastRow ? "┘" : "┤"); }//
                }
                sb.AppendLine();
            }
            sb.AppendLine();
            return sb;
        }



    }
}
