using GitExtensions.AzureDevOps.Services;
using GitExtensions.Extensibility.Git;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            
            // Create default URL
            var processUrl = $"{remoteUrl}/pullrequestcreate?sourceRef={HttpUtility.UrlEncode(branch.Trim())}";

            // Get the target branch of the PR by finding the parent branch
            var commitSha = _commands.Module.GetCurrentCheckout();
            var parentCommits = _commands.Module.GetParentRevisions(commitSha);

            if (parentCommits.Count > 0)
            {
                var remoteBranch = _commands.Module.GetRemoteBranch(branch);
                var parentBranches = parentCommits
                    .SelectMany(parent => _commands.Module.GetAllBranchesWhichContainGivenCommit(parent.ObjectId, getLocal: false, getRemote: true, CancellationToken.None))
                    .Distinct(StringComparer.OrdinalIgnoreCase);
				if (parentBranches != null)
				{
					var originTargetBranch = parentBranches.FirstOrDefault(b => b.EndsWith("/develop", StringComparison.OrdinalIgnoreCase))
							   ?? parentBranches.FirstOrDefault(b => b.EndsWith("/master", StringComparison.OrdinalIgnoreCase))
							   ?? parentBranches.FirstOrDefault(b => !b.Contains(branch, StringComparison.OrdinalIgnoreCase))
							   ?? parentBranches.FirstOrDefault(b => !b.Contains(remoteBranch, StringComparison.OrdinalIgnoreCase))
							   ?? parentBranches.FirstOrDefault("");
					var remoteNames = _commands.Module.GetRemoteNames();
					var temp = remoteNames.Select(r => r + "/");
               var targetBranch = temp.Aggregate(originTargetBranch, (current, prefix) =>
																current.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
																	? current.Substring(prefix.Length)
																	: current);
					if (!string.IsNullOrWhiteSpace(targetBranch))
						processUrl += $"&targetRef={HttpUtility.UrlEncode(targetBranch.Trim())}";
				}
            }

            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = processUrl;
            myProcess.Start();
        }
    }
}
