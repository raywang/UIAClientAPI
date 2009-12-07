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
using System.IO;
using System.Threading;
using Core;
using Core.Factory;
using WhiteWindow = Core.UIItems.WindowItems.Window;
using NF = NUnit.Framework;
using System.Windows.Automation;
using System.Diagnostics;
using SWF = System.Windows.Forms;
using UIAClientTestFramework;

namespace ClientTest 
{
	class KeePassTests : TestBase 
	{
		Window window = null;
		Finder Finder = null;

		protected override void LaunchSample ()
		{
			string sample = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, Config.Instance.KeePassPath);
			procedureLogger.Action ("Launch " + sample);

			try {
				application = Core.Application.Launch (sample);
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				Process.GetCurrentProcess ().Kill ();
			}
		}

		protected override void OnSetup ()
		{
			procedureLogger.ExpectedResult ("KeePass window appears.");
			WhiteWindow win = application.GetWindow ("KeePass Password Safe", InitializeOption.NoCache);
			window = new Window (win);
		}

		//TestCase101 Init Sample, create a new account
		public void TestCase101 ()
		{
			//101.1 Click the "New..." button on the toolbar.
			var toolBar = window.Find<ToolBar> ();
			toolBar.Find<Button> ("New...").Click (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.2 Enter "TestCase101" in the "File Name" combo box of the dailog.
			var newPassDialog = window.Find<Window> ("Create New Password Database");
			var fileNameEdit = newPassDialog.Find<Edit> ("File name:");
			fileNameEdit.Value = "TestCase101";
			procedureLogger.ExpectedResult ("\"TestCase101\" entered in the \"File Name\" box.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.3 Click the "Save" button of the dialog.
			newPassDialog.Save ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.4 Enter "mono-a11y" into  "Master password" text box.
			var createMasterKeyWindow = window.Find<Window> ("Create Composite Master Key");
			var masterPasswdEdit = createMasterKeyWindow.Find<Edit> ("Repeat password:", "m_tbPassword");
			masterPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.5  Re-Enter "mono-a11y" into "Repeat password" text box.
			var repeatPasswdEdit = createMasterKeyWindow.Find<Edit> ("Repeat password:", "m_tbRepeatPassword");
			repeatPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Repeat password\" box.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.6 Check "Key file/option" CheckBox
			procedureLogger.Action ("Check the \"Key file/option\" CheckBox.");
			createMasterKeyWindow.Find<CheckBox> ("Key file / provider:").Toggle ();
			procedureLogger.ExpectedResult ("\"Key file/option\" CheckBox chekced.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.7 Click the " Create..." button.
			createMasterKeyWindow.Find<Button> (" Create...").Click ();
			procedureLogger.ExpectedResult ("The \"Create a new key file\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.8  Click the "Save" button of the dialog.
			var newKeyFileDialog = window.Find<Window> ("Create a new key file");
			newKeyFileDialog.Save();
			
			//in case there is a TestCase101 key exist.
			var comfirmDialog = newKeyFileDialog.Find<Window> ("Confirm Save As");
			if (comfirmDialog != null) {
				procedureLogger.ExpectedResult ("The \"Confirm Save As\" dialog opens.");
				Thread.Sleep(Config.Instance.ShortDelay);

				comfirmDialog.Yes ();
				procedureLogger.ExpectedResult ("The \"Confirm Save As\" dialog disappears.");
				Thread.Sleep(Config.Instance.ShortDelay);
			} else {
				procedureLogger.ExpectedResult ("The \"Entropy Collection\" window opens.");
				Thread.Sleep(Config.Instance.ShortDelay);
			}

			//101.9 Click the "OK" button of the dialog.
			createMasterKeyWindow.Find<Window> ("Entropy Collection").OK ();
			procedureLogger.ExpectedResult ("The \"Entropy Collection\" window disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.10 Click the "OK" button on the "Create Master Key" Window
			createMasterKeyWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Create Master Key\" window disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.11 Select the "Compression" Tab item.
			var newPassDialog2 = window.Find<Window> ("Create New Password Database - Step 2");
			var compressionTabItem = newPassDialog2.Find<TabItem> ("Compression");
			compressionTabItem.Select ();
			procedureLogger.ExpectedResult ("The \"Compression\" tab item opened.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.12 Check the "None" RadioButton.
			compressionTabItem.Find<RadioButton> ("None").Select ();
			procedureLogger.ExpectedResult ("The \"None\" radio button selected.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//TODO: We can't change the value of Spinner by using
			// RangeValuePattern.SetValue, since the maximum
			// and mininum value are all 0, seems like the Spinner
			// did not well provides proper provider here.

			/*
			//101.13 Select "Security" Tab item;
			var securityTabItem = newPassDialog2.FindTabItem ("Security");
			securityTabItem.Select ();
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.14 Enter a number "3000" in "Key transformation" spinner
			var keySpinner = newPassDialog2.Find<Spinner> ("Number of key transformation rounds:");
			SWF.SendKeys.SendWait ("3000");
			procedureLogger.ExpectedResult ("The \"Key transformation\" spinner is set to 3000.");
			Thread.Sleep (Config.Instance.ShortDelay);
			*/

			//101.15 Click the "OK" button to close the dialog.
			newPassDialog2.OK ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);
		}

		//TestCase102 Organize the group
		public void TestCase102 ()
		{
			//101.1
			var toolBar = window.Find<ToolBar> ();
			toolBar.Find<Button> ("New...").Click ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.2 Enter "TestCase101" in the "File Name" combo box of the dailog.
			var newPassDialog = window.Find<Window> ("Create New Password Database");
			var fileNameEdit = newPassDialog.Find<Edit> ("File name:");
			fileNameEdit.Value = "TestCase101";
			procedureLogger.ExpectedResult ("\"TestCase101\" entered in the \"File Name\" box.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.3 Click the "Save" button of the dialog.
			newPassDialog.Save ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.4 Enter "mono-a11y" into  "Master password" text box.
			var createMasterKeyWindow = window.Find<Window> ("Create Composite Master Key");
			var masterPasswdEdit = createMasterKeyWindow.Finder.ByName ("Repeat password:").ByAutomationId ("m_tbPassword").Find<Edit> ();
			masterPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.5  Re-Enter "mono-a11y" into "Repeat password" text box.
			var repeatPasswdEdit = createMasterKeyWindow.Finder.ByName ("Repeat password:").ByAutomationId ("m_tbRepeatPassword").Find<Edit> ();
			repeatPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Repeat password\" box.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.6 Click the "OK" button on the "Create Master Key" Window
			createMasterKeyWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Create Master Key\" window disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//101.7
			var newPassDialog2 = window.Find<Window> ("Create New Password Database - Step 2");
			newPassDialog2.OK ();
			procedureLogger.ExpectedResult ("\"Create New Password Database - Step 2\" dialog disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.1 Click the "Edit" menu item on the menu bar.
			var menuBar = window.Find<MenuBar> ();
			var editMenuItem = menuBar.Find<MenuItem> ("Edit");
			editMenuItem.Find<MenuItem> ("Edit Group").Click ();
			procedureLogger.ExpectedResult ("The \"Edit Group\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//102.2 Click the "Icon" button on the "Edit Group" dialog.
			var editGroupWindow = window.Find<Window> ("Edit Group");
			var generalTabItem = editGroupWindow.Find<TabItem> ("General");
			generalTabItem.Find<Button> ("Icon:").Click ();
			procedureLogger.ExpectedResult ("The \"Icon Picker\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//102.3 Select list item "30" on the "Icon Picker" dialog.
			var iconPickerWindow = editGroupWindow.Find<Window> ("Icon Picker");
			var standardIconList = iconPickerWindow.Find<List> ("", "m_lvIcons");
			var listItem30 = standardIconList.Find<ListItem> ("30");
			listItem30.Select ();
			procedureLogger.ExpectedResult ("The \"30\" list item is selected.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//102.4 Unselect list item "30" on the "Icon Picker" dialog.
			listItem30.RemoveFromSelection ();
			procedureLogger.ExpectedResult ("The \"30\" list item is removed from selection.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.5 Select list item "30" on the "Icon Picker" dialog again.
			listItem30.AddToSelection ();
			procedureLogger.ExpectedResult ("The \"30\" list item is added to selection.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.6 Click list item "68" on the "Icon Picker" dialog.
			standardIconList.Find<ListItem> ("68").Show ();
			procedureLogger.ExpectedResult ("The \"68\" list item is showed in the view.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.7 Click the "OK" button on the "Icon Picker" dialog.
			iconPickerWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Icon Picker\" dialog disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.8 Select the "Behavior" Tab Item from the Tab.
			var behaviorTabItem = editGroupWindow.Find<TabItem> ("Behavior");
			behaviorTabItem.Select ();
			procedureLogger.ExpectedResult ("The \"Behavior\" tab item opens.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.9 Expand the "Searching entries in this group" combo box.
			var searchCombobox = behaviorTabItem.Find<ComboBox> ("Searching entries in this group:");
			searchCombobox.Expand ();
			procedureLogger.ExpectedResult ("\"Searching entries in this group\" combox box is expanded.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.10 Select the "Enabled" from the "Searching entries in this group" combo box.
			searchCombobox.Find<ListItem> ("Enabled").Select ();
			procedureLogger.ExpectedResult ("The \"Enabled\" list item is selected.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.11 Click the "OK" button on the  "Edit Group" dialog.
			editGroupWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Edit Group\" dialog disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);
		}
		
		//TestCase103 test the "Add Entry" dialog
		public void TestCase103 ()
		{
			
			//103.1 Click "new" button on the toolstripbar
			var toolBar = window.Find<ToolBar> ();
			toolBar.Finder.ByName ("New...").Find<Button> ().Click ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.2 Click "Save" button on the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			var newPassDialog = window.Find<Window> ("Create New Password Database");			
			newPassDialog.Save (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.3 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var keyDialog = window.Find<Window> ("Create Composite Master Key");
			keyDialog.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.4 Click "Yes" button on the dialog
			procedureLogger.Action ("Click \"Yes\" button on the KeePass dialog");
			var createMasterKeyWindow = window.Find<Window> ("KeePass");
			createMasterKeyWindow.Yes (false);
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.5 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var newPassDialog2 = window.Find<Window> ("Create New Password Database - Step 2");
			newPassDialog2.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.6 Click "Add Entry" button on the toolstripbar
			procedureLogger.Action ("Click \"Add Entry\" button on the toolstripbar");
			toolBar.Find<Button> ("Add Entry").Click(false);
			procedureLogger.ExpectedResult ("the \"Add Entry\" window appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.7 Check "Add Entry" window's default WindowPattern Property
			procedureLogger.Action ("Check \"Add Entry\" window's CanMaximizeProperty");
			Window addEntryWindow = window.Find<Window> ("Add Entry");
			NF.Assert.AreEqual (false, addEntryWindow.CanMaximize);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's CanMinimizeProperty");
			NF.Assert.AreEqual (false, addEntryWindow.CanMinimize);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's IsModalProperty");
			NF.Assert.AreEqual (true, addEntryWindow.IsModal);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's IsTopmostProperty");
			NF.Assert.AreEqual (false, addEntryWindow.IsTopmost);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's WindowInteractionStateProperty");
			NF.Assert.AreEqual (WindowInteractionState.ReadyForUserInteraction, addEntryWindow.WindowInteractionState);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's WindowVisualStateProperty");
			NF.Assert.AreEqual (WindowVisualState.Minimized, addEntryWindow.WindowVisualState);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.8 move "add entry" window to (200,200 )
			procedureLogger.Action ("move \"add entry\" window to (200,200 )");
			var AddEntryDialog = window.Find<Window> ("Add Entry");
			AddEntryDialog.Move (200, 200);
			procedureLogger.ExpectedResult ("the \"add entry\" window is moved to (200,200 )");
			Thread.Sleep(Config.Instance.ShortDelay);

			//check the transformpattern's property
			procedureLogger.Action ("Check \"Add Entry\" window's CanMoveProperty");
			NF.Assert.AreEqual (true, addEntryWindow.CanMove);
			procedureLogger.ExpectedResult ("The window's CanMoveProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's CanSizeProperty");
			NF.Assert.AreEqual (false, addEntryWindow.CanResize);
			procedureLogger.ExpectedResult ("The window's CanSizeProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's CanRotateProperty");
			NF.Assert.AreEqual (false, addEntryWindow.CanRotate);
			procedureLogger.ExpectedResult ("The window's CanRotateProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			//103.9 Click the "Auto-Type" tab item on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Auto-Type\" tab item on the \"Add Entry\" Window");
			var tabItemAuto = window.Find<TabItem> ("Auto-Type");
			tabItemAuto.Select ();
			procedureLogger.ExpectedResult ("The \"Auto-Type\" tab item appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.10 Click the "Add" button on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Add\" button on the \"Add Entry\" Window");
			AddEntryDialog.Find<Button> ("Add").Click(false);
			procedureLogger.ExpectedResult ("The \"Edit Auto-Type Item\" window appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.11 Drag the scroll bar to the bottom on the "Edit Auto-Type Item" window
			procedureLogger.Action ("drag the scroll bar to the 300 position");
			var autoItemDialog = window.Find<Window> ("Edit Auto-Type Item");
			ScrollBar scrollBar = window.Find<ScrollBar> ();
			scrollBar.SetValue (300);
			procedureLogger.ExpectedResult ("the scroll bar is draged to the 413 position");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.12 Check the scroll bar's property
			procedureLogger.Action ("Check scroll bar's IsReadOnlyProperty");
			NF.Assert.AreEqual (false, scrollBar.IsReadOnly);
			procedureLogger.ExpectedResult ("The scroll bar's IsReadOnlyProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's LargeChangeProperty");
			NF.Assert.AreEqual (131, (int)scrollBar.LargeChange);
			procedureLogger.ExpectedResult ("The scroll bar's large chaged value should be 131");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's LargeChangeProperty");
			NF.Assert.AreEqual (1, (int) scrollBar.SmallChange);
			procedureLogger.ExpectedResult ("The scroll bar's large chaged value should be 131");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's Maximum value");
			NF.Assert.AreEqual (362, (int)scrollBar.Maximum);
			procedureLogger.ExpectedResult ("The scroll bar's Maximum value shoule be 362");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's Minimum value");
			NF.Assert.AreEqual (0, (int)scrollBar.Minimum);
			procedureLogger.ExpectedResult ("The scroll bar's minimum value shoule be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's value whether equal to 300");
			NF.Assert.AreEqual (300, (int)scrollBar.Value);
			procedureLogger.ExpectedResult ("The scroll bar's value should be 300");
			Thread.Sleep (Config.Instance.ShortDelay);

			//103.13 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button on the dialog");
			autoItemDialog.OK (false);
			procedureLogger.ExpectedResult ("\"None\" radio button selected");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.14 Click the "Advanced" tab item on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Advanced\" tab item on the \"Add Entry\" Window");
			var tabItemAdvanced = window.Find<TabItem> ("Advanced");
			tabItemAdvanced.Select ();
			procedureLogger.ExpectedResult ("The \"Advanced\" tab item appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.15 Click the "Add" button on the "Add Entry" Window
			var datagrid2 = addEntryWindow.Find<DataGrid> ();
			procedureLogger.Action ("Click the \"Add\" button on the \"Add Entry\" Window");
			AddEntryDialog.Find<Button> ("Add").Click (false);
			procedureLogger.ExpectedResult ("The \"Edit Entry String\" dialog appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.16 Type the "aa" into the "Name" edit
			procedureLogger.Action ("Type the \"aa\" into the \"Name\" edit");
			var editEntryStringWindow = window.Find<Window> ("Edit Entry String");
			var nameEdit = editEntryStringWindow.Find<Edit> ("Name:");
			SWF.SendKeys.SendWait ("aa");		
			procedureLogger.ExpectedResult("the \"name\" edit 's value is \"aa\"");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.17 Click "OK" button on the "Edit Entry String" dialog
			procedureLogger.Action ("Click \"OK\" button on the \"Edit Entry String\" dialog");
			editEntryStringWindow.OK (false);
			procedureLogger.ExpectedResult ("The \"Edit Entry String\" window closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.18 Check the "aa" text's TableItemPattern
			var aaText = AddEntryDialog.Find<Text> ("aa");
			
			procedureLogger.Action ("Check \"aa\" text's TableItemPattern's Column property");
			NF.Assert.AreEqual (0, aaText.Column);
			procedureLogger.ExpectedResult ("The \"aa\" text's Colum should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

  			procedureLogger.Action ("Check \"aa\" text's TableItemPattern's ColumnSpan property");
			NF.Assert.AreEqual (1, aaText.ColumnSpan);
			procedureLogger.ExpectedResult ("The \"aa\" text's ColumnSpan should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"aa\" text's TableItemPattern's Row property");
			NF.Assert.AreEqual (0, aaText.Row);
			procedureLogger.ExpectedResult ("The \"aa\" text's Row should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"aa\" text's TableItemPattern's RowSpan property");
			NF.Assert.AreEqual (1, aaText.RowSpan);
			procedureLogger.ExpectedResult ("The \"aa\" text's RowSpan should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"aa\" text's TableItemPattern's ContainingGrid property");
			AutomationElement dataGridItem = datagrid2.GetItem (0, 0);
			AutomationElement aatextItem = window.Find<Text> ("aa").AutomationElement;
			NF.Assert.AreEqual (dataGridItem, aatextItem);
			procedureLogger.ExpectedResult ("The \"aa\" text's ContainingGrid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);
			
			newPassDialog2.OK ();
			Thread.Sleep (Config.Instance.ShortDelay);

			//103.19 Close the "Add Entry" Window
			procedureLogger.Action ("Close the \"Add Entry\" Window");
			AddEntryDialog.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window closes");
			Thread.Sleep(Config.Instance.ShortDelay);
		}

		//TestCase104 test the "Password Generator" dialog
		public void TestCase104 ()
		{
			//104.1 Click "new" button on the toolstripbar
			procedureLogger.Action ("Click \"New...\" button on the toolbar");
			var toolBar = window.Find<ToolBar> ();
			toolBar.Find<Button> ("New...").Click (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.2 Click "Save" button on the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			var newPassDialog = window.Find<Window> ("Create New Password Database");
			newPassDialog.Save (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.3 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var keyDialog = window.Find<Window> ("Create Composite Master Key");
			keyDialog.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.4 Click "Yes" button on the dialog
			procedureLogger.Action ("Click \"Yes\" button on the KeePass dialog");
			var createMasterKeyWindow = window.Find<Window> ("KeePass");
			createMasterKeyWindow.Yes (false);
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.5  Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var newPassDialog2 = window.Find<Window> ("Create New Password Database - Step 2");
			newPassDialog2.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.6  Click "Add Entry" button on the toolstripbar
			procedureLogger.Action ("Click \"Add Entry\" button on the toolstripbar");
			toolBar.Find<Button> ("Add Entry").Click (false);
			procedureLogger.ExpectedResult ("The \"Add Entry\" dialog appears");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.7  Input the "email" into the title  edit 
			procedureLogger.Action ("Input the \"email\" into the title  edit ");
			var notesEdit = newPassDialog.Find<Edit> ("Notes:");
			SWF.SendKeys.SendWait ("email");
			procedureLogger.ExpectedResult ("The \"email\" field is inputted into the \"Notes\" edit");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.8 Click "OK" button on the "Add Entry" dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			window.Find<Button> ("OK").Click (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.9 Check the datagrid's Coulum  is 11 and Row count is 2
			procedureLogger.Action ("check GridPattern 's property :ColumnCount");
			DataGrid dataGrid = window.Find<DataGrid>();
			NF.Assert.AreEqual (11, dataGrid.ColumnCount);
			procedureLogger.ExpectedResult ("the data grid 's ColumnCount should be 11");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("check GridPattern 's property :RowCount");
			NF.Assert.AreEqual (2, dataGrid.RowCount);
			procedureLogger.ExpectedResult ("the data grid 's RowCount should be 2");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.10 Click "Add Entry" button on the toolstripbar
			procedureLogger.Action ("Click \"Add Entry\" button on the toolstripbar");
			toolBar.Find<Button> ("Add Entry").Click (false);
			procedureLogger.ExpectedResult ("The \"Add Entry\" dialog appears");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.11 Input the "shopping" into the title edit 
			procedureLogger.Action ("Input the \"shopping\" into the title edit");
			SWF.SendKeys.SendWait ("shopping");
			procedureLogger.ExpectedResult ("The \"shopping\" field is inputted into the \"Notes\" edit");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.12 Click "OK" button on the "Add Entry" dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			window.Find<Button> ("OK").Click (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.13 Check the datagrid's Coulum is 11 and Row count is 3
			procedureLogger.Action ("check GridPattern 's property :ColumnCount");
			NF.Assert.AreEqual (11, dataGrid.ColumnCount);
			procedureLogger.ExpectedResult ("the data grid 's ColumnCount should be 11");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("check GridPattern 's property :RowCount");
			NF.Assert.AreEqual (3, dataGrid.RowCount);
			procedureLogger.ExpectedResult ("the data grid 's RowCount should be 3");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.14 Get the (0,0) element of the datagrid , check if it is "Sample Entry" 
			procedureLogger.Action ("Get the (0,0) element of the datagrid , check if it is \"Sample Entry\"");
			AutomationElement dataGridItem = dataGrid.GetItem (0, 0);
			AutomationElement textItem = window.Find<Text> ("Sample Entry").AutomationElement;
			NF.Assert.AreEqual (dataGridItem, textItem);
			procedureLogger.ExpectedResult ("the (0,0) item in the datagrid should be Sample Entry");
			Thread.Sleep (Config.Instance.ShortDelay);

			//decrease to make the horizontal scroll bar appears
			window.Resize (500, 600);

			//104.15 Check the ScrollPattern's default property of datagrid
			procedureLogger.Action ("Check Horizontal scroll bar's HorizontallyScrollable is True");
			NF.Assert.AreEqual (true, dataGrid.HorizontallyScrollable);
			procedureLogger.ExpectedResult ("The scroll bar's HorizontallyScrollable is True");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check Horizontal scroll bar's HorizontalScrollPercent is 0.0");
			NF.Assert.AreEqual (0.0, dataGrid.HorizontalScrollPercent);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's HorizontalScrollPercent should be 0.0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check Horizontal scroll bar's HorizontalViewSize is 75.711159737417944");
			NF.Assert.AreEqual (75.711159737417944, dataGrid.HorizontalViewSize);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's HorizontalViewSize should be 75.711159737417944");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check Horizontal scroll bar's VerticallyScrollable is false");
			NF.Assert.AreEqual (false, dataGrid.VerticallyScrollable);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's VerticallyScrollable should be false");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check Horizontal scroll bar's VerticalScrollPercent is -1");
			NF.Assert.AreEqual (-1, dataGrid.VerticalScrollPercent);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's VerticalScrollPercent should be -1");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check Horizontal scroll bar's VerticalViewSize is 100");
			NF.Assert.AreEqual (100, dataGrid.VerticalViewSize);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's VerticalViewSize should be 100");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.16  Use Scroll method give the horizotal Scrollbar a LargeIncrement
			procedureLogger.Action ("Use Scroll method give the horizotal Scrollbar a LargeIncrement");
			dataGrid.Scroll (ScrollAmount.LargeIncrement, ScrollAmount.NoAmount);
			procedureLogger.ExpectedResult ("The horizotal Scrollbar increase large");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.17 Use Scroll method give the horizotal Scrollbar a SmallIncrement 
			procedureLogger.Action ("Use Scroll method give the horizotal Scrollbar a SmallIncrement");
			dataGrid.Scroll (ScrollAmount.SmallIncrement, ScrollAmount.NoAmount);
			procedureLogger.ExpectedResult ("The horizotal Scrollbar increase small");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.18 Check the horizotal scrollbar's position should be 100.0d
			procedureLogger.Action ("after the LargeIncrement and SmallIncrement the scroll's position is 100.0d ");
			NF.Assert.AreEqual (100.0d, dataGrid.HorizontalScrollPercent);
			procedureLogger.ExpectedResult ("the scroll's position is 100.0d ");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.19 Use ScrollHorizontalmethod give the horizotal Scrollbar a  SmalIDecrement
			procedureLogger.Action ("Use Scroll method give the horizotal Scrollbar a SmalIDecrement");
			dataGrid.Scroll (ScrollAmount.SmallDecrement, ScrollAmount.NoAmount);
			procedureLogger.ExpectedResult ("The horizotal Scrollbar decrease small");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.20 Use ScrollHorizontalmethod give the horizotal Scrollbar a LargeDecrement
			procedureLogger.Action ("Use Scroll method give the horizotal Scrollbar a LargeDecrement");
			dataGrid.Scroll (ScrollAmount.LargeDecrement, ScrollAmount.NoAmount);
			procedureLogger.ExpectedResult ("The horizotal Scrollbar decrease large");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.21 Check the horizotal scrollbar's position should be 0.0d
			procedureLogger.Action ("after the LargeDecrement and SmallDecrement the scroll's position is 0.0d");
			NF.Assert.AreEqual (0.0d, dataGrid.HorizontalScrollPercent);
			procedureLogger.ExpectedResult ("the scroll's position is 0.0d");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.22 Use SetScrollPercent method make the horizotal scrollbar move to (50, -1)
			procedureLogger.Action ("Use SetScrollPercent method make the horizotal scrollbar move to (50, -1)");
			dataGrid.SetScrollPercent (50, -1);
			procedureLogger.ExpectedResult ("The horizotal Scrollbar move to (50, -1)");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.23 Check the horizotal scrollbar's position should be  54.054054054054056d
			procedureLogger.Action ("after the SetScrollPercent action the scroll's position is  54.054054054054056d");
			NF.Assert.AreEqual (54.054054054054056d, dataGrid.HorizontalScrollPercent);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's VerticallyScrollable should be  54.054054054054056d");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.24 Check the data grid's MultipleViewPattern property
			procedureLogger.Action ("Check the data grid's CurrentView is 1");
			NF.Assert.AreEqual (1, dataGrid.CurrentView);
			procedureLogger.ExpectedResult ("the data grid 's CurrentViewProperty should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.25 Retrieves the first name "Icons" of the view pattern of the data grid
			procedureLogger.Action ("Retrieves the first name \"Icons\" of the view pattern of the data grid");
			dataGrid.GetViewName (1);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's VerticallyScrollable should be  54.054054054054056d");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.26 Retrieves the supported name of the view pattern of the data grid, NF.Assert its value is one
			procedureLogger.Action ("Retrieves the supported name of the view pattern of the data grid, NF.Assert its value is one");
			NF.Assert.AreEqual (1, dataGrid.CurrentView);
			procedureLogger.ExpectedResult ("The supported name's value of the view pattern of the data grid is one");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.27 Check the GridItemPattern's property of text in data grid
			procedureLogger.Action ("Check the column of data grid");
			NF.Assert.AreEqual (0, dataGrid.Column);
			procedureLogger.ExpectedResult ("the column of data grid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the column span of data grid");
			procedureLogger.ExpectedResult ("the column span of data grid should be 1");
			NF.Assert.AreEqual (1, dataGrid.ColumnSpan);
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the row of data grid");
			NF.Assert.AreEqual (0, dataGrid.Row);
			procedureLogger.ExpectedResult ("the row of data grid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the row span of data grid");
			NF.Assert.AreEqual (1, dataGrid.RowSpan);
			procedureLogger.ExpectedResult ("the row span of data grid should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the ContainingGrid of data grid");
			NF.Assert.AreEqual (null, dataGrid.ContainingGrid);
			procedureLogger.ExpectedResult ("the ContainingGrid of data grid should be null");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.28 Check the TableItem's property of text in data grid
			procedureLogger.Action ("Check the TableItemColumn of the text in datagrid");
			Text sampleText = window.Find<Text> ("Sample Entry");
			NF.Assert.AreEqual (0, sampleText.TableItemColumn);
			procedureLogger.ExpectedResult ("the TableItemColumn of the text in datagrid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemColumnSpan of the text in datagrid");
			NF.Assert.AreEqual (1, sampleText.TableItemColumnSpan);
			procedureLogger.ExpectedResult ("the TableItemColumnSpan of the text in datagrid should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemContainingGrid of the text in datagrid");
			NF.Assert.AreEqual (dataGrid.AutomationElement, sampleText.TableItemContainingGrid);
			procedureLogger.ExpectedResult ("the TableItemContainingGrid of the text in datagrid should be dataGrid.AutomationElement");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemRow of the text in datagrid");
			NF.Assert.AreEqual (0, sampleText.TableItemRow);
			procedureLogger.ExpectedResult ("the TableItemRow of the text in datagrid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemRowHeaderItems of the text in datagrid");
			NF.Assert.AreEqual ("", sampleText.TableItemRowHeaderItems);
			procedureLogger.ExpectedResult ("the TableItemRowHeaderItems of the text in datagrid should be none");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemColumnHeaderItems of the text in datagrid");
			NF.Assert.AreEqual (sampleText.AutomationElement.GetCurrentPropertyValue (GridItemPattern.ContainingGridProperty),
				dataGrid.AutomationElement);
			procedureLogger.ExpectedResult ("the TableItemColumnHeaderItems of the text in datagrid should be ");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemRowSpan of the text in datagrid");
			NF.Assert.AreEqual (1, sampleText.TableItemRowSpan);
			procedureLogger.ExpectedResult ("the TableItemRowSpan of the text in datagrid should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.29 Click "Tools" menu item on the menu bar
			procedureLogger.Action ("Click \"Tools\" menu item on the menu bar");
			window.Find<MenuItem> ("Tools").Click (false);
			procedureLogger.ExpectedResult ("The sub menu of \"Tools\" appears");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.30 Click "Generate Password.." menu item on the sub menu 
			procedureLogger.Action ("Click \"Generate Password..\" menu item on the sub menu");
			window.Find<MenuItem> ("Generate Password...").Click (false);
			procedureLogger.ExpectedResult ("The Password Generator dialog appears");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.31 Select "Preview" Tab item
			procedureLogger.Action ("Select \"Preview\" Tab item");
			var tabItemPreview = window.Find<TabItem> ("Preview");
			tabItemPreview.Select ();
			procedureLogger.ExpectedResult ("The \"Preview\" tab item appears");
			Thread.Sleep (Config.Instance.ShortDelay); 

			//104.32 Use Scroll method give the vertical Scrollbar a LargeIncrement
			procedureLogger.Action ("Use Scroll method give the vertical Scrollbar a LargeIncrement");
			var passwordDocument = tabItemPreview.Find<Document> ();
			passwordDocument.Scroll (ScrollAmount.NoAmount, ScrollAmount.LargeIncrement);
			procedureLogger.ExpectedResult ("The vertical Scrollbar increase large");
			Thread.Sleep (Config.Instance.ShortDelay);
 
			//104.33 Use Scroll method give the vertical Scrollbar a SmallIncrement 
			procedureLogger.Action ("Use Scroll method give the vertical Scrollbar a LargeIncrement");
			passwordDocument.Scroll (ScrollAmount.NoAmount, ScrollAmount.SmallIncrement);
			procedureLogger.ExpectedResult ("The vertical Scrollbar increase large");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.34 Check the vertical scrollbar's position
			procedureLogger.Action ("after the SetScrollPercent action the scroll's position is 100.0d");
			NF.Assert.AreEqual (100.0d, passwordDocument.VerticalScrollPercent);
			procedureLogger.ExpectedResult ("The vertical scroll bar's Vertically Scrollable should be 100.0d");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.35 Use ScrollVertical method give the vertical Scrollbar a SmalIDecrement
			procedureLogger.Action ("Use Scroll method give the vertical Scrollbar a LargeIncrement");
			passwordDocument.Scroll (ScrollAmount.NoAmount, ScrollAmount.SmallDecrement);
			procedureLogger.ExpectedResult ("The vertical Scrollbar increase large");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.36 Use ScrollVertical method give the vertical Scrollbar a LargeDecrement
			procedureLogger.Action ("Use Scroll method give the vertical Scrollbar a LargeIncrement");
			passwordDocument.Scroll (ScrollAmount.NoAmount, ScrollAmount.LargeDecrement);
			procedureLogger.ExpectedResult ("The horizotal vertical increase large");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.37 Check the vertical scrollbar's position
			procedureLogger.Action ("after the SetScrollPercent action the scroll's position is 0.0d");
			NF.Assert.AreEqual (0.0d, passwordDocument.VerticalScrollPercent);
			procedureLogger.ExpectedResult ("The vertical scroll bar's VerticallyScrollable should be 0.0d");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.38 Use SetScrollPercent method make the vertical scrollbar move to (-1, 50) 
			procedureLogger.Action ("Use SetScrollPercent method make the horizotal scrollbar move to (-1, 50)");
			passwordDocument.SetScrollPercent (-1, 50);
			procedureLogger.ExpectedResult ("The horizotal Scrollbar move to (-1, 50)");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.39 Check the vertical scrollbar's position
			procedureLogger.Action ("after the SetScrollPercent action the scroll's position is 50.0d");
			NF.Assert.AreEqual (50.0d, passwordDocument.VerticalScrollPercent);
			procedureLogger.ExpectedResult ("The Horizontal scroll bar's VerticallyScrollable should be 50.0d");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.40 Minimize "NewDatabase.kdbx*-KeePass Password Safe" Window to (50,50)
			procedureLogger.Action ("click the Close button");
			window.Find<Button> ("Close").Click (false);
			procedureLogger.ExpectedResult ("The \"Password Generator\" dialogue disappears");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Minimize \"NewDatabase.kdbx*-KeePass Password Safe\" Window to (50, 50)");
			window.Resize (50, 50);
			procedureLogger.ExpectedResult ("NewDatabase.kdbx*-KeePass Password Safe\" Window is minimize to (50, 50)");
			Thread.Sleep (Config.Instance.ShortDelay); 
		}
	}
}