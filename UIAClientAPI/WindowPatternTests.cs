// WindowPatternTests.cs: Tests for Window and Dock Patterns.
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
using WhiteWindow = Core.UIItems.WindowItems.Window;
using System.Diagnostics;

namespace UIAClientAPI
{
	class WindowPatternTests : TestBase
	{
		Window window = null;

		protected override void LaunchSample ()
		{
			string sample = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, Config.Instance.WindowAndTransformPatternProviderPath);
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
			procedureLogger.ExpectedResult ("WindowPattern & TransformPattern Test window appears.");
			WhiteWindow win = application.GetWindow ("WindowPattern & TransformPattern Test", InitializeOption.NoCache);
			window = new Window (win);
		}

		public void TestCase106 ()
		{
			//106.1 Maximize the window
			window.Maximized ();
			procedureLogger.ExpectedResult ("The window would be Maximized.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//106.2 Minimize the window
			window.Minimized ();
			procedureLogger.ExpectedResult ("The window would be Minimized.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//106.3 Restore the window
			window.Normal ();
			procedureLogger.ExpectedResult ("The window would be Restored.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//106.4 Rotate the control for a given degree
			var pane = window.Finder.ByName ("WindowAndTransformPatternProviderControl, r:0").Find<Pane> ();
			pane.Rotate (90.0);
			procedureLogger.ExpectedResult ("The pane would be rotated for 90 degree.");
			Thread.Sleep (Config.Instance.ShortDelay);
		}
	}
}
