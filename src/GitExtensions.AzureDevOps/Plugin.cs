using GitExtensions.AzureDevOps.Properties;
using GitExtensions.AzureDevOps.UI;
using GitUI;
using GitUI.CommandsDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using GitExtensions.Extensibility.Git;
using GitExtensions.Extensibility.Plugins;
using GitExtensions.Extensibility.Settings;

namespace GitExtensions.AzureDevOps
{
    [Export(typeof(IGitPlugin))]
    public class Plugin : GitPluginBase
    {
        internal PluginSettings Configuration { get; private set; }

        public Plugin()
            : base(PluginSettings.HasProperties)
        {
            Id = new Guid("cae811d0-7158-4e59-96b0-32e17612b3f9");
            Name = "AzureDevOps";
            Description = "Azure DevOps";
            Icon = Resources.Icon;
        }

        public override bool Execute(GitUIEventArgs e)
        {
            e.GitUICommands.StartSettingsDialog(this);
            return false;
        }

        public override IEnumerable<ISetting> GetSettings() => Configuration;

        private MenuStripEx FindMainMenu(IGitUICommands commands)
        {
            FormBrowse form = (FormBrowse)((GitUICommands)commands).BrowseRepo;
            if (form != null)
            {
                MenuStripEx mainMenu = form.Controls.OfType<MenuStripEx>().FirstOrDefault();
                return mainMenu;
            }

            return null;
        }

        private ListMenuItem FindMainMenuItem(IGitUICommands commands, MenuStripEx mainMenu = null)
        {
            if (mainMenu == null)
                mainMenu = FindMainMenu(commands);

            if (mainMenu == null)
                return null;

            return mainMenu.Items.OfType<ListMenuItem>().FirstOrDefault();
        }

        public override void Register(IGitUICommands commands)
        {
            base.Register(commands);

            Configuration = new PluginSettings(Settings);
            if (commands.Module.IsValidGitWorkingDir())
            {
                MenuStripEx mainMenu = FindMainMenu(commands);
                if (mainMenu != null && FindMainMenuItem(commands, mainMenu) == null)
                {
                    mainMenu.Items.Add(new ListMenuItem(Configuration, commands));
                }
            }
        }

        public override void Unregister(IGitUICommands commands)
        {
            base.Unregister(commands);

            MenuStripEx mainMenu = FindMainMenu(commands);
            if (mainMenu != null)
            {
                ListMenuItem mainMenuItem = FindMainMenuItem(commands, mainMenu);
                if (mainMenuItem != null)
                {
                    mainMenu.Items.Remove(mainMenuItem);
                    mainMenuItem.Dispose();
                }
            }
        }
    }
}
