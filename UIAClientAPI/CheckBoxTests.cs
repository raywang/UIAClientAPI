// 
// CheckBoxTests.cs: CheckBox tests using UIA Client API.
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
using System.Diagnostics;
using System.Windows.Automation;
using System.Threading;
using Core;
using Core.Factory;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.WindowItems;
using NUnit.Framework;
using log4net;

namespace UIAClientAPI {
	class CheckBoxTests {

		// define variables
		static string ipy = @"ipy.exe";
		static string sample = @"D:\Accessibility\uia2atk\samples\winforms\checkbox.py";

		// declare the controls.
		static Application application = null;
		static Core.UIItems.CheckBox check1 = null;
		static Core.UIItems.CheckBox check2 = null;
		static Core.UIItems.CheckBox check3 = null;
		static Core.UIItems.CheckBox check4 = null;
		static Core.UIItems.CheckBox check5 = null;
		static Core.UIItems.CheckBox check6 = null;

		// init logger: 
		static ILog logger = LogManager.GetLogger ("UIAClientAPI");

		static void Main (string [] args)
		{
			InitSample ();
			RunTests ();
			application.Kill ();
			Console.ReadKey ();
		}

		public static void InitSample ()
		{
			// start sample.
			ProcessStartInfo processInfo = new ProcessStartInfo (ipy, sample);
			application = Application.Launch (processInfo);
			Thread.Sleep (5000);
			Window window = application.GetWindow ("CheckBox control", InitializeOption.NoCache);
			Assert.IsNotNull (window);

			check1 = window.Get<Core.UIItems.CheckBox> (SearchCriteria.ByText ("Bananas"));
			check2 = window.Get<Core.UIItems.CheckBox> (SearchCriteria.ByText ("Chicken"));
			check3 = window.Get<Core.UIItems.CheckBox> (SearchCriteria.ByText ("Stuffed Peppers"));
			check4 = window.Get<Core.UIItems.CheckBox> (SearchCriteria.ByText ("Beef"));
			check5 = window.Get<Core.UIItems.CheckBox> (SearchCriteria.ByText ("Fried Lizard"));
			check6 = window.Get<Core.UIItems.CheckBox> (SearchCriteria.ByText ("Soylent Green"));
		}

		public static void RunTests ()
		{
			List<CheckBox> checkBoxList = new List<CheckBox> ();
			checkBoxList.Add (check1);
			checkBoxList.Add (check2);
			checkBoxList.Add (check3);
			checkBoxList.Add (check4);
			checkBoxList.Add (check5);
			checkBoxList.Add (check6);

			// common properties checks
			foreach (CheckBox checkBox in checkBoxList) {
				Assert.IsNotNull (checkBox);
				CommonChecks (checkBox);
			}

			// check all the Enabled CheckBoxes
			checkBoxList.Remove (check4);
			foreach (CheckBox checkBox in checkBoxList) {
				EnabledCheckBoxTests (checkBox);
			}

			// check focus
			foreach (CheckBox checkBox in checkBoxList) {
				SwitchFocusTests (checkBox);
			}

			// check checkBox4 respectively
			checkBox4Test ();

			// check the results.         
			logger.Info("Success.");
		}

		public static void CommonChecks (CheckBox checkBox)
		{
			AutomationElement check = checkBox.AutomationElement;

			Assert.IsTrue ((bool) check.Current.IsContentElement);
			Assert.IsTrue ((bool) check.Current.IsControlElement);
			Assert.IsFalse ((bool) check.Current.IsPassword);
			Assert.IsFalse ((bool) check.Current.IsOffscreen);
			Assert.AreEqual (ControlType.CheckBox, check.Current.ControlType);
			Assert.AreEqual ("WinForm", check.Current.FrameworkId);
			Assert.AreEqual ("check box", check.Current.LocalizedControlType);
		}

		public static void EnabledCheckBoxTests (CheckBox checkBox)
		{
			AutomationElement check_AE = checkBox.AutomationElement;
			string checkName = check_AE.Current.Name;

			if (checkBox == check1)
				Assert.AreEqual ("Bananas", checkName);
			else if (checkBox == check2)
				Assert.AreEqual ("Chicken", checkName);
			else if (checkBox == check3)
				Assert.AreEqual ("Stuffed Peppers", checkName);
			else if (checkBox == check5)
				Assert.AreEqual ("Fried Lizard", checkName);
			else if (checkBox == check6)
				Assert.AreEqual ("Soylent Green", checkName);

			Assert.IsTrue ((bool) check_AE.Current.IsEnabled);
			Assert.IsTrue ((bool) check_AE.Current.IsKeyboardFocusable);

			// check TogglePattern
			TogglePattern check_TP = (TogglePattern) check_AE.GetCurrentPattern (TogglePattern.Pattern);

			if (checkBox == check3) {
				check_TP.Toggle ();
				Assert.AreEqual (ToggleState.Off, check_TP.Current.ToggleState);
				check_TP.Toggle ();
				Assert.AreEqual (ToggleState.On, check_TP.Current.ToggleState);
			} else {
				check_TP.Toggle ();
				Assert.AreEqual (ToggleState.On, check_TP.Current.ToggleState);
				check_TP.Toggle ();
				Assert.AreEqual (ToggleState.Off, check_TP.Current.ToggleState);
			}
		}


		public static void SwitchFocusTests (CheckBox checkBox)
		{
			// check Focus property.
			AutomationElement check_AE = checkBox.AutomationElement;
			if (check_AE.Current.HasKeyboardFocus != true) {
				check_AE.SetFocus ();
				Thread.Sleep (1000);
				Assert.IsTrue ((bool) check_AE.Current.HasKeyboardFocus);
			} else {
				Assert.IsTrue ((bool) check_AE.Current.HasKeyboardFocus);
			}
		}

		public static void checkBox4Test ()
		{
			// check Name and Focus properties.
			Assert.AreEqual ("Beef", check4.AutomationElement.Current.Name);
			Assert.IsFalse ((bool) check4.AutomationElement.Current.IsEnabled);
			Assert.IsFalse ((bool) check4.AutomationElement.Current.IsKeyboardFocusable);
			Assert.IsFalse ((bool) check4.AutomationElement.Current.HasKeyboardFocus);

			// trying to setFocus and check its results
			try {
				check4.AutomationElement.SetFocus ();
				Assert.Fail ("Expected Exception: InvalidOperationException");
			} catch (InvalidOperationException) {}	
			
			// check TogglePattern
			try {
				TogglePattern check4_TP = (TogglePattern) check4.AutomationElement.GetCurrentPattern (TogglePattern.Pattern);
				check4_TP.Toggle ();
				Assert.Fail ("Expected Exception: ElementNotEnabledException");
			} catch (ElementNotEnabledException) {}
		}
	}
}
