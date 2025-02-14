﻿using GitExtensions.AzureDevOps.Services;
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

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string remoteUrl = GitUICommandsService.GetRemoteUrl(_commands);
            var branch = _commands.Module.GetSelectedBranch();
            var processUrl = $"{remoteUrl}/pullrequestcreate?sourceRef={HttpUtility.UrlEncode(branch.Trim())}";
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = processUrl;
            myProcess.Start();
        }
    }
}
