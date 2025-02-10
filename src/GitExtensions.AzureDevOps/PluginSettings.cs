using System.Collections;
using System.Collections.Generic;
using GitExtensions.Extensibility.Settings;

namespace GitExtensions.AzureDevOps
{
    internal class PluginSettings : IEnumerable<ISetting>
    {
        private readonly SettingsSource source;

        public PluginSettings(SettingsSource source)
        {
            this.source = source;
        }

        #region IEnumerable<ISetting>

        private static readonly List<ISetting> properties;

        public static bool HasProperties => properties.Count > 0;

        static PluginSettings()
        {
            properties = new List<ISetting>(0)
            {
            };
        }

        public IEnumerator<ISetting> GetEnumerator()
            => properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion
    }
}
