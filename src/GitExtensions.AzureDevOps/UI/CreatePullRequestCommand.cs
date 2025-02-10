using GitExtensions.Extensibility.Git;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Windows.Input;

namespace GitExtensions.AzureDevOps.UI
{
    internal class CreatePullRequestCommand(IGitUICommands _commands) : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private static char[] splitCharacters = { '=', ' ' };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var configLines = File.ReadAllLines(_commands.Module.WorkingDirGitDir + "config");
            var urlLine = configLines.FirstOrDefault(x => x.Contains("url = "));
            var remoteUrl = urlLine.Split(splitCharacters).LastOrDefault()?.Trim();
            var branch = _commands.Module.GetSelectedBranch();
            var processUrl = $"{remoteUrl}/pullrequestcreate?sourceRef={HttpUtility.UrlEncode(branch.Trim())}";
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = processUrl;
            myProcess.Start();
        }
    }
}
