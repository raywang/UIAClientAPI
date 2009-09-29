// Helper.cs: Common use classes, methods.
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
using Core;
using System.Threading;
using NUnit.Framework;
using Core.UIItems.WindowStripControls;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.WindowItems;
using System.Windows.Automation;

namespace UIAClientAPI
{
	[TestFixture]
	/// TestBase class helps to launch sample, initiation etc.
	public class TestBase
	{
		// You could donwload our KeePass sampe from http://downloads.sourceforge.net/keepass/KeePass-2.09.zip
		// Note that the vesionm we use is 2.09
		private const string sample = @"Z:\Accessibility\KeePass-2.09\KeePass.exe";
		protected Application application = null;
		protected ProcedureLogger procedureLogger = new ProcedureLogger ();

		[TestFixtureSetUp]
		public void Init ()
		{
			//procedureLogger.Init ();
			LaunchSample ();
			OnSetup ();
		}

		[TestFixtureTearDown]
		public void Quit ()
		{
			OnQuit ();
			//application.Kill ();
			procedureLogger.Save ();
		}

		private void LaunchSample () {
			// start sample.
			application = Application.Launch (sample);
		}

		protected virtual void OnSetup ()
		{
		}

		protected virtual void OnQuit ()
		{
		}

	}

	// Basic instance which provides Find methods (e.g. FindButton, FindEdit..
	public class Element
	{
		protected AutomationElement element;

		// AutomationElement Name property.>
		public string Name
		{
			get { return element.Current.Name; }
		}

		public Element (Core.UIItems.IUIItem item) : this (item.AutomationElement)
		{
		}

		public Element (AutomationElement element)
		{
			this.element = element;
		}

		/// <summary>
		/// Find Element from a AutomationElement.
		/// </summary>
		/// <param name="type">the ControlType of the AutomationElement.</param>
		/// <param name="name">the AutomationId of the AutomationElement.</param>
		/// <returns></returns>
		public Element Find (ControlType type, string name)
		{
			for (int i = 0; i < Utils.RETRY_TIMES; i++) {
				var cond = new AndCondition (new PropertyCondition (AutomationElementIdentifiers.ControlTypeProperty, type),
					new PropertyCondition (AutomationElementIdentifiers.NameProperty, name));

				AutomationElement control = element.FindFirst (TreeScope.Descendants, cond);
				if (control != null)
					return Promote (control);

				Thread.Sleep (Utils.RETRY_INTERVAL);
			}
			return null;
		}

		public T[] FindAll<T> (ControlType type) where T : Element
		{
			for (int i = 0; i < Utils.RETRY_TIMES; i++) {
				var cond = new PropertyCondition (AutomationElementIdentifiers.ControlTypeProperty, type);
				AutomationElementCollection controls = element.FindAll (TreeScope.Descendants, cond);
				if (controls != null)
					return Promote<T> (controls);

				Thread.Sleep (Utils.RETRY_INTERVAL);
			}
			return null;
		}

		/// <summary>
		/// To promote a AutomationElement to a certain instance of a class,
		/// in order to get more specific mothods.
		/// </summary>
		/// <param name="elm">the element which will be promoted</param>
		/// <returns>a certain instance of a class</returns>
		protected Element Promote (AutomationElement elm)
		{
			if (elm.Current.ControlType == ControlType.Window)
				return new Window (elm);
			else if (elm.Current.ControlType == ControlType.Button)
				return new Button (elm);
			else if (elm.Current.ControlType == ControlType.Edit)
				return new Edit (elm);
			else if (elm.Current.ControlType == ControlType.CheckBox)
				return new CheckBox (elm);
			else if (elm.Current.ControlType == ControlType.RadioButton)
				return new RadioButton  (elm);
			else if (elm.Current.ControlType == ControlType.TabItem)
				return new TabItem (elm);
			else if (elm.Current.ControlType == ControlType.Spinner)
				return new Spinner (elm);

			return new Element (elm);
		}

		protected T [] Promote<T> (AutomationElementCollection elm) where T : Element
		{
			T [] ret = new T [elm.Count];
			for (int i = 0; i < elm.Count; i++)
				ret [i] = Promote (elm [i]) as T;
			return ret;
		}

		/// <summary>
		/// Find a Window object by its name.
		/// </summary>
		/// <param name="name">the title of the window.</param>
		/// <returns>a Window instance of UIAClientAPI namespace.</returns>
		public Window FindWindow (string name)
		{
			// use Client API to find window named name
			return (Window) Find (ControlType.Window, name);
		}

		/// <summary>
		/// Find a Button object by its name.
		/// </summary>
		/// <param name="name">the AutomationId of the button</param>
		/// <returns>a Button instance of UIAClientAPI namespace.</returns>
		public Button FindButton (string name)
		{
			return (Button) Find (ControlType.Button, name);
		}

		/// <summary>
		/// Find a Edit object by its name.
		/// </summary>
		/// <param name="name">>the AutomationId of the Edit</param>
		/// <returns>a Edit instance of UIAClientAPI namespace.</returns>
		public Edit FindEdit (string name)
		{
			return (Edit) Find (ControlType.Edit, name);
		}

