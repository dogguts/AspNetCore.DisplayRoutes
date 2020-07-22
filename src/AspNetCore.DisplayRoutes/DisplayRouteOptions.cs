using System.Collections.Generic;
using AspNetCore.DisplayRoutes.Render;

namespace AspNetCore.DisplayRoutes {
    public class DisplayRouteOptions {

        public DisplayRouteOptions() { }

        /// <summary>The route pattern. Default: "/routes"</summary>
        public string Pattern { get; set; } = "/routes";
        /// <summary>The output renderer. Default "UnicodeBoxRenderer"</summary>
        public RenderBase Renderer { get; set; } = new UnicodeBoxRenderer();

        /// <summary>Allowed HTTP methods. Default "GET"</summary>
        public IEnumerable<string> HttpMethods = new[] { "GET" };

        public override string ToString() {
            return $"Pattern: {Pattern}\r\n" +
                   $"Renderer: {Renderer.GetType().Name}\r\n" +
                   $"HttpMethods: {string.Join(",", HttpMethods)}";
        }
    }
}