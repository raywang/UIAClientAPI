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

		//protected override void OnSetup()
		//{
		//        procedureLogger.ExpectedResult ("KeePass window appears.");
		//        WhiteWindow win = application.GetWindow ("KeePass Password Safe", InitializeOption.NoCache);
		//        window = new Window (win);			
		//}

		protected override void OnSetup ()
		{
			procedureLogger.ExpectedResult ("WindowPattern & TransformPattern Test window appears.");
			WhiteWindow win = application.GetWindow ("WindowPattern & TransformPattern Test", InitializeOption.NoCache);
			window = new Window (win);
		}

		[Test]
		//TestCase101 Init Sample, create a new account
		public void TestCase101 ()
		{
			//101.1 Click the "New..." button on the toolbar.
			var toolBar = window.FindToolBar ("");
			toolBar.FindButton ("New...").Click();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.2 Enter "TestCase101" in the "File Name" combo box of the dailog.
			var newPassDialog = window.FindWindow ("Create New Password Database");
			var fileNameEdit = newPassDialog.FindEdit ("File name:");
			fileNameEdit.Value = "TestCase101";
			procedureLogger.ExpectedResult ("\"TestCase101\" entered in the \"File Name\" box.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.3 Click the "Save" button of the dialog.
			newPassDialog.Save ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.4 Enter "mono-a11y" into  "Master password" text box.
			var createMasterKeyWindow = window.FindWindow ("Create Composite Master Key");
			var masterPasswdEdit = createMasterKeyWindow.FindEdit ("Repeat password:", "m_tbPassword");
			masterPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.5  Re-Enter "mono-a11y" into "Repeat password" text box.
			var repeatPasswdEdit = createMasterKeyWindow.FindEdit ("Repeat password:", "m_tbRepeatPassword");
			repeatPasswdEdit.Value = "mono-a11y";
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Repeat password\" box.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.6 Check "Key file/option" CheckBox
			procedureLogger.Action ("Check the \"Key file/option\" CheckBox.");
			createMasterKeyWindow.FindCheckBox ("Key file / provider:").Toggle();
			procedureLogger.ExpectedResult ("\"Key file/option\" CheckBox chekced.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.7 Click the " Create..." button.
			createMasterKeyWindow.FindButton (" Create...").Click();
			procedureLogger.ExpectedResult ("The \"Create a new key file\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.8  Click the "Save" button of the dialog.
			var newKeyFileDialog = window.FindWindow ("Create a new key file");
			newKeyFileDialog.Save();
			
			//in case there is a TestCase101 key exist.
			var comfirmDialog = newKeyFileDialog.FindWindow("Confirm Save As");
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
			createMasterKeyWindow.FindWindow ("Entropy Collection").OK ();
			procedureLogger.ExpectedResult ("The \"Entropy Collection\" window disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.10 Click the "OK" button on the "Create Master Key" Window
			createMasterKeyWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Create Master Key\" window disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.11 Select the "Compression" Tab item.
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			var compressionTabItem = newPassDialog2.FindTabItem ("Compression");
			compressionTabItem.Select ();
			procedureLogger.ExpectedResult ("The \"Compression\" tab item opened.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//101.12 Check the "None" RadioButton.
			compressionTabItem.FindRadioButton ("None").Select ();
			procedureLogger.ExpectedResult ("The \"None\" radio button selected.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//TODO: We can't change the value of Spinner by using
			// RangeValuePattern.SetValue, since the maximum 
			// and mininum value are all 0, seems like the Spinner 
			// did not well provides proper provider here.

			////101.13 Select "Security" Tab item;
			//var securityTabItem = newPassDialog2.FindTabItem ("Security");
			//securityTabItem.Select ();
			//Thread.Sleep(Config.Instance.ShortDelay);

			////101.14 Enter a number "3000" in "Key transformation" spinner
			//var keySpinner = newPassDialog2.FindSpinner ("Number of key transformation rounds:");
			////keySpinner.Value = 3000;
			//Thread.Sleep(Config.Instance.ShortDelay);

			//101.15 Click the "OK" button to close the dialog.
			newPassDialog2.OK ();
			procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window disappears.");
			Thread.Sleep(Config.Instance.ShortDelay);
		}

		[Test]
		//TestCase102 Organize the group
		public void TestCase102 ()
		{
			//102.1 Click the "Edit" menu item on the menu bar.
			var menuBar = window.FindMenuBar ();
			var editMenuItem = menuBar.FindMenuItem ("Edit");
			editMenuItem.FindMenuItem ("Edit Group").Click ();
			procedureLogger.ExpectedResult ("The \"Edit Group\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//102.2 Click the "Icon" button on the "Edit Group" dialog.
			var editGroupWindow = window.FindWindow ("Edit Group");
			var generalTabItem = editGroupWindow.FindTabItem ("General");
			generalTabItem.FindButton ("Icon:").Click ();
			procedureLogger.ExpectedResult ("The \"Icon Picker\" dialog opens.");
			Thread.Sleep(Config.Instance.ShortDelay);

			//102.3 Select list item "30" on the "Icon Picker" dialog.
			var iconPickerWindow = editGroupWindow.FindWindow ("Icon Picker");
			var standardIconList = iconPickerWindow.FindList ("", "m_lvIcons");
			var listItem30 = standardIconList.FindListItem ("30");
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
			standardIconList.FindListItem ("68").Show ();
			procedureLogger.ExpectedResult ("The \"68\" list item is showed in the view.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.7 Click the "OK" button on the "Icon Picker" dialog.
			iconPickerWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Icon Picker\" dialog disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.8 Select the "Behavior" Tab Item from the Tab.
			var behaviorTabItem = editGroupWindow.FindTabItem ("Behavior");
			behaviorTabItem.Select ();
			procedureLogger.ExpectedResult ("The \"Behavior\" tab item opens.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.9 Expand the "Searching entries in this group" combo box.
			var searchCombobox = behaviorTabItem.FindComboBox ("Searching entries in this group:");
			searchCombobox.Expand ();
			procedureLogger.ExpectedResult ("\"Searching entries in this group\" combox box is expanded.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.10 Select the "Enabled" from the "Searching entries in this group" combo box.
			searchCombobox.FindListItem ("Enabled").Select ();
			procedureLogger.ExpectedResult ("The \"Enabled\" list item is selected.");
			Thread.Sleep (Config.Instance.ShortDelay);

			//102.11 Click the "OK" button on the  "Edit Group" dialog.
			editGroupWindow.OK ();
			procedureLogger.ExpectedResult ("The \"Edit Group\" dialog disappears.");
			Thread.Sleep (Config.Instance.ShortDelay);
		}

		[Test]
		//TestCase103 test the "Add Entry" dialog
		public void TestCase103 ()
		{
			//103.1 Click "new" button on the toolstripbar
			procedureLogger.Action ("Click \"New...\" button on the toolbar");
			var toolBar = window.FindToolBar ("");
			toolBar.FindButton ("New...").Click (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.2 Click "Save" button on the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			var newPassDialog = window.FindWindow ("Create New Password Database");
			newPassDialog.Save (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.3 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var keyDialog = window.FindWindow ("Create Composite Master Key");
			keyDialog.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.4 Click "Yes" button on the dialog
			procedureLogger.Action ("Click \"Yes\" button on the KeePass dialog");
			var createMasterKeyWindow = window.FindWindow ("KeePass");
			createMasterKeyWindow.Yes (false);
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.5 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			newPassDialog2.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.6 Click "Add Entry" button on the toolstripbar
			procedureLogger.Action ("Click \"Add Entry\" button on the toolstripbar");
			toolBar.FindButton ("Add Entry").Click (false);
			procedureLogger.ExpectedResult ("\"Key file/option\" CheckBox chekced");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.7 Check "Add Entry" window's default WindowPattern Property
			procedureLogger.Action ("Check \"Add Entry\" window's CanMaximizeProperty");
			Window addEntryWindow = window.FindWindow ("Add Entry");
			Assert.AreEqual (false, addEntryWindow.CanMaximize);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's CanMinimizeProperty");
			Assert.AreEqual (false, addEntryWindow.CanMinimize);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's IsModalProperty");
			Assert.AreEqual (true, addEntryWindow.IsModal);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's IsTopmostProperty");
			Assert.AreEqual (false, addEntryWindow.IsTopmost);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's WindowInteractionStateProperty");
			Assert.AreEqual (WindowInteractionState.ReadyForUserInteraction, addEntryWindow.WindowInteractionState);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's WindowVisualStateProperty");
			Assert.AreEqual (WindowVisualState.Minimized, addEntryWindow.WindowVisualState);
			procedureLogger.ExpectedResult ("The window's CanMaximizeProperty should be False");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.8 move "add entry" window to (200,200 )
			procedureLogger.Action ("move \"add entry\" window to (200,200 )");
			var AddEntryDialog = window.FindWindow ("Add Entry");
			AddEntryDialog.Move (200, 200);
			procedureLogger.ExpectedResult ("the \"add entry\" window is moved to (200,200 )");
			Thread.Sleep(Config.Instance.ShortDelay);

			//check the transformpattern's property
			procedureLogger.Action ("Check \"Add Entry\" window's CanMoveProperty");
			Assert.AreEqual (true, addEntryWindow.CanMove);
			procedureLogger.ExpectedResult ("The window's CanMoveProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's CanSizeProperty");
			Assert.AreEqual (false, addEntryWindow.CanResize);
			procedureLogger.ExpectedResult ("The window's CanSizeProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check \"Add Entry\" window's CanRotateProperty");
			Assert.AreEqual (false, addEntryWindow.CanRotate);
			procedureLogger.ExpectedResult ("The window's CanRotateProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			//103.9 Click the "Auto-Type" tab item on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Auto-Type\" tab item on the \"Add Entry\" Window");
			var tabItemAuto = window.FindTabItem ("Auto-Type");
			tabItemAuto.Select ();
			procedureLogger.ExpectedResult ("The \"Auto-Type\" tab item appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.10 Click the "Add" button on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Add\" button on the \"Add Entry\" Window");
			AddEntryDialog.FindButton("Add").Click(false);
			procedureLogger.ExpectedResult ("The \"Edit Auto-Type Item\" window appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.11 Drag the scroll bar to the bottom on the "Edit Auto-Type Item" window
			procedureLogger.Action ("drag the scroll bar to the 300 position");
			var autoItemDialog = window.FindWindow ("Edit Auto-Type Item");
			ScrollBar scrollBar = window.FindScrollBar ("");
			scrollBar.SetValue (300);
			procedureLogger.ExpectedResult ("the scroll bar is draged to the 413 position");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.12 Check the scroll bar's property
			procedureLogger.Action ("Check scroll bar's IsReadOnlyProperty");
			Assert.AreEqual (false, scrollBar.IsReadOnly);
			procedureLogger.ExpectedResult ("The scroll bar's IsReadOnlyProperty should be False");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's LargeChangeProperty");
			Assert.AreEqual (131, (int)scrollBar.LargeChange);
			procedureLogger.ExpectedResult ("The scroll bar's large chaged value should be 131");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's LargeChangeProperty");
			Assert.AreEqual (1, (int) scrollBar.SmallChange);
			procedureLogger.ExpectedResult ("The scroll bar's large chaged value should be 131");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's Maximum value");
			Assert.AreEqual (362, (int)scrollBar.Maximum);
			procedureLogger.ExpectedResult ("The scroll bar's Maximum value shoule be 362");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's Minimum value");
			Assert.AreEqual (0, (int)scrollBar.Minimum);
			procedureLogger.ExpectedResult ("The scroll bar's minimum value shoule be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check scroll bar's value whether equal to 300");
			Assert.AreEqual (300, (int)scrollBar.Value);
			procedureLogger.ExpectedResult ("The scroll bar's value should be 300");
			Thread.Sleep (Config.Instance.ShortDelay);

			//103.13 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button on the dialog");
			autoItemDialog.OK (false);
			procedureLogger.ExpectedResult ("\"None\" radio button selected");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.14 Click the "Advanced" tab item on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Advanced\" tab item on the \"Add Entry\" Window");
			var tabItemAdvanced = window.FindTabItem ("Advanced");
			tabItemAdvanced.Select ();
			procedureLogger.ExpectedResult ("The \"Advanced\" tab item appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//103.15 Click the "Add" button on the "Add Entry" Window
			procedureLogger.Action ("Click the \"Add\" button on the \"Add Entry\" Window");
			AddEntryDialog.FindButton ("Add").Click (false);
			procedureLogger.ExpectedResult ("The \"Edit Entry String\" dialog appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//TODO: need to call a keyboard operation API in another dll
			//103.16 Type the "aa" into the "Name" edit
			//procedureLogger.Action ("Type the \"aa\" into the \"Name\" edit");
			var editEntryStringWindow = window.FindWindow ("Edit Entry String");
			//var nameEdit = editEntryStringWindow.FindEdit ("Name:");
			//nameEdit.Value = "aa";
			//procedureLogger.ExpectedResult("the \"name\" edit 's value is \"aa\"");
			//Thread.Sleep(Config.Instance.ShortDelay);

			//TODO: replace "OK" by "Cancel" because be blocked by 103.16
			//103.17 Click "OK" button on the "Edit Entry String" dialog
			procedureLogger.Action ("Click \"OK\" button on the \"Edit Entry String\" dialog");
			editEntryStringWindow.Cancel (false);
			procedureLogger.ExpectedResult ("The \"Edit Entry String\" window closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//TODO blocked by 103.16
			//103.18 Check the "aa" text's TableItemPattern
			//procedureLogger.Action ("Check the \"aa\" text's TableItemPattern");
			//procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window closes");
			//newPassDialog2.OK ();
			//Thread.Sleep(Config.Instance.ShortDelay);

			//103.19 Close the "Add Entry" Window
			procedureLogger.Action ("Close the \"Add Entry\" Window");
			AddEntryDialog.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database - Step 2\" window closes");
			Thread.Sleep(Config.Instance.ShortDelay);
		}

		[Test]
		//TestCase104 test the "Password Generator" dialog
		public void TestCase104 ()
		{
			//104.1 Click "new" button on the toolstripbar
			procedureLogger.Action ("Click \"New...\" button on the toolbar");			
			var toolBar = window.FindToolBar ("");
			toolBar.FindButton ("New...").Click (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog opens");
			Thread.Sleep(Config.Instance.ShortDelay);

			//104.2 Click "Save" button on the dialog
			procedureLogger.Action ("Click \"Save\" button of the dialog");
			var newPassDialog = window.FindWindow ("Create New Password Database");
			newPassDialog.Save (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep(Config.Instance.ShortDelay);

			//104.3 Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var keyDialog = window.FindWindow ("Create Composite Master Key");
			keyDialog.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.4 Click "Yes" button on the dialog
			procedureLogger.Action ("Click \"Yes\" button on the KeePass dialog");
			var createMasterKeyWindow = window.FindWindow ("KeePass");
			createMasterKeyWindow.Yes (false);
			procedureLogger.ExpectedResult ("\"mono-a11y\" entered in the \"Master password\" box");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.5  Click "OK" button on the dialog
			procedureLogger.Action ("Click \"OK\" button of the dialog");
			var newPassDialog2 = window.FindWindow ("Create New Password Database - Step 2");
			newPassDialog2.OK (false);
			procedureLogger.ExpectedResult ("The \"Create New Password Database\" dialog closes");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.6 Check the "title" of the exist record 
			procedureLogger.Action ("Check the fist item in the datagrid");
			DataGrid dataGrid = window.FindDataGrid ("");
			dataGrid.GetItem (0, 0);
			//Assert.AreEqual ("Record", dataGrid.GetItem (0, 0));
			procedureLogger.ExpectedResult ("the first item in the datagrid should be Record");
			Thread.Sleep (Config.Instance.ShortDelay);
			
			//check GridPattern 's property
			procedureLogger.Action ("check GridPattern 's property :ColumnCount");
			Assert.AreEqual (11, dataGrid.ColumnCount);
			procedureLogger.ExpectedResult ("the data grid 's ColumnCount should be 11");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("check GridPattern 's property :RowCount");
			Assert.AreEqual (1, dataGrid.RowCount);
			procedureLogger.ExpectedResult ("the data grid 's RowCount should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.7 Check the data grid's MultipleViewPattern property
			procedureLogger.Action ("Click \" Create...\" button");
			Assert.AreEqual (1,dataGrid.CurrentViewProperty);
			procedureLogger.ExpectedResult ("the data grid 's CurrentViewProperty should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.8 Sets the current control-specific view
			procedureLogger.Action ("Sets the current control-specific view");
			dataGrid.SetCurrentView (0);
			procedureLogger.ExpectedResult ("Sets the current control-specific view to 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			//TO DO. viewname
			//104.9  Retrieves the name of a control-specific view  of the data grid
			procedureLogger.Action ("Retrieves the name of a control-specific view  of the data grid");
			//string viewname= dataGrid.GetViewName (0);
			//Assert.AreEqual ("",viewname);
			procedureLogger.ExpectedResult ("the name of a control-specific view  of the data grid should be");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.10  Check the GridItemPattern's property of text in data grid
			procedureLogger.Action ("Check the column of data grid");
			Assert.AreEqual (0,dataGrid.Column);
			procedureLogger.ExpectedResult ("the column of data grid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the column span of data grid");
			procedureLogger.ExpectedResult ("the column span of data grid should be 1");
			Assert.AreEqual (1, dataGrid.ColumnSpan);
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the row of data grid");
			Assert.AreEqual (0, dataGrid.Row);
			procedureLogger.ExpectedResult ("the row of data grid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the row span of data grid");
			Assert.AreEqual (1, dataGrid.RowSpan);
			procedureLogger.ExpectedResult ("the row span of data grid should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the ContainingGrid of data grid");
			Assert.AreEqual (null, dataGrid.ContainingGrid);
			procedureLogger.ExpectedResult ("the ContainingGrid of data grid should be null");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.11 Check the TableItem's property of text in data grid
			procedureLogger.Action ("Check the TableItemColumn of the text in datagrid");
			Text sampleText = window.FindText ("Sample Entry");
			Assert.AreEqual (0, sampleText.TableItemColumn);
			procedureLogger.ExpectedResult ("the TableItemColumn of the text in datagrid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemColumnSpan of the text in datagrid");
			Assert.AreEqual (1, sampleText.TableItemColumnSpan);
			procedureLogger.ExpectedResult ("the TableItemColumnSpan of the text in datagrid should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			//TODO.
			//procedureLogger.Action ("Check the TableItemContainingGrid of the text in datagrid");
			//Assert.AreEqual (AutomationProperty.LookupById, sampleText.TableItemContainingGrid);
			//procedureLogger.ExpectedResult ("the TableItemContainingGrid of the text in datagrid should be");
			//Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemContainingGrid of the text in datagrid");
			Assert.AreEqual (0, sampleText.TableItemRow);
			procedureLogger.ExpectedResult ("the TableItemContainingGrid of the text in datagrid should be 0");
			Thread.Sleep (Config.Instance.ShortDelay);

			//procedureLogger.Action ("Check the TableItemRowHeaderItems of the text in datagrid");
			//Assert.AreEqual (1, sampleText.TableItemRowHeaderItems);
			//procedureLogger.ExpectedResult ("the TableItemRowHeaderItems of the text in datagrid should be");
			//Thread.Sleep (Config.Instance.ShortDelay);

			//procedureLogger.Action ("Check the TableItemRowHeaderItems of the text in datagrid");
			//Assert.AreEqual (1, sampleText.TableItemColumnHeaderItems);
			//procedureLogger.ExpectedResult ("the TableItemColumnHeaderItems of the text in datagrid should be");
			//Thread.Sleep (Config.Instance.ShortDelay);

			procedureLogger.Action ("Check the TableItemRowSpan of the text in datagrid");
			Assert.AreEqual (1, sampleText.TableItemRowSpan);
			procedureLogger.ExpectedResult ("the TableItemRowSpan of the text in datagrid should be 1");
			Thread.Sleep (Config.Instance.ShortDelay);

			//104.12 Click "Tools" menu item on the menu bar
			procedureLogger.Action ("Click \"Tools\" menu item on the menu bar");
			window.FindMenuItem ("Tools").Click (false);
			procedureLogger.ExpectedResult ("The sub menu of \"Tools\" appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//104.13 Click "Generate Password.." menu item on the sub menu 
			procedureLogger.Action ("Click \"Generate Password..\" menu item on the sub menu");
			window.FindMenuItem ("Generate Password...").Click (false);
			procedureLogger.ExpectedResult ("The Password Generator dialog appears");
			Thread.Sleep(Config.Instance.ShortDelay);

			//104.14 Select "Preview" Tab item
			procedureLogger.Action ("Select \"Preview\" Tab item");
			var tabItemCompression = window.FindTabItem ("Preview");
			tabItemCompression.Select ();
			procedureLogger.ExpectedResult ("The \"Preview\" tab item appears");
			Thread.Sleep (Config.Instance.ShortDelay); 

			//104.15 Set the Scroll's VerticalAmount to "smallDecrement"
			var document = window.FindDocument (" ");
			//document.Scroll (ScrollAmount.SmallDecrement, ScrollAmount.SmallDecrement);
			//procedureLogger.ExpectedResult ("the Scroll's VerticalAmount is set to \"smallDecrement\"");
			//Thread.Sleep (Config.Instance.ShortDelay); 

			//104.16 Set the Scroll HorizontalAmount to "smallDecrement"
			//procedureLogger.Action ("Set the ScrollHorizontal's VerticalAmount to \"smallDecrement\"");
			//document.ScrollHorizontal (ScrollAmount.LargeIncrement);
			//procedureLogger.ExpectedResult ("the Scroll's Horizontal is set to \"smallDecrement\"");
			//Thread.Sleep (Config.Instance.ShortDelay); 

			//104.17 Set the ScrollVertical's VerticalAmount to "smallDecrement"
			//procedureLogger.Action ("Set the ScrollVertical's VerticalAmount to \"smallDecrement\"");
			//document.ScrollVertical (ScrollAmount.LargeIncrement);
			//procedureLogger.ExpectedResult ("the Scroll's ScrollVertical is set to \"smallDecrement\"");
			//Thread.Sleep (Config.Instance.ShortDelay); 

			//104.18 Set the SetScrollPercent's VerticalAmount to "smallDecrement"
			//procedureLogger.Action ("Set the ScrollVertical's VerticalAmount to \"smallDecrement\"");
			//document.SetScrollPercent (0, 100);
			//procedureLogger.ExpectedResult ("the Scroll's ScrollPercent is set to ");
			//Thread.Sleep (Config.Instance.ShortDelay); 

			procedureLogger.Action ("click the OK button");
			window.FindButton ("OK").Click();
			procedureLogger.ExpectedResult ("The \"Password Generator\" dialogue disappears");
			Thread.Sleep (Config.Instance.ShortDelay); 

			//104.19 Minimize "NewDatabase.kdbx*-KeePass Password Safe" Window
			procedureLogger.Action ("Minimize \"NewDatabase.kdbx*-KeePass Password Safe\" Window to (50, 50)");
			window.Resize (50, 50);
			procedureLogger.ExpectedResult ("NewDatabase.kdbx*-KeePass Password Safe\" Window is minimize to (50, 50)");
			Thread.Sleep (Config.Instance.ShortDelay); 
		}

		public void TestCase105 ()
		{
			//105.1 Move the dock to the Left
			var dock = window.FindPane ("Top");
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
			var pane = window.FindPane ("TransformProviderControl1, r:0");
			pane.Rotate (90.0);
			procedureLogger.ExpectedResult ("The pane would be rotated for 90 degree.");
			Thread.Sleep (Config.Instance.ShortDelay);
		}
	}
}