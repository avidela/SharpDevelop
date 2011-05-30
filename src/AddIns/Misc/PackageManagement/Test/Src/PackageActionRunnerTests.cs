﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using ICSharpCode.PackageManagement;
using ICSharpCode.PackageManagement.Design;
using NuGet;
using NUnit.Framework;
using PackageManagement.Tests.Helpers;

namespace PackageManagement.Tests
{
	[TestFixture]
	public class PackageActionRunnerTests
	{
		FakePackageActionRunner fakeConsoleActionRunner;
		PackageActionRunner runner;
		FakeInstallPackageAction fakeAction;
		FakePowerShellDetection powerShellDetection;
		FakePackageManagementEvents fakeEvents;
		
		void CreateRunner()
		{
			fakeConsoleActionRunner = new FakePackageActionRunner();
			powerShellDetection = new FakePowerShellDetection();
			fakeEvents = new FakePackageManagementEvents();
			runner = new PackageActionRunner(fakeConsoleActionRunner, fakeEvents, powerShellDetection);
		}
		
		void CreateInstallActionWithNoPowerShellScripts()
		{
			var fakeProject = new FakePackageManagementProject();
			fakeAction = new FakeInstallPackageAction(fakeProject);
			fakeAction.Operations = new PackageOperation[0];
		}
		
		void CreateInstallActionWithOnePowerShellScript()
		{
			CreateInstallActionWithNoPowerShellScripts();
			
			var package = new FakePackage();
			package.AddFile(@"tools\init.ps1");
			
			var operation = new PackageOperation(package, PackageAction.Install);
			var operations = new List<PackageOperation>();
			operations.Add(operation);
			
			fakeAction.Operations = operations;
		}
		
		void Run()
		{
			runner.Run(fakeAction);
		}
		
		[Test]
		public void Run_InstallActionHasNoPowerShellScripts_ActionIsExecutedDirectly()
		{
			CreateRunner();
			CreateInstallActionWithNoPowerShellScripts();
			Run();
			
			bool executed = fakeAction.IsExecuteCalled;
			
			Assert.IsTrue(executed);
		}
		
		[Test]
		public void Run_InstallActionHasOnePowerShellScript_ActionIsPassedToConsoleToRun()
		{
			CreateRunner();
			CreateInstallActionWithOnePowerShellScript();
			powerShellDetection.IsPowerShell2InstalledReturnValue = true;
			Run();
			
			ProcessPackageAction action = fakeConsoleActionRunner.ActionPassedToRun;
			
			Assert.AreEqual(fakeAction, action);
		}
		
		[Test]
		public void Run_InstallActionHasOnePowerShellScript_ActionIsNotExecutedDirectly()
		{
			CreateRunner();
			CreateInstallActionWithOnePowerShellScript();
			powerShellDetection.IsPowerShell2InstalledReturnValue = true;
			Run();
			
			bool executed = fakeAction.IsExecuteCalled;
			
			Assert.IsFalse(executed);
		}
		
		[Test]
		public void Run_InstallActionHasOnePowerShellScriptAndPowerShellIsNotInstalled_ActionIsExecutedDirectly()
		{
			CreateRunner();
			CreateInstallActionWithOnePowerShellScript();
			powerShellDetection.IsPowerShell2InstalledReturnValue = false;
			
			Run();
			
			bool executed = fakeAction.IsExecuteCalled;
			
			Assert.IsTrue(executed);
		}
		
		[Test]
		public void Run_InstallActionHasOnePowerShellScriptAndPowerShellIsNotInstalled_MessageIsReportedThatPowerShellScriptsCannotBeRun()
		{
			CreateRunner();
			CreateInstallActionWithOnePowerShellScript();
			powerShellDetection.IsPowerShell2InstalledReturnValue = false;
			
			Run();
			
			string message = fakeEvents.FormattedStringPassedToOnPackageOperationMessageLogged;
			string expectedMessage = 
				"PowerShell is not installed. PowerShell scripts will not be run for the package.";
			
			Assert.AreEqual(expectedMessage, message);
		}
	}
}
