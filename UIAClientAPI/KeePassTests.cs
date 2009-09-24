// KeePassTests.cs: Tests for KeePass
//
// Author:
//   Ray Wang  (rawang@novell.com)
//
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Threading;
using Core;
using Core.Factory;
using Core.UIItems;
using Core.UIItems.Finders;
using WhiteWindow = Core.UIItems.WindowItems.Window;
using NUnit.Framework;
using Core.UIItems.WindowStripControls;
using Core.UIItems.MenuItems;

namespace UIAClientAPI 
{
	[TestFixture]
	class KeePassTests : TestBase 
	{
		Window window = null;

		protected override void OnSetup ()
		{
			// window = new Window (application.GetWindow ...)
			WhiteWindow win = application.GetWindow ("KeePass Password Safe", InitializeOption.NoCache);
			window = new Window (win);
			Assert.IsNotNull (window, "Window object is null");
			
		}

		[Test]
		//TestCase101  Init Sample, create a new account
		public void TestCase101 ()
		{
			//TODO implement a logger class to log the action of each process

			//101.1 Click "new" button on the toolstripbar
			// var toolbar = window.Find (ControlType.ToolBar, "");
			// window.findButton("New...").click()
			// toolbar.findButton ("New...").Click ();
			// procedureLogger.expectedResult ("The \"Create New Password Database\" window opens");
			var toolBar = window.Find (ControlType .ToolBar , "");
			toolBar.FindButton ("New...").Click();
			Thread.Sleep (1000);

			//101.2 Enter a new filename on the "filename" combo box of the dailog

			// var newPassDialog = window.FindWindow ("Create New Password Database");
			// var fileNameEdit = newPassDialog.FindEdit ("File name:");
			// fileNameEdit.Value = "TestCase101";

			var newPassDialog = window.FindWindow ("Create New Password Database");
			Edit fileNameEdit = newPassDialog.FindEdit ("File name:");
			fileNameEdit.Value = "TestCase101";
			Thread.Sleep (1000);

			//101.3 Click "Save" button on the dialog

			// newPassDialog.Save ();

			newPassDialog.Save ();
			Thread.Sleep (1000);

			//101.4 Enter a password “hello brad” into "Password" text box

		}
	}
}