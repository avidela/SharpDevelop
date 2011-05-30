﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.PackageManagement.Scripting;

namespace ICSharpCode.PackageManagement
{
	public static class PackageManagementServices
	{
		static readonly PackageManagementOptions options;
		static readonly PackageManagementSolution solution;
		static readonly PackageManagementConsoleHostProvider consoleHostProvider;
		static readonly RegisteredPackageRepositories registeredPackageRepositories;
		static readonly PackageManagementEvents packageManagementEvents = new PackageManagementEvents();
		static readonly PackageManagementProjectService projectService = new PackageManagementProjectService();
		static readonly ProjectBrowserRefresher projectBrowserRefresher;
		static readonly PackageManagementOutputMessagesView outputMessagesView;
		static readonly RunPackageInitializationScriptsOnSolutionOpen runPackageInitializationScripts;
		static readonly ResetPowerShellWorkingDirectoryOnSolutionClosed resetPowerShellWorkingDirectory;
		static readonly PackageActionsToRun packageActionsToRun = new PackageActionsToRun();
		static readonly PackageActionRunner packageActionRunner;
		
		static PackageManagementServices()
		{
			options = new PackageManagementOptions();
			registeredPackageRepositories = new RegisteredPackageRepositories(options);
			outputMessagesView = new PackageManagementOutputMessagesView(packageManagementEvents);
			solution = new PackageManagementSolution(registeredPackageRepositories, packageManagementEvents);
			consoleHostProvider = new PackageManagementConsoleHostProvider(solution, registeredPackageRepositories);
			projectBrowserRefresher = new ProjectBrowserRefresher(projectService, packageManagementEvents);
			runPackageInitializationScripts = new RunPackageInitializationScriptsOnSolutionOpen(projectService);
			resetPowerShellWorkingDirectory = new ResetPowerShellWorkingDirectoryOnSolutionClosed(projectService, ConsoleHost);
			var consolePackageActionRunner = new ConsolePackageActionRunner(ConsoleHost, packageActionsToRun);
			packageActionRunner = new PackageActionRunner(consolePackageActionRunner, packageManagementEvents);
		}
		
		public static PackageManagementOptions Options {
			get { return options; }
		}
		
		public static IPackageManagementSolution Solution {
			get { return solution; }
		}
		
		public static IPackageManagementConsoleHost ConsoleHost {
			get { return consoleHostProvider.ConsoleHost; }
		}
		
		public static IRegisteredPackageRepositories RegisteredPackageRepositories {
			get { return registeredPackageRepositories; }
		}
		
		public static IPackageManagementEvents PackageManagementEvents {
			get { return packageManagementEvents; }
		}
		
		public static IPackageManagementOutputMessagesView OutputMessagesView {
			get { return outputMessagesView; }
		}
		
		public static IPackageManagementProjectService ProjectService {
			get { return projectService; }
		}
		
		public static PackageActionsToRun PackageActionsToRun {
			get { return packageActionsToRun; }
		}
		
		public static IPackageActionRunner PackageActionRunner {
			get { return packageActionRunner; }
		}
	}
}
