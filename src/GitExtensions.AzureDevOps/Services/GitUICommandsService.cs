using GitExtensions.Extensibility.Git;
using System;
using System.IO;
using System.Linq;

namespace GitExtensions.AzureDevOps.Services
{
    internal static class GitUICommandsService
    {
        private static char[] splitCharacters = { '=', ' ' };

        internal static string GetRemoteUrl(IGitUICommands _commands)
        {
            var configLines = File.ReadAllLines(_commands.Module.WorkingDirGitDir + "config");
            var urlLine = configLines.FirstOrDefault(x => x.Contains("url = "));
            var remoteUrl = urlLine.Split(splitCharacters).LastOrDefault()?.Trim();
            return remoteUrl;
        }
    }
}