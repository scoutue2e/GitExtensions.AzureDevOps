using GitExtensions.AzureDevOps.Services;
using GitExtensions.Extensibility.Git;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace GitExtensions.AzureDevOps.UI
{
    internal class ViewListOfPullRequestsCommand(IGitUICommands _commands) : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string remoteUrl = GitUICommandsService.GetRemoteUrl(_commands);
            var processUrl = $"{remoteUrl}/pullrequests?_a=active";
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = processUrl;
            myProcess.Start();
        }
    }
}
