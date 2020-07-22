using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.DisplayRoutes.Report.Json {
    // Based on https://stackoverflow.com/a/10454062 
    public class DepthCountingJsonTextWriter : JsonTextWriter {
        public DepthCountingJsonTextWriter(TextWriter textWriter) : base(textWriter) { }

        public int CurrentDepth { get; private set; }

        public override void WriteStartObject() {
            CurrentDepth++;
            base.WriteStartObject();
        }

        public override void WriteEndObject() {
            CurrentDepth--;
            base.WriteEndObject();
        }
    }
}
