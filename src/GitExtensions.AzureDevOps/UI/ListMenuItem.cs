using GitExtensions.Extensibility.Git;
using System;
using System.Windows.Forms;

namespace GitExtensions.AzureDevOps.UI
{
    /// <summary>
    /// Main menu item.
    /// </summary>
    public class ListMenuItem : ToolStripMenuItem
    {
        private readonly PluginSettings _settings;
        private readonly IGitUICommands _commands;

        internal ListMenuItem(PluginSettings settings, IGitUICommands commands)
        {
            _settings = settings;
            _commands = commands;

            Text = "A&zure Dev Ops";
            DropDownOpening += OnDropDownOpening;
        }

        private void OnDropDownOpening(object sender, EventArgs e)
        {
            DropDown.Items.Clear();

            DropDown.Items.Add(new ToolStripMenuItem
            {
                Text = "Create pull request...",
                Command = new CreatePullRequestCommand(_commands),
            });
            DropDown.Items.Add(new ToolStripMenuItem
            {
                Text = "View list of pull requests...",
                Command = new ViewListOfPullRequestsCommand(_commands),
            });
        }
    }
}
