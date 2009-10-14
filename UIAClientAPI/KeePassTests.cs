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
			WhiteWindow win = application.GetWindow ("KeePass Password Safe", InitializeOption.NoCache);
			window = new Window (win);			
		}

		[Test]
		//TestCase101 Init Sample, create a new account
		public void TestCase101 ()
		{
			//101.1 Click "New..." button on the toolbar
			procedureLogger.Action ("Click \"New...\" button on the toolbar");
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens");
			var toolBar = window.FindToolBar ("");
			toolBar.FindButton ("New...").Click();
			Thread.Sleep (1000);

			//101.2 Enter "TestCase101" in the "File Name" combo box of the dailog
			procedureLogger.Action ("Enter \"TestCase101\" in the \"File Name\" combo box of the dailog");
			procedureLogger.ExpectedResult ("\"TestCase101\" entered in the \"File Name\" box");
			var newPassDialog = window.FindWindow ("Create New Password Database");
			var fileNameEdit = newPassDialog.FindEdit ("File name:");
			fileNameEdit.Value = "TestCase101";
			Thread.Sleep (1000);

			//101.3 Click "Save" button of the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			newPassDialog.Save ();
			Thread.Sleep (1000);

			//101.4 Enter "mono-a11y" into  "Master password" text box
			procedureLogger.Action ("Enter \"mono-a11y\" into \"Master password\" text box");
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box");
			var createMasterKeyWindow = window.FindWindow ("Create Composite Master Key");
			var masterPasswdEdit = createMasterKeyWindow.FindEdit ("Repeat password:", "m_tbPassword");
			masterPasswdEdit.Value = "mono-a11y";
			Thread.Sleep (1000);

			//101.5  Re-Enter "mono-a11y" into "Repeat password" text box
			procedureLogger.Action ("Re-Enter \"mono-a11y\" into \"Repeat password\" text box");
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Repeat password\" box");
			var repeatPasswdEdit = createMasterKeyWindow.FindEdit ("Repeat password:", "m_tbRepeatPassword");
			repeatPasswdEdit.Value = "mono-a11y";
			Thread.Sleep (1000);

			//101.6 Check "Key file/option" CheckBox
			procedureLogger.Action ("Check \"Key file/option\" CheckBox");
			procedureLogger.ExpectedResult ("\"Key file/option\" CheckBox chekced");
			createMasterKeyWindow.FindCheckBox ("Key file / provider:").Toggle();
			Thread.Sleep (1000);

			//101.7 Click "Create a new key file" button
			procedureLogger.Action ("Click \" Create...\" button");
			procedureLogger.ExpectedResult ("The \"Create a new key file\" dialog opens");
			createMasterKeyWindow.FindButton (" Create...").Click();
			Thread.Sleep (1000);

			//101.8  Click "Save" button of the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			procedureLogger.ExpectedResult ("The \"Entropy Collection\" window opens");
			var newKeyFileDialog = window.FindWindow ("Create a new key file");
			newKeyFileDialog.Save();
			Thread.Sleep (1000);

			//in case there is a TestCase101 key exist.
			var comfirmDialog = newKeyFileDialog.FindWindow("Confirm Save As");
			if (comfirmDialog != null) {
				procedureLogger.Action ("Overwrite new key file");
				procedureLogger.ExpectedResult ("\"Confirm Save As\" dialog closes");
				comfirmDialog.Yes ();
				Thread.Sleep (1000);
			}

			//101.9 Click "OK" button of the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			procedureLogger.ExpectedResult ("The \"Entropy Collection\" window closes");
			createMasterKeyWindow.FindWindow ("Entropy Collection").OK ();
			Thread.Sleep (1000);

			//101.10 Click "OK" button on the "Create Master Key" Window
			procedureLogger.Action ("Click \"OK\" button");
			procedureLogger.ExpectedResult ("The \"Create Master Key\" window closes");
			createMasterKeyWindow.OK ();
			Thread.Sleep (1000);

			//101.11 Select "Compression" Tab item
			procedureLogger.Action("Select \"Compression\" Tab item");
			procedureLogger.ExpectedResult("\"Compression\" tab item opened");
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			var compressionTabItem = newPassDialog2.FindTabItem ("Compression");
			compressionTabItem.Select ();
			Thread.Sleep (1000);

			//101.12 Check "None" RadioButton
			procedureLogger.Action("Check \"None\" RadioButton");
			procedureLogger.ExpectedResult("\"None\" radio button selected");
			compressionTabItem.FindRadioButton ("None").Select ();
			Thread.Sleep (1000);

			//TODO: We can't change the value of Spinner by using
			// RangeValuePattern.SetValue, since the maximum 
			// and mininum value are all 0, seems like the Spinner 
			// did not well provides proper provider here.

			////101.13 Select "Security" Tab item;
			//var securityTabItem = newPassDialog2.FindTabItem ("Security");
			//securityTabItem.Select ();
			//Thread.Sleep (1000);

			////101.14 Enter a number "3000" in "Key transformation" spinner
			//var keySpinner = newPassDialog2.FindSpinner ("Number of key transformation rounds:");
			////keySpinner.Value = 1000;
			//Thread.Sleep (1000);

			//101.15 Click "OK" button to close the dialog
			procedureLogger.Action("Click \"OK\" button of the window");
			procedureLogger.ExpectedResult("The \"Create New Password Database - Step 2\" window closes");
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