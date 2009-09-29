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

	public class Element
	{
		protected AutomationElement element;

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
		
		// window.Find (ControlType.Button, "OK")
		public Button FindButton (string name)
		{
			return (Button) Find (ControlType.Button, name);
		}

		public Edit FindEdit (string name)
		{
			return (Edit) Find (ControlType.Edit, name);
		}

		public Window FindWindow (string name)
		{
			// use Client API to find window named name
			return (Window) Find (ControlType.Window, name);
		}

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

		protected Element Promote (AutomationElement elm)
		{
			if (elm.Current.ControlType == ControlType.Button)
				return new Button (elm);
			else if (elm.Current.ControlType == ControlType.Window)
				return new Window (elm);
			else if (elm.Current.ControlType == ControlType.Edit)
				return new Edit (elm);

			return new Element (elm);
		}
	}

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

		public void Ok ()
		{
			ClickButton ("Ok");
		}

		public void Cancel ()
		{
			ClickButton ("Cancel");
		}

		public void Save ()
		{
			ClickButton ("Save");
		}

		public void ClickButton (string name)
		{
            //using find method to find a button
            try
            {
                Button button = FindButton(name);
                button.Click();
            }

            //if the button does not exist, throw an exception
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }

			// ProcedureLogger.Action ("Click the "{0}" button in the {1}", button.Name, ...?);

			
		}
	}

	public class Button : Element
	{
		public Button (AutomationElement elm)
			: base (elm)
		{
		}

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

	/*
	public class KeePassToolBar
	{
		private Window window = null;

		public KeePassToolBar (Window win)
		{
			this.window = win;
		}

		public void ClickButton (string primaryIdentification)
		{
			AutomationElement button = GetToolBarButton (primaryIdentification);
			InvokePattern ip = (InvokePattern) button.GetCurrentPattern (InvokePattern.Pattern);
			ip.Invoke ();
		}

		public AutomationElement GetToolBarButton (string primaryIdentification)
		{
			ToolStrip toolBar = window.ToolStrip;
			Assert.IsNotNull (toolBar, "ToolBar item is null");

			AutomationElement button = toolBar.Get<Button> (SearchCriteria.ByText (primaryIdentification)).AutomationElement;
			Assert.IsNotNull (button, "%s button item is null", primaryIdentification);

			return button;
		}
	}

	public class KeePassNewPasswdDialog
	{
		private Window window = null;

		public KeePassNewPasswdDialog (Window win)
		{
			this.window = win;
		}

		public AutomationElement GetElement (ControlType controlType, string primaryIdentification)
		{
			Window dialog = window.ModalWindow ("Create New Password Database");
			Assert.IsNotNull (dialog, "new password dailog item is null");

			AutomationElement control = dialog.Get
				(SearchCriteria.ByControlType (controlType).AndByText (primaryIdentification)).AutomationElement;
			//var cond = new AndCondition (new PropertyCondition(AutomationElementIdentifiers.ControlTypeProperty, controlType),
			//        new PropertyCondition(AutomationElementIdentifiers.NameProperty, primaryIdentification));
			//var control = dialog.AutomationElement.FindFirst (TreeScope.Descendants, cond);

			return control;
		}


		public void ClickButton (string primaryIdentification)
		{
			AutomationElement button = GetElement (ControlType.Button, primaryIdentification);
			InvokePattern ip = (InvokePattern) button.GetCurrentPattern (InvokePattern.Pattern);
			ip.Invoke ();
		}

		public void SetValue (AutomationElement edit, string text)
		{
			ValuePattern vp = (ValuePattern) edit.GetCurrentPattern (ValuePattern.Pattern);
			vp.SetValue (text);
		}
	}
	 */

}