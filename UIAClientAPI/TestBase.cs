// Helper.cs: Common use classes, methods.
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
using Core;
using System.Threading;
using NUnit.Framework;

namespace UIAClientAPI
{
	// TestBase class helps to launch sample, initiation etc....
	public class TestBase
	{
		Config config = new Config ();
		
		protected Application application = null;
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		public void Init ()
		{
			ProcedureLogger.Init ();
			LaunchSample ();
			Thread.Sleep (Config.Instance.MediumDelay);
			OnSetup ();
		}

		public void Quit ()
		{
			OnQuit ();
			procedureLogger.Action ("Close the application.");
			application.Kill ();
			procedureLogger.Save ();
		}

		protected virtual void LaunchSample ()
		{
		}

		protected virtual void OnSetup ()
		{
		}

		protected virtual void OnQuit ()
		{
		}
	}
}