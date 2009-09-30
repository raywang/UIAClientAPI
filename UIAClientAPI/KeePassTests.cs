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
		//TestCase101 Init Sample, create a new account
		public void TestCase101 ()
		{
			//TODO implement a logger class to log the action of each process

			//101.1 Click "new" button on the toolstripbar
			// procedureLogger.expectedResult ("The \"Create New Password Database\" window opens");
			var toolBar = window.Find (ControlType .ToolBar , "");
			toolBar.FindButton ("New...").Click();
			Thread.Sleep (1000);

			//101.2 Enter a new filename on the "filename" combo box of the dailog
			// procedureLogger.expectedResult ("");
			var newPassDialog = window.FindWindow ("Create New Password Database");
			var fileNameEdit = newPassDialog.FindEdit ("File name:");
			fileNameEdit.Value = "TestCase101";
			Thread.Sleep (1000);

			//101.3 Click "Save" button on the dialog
			// procedureLogger.expectedResult ("");
			newPassDialog.Save ();
			Thread.Sleep (1000);

			//101.4 Enter a password “mono-a11y” into "Password" text box
			var createMasterKeyWindow = window.FindWindow ("Create Composite Master Key");
			var edits = createMasterKeyWindow.FindAll<Edit> (ControlType.Edit);
			var masterPasswdEdit = edits [1];
			masterPasswdEdit.Value = "mono-a11y";
			Thread.Sleep (1000);

			//101.5  Re-Enter the password "mono-a11y" into "Password" repeat text box
			var repeatPasswdEdit = edits [0];
			repeatPasswdEdit.Value = "mono-a11y";
			Thread.Sleep (1000);

			//101.6 Check "Key file/option" CheckBox
			createMasterKeyWindow.FindCheckBox ("Key file / provider:").Toggle();
			Thread.Sleep (1000);

			//101.7 Click "Create a new key file" button
			//createMasterKeyWindow.FindButton ("Create...").Click ();
			createMasterKeyWindow.FindButton (" Create...").Click();
			Thread.Sleep (1000);

			//101.8  Click "Save" button on the dialog
			var newKeyFileDialog = window.FindWindow ("Create a new key file");
			newKeyFileDialog.Save();
			Thread.Sleep (1000);

			//in case there is a TestCase101 key exist.
			var comfirmDialog = newKeyFileDialog.FindWindow("Confirm Save As");
			if (comfirmDialog != null) {
				comfirmDialog.Yes ();
				Thread.Sleep (1000);
			}

			//101.9 Click "OK" button on the dialog
			createMasterKeyWindow.FindWindow ("Entropy Collection").OK ();
			Thread.Sleep (1000);

			//101.10 Click "OK" button on the "Create Master Key" Window
			createMasterKeyWindow.OK ();
			Thread.Sleep (1000);

			//101.11 Select "Compression" Tab item
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			var compressionTabItem = newPassDialog2.FindTabItem ("Compression");
			compressionTabItem.Select ();
			Thread.Sleep (1000);

			//101.12 Check "None" RadioButton
			compressionTabItem.FindRadioButton ("None").Select ();
			Thread.Sleep (1000);

			//101.13 Select "Security" Tab item;
			var securityTabItem = newPassDialog2.FindTabItem ("Security");
			securityTabItem.Select ();
			Thread.Sleep (1000);

			//101.14 Enter a number "3000" in "Key transformation" spinner
			var keySpinner = newPassDialog2.FindSpinner ("Number of key transformation rounds:");
			//keySpinner.Value = 1000;
			Thread.Sleep (1000);

			//101.15 Click "OK" button to close the dialog
			newPassDialog2.OK ();
			Thread.Sleep (1000);
		}

		[Test]
		//TestCase102 Organize the group
		public void TestCase102 ()
		{
			//102.1 Click "Edit" menu item on the menu bar
			var menuBar = window.FindMenuBar ();
			var editMenuItem = menuBar.FindMenuItem ("Edit");
			editMenuItem.FindMenuItem ("Edit Group").Click ();
			Thread.Sleep (1000);

			//102.2 Click "Icon" button on the "Edit Group" window
			var editGroupWindow = window.FindWindow ("Edit Group");
			var generalTabItem = editGroupWindow.FindTabItem ("General");
			generalTabItem.FindButton ("Icon:").Click ();
			Thread.Sleep (1000);

			//102.3 Select list item "30" on the "Icon Picker" window
			var iconPickerWindow = editGroupWindow.FindWindow ("Icon Picker");
			var standardIconList = iconPickerWindow.FindList ("", "m_lvIcons");
			standardIconList.FindListItem ("30").Select ();
			Thread.Sleep (1000);
		}
	}
}