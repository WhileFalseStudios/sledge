using Sledge.Common.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sledge.Plugins.Settings
{
    [Export(typeof(ISettingsContainer))]
    public class PluginSettingsContainer : ISettingsContainer
    {
        public string Name => "Sledge.Plugins.PluginSettingsContainer";

        [Setting("EnabledPlugins")] public Dictionary<string, bool> toggledPlugins;

        public IEnumerable<SettingKey> GetKeys()
        {
            yield return new SettingKey("Plugins", "EnabledPlugins", typeof(Dictionary<string, bool>));
        }

        public void LoadValues(ISettingsStore store)
        {
            throw new NotImplementedException();
        }

        public void StoreValues(ISettingsStore store)
        {
            throw new NotImplementedException();
        }
    }
}
