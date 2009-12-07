// DockPattern.cs: Tests for DockPattern
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions: 
//  
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software. 
//  


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// 
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com) 
// 
// Authors: 
//      Ray Wang <rawang@novell.com>
//	Felicia Mu <fxmu@novell.com>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Core;
using Core.Factory;
using System.Windows.Automation;
using WhiteWindow = Core.UIItems.WindowItems.Window;
using System.Diagnostics;
using UIAClientTestFramework;
using NUnit.Framework;

namespace ClientTest
{
	[TestFixture]
	class DockPatternTests : TestBase
	{
		Window window = null;

		[TestFixtureSetUp]
		protected override void LaunchSample ()
		{
			string sample = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, Config.Instance.DockPatternProviderPath);
			procedureLogger.Action ("Launch " + sample);

			try {
				application = Application.Launch (sample);
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				Process.GetCurrentProcess ().Kill ();
			}
		}

		protected override void OnSetup ()
		{
			procedureLogger.ExpectedResult ("DockPattern Test window appears.");
			WhiteWindow win = application.GetWindow ("DockPattern Test", InitializeOption.NoCache);
			window = new Window (win);
		}

		[Test]
		public void TestCase105 ()
		{
			//105.1 Move the dock to the Left
			var dock = window.Find<Pane> ("Top");
			dock.DockPosition = DockPosition.Left;
			procedureLogger.ExpectedResult ("The Dock control is docked to the left.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//105.2 Move the dock to the Right
			dock.DockPosition = DockPosition.Right;
			procedureLogger.ExpectedResult ("The Dock control is docked to the right.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//105.3 Move the dock to the Bottom
			dock.DockPosition = DockPosition.Bottom;
			procedureLogger.ExpectedResult ("The Dock control is docked to the bottom.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//105.4 Move the dock to the Top
			dock.DockPosition = DockPosition.Top;
			procedureLogger.ExpectedResult ("The Dock control is docked to the top.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//105.4 Move the dock to be filled.
			dock.DockPosition = DockPosition.Fill;
			procedureLogger.ExpectedResult ("The Dock control is docked to be filled.");
			Thread.Sleep (Config.Instance.ShortDelay);
		}

	}
}
