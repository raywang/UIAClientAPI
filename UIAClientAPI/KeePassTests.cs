// KeePassTests.cs: Tests for KeePass
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
			procedureLogger.ExpectedResult ("KeePass window appears.");
			WhiteWindow win = application.GetWindow ("KeePass Password Safe", InitializeOption.NoCache);
			window = new Window (win);			
		}

		[Test]
		//TestCase101 Init Sample, create a new account
		public void TestCase101 ()
		{
			//101.1 Click "New..." button on the toolbar
			procedureLogger.Action ("Click the \"New...\" button on the toolbar.");
			var toolBar = window.FindToolBar ("");
			toolBar.FindButton ("New...").Click();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens.");
			Thread.Sleep (1000);

			//101.2 Enter "TestCase101" in the "File Name" combo box of the dailog
			procedureLogger.Action ("Enter \"TestCase101\" in the \"File Name\" combo box of the dailog.");
			var newPassDialog = window.FindWindow ("Create New Password Database");
			var fileNameEdit = newPassDialog.FindEdit ("File name:");
			fileNameEdit.Value = "TestCase101";
			procedureLogger.ExpectedResult ("\"TestCase101\" entered in the \"File Name\" box.");
			Thread.Sleep (1000);

			//101.3 Click "Save" button of the dialog
			procedureLogger.Action ("Click the \"Save\" button of the dialog.");
			newPassDialog.Save ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog disappears.");
			Thread.Sleep (1000);

			//101.4 Enter "mono-a11y" into  "Master password" text box
			procedureLogger.Action ("Enter \"mono-a11y\" into \"Master password\" text box.");
			var createMasterKeyWindow = window.FindWindow ("Create Composite Master Key");
			var masterPasswdEdit = createMasterKeyWindow.FindEdit ("Repeat password:", "m_tbPassword");
			masterPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box.");
			Thread.Sleep (1000);

			//101.5  Re-Enter "mono-a11y" into "Repeat password" text box
			procedureLogger.Action ("Re-Enter \"mono-a11y\" into \"Repeat password\" text box.");
			var repeatPasswdEdit = createMasterKeyWindow.FindEdit ("Repeat password:", "m_tbRepeatPassword");
			repeatPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Repeat password\" box.");
			Thread.Sleep (1000);

			//101.6 Check "Key file/option" CheckBox
			procedureLogger.Action ("Check the \"Key file/option\" CheckBox.");
			createMasterKeyWindow.FindCheckBox ("Key file / provider:").Toggle();
			procedureLogger.ExpectedResult ("\"Key file/option\" CheckBox chekced.");
			Thread.Sleep (1000);

			//101.7 Click "Create a new key file" button
			procedureLogger.Action ("Click the \" Create...\" button.");
			createMasterKeyWindow.FindButton (" Create...").Click();
			procedureLogger.ExpectedResult ("The \"Create a new key file\" dialog opens.");
			Thread.Sleep (1000);

			//101.8  Click "Save" button of the dialog
			procedureLogger.Action ("Click the \"Save\" button of the dialog.");
			var newKeyFileDialog = window.FindWindow ("Create a new key file");
			newKeyFileDialog.Save();
			procedureLogger.ExpectedResult ("The \"Entropy Collection\" window opens.");
			Thread.Sleep (1000);

			//in case there is a TestCase101 key exist.
			var comfirmDialog = newKeyFileDialog.FindWindow("Confirm Save As");
			if (comfirmDialog != null) {
				procedureLogger.Action ("Overwrite new key file.");
				comfirmDialog.Yes ();
				procedureLogger.ExpectedResult ("\"Confirm Save As\" dialog disappears.");
				Thread.Sleep (1000);
			}

			//101.9 Click "OK" button of the dialog
			procedureLogger.Action ("Click the \"OK\" button of the dialog.");
			createMasterKeyWindow.FindWindow ("Entropy Collection").OK ();
			procedureLogger.ExpectedResult ("The \"Entropy Collection\" window disappears.");
			Thread.Sleep (1000);

			//101.10 Click "OK" button on the "Create Master Key" Window
			procedureLogger.Action ("Click the \"OK\" button.");
			createMasterKeyWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Create Master Key\" window disappears.");
			Thread.Sleep (1000);

			//101.11 Select "Compression" Tab item
			procedureLogger.Action("Select the \"Compression\" Tab item.");
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			var compressionTabItem = newPassDialog2.FindTabItem ("Compression");
			compressionTabItem.Select ();
			procedureLogger.ExpectedResult ("The \"Compression\" tab item opened.");
			Thread.Sleep (1000);

			//101.12 Check "None" RadioButton
			procedureLogger.Action("Check the \"None\" RadioButton.");
			compressionTabItem.FindRadioButton ("None").Select ();
			procedureLogger.ExpectedResult ("The \"None\" radio button selected.");
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
			procedureLogger.Action("Click the \"OK\" button of the window.");
			newPassDialog2.OK ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window disappears.");
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

		[Test]
		//TestCase103 test the "Add Entry" dialog
		public void TestCase103 ()
		{
			//103.1 Click "new" button on the toolstripbar
			procedureLogger.Action ("Click \"New...\" button on the toolbar");
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens");
			var toolBar = window.FindToolBar ("");
			toolBar.FindButton ("New...").Click ();
			Thread.Sleep (1000);

			//103.2 Click "Save" button on the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			var newPassDialog = window.FindWindow ("Create New Password Database");
			newPassDialog.Save ();
			Thread.Sleep (1000);
			Console.WriteLine ("test case 02   ....................");

			//103.3 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			var keyDialog = window.FindWindow ("Create Composite Master Key");
			keyDialog.OK ();
			Thread.Sleep (1000);

			//103.4 Click "Yes" button on the dialog
			procedureLogger.Action ("Click \"Yes\" button on the KeePass dialog");
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box");
			var createMasterKeyWindow = window.FindWindow ("KeePass");
			createMasterKeyWindow.Yes ();
			Thread.Sleep (1000);

			//103.5  Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			newPassDialog2.OK ();
			Thread.Sleep (1000);

			//103.6 Click "Add Entry" button on the toolstripbar
			procedureLogger.Action ("Click \"Add Entry\" button on the toolstripbar");
			procedureLogger.ExpectedResult ("\"Key file/option\" CheckBox chekced");
			toolBar.FindButton ("Add Entry").Click ();
			Thread.Sleep (1000);

			//103.7 Check "Add Entry" window's WindowPattern Property
			//procedureLogger.Action ("Click \" Create...\" button");
			//procedureLogger.ExpectedResult ("The \"Create a new key file\" dialog opens");
			//createMasterKeyWindow.FindButton (" Create...").Click ();
			//Thread.Sleep (1000);

			//103.8  move "add entry" window to (200,200 )
			procedureLogger.Action ("move \"add entry\" window to (200,200 )");
			var AddEntryDialog = window.FindWindow ("Add Entry");
			AddEntryDialog.Move (200,200);
			procedureLogger.ExpectedResult ("the \"add entry\" window is moved to (200,200 )");
			Thread.Sleep (1000);

			//check the transformpattern property
			//procedureLogger.Action ("check the TransformPattern Property");
			//procedureLogger.ExpectedResult ("The \"Entropy Collection\" window opens");
			//var newKeyFileDialog = window.FindWindow ("Create a new key file");
			//newKeyFileDialog.Save ();
			//Thread.Sleep (1000);

			//103.9 Click the "Auto-Type" tab item on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Auto-Type\" tab item on the \"Add Entry\" Window");
			var tabItemAuto = window.FindTabItem ("Auto-Type");
			tabItemAuto.Select ();
			procedureLogger.ExpectedResult ("The \"Auto-Type\" tab item appears");
			Thread.Sleep (1000);

			//103.10 Click the "Add" button on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Add\" button on the \"Add Entry\" Window");
			AddEntryDialog.ClickButton ("Add");
			procedureLogger.ExpectedResult ("The \"Edit Auto-Type Item\" window appears");
			Thread.Sleep (1000);

			//103.11 Drag the scroll bar to the bottom on the "Edit Auto-Type Item" window
			procedureLogger.Action ("Drag the scroll bar to the bottom on the \"Edit Auto-Type Item\" window");
			var autoItemDialog = window.FindWindow ("Edit Auto-Type Item");
			autoItemDialog.ClickButton ("Forward by large amount");
			autoItemDialog.ClickButton ("Forward by large amount");
			//autoItemDialog.FindButton ("Back by small amount").Click ();
			procedureLogger.ExpectedResult ("the scroll bar is draged to the bottom");
			Thread.Sleep (1000);

			//103.12 Check the scroll bar's property
			//procedureLogger.Action ("Check the scroll bar's property");
			//procedureLogger.ExpectedResult ("\"None\" radio button selected");
			//compressionTabItem.FindRadioButton ("None").Select ();
			//Thread.Sleep (1000);

			//103.13 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button on the dialog");
			autoItemDialog.OK ();
			procedureLogger.ExpectedResult ("\"None\" radio button selected");
			Thread.Sleep (1000);

			//103.14 Click the "Advanced" tab item on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Advanced\" tab item on the \"Add Entry\" Window");
			var tabItemAdvanced = window.FindTabItem ("Advanced");
			tabItemAdvanced.Select ();
			procedureLogger.ExpectedResult ("The \"Advanced\" tab item appears");
			Thread.Sleep (1000);

			//103.15 Click the "Add" button on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Add\" button on the \"Add Entry\" Window");
			AddEntryDialog.FindButton ("Add").Click();
			procedureLogger.ExpectedResult ("The \"Edit Entry String\" dialog appears");
			Thread.Sleep (1000);

			//103.16 Type the "aa" into the "Name" edit
			procedureLogger.Action ("Type the \"aa\" into the \"Name\" edit");
			var editEntryStringWindow = window.FindWindow ("Edit Entry String");
			var nameEdit = editEntryStringWindow.FindEdit ("Name:");
			nameEdit.Value = "aa";
			procedureLogger.ExpectedResult("the \"name\" edit 's value is \"aa\"");
			Thread.Sleep (1000);

			//103.17 Click "OK" button on the "Edit Entry String" dialog
			procedureLogger.Action ("Click \"OK\" button on the \"Edit Entry String\" dialog");
			createMasterKeyWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Edit Entry String\" window closes");
			Thread.Sleep (1000);

			//103.18 Check the "aa" text's TableItemPattern
			//procedureLogger.Action ("Check the \"aa\" text's TableItemPattern");
			//procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window closes");
			//newPassDialog2.OK ();
			//Thread.Sleep (1000);

			//103.19 Close the "Add Entry" Window
			procedureLogger.Action ("Close the \"Add Entry\" Window");
			AddEntryDialog.OK ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window closes");
			Thread.Sleep (1000);
		}
	}
}