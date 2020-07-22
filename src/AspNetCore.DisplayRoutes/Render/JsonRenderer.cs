using AspNetCore.DisplayRoutes.Report;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.DisplayRoutes.Render {
    public class JsonRenderer : RenderBase {

        //private class JsonReportObject {
        //    public string Title { get; set; }
        //    public IEnumerable<object> Data { get; set; }
        //}

        public override string ContentType => "application/json";

        protected override StringBuilder Render(string title) {
            throw new NotImplementedException();
        }

        protected override StringBuilder Render(ReportBase report) {
            throw new NotImplementedException();
        }

        public override StringBuilder Render(IDictionary<string, ReportBase> reports) {
            var jsonSerializerSettings = new JsonSerializerSettings() { Formatting = Formatting.Indented };

            var json = new JObject(
                            new JProperty("routes",
                                new JArray(
                                        reports.Select(kvp => new JObject(new JProperty(kvp.Key, new JArray(
                                            kvp.Value.Rows.Select(r => new JObject(
                                                new JObject(Enumerable.Range(0, r.Length).Select(i => new JProperty(kvp.Value.HeaderNames[i], r[i])))
                                            ))
                                        ))))
                                )
                            )
            );

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb)) {
                using (var jsonWriter = new JsonTextWriter(writer)) {
                    JsonSerializer.Create(jsonSerializerSettings).Serialize(jsonWriter, json);
                }
            }
            return sb;


            //return JsonConvert.SerializeObject(json, jsonSerializerSettings);
        }
    }
}
