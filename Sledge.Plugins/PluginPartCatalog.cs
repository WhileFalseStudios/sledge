using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sledge.Plugins
{
    public class PluginPartCatalog : ComposablePartCatalog
    {
        private readonly string _pluginDir;
        private readonly Lazy<AggregateCatalog> _innerCatalog;

        public PluginPartCatalog(string directory)
        {
            _pluginDir = directory;
            _innerCatalog = new Lazy<AggregateCatalog>(CreateInnerCatalog);
        }

        private AggregateCatalog CreateInnerCatalog()
        {
            var parts = new List<ComposablePartCatalog>();


        }

        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return _innerCatalog.Value.GetEnumerator();
        }
    }
}