		/// <summary>
		/// Find a CheckBox object by its name.
		/// </summary>
		/// <param name="name">the AutomationId of the CheckBox</param>
		/// <returns>a CheckBox instance of UIAClientAPI namespace</returns>
		public CheckBox FindCheckBox (string name)
		{
			return (CheckBox) Find (ControlType.CheckBox, name);
		}

		public RadioButton FindRadioButton (string name)
		{
			return (RadioButton) Find (ControlType.RadioButton, name);
		}

		public TabItem FindTabItem (string name)
		{
			return (TabItem) Find (ControlType.TabItem, name);
		}

		public Spinner FindSpinner (string name)
		{
			return (Spinner) Find (ControlType.Spinner, name);
		}
	}

	// Utils class runs until it gets True.
	public static class Utils
	{
		public const int RETRY_TIMES = 20;
		public const int RETRY_INTERVAL = 500;

		public static bool RetryUntilTrue (Func<bool> d)
		{
			for (int i = 0; i < RETRY_TIMES; i++) {
				if (d ())
					return true;

				Thread.Sleep (RETRY_INTERVAL);
			}

			return false;
		}
	}

	// Logger class which provides Actions logger and ExpectedResults logger.
	public class ProcedureLogger
	{
		public void Action (string action)
		{
		}

		public void ExpectedResult (string expectedResult)
		{
		}

		public void Save ()
		{
		}
	}

	// The wrapper class of Window class.
	public class Window : Element
	{
		public Window (Core.UIItems.WindowItems.Window elm)
			: base (elm)
		{
		}

		public Window (AutomationElement elm)
			: base (elm)
		{
		}

		// Click "OK" button of the window.
		public void OK ()
		{
			ClickButton ("OK");
		}

		// Click "Cancel" button of the window.
		public void Cancel ()
		{
			ClickButton ("Cancel");
		}

		// Click "Save" button of the window.
		public void Save ()
		{
			ClickButton ("Save");
		}

		// Click "Yes" button of the window
		public void Yes ()
		{
			ClickButton ("Yes");
		}

		// Click "No" button of the window
		public void No ()
		{
			ClickButton ("No");
		}

		// Click button by its name.
		public void ClickButton (string name)
		{
			try
			{
				Button button = FindButton(name);
				// ProcedureLogger.Action ("Click the "{0}" button in the {1}", button.Name, ...?);
				button.Click();
			}
			catch (NullReferenceException e)
			{
				Console.WriteLine(e);
			}			
		}
	}

	// The wrapper class of Button class.
	public class Button : Element
	{
		public Button (AutomationElement elm)
			: base (elm)
		{
		}

		/// <summary>
		/// Perform "Click" action.
		/// </summary>
		public void Click ()
		{
			InvokePattern ip = (InvokePattern) element.GetCurrentPattern (InvokePattern.Pattern);
			ip.Invoke ();
		}
	}

	public class Edit : Element
	{
		public Edit (AutomationElement elm)
			: base (elm)
		{
		}

		/// <summary>
		/// wrapper SetValue method as a property to set value to the Edit control.
		/// </summary>
		public string Value
		{
			get
			{
				ValuePattern vp = (ValuePattern) element.GetCurrentPattern (ValuePattern.Pattern);
				return vp.Current.Value;
			}
			set
			{
				ValuePattern vp = (ValuePattern) element.GetCurrentPattern (ValuePattern.Pattern);
				vp.SetValue (value);
			}
		}
	}

	public class CheckBox : Element
	{
		public CheckBox (AutomationElement elm)
			: base (elm)
		{
		}

		/// <summary>
		/// Perform "Toggle" action.
		/// </summary>
		public void Toggle ()
		{
			TogglePattern tp = (TogglePattern) element.GetCurrentPattern (TogglePattern.Pattern);
			tp.Toggle ();
		}
	}

	public class RadioButton : Element
	{
		public RadioButton (AutomationElement elm)
			: base (elm)
		{
		}

		public void Select ()
		{
			SelectionItemPattern sp = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sp.Select ();
		}
	}

	public class TabItem : Element
	{
		public TabItem (AutomationElement elm)
			: base (elm)
		{
		}

		public void Select ()
		{
			SelectionItemPattern sp = (SelectionItemPattern) element.GetCurrentPattern (SelectionItemPattern.Pattern);
			sp.Select ();
		}
	}

	public class Spinner : Element
	{
		public Spinner (AutomationElement elm)
			: base (elm)
		{
		}

		public double Value
		{
			get
			{
				RangeValuePattern rp = (RangeValuePattern) element.GetCurrentPattern (RangeValuePattern.Pattern);
				return rp.Current.Value;
			}
			set
			{
				RangeValuePattern rp = (RangeValuePattern) element.GetCurrentPattern (RangeValuePattern.Pattern);
				rp.SetValue (value);
			}
		}
	}
}