using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNetCore.DisplayRoutes.Report.Json {
    // Based on https://stackoverflow.com/a/10454062 
    public class DepthLimitedContractResolver : DefaultContractResolver {
        private readonly Func<bool> _includeProperty;

        public DepthLimitedContractResolver(Func<bool> includeProperty) {
            _includeProperty = includeProperty;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);
            var shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize = obj => _includeProperty() && (shouldSerialize == null || shouldSerialize(obj));
            return property;
        }
    }
}
